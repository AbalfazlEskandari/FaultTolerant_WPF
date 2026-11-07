// Author: Abalfazl Eskandari - aeskandari.ir
using System.Windows;
using FaultTolerance.Services;
using FaultTolerance.ViewModels;

namespace FaultTolerance;

/// <summary>Composition root: wires the mock backend into the view model tree.</summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IBackendService backend = new MockBackendService();
        Window shell = new MainWindow { DataContext = new MainViewModel(backend) };
        MainWindow = shell;
        shell.Show();
    }
}
