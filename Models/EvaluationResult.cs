// Author: Abalfazl Eskandari - aeskandari.ir
namespace FaultTolerance.Models;

/// <summary>Outcome of a single evaluation: what was expected and what the circuit produced.</summary>
public sealed record EvaluationResult(bool Expected, bool Actual)
{
    public bool IsCorrect => Expected == Actual;
}
