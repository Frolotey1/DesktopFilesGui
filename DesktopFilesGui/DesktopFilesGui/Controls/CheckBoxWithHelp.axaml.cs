using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace DesktopFilesGui.Controls;

public sealed partial class CheckBoxWithHelp : UserControl
{
    public new static readonly StyledProperty<string> ContentProperty =
        AvaloniaProperty.Register<CheckBoxWithHelp, string>(nameof(Content));
    
    public static readonly StyledProperty<string> ToolTipProperty =
        AvaloniaProperty.Register<CheckBoxWithHelp, string>(nameof(ToolTip));
    
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<CheckBoxWithHelp, bool>(nameof(IsChecked), defaultBindingMode: BindingMode.TwoWay);
    
    public new string Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string ToolTip
    {
        get => GetValue(ToolTipProperty);
        set => SetValue(ToolTipProperty, value);
    }

    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public CheckBoxWithHelp()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}