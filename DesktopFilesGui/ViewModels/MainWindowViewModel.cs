using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DesktopFilesGui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isCodePopupVisible = false;

    #region Desktop file fields

    [ObservableProperty] private string? _fileName;
    [ObservableProperty] private string? _applicationName;
    [ObservableProperty] private bool _enableLocalization;

    #endregion
    
    [RelayCommand]
    private void ChangeCodePopupVisibility()
    {
        IsCodePopupVisible = !IsCodePopupVisible;
    }
    
    
}