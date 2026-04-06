using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace DesktopFilesGui.Converters;

public sealed class ReverseBooleanConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolean
            ? !boolean
            : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolean
            ? !boolean
            : null;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}