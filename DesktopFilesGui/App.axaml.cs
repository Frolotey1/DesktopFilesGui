using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using DesktopFilesGui.ViewModels;
using DesktopFilesGui.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DesktopFilesGui;

public partial class App : Application
{
   
    public override void Initialize()
    {
        ConfigureSerilog();
        ConfigureServices();
        AvaloniaXamlLoader.Load(this);
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindow>();
    }
    
    private void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: Configuration.SERILOG_OUTPUT_TEMPLATE)
            .WriteTo.File(
                outputTemplate:  Configuration.SERILOG_OUTPUT_TEMPLATE, 
                rollingInterval: RollingInterval.Day, 
                path: Path.Combine(Configuration.APPLICATION_DATA, "logs"))
            .Enrich.WithThreadId()
            .CreateLogger();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}