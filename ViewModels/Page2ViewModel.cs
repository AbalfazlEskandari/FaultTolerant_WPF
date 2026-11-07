// Author: Abalfazl Eskandari - aeskandari.ir
using System.ComponentModel;
using FaultTolerance.Models;
using FaultTolerance.Services;

namespace FaultTolerance.ViewModels;

/// <summary>
/// Page 2: AND(x1, x2) and AND(x2, x3) feeding an OR gate by default.
/// Click a gate in the drawing to pick another type.
/// </summary>
public sealed class Page2ViewModel : CircuitPageViewModelBase
{
    public Page2ViewModel(IBackendService backend)
        : base(
            backend,
            title: "Complex Logic Circuit Diagnosis",
            subtitle: "Click a gate in the drawing to change its type, then evaluate the circuit.",
            inputNames: new[] { "x1", "x2", "x3" },
            actionLabel: "Evaluate Circuit")
    {
        TopGate = new GateViewModel(GateType.AND);
        BottomGate = new GateViewModel(GateType.AND);
        OutputGate = new GateViewModel(GateType.OR);

        foreach (GateViewModel gate in new[] { TopGate, BottomGate, OutputGate })
        {
            gate.PropertyChanged += OnGatePropertyChanged;
        }
    }

    /// <summary>Top-left gate, takes x1 and x2.</summary>
    public GateViewModel TopGate { get; }

    /// <summary>Bottom-left gate, takes x2 and x3.</summary>
    public GateViewModel BottomGate { get; }

    /// <summary>Right-hand gate, takes the outputs of the two left gates.</summary>
    public GateViewModel OutputGate { get; }

    protected override EvaluationResult Run(IReadOnlyList<bool> inputs, bool expected)
    {
        var circuit = new CircuitDefinition(TopGate.Type, BottomGate.Type, OutputGate.Type);
        return Backend.TestCircuit(circuit, inputs[0], inputs[1], inputs[2], expected);
    }

    /// <summary>A changed gate makes the old result and any running diagnosis obsolete.</summary>
    private void OnGatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GateViewModel.Type))
        {
            ResetResult();
            Diagnostic.Hide();
        }
    }
}
