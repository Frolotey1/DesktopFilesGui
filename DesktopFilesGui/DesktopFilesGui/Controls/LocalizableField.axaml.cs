using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using DesktopFilesGui.Extensions;
using DesktopFilesGui.Models;
using Material.Icons;
using Material.Styles.Themes;

namespace DesktopFilesGui.Controls;

public sealed partial class LocalizableField : UserControl
{
    private MaterialThemeBase? _theme;
    private IReadOnlyCollection<string> _countyKeyes = [];
    private object _lock = new object();

    public static readonly StyledProperty<MaterialIconKind> IconProperty =
        AvaloniaProperty.Register<LocalizableField, MaterialIconKind>(nameof(Icon));

    public static readonly StyledProperty<string> WatermarkProperty =
        AvaloniaProperty.Register<LocalizableField, string>(nameof(Watermark));

    public static readonly StyledProperty<bool> EnableLocalizationProperty =
        AvaloniaProperty.Register<LocalizableField, bool>(nameof(EnableLocalization));

    public static readonly StyledProperty<IDictionary<string, string>?> LocalizationDictionaryProperty = AvaloniaProperty.Register<LocalizableField, IDictionary<string, string>?>(
        nameof(LocalizationDictionary));

    public static readonly StyledProperty<string> UnlocalizedValueProperty = AvaloniaProperty.Register<LocalizableField, string>(
        nameof(UnlocalizedValue));

    public string UnlocalizedValue
    {
        get => GetValue(UnlocalizedValueProperty);
        set => SetValue(UnlocalizedValueProperty, value);
    }

    public IDictionary<string, string>? LocalizationDictionary
    {
        get => GetValue(LocalizationDictionaryProperty);
        set => SetValue(LocalizationDictionaryProperty, value);
    }

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
        _theme = Application.Current?.LocateMaterialTheme<MaterialThemeBase>();
    }


    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var item = ((Control)sender!).GetLogicalParent<ComboBoxItem>();
        var country = (CountryInfo?)item?.DataContext;
        
        if (_theme is null)
            return;
        
        if (LocalizationDictionary?.ContainsRangeKey(country?.Keys ?? []) ?? false)
            item.Background = new SolidColorBrush(_theme.CurrentTheme.PrimaryMid.Color);
    }

    private void UpdateTextBoxBinding(object? sender, SelectionChangedEventArgs e)
    {
        //Here is no real binding
        lock (_lock)
        {
            var comboBox = (ComboBox)sender!;
            var selectedItem = comboBox.SelectedItem as CountryInfo;

            if (selectedItem is null)
                return;
        
            if (LocalizationDictionary is null)
                return;

            if (LocalizedTextBox?.Text is null)
                return;
            
            if (_countyKeyes.Any())
                foreach (var countyKey in _countyKeyes)
                     LocalizationDictionary[countyKey] = LocalizedTextBox.Text;

            _countyKeyes = selectedItem.Keys.AsReadOnly();
        }
    
    }
}