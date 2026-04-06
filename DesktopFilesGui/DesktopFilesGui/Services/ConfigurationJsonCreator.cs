using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopFilesGui.JsonSerializerOptions;
using DesktopFilesGui.Models;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class ConfigurationJsonCreator(ILogger logger) : IConfigurationJsonCreator
{
    public async ValueTask EnsureCreatedAsync()
    {
        if (File.Exists(StaticConfiguration.CONFIGURATION_JSON_PATH))
        {
            logger.Information("{path} is already exists and won't be created", StaticConfiguration.CONFIGURATION_JSON_PATH);
            return;
        }
           
        
        await using var stream = File.Create(StaticConfiguration.CONFIGURATION_JSON_PATH);
        await JsonSerializer.SerializeAsync(
            stream, 
            StaticConfiguration.DEFAULT_CONFIGURATION,
            ConfigurationSerializerContext.Default.Options);
        logger.Information("{path} was created", StaticConfiguration.CONFIGURATION_JSON_PATH);
    }
}