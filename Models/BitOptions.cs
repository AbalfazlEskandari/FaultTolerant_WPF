// Author: Abalfazl Eskandari - aeskandari.ir
namespace FaultTolerance.Models;

/// <summary>Selectable logic levels, shared by every ComboBox that edits a bit.</summary>
public static class BitOptions
{
    public static IReadOnlyList<int> Values { get; } = new[] { 0, 1 };
}
