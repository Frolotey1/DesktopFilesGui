using DesktopFilesGui.Models.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopFilesGui.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWindow<TWindowBase, TActualWindow, TViewModel>(this IServiceCollection services, ViewType viewType) 
        where TActualWindow : class, TWindowBase 
        where TWindowBase : class
        where TViewModel : class
    {
        services.AddKeyedScoped<TWindowBase, TActualWindow>(viewType);
        services.AddKeyedScoped<object, TViewModel>(viewType);
        
        return services;
    }
}