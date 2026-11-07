// Author: Abalfazl Eskandari - aeskandari.ir
using System.Windows.Input;
using FaultTolerance.Commands;
using FaultTolerance.Models;

namespace FaultTolerance.ViewModels;

/// <summary>
/// One drawn gate. Clicking it opens a small box (Popup) with the four gate types;
/// choosing one changes <see cref="Type"/> and closes the box.
/// </summary>
public sealed class GateViewModel : ViewModelBase
{
    private GateType _type;
    private bool _isPickerOpen;

    public GateViewModel(GateType type, bool isInteractive = true)
    {
        _type = type;
        IsInteractive = isInteractive;

        OpenPickerCommand = new RelayCommand(_ => IsPickerOpen = true, _ => IsInteractive);
        SelectTypeCommand = new RelayCommand(parameter =>
        {
            if (parameter is GateType selected)
            {
                Type = selected;
            }

            IsPickerOpen = false;
        });
    }

    public GateType Type
    {
        get => _type;
        set
        {
            if (SetProperty(ref _type, value))
            {
                // Everything the drawing needs is derived from Type.
                OnPropertyChanged(nameof(Label));
                OnPropertyChanged(nameof(IsOrShape));
                OnPropertyChanged(nameof(IsInverted));
            }
        }
    }

    /// <summary>Text drawn inside the gate.</summary>
    public string Label => _type.ToString();

    /// <summary>OR / NOR use the curved-back body, AND / NAND the flat-back body.</summary>
    public bool IsOrShape => _type is GateType.OR or GateType.NOR;

    /// <summary>NAND / NOR draw the small inversion bubble.</summary>
    public bool IsInverted => _type is GateType.NAND or GateType.NOR;

    /// <summary>False for the fixed OR gate on page 1.</summary>
    public bool IsInteractive { get; }

    /// <summary>Bound two-way to the Popup, so clicking outside closes it.</summary>
    public bool IsPickerOpen
    {
        get => _isPickerOpen;
        set => SetProperty(ref _isPickerOpen, value);
    }

    public ICommand OpenPickerCommand { get; }

    /// <summary>CommandParameter = the GateType that was chosen.</summary>
    public ICommand SelectTypeCommand { get; }
}
