// Author: Abalfazl Eskandari - aeskandari.ir
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FaultTolerance.Converters;

/// <summary>
/// true -> Visible, false -> Collapsed.
/// Pass ConverterParameter="Invert" to reverse the mapping.
/// </summary>
public sealed class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool flag = value is bool b && b;
        if (parameter is string s && s.Equals("Invert", StringComparison.OrdinalIgnoreCase))
        {
            flag = !flag;
        }

        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
