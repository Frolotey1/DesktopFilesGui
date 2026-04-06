using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MimeSharp;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace DesktopFilesGui.DataAnnotation;

public class IsMimeAttribute : ValidationAttribute
{
    private static readonly Mime Mime = new Mime();
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is not string mimeType)
            return new ValidationResult("Value is empty");

        if(string.IsNullOrWhiteSpace(mimeType))
            return new ValidationResult("Value is empty");

        var result = Mime.Extension(mimeType).Any();
        return result ? ValidationResult.Success : new ValidationResult("Value is not a supported MIME type");
    }
}