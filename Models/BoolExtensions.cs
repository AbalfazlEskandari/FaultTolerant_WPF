// Author: Abalfazl Eskandari - aeskandari.ir
namespace FaultTolerance.Models;

public static class BoolExtensions
{
    /// <summary>Converts a logic level to its 0/1 representation.</summary>
    public static int ToBit(this bool value) => value ? 1 : 0;
}
