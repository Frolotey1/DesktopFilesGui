using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopFilesGui.JsonSerializerOptions;
using DesktopFilesGui.Models;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class ExecutionTriggersJsonCreator(ILogger logger) : IExecutionTriggersJsonCreator
{
    public async ValueTask EnsureCreatedAsync()
    {
        if (File.Exists(Configuration.EXECUTION_TEMPLATES_JSON_PATH))
        {
            logger.Information("{path} is already exists and won't be created", Configuration.EXECUTION_TEMPLATES_JSON_PATH);
            return;
        }
           
        
        await using var stream = File.Create(Configuration.EXECUTION_TEMPLATES_JSON_PATH);
        await JsonSerializer.SerializeAsync(
            stream, 
            Configuration.DEFAULT_TEMPLATES_INFO,
            TemplatesInfoSerializerOptions.Default.Options);
        logger.Information("{path} was created", Configuration.EXECUTION_TEMPLATES_JSON_PATH);
    }
}