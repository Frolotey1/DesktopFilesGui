using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace DesktopFilesGui.Controls;

public sealed partial class ReadOnlyTextEditor : UserControl
{
    public static readonly StyledProperty<bool> ShowLineNumbersProperty = AvaloniaProperty
        .Register<ReadOnlyTextEditor, bool>(nameof(ShowLineNumbers));

    public bool ShowLineNumbers
    {
        get => GetValue(ShowLineNumbersProperty);
        set => SetValue(ShowLineNumbersProperty, value);
    }
    
    public static readonly StyledProperty<string> CodeProperty = AvaloniaProperty
        .Register<ReadOnlyTextEditor, string>(nameof(Code));

    public string Code
    {
        get => GetValue(CodeProperty);
        set
        {
            AvaloniaEditCore.Text = value;
            SetValue(CodeProperty, value);
        }
    }
    
    public ReadOnlyTextEditor()
    {
        InitializeComponent();
        AvaloniaEditCore.Options.EnableRectangularSelection = true;
        CodeProperty.Changed.AddClassHandler<ReadOnlyTextEditor>((o, e)
            => AvaloniaEditCore.Text = e.NewValue?.ToString() ?? string.Empty);
    }

    public TextMate.Installation InstallTextMate(RegistryOptions registryOptions)
    {
        return AvaloniaEditCore.InstallTextMate(registryOptions);
    }
}