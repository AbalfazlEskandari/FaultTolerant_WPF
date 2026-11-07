// Author: Abalfazl Eskandari - aeskandari.ir
using System.Collections.ObjectModel;
using System.Windows.Input;
using FaultTolerance.Commands;
using FaultTolerance.Models;
using FaultTolerance.Services;

namespace FaultTolerance.ViewModels;

/// <summary>
/// Shared behaviour of both pages: inputs, expected output, result banner and the diagnostic box.
/// Flow: user presses the action button -> backend checks the circuit ->
/// if the output is wrong the diagnostic box opens and YOUR logic (StartDiagnosis / OnMeasuredOutput) takes over.
/// </summary>
public abstract class CircuitPageViewModelBase : ViewModelBase
{
    private int _expectedOutput = 1;
    private EvaluationState _state;
    private string _resultMessage = string.Empty;
    private string _resultDetail = string.Empty;
    private string _outputDisplay = "?";

    protected CircuitPageViewModelBase(
        IBackendService backend,
        string title,
        string subtitle,
        IReadOnlyList<string> inputNames,
        string actionLabel)
    {
        Backend = backend;
        Title = title;
        Subtitle = subtitle;
        ActionLabel = actionLabel;

        Inputs = new ObservableCollection<InputViewModel>(inputNames.Select(name => new InputViewModel(name)));
        foreach (InputViewModel input in Inputs)
        {
            input.PropertyChanged += (_, _) => ResetResult();
        }

        Diagnostic = new DiagnosticViewModel(inputNames, OnMeasuredOutput);
        EvaluateCommand = new RelayCommand(_ => Evaluate());
    }

    protected IBackendService Backend { get; }

    public string Title { get; }

    public string Subtitle { get; }

    public string ActionLabel { get; }

    /// <summary>Inputs of the first test (settings panel + labels in the drawing).</summary>
    public ObservableCollection<InputViewModel> Inputs { get; }

    /// <summary>Diagnostic box state; drive it from StartDiagnosis / OnMeasuredOutput.</summary>
    public DiagnosticViewModel Diagnostic { get; }

    public int ExpectedOutput
    {
        get => _expectedOutput;
        set
        {
            if (SetProperty(ref _expectedOutput, value))
            {
                ResetResult();
            }
        }
    }

    public EvaluationState State
    {
        get => _state;
        private set => SetProperty(ref _state, value);
    }

    public string ResultMessage
    {
        get => _resultMessage;
        private set => SetProperty(ref _resultMessage, value);
    }

    public string ResultDetail
    {
        get => _resultDetail;
        private set => SetProperty(ref _resultDetail, value);
    }

    /// <summary>Text of the output badge in the drawing: "?" until evaluated.</summary>
    public string OutputDisplay
    {
        get => _outputDisplay;
        private set => SetProperty(ref _outputDisplay, value);
    }

    public ICommand EvaluateCommand { get; }

    /// <summary>Runs this page's circuit through the backend.</summary>
    protected abstract EvaluationResult Run(IReadOnlyList<bool> inputs, bool expected);

    protected void ResetResult()
    {
        State = EvaluationState.None;
        ResultMessage = string.Empty;
        ResultDetail = string.Empty;
        OutputDisplay = "?";
    }

    private void Evaluate()
    {
        EvaluationResult result = Run(Inputs.Select(input => input.Bit).ToArray(), ExpectedOutput != 0);

        OutputDisplay = result.Actual.ToBit().ToString();
        State = result.IsCorrect ? EvaluationState.Correct : EvaluationState.Wrong;
        ResultMessage = result.IsCorrect ? "Correct!" : "Wrong!";
        ResultDetail = $"Expected output: {result.Expected.ToBit()}   |   Actual output: {result.Actual.ToBit()}";

        if (result.IsCorrect)
        {
            Diagnostic.Hide();
        }
        else
        {
            Diagnostic.Hide();      // restart numbering at Test #1
            StartDiagnosis();
        }
    }

    // =====================================================================================
    //  YOUR DIAGNOSIS LOGIC GOES HERE
    //  The two methods below are only a working EXAMPLE of how to use the diagnostic box.
    //  Override them in Page2ViewModel, or replace the bodies. Delete the example freely.
    //
    //    Diagnostic.ShowTest(new[] { 1, 0, 1 }, "text")  -> box shows "Test #n" and the inputs x1=1 x2=0 x3=1
    //    OnMeasuredOutput(int measured)                  -> called when the user presses "Submit result"
    //    Diagnostic.Status = "..."                       -> free message line
    //    Diagnostic.Finish("...")                        -> done, no more results are requested
    // =====================================================================================

    private int _exampleStep;

    /// <summary>Called once, right after a wrong result. Show the first test here.</summary>
    protected virtual void StartDiagnosis()
    {
        _exampleStep = 0;
        ShowExampleTest();
    }

    /// <summary>Called with the output the user measured on the real circuit for the current test.</summary>
    protected virtual void OnMeasuredOutput(int measuredOutput)
    {
        // Your logic: compare measuredOutput with what a healthy circuit gives, narrow down the faulty wire, ...
        _exampleStep++;

        if (_exampleStep >= 1 << Inputs.Count)
        {
            Diagnostic.Finish("Example finished - put your own diagnosis here.");
        }
        else
        {
            ShowExampleTest();
        }
    }

    /// <summary>Example only: walks through the truth table (000, 001, 010, ...).</summary>
    private void ShowExampleTest()
    {
        int count = Inputs.Count;
        int[] bits = Enumerable.Range(0, count).Select(i => (_exampleStep >> (count - 1 - i)) & 1).ToArray();
        Diagnostic.ShowTest(bits, "Apply these inputs to your circuit, then enter the output you measure.");
    }
}
