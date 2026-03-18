using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Editing;
using AvaloniaEdit.TextMate;
using DesktopFilesGui.Models.Enums;
using TextMateSharp.Grammars;

namespace DesktopFilesGui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadAvaloniaEdit();
        
    }

    private void LoadAvaloniaEdit()
    {
        var registryOptions = new RegistryOptions(ThemeName.AtomOneDark);
        var textMateInstallation = Editor.InstallTextMate(registryOptions);
        Editor.Background = Background;
        textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId(registryOptions.GetLanguageByExtension(".ini").Id));
    }

    private void CloseApp(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void LoadItems(object? sender, RoutedEventArgs e)
    {
        var combobox = (ComboBox)sender;

        foreach (var value in Enum.GetValues<DesktopFileType>())
        {
            combobox.Items.Add(value);
        }
    }
    
}