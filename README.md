# Fault Tolerance - WPF MVVM

Author: Abalfazl Eskandari - aeskandari.ir

Run (Windows, .NET 8 SDK): `dotnet run --project FaultTolerance.csproj`

## How the diagnostic box works (tutorial)

1. User sets inputs + expected output in the right panel and presses the action button.
2. If the result is wrong, `CircuitPageViewModelBase.Evaluate()` calls `StartDiagnosis()`.
3. YOUR logic shows a test with `Diagnostic.ShowTest(inputs, text)`:
   the box displays "Test #n", the inputs the user must apply (labels `x1 = 1`, `x2 = 0` ...)
   and a ComboBox for the measured output.
4. The user presses "Submit result" -> `OnMeasuredOutput(int measured)` is called with that value.
5. In it you decide: ask another test (`ShowTest` again), show a message (`Diagnostic.Status = "..."`),
   or stop (`Diagnostic.Finish("...")`).

Example (already in the code, in `CircuitPageViewModelBase`, delete it when you add your own logic):

    protected override void StartDiagnosis()
        => Diagnostic.ShowTest(new[] { 1, 0, 1 }, "Apply these inputs and enter the output you measure.");

    protected override void OnMeasuredOutput(int measured)
    {
        if (measured == 0) Diagnostic.Finish("Wire from x2 to the top gate is suspicious.");
        else Diagnostic.ShowTest(new[] { 0, 1, 1 }, "Next test");
    }

Override these two methods in `Page2ViewModel` (they are `protected virtual`).

## Moving the box

`Views/DiagnosticBox.xaml` is a stand-alone UserControl (DataContext = `DiagnosticViewModel`).
Currently it sits under the result banner in `Page1View` / `Page2View`.
To replace the right-hand panel instead, swap `<views:SettingsPanel/>` with
`<views:DiagnosticBox DataContext="{Binding Diagnostic}"/>`.

## Gates

Click a gate on page 2: a box with AND / OR / NAND / NOR opens; choose one (click outside to cancel).
