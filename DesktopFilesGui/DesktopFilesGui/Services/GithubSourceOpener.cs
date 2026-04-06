using System.Diagnostics;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class GithubSourceOpener(ILogger logger) : IGithubSourceOpener
{
    public void Open()
    {
        logger.Information("Opening Github project's source {source}", StaticConfiguration.GITHUB_LINK);
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = StaticConfiguration.GITHUB_LINK
        };
        
        Process.Start(startInfo);
    }
}