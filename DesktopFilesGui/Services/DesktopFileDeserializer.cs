using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DesktopFilesGui.Attributes;
using DesktopFilesGui.Extensions;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;
using DesktopFilesGui.Services.Interfaces;
using Serilog;

namespace DesktopFilesGui.Services;

public sealed class DesktopFileDeserializer(ILogger logger) : IDesktopFileDeserializer
{
    public async Task<DesktopFile> DeserializeAsync(IEnumerable<string> lines)
    {
        return await Task.Run(() => Deserialize(lines));
    }
    
    public DesktopFile Deserialize(IEnumerable<string> lines)
    {
        logger.Debug("Starting deserialization of desktop file");
        
        var desktopFile = new DesktopFile();
        
        var properties = GetPropertiesWithAttributes(desktopFile);
        
        bool inDesktopEntry = false;
        
        foreach (var line in lines)
        {
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
            
            SetProperty(desktopFile, key, value, properties);
        }
        
        logger.Information("Desktop file deserialization completed. Type: {Type}", desktopFile.Type);
        return desktopFile;
    }
    
    private Dictionary<string, PropertyInfo> GetPropertiesWithAttributes(DesktopFile desktopFile)
    {
        var result = new Dictionary<string, PropertyInfo>();
        var type = desktopFile.GetType();
        
        foreach (var prop in type.GetProperties())
        {
            var attribute = prop.GetCustomAttribute<DesktopFilePropertyAttribute>();
            if (attribute is not null)
            {
                result[attribute.Key] = prop;
            }
        }
        
        foreach (var prop in type.GetProperties())
        {
            var attribute = prop.GetCustomAttribute<LocalizedDesktopFilePropertyAttribute>();
            if (attribute is not null)
            {
                result[$"LOCALIZED_{attribute.Key}"] = prop;
            }
        }
        
        return result;
    }
    
    private void SetProperty(DesktopFile desktopFile, string key, string value, Dictionary<string, PropertyInfo> properties)
    {
        if (key.Contains('[') && key.EndsWith(']'))
        {
            var bracketIndex = key.IndexOf('[');
            var baseKey = key[..bracketIndex];
            var locale = key[(bracketIndex + 1)..^1];
            
            var localizedPropKey = $"LOCALIZED_{baseKey}";
            if (properties.TryGetValue(localizedPropKey, out var prop))
            {
                SetLocalizedProperty(desktopFile, prop, locale, value);
            }
            return;
        }
        
        if (properties.TryGetValue(key, out var propertyInfo))
        {
            SetRegularProperty(desktopFile, propertyInfo, value);
        }
        else
        {
            logger.Verbose("Unhandled desktop file key: {Key} = {Value}", key, value);
        }
    }
    
    private void SetRegularProperty(DesktopFile desktopFile, PropertyInfo property, string value)
    {
        var targetType = property.PropertyType;
        
        try
        {
            if (targetType == typeof(string))
            {
                property.SetValue(desktopFile, value);
            }
            else if (targetType == typeof(bool))
            {
                if (bool.TryParse(value, out var boolResult))
                    property.SetValue(desktopFile, boolResult);
                else
                    logger.Warning("Failed to parse boolean for {Property} with value {Value}", property.Name, value);
            }
            else if (targetType == typeof(DesktopFileType))
            {
                if (Enum.TryParse<DesktopFileType>(value, true, out var enumResult))
                    property.SetValue(desktopFile, enumResult);
                else
                    logger.Warning("Failed to parse DesktopFileType for {Property} with value {Value}", property.Name, value);
            }
            else if (targetType == typeof(IEnumerable<string>))
            {
                property.SetValue(desktopFile, value.ToEnumerableFromDesktopFileArray());
            }
            else
            {
                logger.Warning("Unsupported property type {Type} for key {Property}", targetType, property.Name);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to set property {Property} with value {Value}", property.Name, value);
        }
    }
    
    private void SetLocalizedProperty(DesktopFile desktopFile, PropertyInfo property, string locale, string value)
    {
        if (property.GetValue(desktopFile) is not IDictionary dictionary)
        {
            var dictType = typeof(Dictionary<string, string>);
            var newDict = Activator.CreateInstance(dictType) as IDictionary;
            property.SetValue(desktopFile, newDict);
            dictionary = newDict!;
        }
        
        dictionary[locale] = value;
        
        var enableLocalizationProp = desktopFile.GetType().GetProperty(nameof(DesktopFile.EnableLocalization));
        if (enableLocalizationProp is not null && enableLocalizationProp.PropertyType == typeof(bool))
        {
            enableLocalizationProp.SetValue(desktopFile, true);
        }
    }
}
