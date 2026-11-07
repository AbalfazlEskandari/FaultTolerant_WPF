// Author: Abalfazl Eskandari - aeskandari.ir
using FaultTolerance.Models;

namespace FaultTolerance.Services;

/// <summary>
/// Abstraction of the "server". The application ships with a local mock,
/// but a real remote implementation could be swapped in without touching the view models.
/// </summary>
public interface IBackendService
{
    /// <summary>Computes the output of a single two-input gate.</summary>
    bool EvaluateGate(GateType type, bool a, bool b);

    /// <summary>Evaluates a single gate and compares it with the expected output.</summary>
    EvaluationResult TestSingleGate(GateType type, bool a, bool b, bool expected);

    /// <summary>Evaluates the three-gate circuit and compares it with the expected output.</summary>
    EvaluationResult TestCircuit(CircuitDefinition circuit, bool x1, bool x2, bool x3, bool expected);
}
