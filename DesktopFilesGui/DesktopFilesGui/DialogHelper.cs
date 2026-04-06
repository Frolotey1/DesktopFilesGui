using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using DesktopFilesGui.Models.Enums;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DesktopFilesGui;

public static class DialogHelper
{
    public static void ShowDialog(Window owner, ViewType viewType)
    {
        var logger = Ioc.Default.GetRequiredService<ILogger>();
        
        using var scope = Ioc.Default
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
        
        var view = scope.ServiceProvider.GetRequiredKeyedService<Window>(viewType);
        var viewModel = scope.ServiceProvider.GetRequiredKeyedService<object>(viewType);
        view.DataContext = viewModel;

        logger.Information($"Showing dialog for {viewType}");
        view.ShowDialog(owner);
    }
}