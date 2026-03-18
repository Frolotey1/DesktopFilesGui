using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Platform.Storage;
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

    private async void SelectFile(object? sender, RoutedEventArgs e)
    {
        var textBox = GetTextBoxFromSearchButton(sender);
        var topLevel = GetTopLevel(this);

        if(topLevel is null)
            throw new InvalidOperationException("Could not find TopLevel");
        
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new(){AllowMultiple = false});
        if (files.Any())
            textBox.Text = files.First().TryGetLocalPath();
    }   
    
    private async void SelectDirectory(object? sender, RoutedEventArgs e)
    {
        var textBox = GetTextBoxFromSearchButton(sender);
        var topLevel = GetTopLevel(this);

        if(topLevel is null)
            throw new InvalidOperationException("Could not find TopLevel");
        
        var directories = await topLevel.StorageProvider.OpenFolderPickerAsync(new(){AllowMultiple = false});
        if (directories.Any())
            textBox.Text = directories.First().TryGetLocalPath()!;
    }   

    private TextBox GetTextBoxFromSearchButton(object? sender)
    {
        var button = (Button)sender;
        var parent = button.GetLogicalParent();
        var textBox = parent?.GetLogicalChildren().First(child => child is TextBox) as TextBox;
        
        if(textBox is null)
            throw new InvalidOperationException("Could not find TextBox");
        
        return textBox;
    }
}