// Author: Abalfazl Eskandari - aeskandari.ir
namespace FaultTolerance.ViewModels;

/// <summary>A named binary circuit input (A, B, x1, x2, x3 ...).</summary>
public sealed class InputViewModel : ViewModelBase
{
    private int _value;

    public InputViewModel(string name, int value = 0)
    {
        Name = name;
        _value = value;
    }

    public string Name { get; }

    /// <summary>Logic level, 0 or 1.</summary>
    public int Value
    {
        get => _value;
        set
        {
            if (SetProperty(ref _value, value))
            {
                OnPropertyChanged(nameof(Bit));
            }
        }
    }

    public bool Bit => _value != 0;
}
