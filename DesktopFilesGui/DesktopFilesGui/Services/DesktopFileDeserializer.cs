using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class DesktopFileDeserializer(ILogger logger) : IDesktopFileDeserializer {
    private static readonly Regex LocalizedKeyRegex = new(@"^([A-Za-z]+)\[([a-zA-Z\-_@]+)\]$", RegexOptions.Compiled);
    
    public async Task<DesktopFile> DeserializeAsync(IEnumerable<string> lines) {
        return await Task.Run(() => Deserialize(lines));
    }
    
    public DesktopFile Deserialize(IEnumerable<string> lines) {
        logger.Debug("Starting deserialization of desktop file");
        
        var desktopFile = new DesktopFile {
            SupportedMimeTypes = new List<string>(),
            LocalizedName = new Dictionary<string, string>(),
            LocalizedGenericName = new Dictionary<string, string>(),
            LocalizedComment = new Dictionary<string, string>()
        };
        
        bool inDesktopEntry = false;
        
        foreach (var line in lines) {
            var trimmedLine = line.Trim();
            
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
                continue;
            
            if (trimmedLine.StartsWith('[') && trimmedLine.EndsWith(']'))
            {
                inDesktopEntry = trimmedLine == "[Desktop Entry]";
                continue;
            }
            
            if (!inDesktopEntry)
                continue;
            
            var equalIndex = trimmedLine.IndexOf('=');
            if (equalIndex <= 0)
                continue;
            
            var key = trimmedLine[..equalIndex];
            var value = equalIndex + 1 < trimmedLine.Length 
                ? trimmedLine[(equalIndex + 1)..] 
                : string.Empty;
            
            SetProperty(desktopFile, key, value);
        }
        
        logger.Information("Desktop file deserialization completed. Type: {Type}", desktopFile.Type);
        return desktopFile;
    }
    
    private void SetProperty(DesktopFile desktopFile, string key, string value) {
        var localizedMatch = LocalizedKeyRegex.Match(key);
        if (localizedMatch.Success)  {
            SetLocalizedProperty(desktopFile, localizedMatch.Groups[1].Value, localizedMatch.Groups[2].Value, value);
            return;
        }
        
        switch (key)  {
            case "Type":
                if (Enum.TryParse<DesktopFileType>(value, true, out var type))
                    desktopFile.Type = type;
                else
                    logger.Warning("Unknown Desktop File Type: {Type}", value);
                break;
                
            case "Name":
                desktopFile.Name = value;
                break;
                
            case "GenericName":
                desktopFile.GenericName = value;
                break;
                
            case "Comment":
                desktopFile.Comment = value;
                break;
                
            case "Exec":
                // Store custom exec command if needed
                desktopFile.CustomExecCommand = value;
                break;
                
            case "TryExec":
                desktopFile.CustomTryExecCommand = value;
                break;
                
            case "Icon":
                desktopFile.PathToIcon = value;
                break;
                
            case "Path":
                desktopFile.Path = value;
                break;
                
            case "Terminal":
                desktopFile.ShowTerminal = ParseBoolean(value);
                break;
                
            case "Hidden":
                desktopFile.IsHidden = ParseBoolean(value);
                break;
                
            case "DBusActivatable":
                desktopFile.RunFromDBus = ParseBoolean(value);
                break;
                
            case "StartupNotify":
                desktopFile.StartupNotifySupport = ParseBoolean(value);
                break;
                
            case "MimeType":
                desktopFile.SupportedMimeTypes = ParseMimeTypes(value);
                break;
                
            default:
                logger.Verbose("Unhandled desktop file key: {Key} = {Value}", key, value);
                break;
        }
    }
    
    private void SetLocalizedProperty(DesktopFile desktopFile, string baseKey, string locale, string value) {
        switch (baseKey) {
            case "Name":
                desktopFile.LocalizedName ??= new Dictionary<string, string>();
                desktopFile.LocalizedName[locale] = value;
                desktopFile.EnableLocalization = true;
                break;
                
            case "GenericName":
                desktopFile.LocalizedGenericName ??= new Dictionary<string, string>();
                desktopFile.LocalizedGenericName[locale] = value;
                desktopFile.EnableLocalization = true;
                break;
                
            case "Comment":
                desktopFile.LocalizedComment ??= new Dictionary<string, string>();
                desktopFile.LocalizedComment[locale] = value;
                desktopFile.EnableLocalization = true;
                break;
        }
    }
    
    private static bool ParseBoolean(string value) {
        return value.Equals("true", StringComparison.OrdinalIgnoreCase) || value == "1";
    }
    
    private static IEnumerable<string> ParseMimeTypes(string value) {
        if (string.IsNullOrWhiteSpace(value))
            return Enumerable.Empty<string>();
        
        return value
            .TrimEnd(';')
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(m => m.Trim())
            .Where(m => !string.IsNullOrWhiteSpace(m));
    }
}
