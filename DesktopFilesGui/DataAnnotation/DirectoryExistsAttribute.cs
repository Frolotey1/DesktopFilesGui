using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DesktopFilesGui.DataAnnotation;

public class DirectoryExistsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path)
            return new ValidationResult("Path is not string");

        if (string.IsNullOrEmpty(path))
            return new ValidationResult("Path is empty");
          
        
        if(Directory.Exists(path))
            return ValidationResult.Success;
        
        return new ValidationResult("Folder not found");
    }
}