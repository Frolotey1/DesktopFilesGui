using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public class RootRequirer(ILogger logger) : IRootRequirer
{ 
    //This code requires polkitd and pkexec
    public async Task RequireRootAsync()
    {
        //DO NOT USE Assembly.GetCurrentAssebmly().Location IT DO NOT WORK IN NATIVE AOT
        var executionPath = Environment.ProcessPath ?? AppDomain.CurrentDomain.BaseDirectory;
        var processStartInfo = new ProcessStartInfo()
        {
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Normal,
            FileName = "pkexec",
            Arguments = executionPath
        };
        
        logger.Information($"Starting policykit for {processStartInfo.Arguments}");
        
        var policyKitProcess = Process.Start(processStartInfo);

        if (policyKitProcess is null)
            throw new InvalidOperationException($"Could not found pkexec to start");

        await policyKitProcess.WaitForExitAsync();
        logger.Information($"Policykit exited with {policyKitProcess.ExitCode}");
    }
}