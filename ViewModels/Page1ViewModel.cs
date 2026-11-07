// Author: Abalfazl Eskandari - aeskandari.ir
using FaultTolerance.Models;
using FaultTolerance.Services;

namespace FaultTolerance.ViewModels;

/// <summary>Page 1: a single OR gate. The gate type is fixed.</summary>
public sealed class Page1ViewModel : CircuitPageViewModelBase
{
    public Page1ViewModel(IBackendService backend)
        : base(
            backend,
            title: "Single OR Gate Test",
            subtitle: "Choose the inputs and the expected output, then send the test.",
            inputNames: new[] { "A", "B" },
            actionLabel: "Send to Server")
    {
        Gate = new GateViewModel(GateType.OR, isInteractive: false);
    }

    public GateViewModel Gate { get; }

    protected override EvaluationResult Run(IReadOnlyList<bool> inputs, bool expected)
        => Backend.TestSingleGate(GateType.OR, inputs[0], inputs[1], expected);
}
