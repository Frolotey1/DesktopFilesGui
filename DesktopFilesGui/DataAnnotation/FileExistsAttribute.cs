using System.ComponentModel.DataAnnotations;
using System.IO;
using DesktopFilesGui.Extensions;

namespace DesktopFilesGui.DataAnnotation;

public class FileExistsAttribute(string? directoryProperty = null, string? extension = null) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path)
            return new ValidationResult("Path is not string");

        if (directoryProperty is not null)
        {
            var directory = validationContext.ObjectInstance
                .GetType()
                .GetProperty(directoryProperty)
                ?.GetValue(validationContext.ObjectInstance)
                ?.ToString() ?? string.Empty;
            path = Path.Combine(directory, path);
        }
           
        if(extension is not null)
            path = path.AddPostfix(extension);
        
        if(string.IsNullOrEmpty(path))
            return new ValidationResult("Path is empty");
        
        if(File.Exists(path))
            return ValidationResult.Success;
        return new ValidationResult("File not found");
    }
}