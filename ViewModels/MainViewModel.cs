// Author: Abalfazl Eskandari - aeskandari.ir
using System.Windows.Input;
using FaultTolerance.Commands;
using FaultTolerance.Services;

namespace FaultTolerance.ViewModels;

/// <summary>Root view model: owns both pages and the navigation state.</summary>
public sealed class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentPage;

    public MainViewModel(IBackendService backend)
    {
        Page1 = new Page1ViewModel(backend);
        Page2 = new Page2ViewModel(backend);
        _currentPage = Page1;

        ShowPage1Command = new RelayCommand(_ => CurrentPage = Page1);
        ShowPage2Command = new RelayCommand(_ => CurrentPage = Page2);
    }

    public Page1ViewModel Page1 { get; }

    public Page2ViewModel Page2 { get; }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        private set
        {
            if (SetProperty(ref _currentPage, value))
            {
                OnPropertyChanged(nameof(IsPage1Selected));
                OnPropertyChanged(nameof(IsPage2Selected));
            }
        }
    }

    public bool IsPage1Selected => ReferenceEquals(_currentPage, Page1);

    public bool IsPage2Selected => ReferenceEquals(_currentPage, Page2);

    public ICommand ShowPage1Command { get; }

    public ICommand ShowPage2Command { get; }
}
