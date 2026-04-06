using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalonia.Input;
using DesktopFilesGui.Attributes;
using DesktopFilesGui.Extensions;
using DesktopFilesGui.Models;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class DesktopFileSerializer(ILogger logger) : IDesktopFileSerializer
{
    public string Serialize(DesktopFile desktopFile)
    {
        var fileContentBuilder = new StringBuilder();
        
        logger.Debug($"Generating desktop file from: {desktopFile}...");

        fileContentBuilder.AppendLine(StaticConfiguration.DESKTOP_FILE_STARTING);
        
        var properties = ((IEnumerable<KeyValuePair<string, string?>>)GetDesktopProperties(desktopFile))
            .Concat(GetLocalizedProperties(desktopFile));

        foreach (var property in properties)
        {
            if(IsNotEmptyValue(property.Value))
                 fileContentBuilder.AppendLine($"{property.Key}={property.Value}");
        }
          
        
        var result = fileContentBuilder.ToString();
        logger.Information("Generated .desktop file \n {code}", result);
        return result;
    }

    private bool IsNotEmptyValue(string? propertyValue)
    {
        // ; means empty desktop file array
        return propertyValue != ";" && !string.IsNullOrWhiteSpace(propertyValue);
    }

    private IEnumerable<KeyValuePair<string, string>> GetDesktopProperties(DesktopFile desktopFile)
    {
        var properties = desktopFile
            .GetDesktopFileProperties<DesktopFilePropertyAttribute>()
            .Select(prop =>
            {
                var value = prop.ValueActual is IEnumerable<string> enumerable
                    ? enumerable.ToDesktopFileArray()
                    : prop.ValueActual.ToString() ?? string.Empty;

                var key = prop.Attribute!.Key;

                if (string.IsNullOrWhiteSpace(value))
                    logger.Warning(
                        "This desktop file has invalidated property values at key {key} and may be corrupted", key);

                return KeyValuePair.Create(key, value);

            });
        
        return properties;
    }

    private IEnumerable<KeyValuePair<string, string?>> GetLocalizedProperties(DesktopFile desktopFile)
    {
        var properties = desktopFile
            .GetDesktopFileProperties<LocalizedDesktopFilePropertyAttribute>()
            .Select(property =>
            {
                var objectValue = property.ValueActual;
             
                if(objectValue is not IDictionary dictionary)
                    throw new InvalidOperationException($"Property that has {nameof(LocalizedDesktopFilePropertyAttribute)} must implement {nameof(IDictionary)}");

                var formattedDictionary = new Dictionary<string, string?>(capacity: dictionary.Count);
                
                foreach (var key in dictionary.Keys)
                {
                    if(key is null)
                        throw new InvalidOperationException($"Dictionary that has {nameof(LocalizedDesktopFilePropertyAttribute)} must have no contains null-keys");
                    
                    var value = dictionary[key];
                    var desktopFileValue = value is IEnumerable<string> enumerable 
                        ? enumerable.ToDesktopFileArray()
                        : (string?)value;

                    var desktopFileKey = $"{property.Attribute!.Key}[{key as string ?? string.Empty}]";
                    formattedDictionary[desktopFileKey] = desktopFileValue;
                }
                
                return formattedDictionary;
            });

        foreach (var property in properties)
          foreach (var keyValuePair in property)
                 yield return keyValuePair;
    }
}