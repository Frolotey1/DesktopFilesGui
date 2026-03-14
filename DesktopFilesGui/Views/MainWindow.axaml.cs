using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Editing;
using AvaloniaEdit.TextMate;
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
        var registryOptions = new RegistryOptions(ThemeName.SolarizedDark);
        var textMateInstallation = Editor.InstallTextMate(registryOptions);
        Editor.Background = Background;
        textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId(registryOptions.GetLanguageByExtension(".ini").Id));
        Editor.Text = """
[Desktop Entry]
#Powered by DesktopFilesGui
Name=Test
Icon=/Path/To/Tests
Terminal=False
""";
    }

    private void CloseApp(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}