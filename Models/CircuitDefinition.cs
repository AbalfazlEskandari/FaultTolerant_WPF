// Author: Abalfazl Eskandari - aeskandari.ir
namespace FaultTolerance.Models;

/// <summary>
/// The fixed three-gate topology of page 2:
/// TopLeft(x1, x2) and BottomLeft(x2, x3) feed the Output gate.
/// </summary>
public sealed record CircuitDefinition(GateType TopLeft, GateType BottomLeft, GateType Output);
