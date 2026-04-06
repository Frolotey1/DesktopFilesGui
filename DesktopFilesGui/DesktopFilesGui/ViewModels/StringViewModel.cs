using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace DesktopFilesGui.ViewModels;

public sealed partial class StringViewModel : ViewModelBase, INotifyDataErrorInfo
{
    [ObservableProperty] private string? _value = null;

    public IEnumerable<ValidationAttribute> DynamicValidation { get; set; } = [];
    
    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        if (propertyName != nameof(Value))
            return Enumerable.Empty<ValidationResult>();

        var context = new ValidationContext(this, Ioc.Default, null)
        {
            MemberName = propertyName
        };

        var errors = DynamicValidation
            .Select(validator => validator.GetValidationResult(Value, context))
            .Where(IsNotValid)
            .OfType<ValidationResult>()
            .Concat(base.GetErrors(propertyName))
            .ToList();
      
        return errors;
    }

    private bool IsNotValid(ValidationResult? result)
    {
        return result != ValidationResult.Success;
    }
}