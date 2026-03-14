using System;
using System.Runtime.InteropServices;
using DesktopFilesGui.Attributes;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;
using Serilog;

namespace DesktopFilesGui.Services.ShellThemeLoader.Strategies;

using System;
using System.Runtime.InteropServices;

[ShellType(Shell.GNOME)]
public class GNOMEThemeLoaderStrategy(ILogger logger) : IBaseThemeLoaderStrategy
{
    const string GTK = "libgtk-3.so.0";

    [DllImport(GTK)] private static extern void gtk_init(IntPtr argc, IntPtr argv);
    [DllImport(GTK)] private static extern IntPtr gtk_window_new(int type);
    [DllImport(GTK)] private static extern IntPtr gtk_widget_get_style_context(IntPtr widget);
    [DllImport(GTK)] private static extern void gtk_style_context_get_color(IntPtr context, int state, out GdkRGBA color);
    [DllImport(GTK)] private static extern void gtk_style_context_get_background_color(IntPtr context, int state, out GdkRGBA color);
    [DllImport(GTK)] private static extern void gtk_style_context_add_class(IntPtr context, string class_name);
    [DllImport(GTK)] private static extern void gtk_style_context_remove_class(IntPtr context, string class_name);
    [DllImport(GTK)] private static extern void gtk_style_context_save(IntPtr context);
    [DllImport(GTK)] private static extern void gtk_style_context_restore(IntPtr context);

    private const int GTK_STATE_FLAG_NORMAL = 0;
    private const int GTK_WINDOW_TOPLEVEL = 0;

    [StructLayout(LayoutKind.Sequential)]
    private struct GdkRGBA
    {
        public double red;
        public double green;
        public double blue;
        public double alpha;
    }

    public ShellTheme LoadTheme()
    {
        try
        {
            gtk_init(IntPtr.Zero, IntPtr.Zero);
    
            var window = gtk_window_new(GTK_WINDOW_TOPLEVEL);
            var context = gtk_widget_get_style_context(window);
    
            gtk_style_context_save(context);
    
            gtk_style_context_add_class(context, "selected");
            gtk_style_context_get_background_color(context, GTK_STATE_FLAG_NORMAL, out var primaryColor);
            gtk_style_context_remove_class(context, "selected");
    
            gtk_style_context_add_class(context, "link");
            gtk_style_context_get_color(context, GTK_STATE_FLAG_NORMAL, out var secondaryColor);
            gtk_style_context_remove_class(context, "link");
    
            gtk_style_context_restore(context);
    
            return new ShellTheme
            {
                Primary = GdkRgbaToShellColor(primaryColor),
                Secondary = GdkRgbaToShellColor(secondaryColor),
            };
        }
        catch (Exception ex)
        {
            logger.Error("Failed to load GNOME theme: {error}", ex.Message);
            
            return ShellTheme.Unknown;
        }
    }
    
    private ShellColor GdkRgbaToShellColor(GdkRGBA gdkColor)
    {
        return new ShellColor
        {
            Red = (float)Math.Clamp(gdkColor.red, 0, 1),
            Green = (float)Math.Clamp(gdkColor.green, 0, 1),
            Blue = (float)Math.Clamp(gdkColor.blue, 0, 1),
            Alpha = (float)Math.Clamp(gdkColor.alpha, 0, 1)
        };
    }
}