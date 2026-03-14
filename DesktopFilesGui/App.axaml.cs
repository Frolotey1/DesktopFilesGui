using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Autofac;
using Avalonia.Markup.Xaml;
using DesktopFilesGui.Services.ShellThemeLoader.Strategies;
using DesktopFilesGui.ViewModels;
using DesktopFilesGui.Views;
using Serilog;

namespace DesktopFilesGui;

public partial class App : Application
{
    private const string SERILOG_OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level}] [Thread: {Thread}] {Message:lj}{NewLine}{Exception}";
    private static readonly string APPLICATION_DATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopFilesGui");
    
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
        var builder = new ContainerBuilder();
        
        builder.RegisterTypes(typeof(GNOMEThemeLoaderStrategy))
            .AsImplementedInterfaces();
    }
    
    private void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: SERILOG_OUTPUT_TEMPLATE)
            .WriteTo.File(
                outputTemplate: SERILOG_OUTPUT_TEMPLATE, 
                rollingInterval: RollingInterval.Day, 
                path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"))
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