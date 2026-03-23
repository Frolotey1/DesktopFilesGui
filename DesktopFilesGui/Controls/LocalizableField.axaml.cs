using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Material.Icons;

namespace DesktopFilesGui.Controls;

public partial class LocalizableField : UserControl
{
    public static readonly StyledProperty<MaterialIconKind> IconProperty =
        AvaloniaProperty.Register<LocalizableField, MaterialIconKind>(nameof(Icon));

    public static readonly StyledProperty<string> WatermarkProperty =
        AvaloniaProperty.Register<LocalizableField, string>(nameof(Watermark));

    public static readonly StyledProperty<bool> EnableLocalizationProperty =
        AvaloniaProperty.Register<LocalizableField, bool>(nameof(EnableLocalization));

    public MaterialIconKind Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public bool EnableLocalization
    {
        get => GetValue(EnableLocalizationProperty);
        set => SetValue(EnableLocalizationProperty, value);
    }

    public LocalizableField()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CountryComboBox_OnDropDownOpened(object? sender, EventArgs e)
    {
        
    }
}