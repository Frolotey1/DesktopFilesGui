using System;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class RootRequirer(ILogger logger) : IRootRequirer
{
    private const int SUCCESS_EXIT_CODE = 0;
    
    //This code requires polkitd and pkexec
    public async Task RequireRootAsync(CancellationTokenSource token)
    {
        var policyKitProcess = StartPolicyKit();
        await policyKitProcess.WaitForExitAsync();
        logger.Information($"Policykit exited with {policyKitProcess.ExitCode}");
        CancelIfUnsuccess(token, policyKitProcess.ExitCode);
    }
    

    private static void CancelIfUnsuccess(CancellationTokenSource token, int exitCode)
    {
        if (exitCode == SUCCESS_EXIT_CODE)
            return;
        
        token.Cancel();
        token.Token.ThrowIfCancellationRequested();
    }

    private Process StartPolicyKit()
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

        return policyKitProcess ?? throw new InvalidOperationException($"Could not found pkexec to start");
    }
}