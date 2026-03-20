using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using DesktopFilesGui.Services;
using DesktopFilesGui.Services.Interfaces;
using DesktopFilesGui.ViewModels;
using DesktopFilesGui.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DesktopFilesGui;

public partial class App : Application
{
    private ServiceProvider _provider;
    
    public override void Initialize()
    {
        ConfigureSerilog();
        ConfigureServices();
        AvaloniaXamlLoader.Load(this);
    }
    
    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var window = _provider.GetRequiredService<MainWindow>();
            window.DataContext = _provider.GetRequiredService<MainWindowViewModel>();
    
            desktop.MainWindow = window;
        }

        await _provider.GetRequiredService<IExecutionTriggersJsonCreator>()
            .EnsureCreatedAsync();
        
        base.OnFrameworkInitializationCompleted();
    }
    
    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services
            .AddSingleton<IDesktopFileGenerator, DesktopFileGenerator>()
            .AddSingleton<IGithubSourceOpener, GithubSourceOpener>()
            .AddSingleton(Log.Logger)
            .AddTransient<IExecutionTriggersJsonCreator, ExecutionTriggersJsonCreator>()
            .AddSingleton<MainWindowViewModel>()
            .AddSingleton<MainWindow>();
        
        _provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(_provider);
    }
    
    private void ConfigureSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
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