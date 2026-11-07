// Author: Abalfazl Eskandari - aeskandari.ir
using System.Collections.ObjectModel;
using System.Windows.Input;
using FaultTolerance.Commands;

namespace FaultTolerance.ViewModels;

/// <summary>
/// UI state of the diagnostic box. It contains NO diagnosis logic, only the pieces you drive from your own code:
///
///   ShowTest(inputs, text) -> "Test #n": tells the user which inputs to apply
///   Finish(text)           -> stops asking for results and shows a final message
///   Hide()                 -> hides the box
///
/// When the user presses "Submit result", the measured output is passed to the callback
/// given in the constructor (see CircuitPageViewModelBase.OnMeasuredOutput).
/// </summary>
public sealed class DiagnosticViewModel : ViewModelBase
{
    private readonly Action<int> _onResultSubmitted;

    private bool _isActive;
    private bool _isAwaitingResult;
    private int _testNumber;
    private int _measuredOutput;
    private string _instruction = string.Empty;
    private string _status = string.Empty;

    public DiagnosticViewModel(IEnumerable<string> inputNames, Action<int> onResultSubmitted)
    {
        _onResultSubmitted = onResultSubmitted;
        RequiredInputs = new ObservableCollection<InputViewModel>(inputNames.Select(name => new InputViewModel(name)));
        SubmitCommand = new RelayCommand(_ => Submit(), _ => IsAwaitingResult);
    }

    /// <summary>The inputs the user must apply to the real circuit (labels).</summary>
    public ObservableCollection<InputViewModel> RequiredInputs { get; }

    /// <summary>Whole box visible or not.</summary>
    public bool IsActive
    {
        get => _isActive;
        private set => SetProperty(ref _isActive, value);
    }

    /// <summary>True while we wait for the user to type the measured output.</summary>
    public bool IsAwaitingResult
    {
        get => _isAwaitingResult;
        private set => SetProperty(ref _isAwaitingResult, value);
    }

    /// <summary>1-based number of the current diagnostic test.</summary>
    public int TestNumber
    {
        get => _testNumber;
        private set
        {
            if (SetProperty(ref _testNumber, value))
            {
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    /// <summary>Header label of the box, e.g. "Test #3".</summary>
    public string Title => $"Test #{TestNumber}";

    public string Instruction
    {
        get => _instruction;
        private set => SetProperty(ref _instruction, value);
    }

    /// <summary>Free text line for your own messages (e.g. "Fault found in the wire from x2").</summary>
    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    /// <summary>Value typed by the user: the output measured on the real circuit.</summary>
    public int MeasuredOutput
    {
        get => _measuredOutput;
        set => SetProperty(ref _measuredOutput, value);
    }

    public ICommand SubmitCommand { get; }

    // ===================== API for your logic =====================

    /// <summary>Shows the next test: "Test #n" and the inputs to apply (one value per input, in order).</summary>
    public void ShowTest(IReadOnlyList<int> inputs, string instruction)
    {
        for (int i = 0; i < RequiredInputs.Count && i < inputs.Count; i++)
        {
            RequiredInputs[i].Value = inputs[i];
        }

        TestNumber++;
        Instruction = instruction;
        Status = string.Empty;
        MeasuredOutput = 0;
        IsAwaitingResult = true;
        IsActive = true;
    }

    /// <summary>Stops waiting for results and shows a final message.</summary>
    public void Finish(string message)
    {
        IsAwaitingResult = false;
        Status = message;
    }

    /// <summary>Hides the box and restarts numbering for the next diagnosis.</summary>
    public void Hide()
    {
        IsAwaitingResult = false;
        IsActive = false;
        TestNumber = 0;
        Status = string.Empty;
    }

    private void Submit()
    {
        IsAwaitingResult = false;               // your logic calls ShowTest again to ask for another test
        _onResultSubmitted(MeasuredOutput);
    }
}
