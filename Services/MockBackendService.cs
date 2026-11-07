// Author: Abalfazl Eskandari - aeskandari.ir
using FaultTolerance.Models;

namespace FaultTolerance.Services;

/// <summary>Purely local, offline implementation of the backend logic.</summary>
public sealed class MockBackendService : IBackendService
{
    public bool EvaluateGate(GateType type, bool a, bool b) => type switch
    {
        GateType.AND => a && b,
        GateType.OR => a || b,
        GateType.NAND => !(a && b),
        GateType.NOR => !(a || b),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown gate type.")
    };

    public EvaluationResult TestSingleGate(GateType type, bool a, bool b, bool expected)
    {
        bool actual = EvaluateGate(type, a, b);
        return new EvaluationResult(expected, actual);
    }

    public EvaluationResult TestCircuit(CircuitDefinition circuit, bool x1, bool x2, bool x3, bool expected)
    {
        bool topLeft = EvaluateGate(circuit.TopLeft, x1, x2);
        bool bottomLeft = EvaluateGate(circuit.BottomLeft, x2, x3);
        bool actual = EvaluateGate(circuit.Output, topLeft, bottomLeft);
        return new EvaluationResult(expected, actual);
    }
}
