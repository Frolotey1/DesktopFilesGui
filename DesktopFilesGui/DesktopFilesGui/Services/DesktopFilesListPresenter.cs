using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DesktopFilesGui.Models;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class DesktopFilesListPresenter(ILogger logger, IRootRequirer rootRequirer) : IDesktopFilesListPresenter
{
    private const string ICON_START_PATTERN = "Icon=";
    private const string NAME_START_PATTERN = "Name=";
    
    public async Task<IReadOnlyCollection<DesktopFileHeader>> GetDesktopFilesAsync(string path, CancellationTokenSource cancellationTokenSource)
    {
        //You don't need return IEnumerable because it may skip exception
        try
        {
            GetSearchPrefix(ref path, out var pattern);
            var result = await Directory
                .EnumerateFiles(path,  pattern, searchOption: SearchOption.AllDirectories)
                .Where(file => Path.GetExtension(file) == StaticConfiguration.DESKTOP_FILE_EXTENSION)
                .ToAsyncEnumerable()
                .Select(async (file, token) =>
                {

                    var icon = (string?)null;
                    var name = (string?)null;

                    await foreach (var untrimmedLine in File.ReadLinesAsync(file, token))
                    {
                        var line = untrimmedLine.TrimStart();

                        if (line.StartsWith(ICON_START_PATTERN))
                            icon = CutLineWithTrimming(line, ICON_START_PATTERN.Length);

                        if (line.StartsWith(NAME_START_PATTERN))
                            name = CutLineWithTrimming(line, NAME_START_PATTERN.Length);

                        if (icon is not null && name is not null)
                            break;

                    }

                    if (icon is null)
                        logger.Warning($"Could not find icon for {Path.GetFileName(file)}");

                    if (name is null)
                        logger.Error($"Could not find name for {Path.GetFileName(file)}");

                    return new DesktopFileHeader()
                        { Path = file, IconPath = icon ?? string.Empty, Name = name ?? string.Empty };

                }).ToListAsync(cancellationTokenSource.Token);
            
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            return result.AsReadOnly();
        }
        catch (UnauthorizedAccessException)
        {
            try
            {
                await rootRequirer.RequireRootAsync(cancellationTokenSource);
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
            catch(OperationCanceledException)
            {
                logger.Warning($"Access to the desktop files could not be accessed, user cancel the operation");
                return [];
            }
            return await GetDesktopFilesAsync(path, cancellationTokenSource);
        }
    }

    private void GetSearchPrefix(ref string path, out string pattern)
    {
        if (Directory.Exists(path))
        {
            pattern = "*";
            return;
        }

        var actualDirectory = path;
        while (!Directory.Exists(actualDirectory))
        {
            actualDirectory = Path.GetDirectoryName(actualDirectory);
        }

        path = actualDirectory;
        pattern = path[..actualDirectory.Length] + "*";
        
    }

    private static string CutLineWithTrimming(string line, int length)
    {
       line = line[length..].Trim();
       return line.EndsWith(Environment.NewLine) ? line[..Environment.NewLine.Length] : line;
    }
}