using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;

namespace DesktopFilesGui.Converters;

public class DesktopFileTypeToBooleanConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DesktopFileType desktopFileType)
            return false;
        
        if(parameter is not DesktopFileType expectedDesktopFileType)
            return false;
            
        return desktopFileType == expectedDesktopFileType;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}