using DesktopFilesGui.Models;

namespace DesktopFilesGui.Services.ShellThemeLoader.Strategies;

public interface IBaseThemeLoaderStrategy
{
    public ShellTheme LoadTheme();
}