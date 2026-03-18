using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopFilesGui.DataAnnotation;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;
using DesktopFilesGui.Services.Interfaces;

namespace DesktopFilesGui.ViewModels;

public partial class MainWindowViewModel(IGithubSourceOpener githubSourceOpener, IDesktopFileGenerator desktopFileGenerator) : ViewModelBase
{
    [ObservableProperty] private bool _isCodePopupVisible = false;
    [ObservableProperty] private string? _fileName;
    [ObservableProperty] private string? _applicationName;
    [ObservableProperty] private bool _enableLocalization;
    [ObservableProperty] private DesktopFileType _selectedFileType = Configuration.DEFAULT_DESKTOP_FILE_TYPE;
    [ObservableProperty] private string? _pathToFile;
    [ObservableProperty] private string? _pathToIcon;
    [ObservableProperty] private bool _showTerminal;           
    [ObservableProperty] private bool _requireSudo;            
    [ObservableProperty] private bool _displayInMenu = true;   
    [ObservableProperty] private bool _isHidden;             
    [ObservableProperty] private bool _runFromDBus;           
    [ObservableProperty] private bool _startupNotifySupport;   
    [ObservableProperty] private bool _useCustomExecCommand;      
    [ObservableProperty] private ObservableCollection<StringViewModel> _supportedMimeTypes = new();
    [ObservableProperty] private string _code = string.Empty;

    private bool _mustUpdateDesktopFile = true;
    private DesktopFile? _desktopFile;
    private SemaphoreSlim semaphoreSlim = new(1, 1);

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        _mustUpdateDesktopFile = true;
        base.OnPropertyChanged(e);
    }

    [RelayCommand]
    private async Task ChangeCodePopupVisibilityAsync()
    {
        if (!IsCodePopupVisible && _mustUpdateDesktopFile)
            await UpdateDesktopFileAsync();
        IsCodePopupVisible = !IsCodePopupVisible;
    }

    [RelayCommand]
    private void AddMimeType()
    {
        SupportedMimeTypes.Add(new StringViewModel()
        {
            Value = null,
            DynamicValidation = [new IsMimeAttribute()]
        });
    }

    [RelayCommand]
    private void OpenGithubSource()
    {
        Task.Run(githubSourceOpener.Open);
    }
    
    
    [RelayCommand]
    private void ClearMimeTypes()
    {
        Console.WriteLine(string.Join(",", SupportedMimeTypes.Select(s => s.Value)));
        SupportedMimeTypes.Clear();
    }

    [RelayCommand]
    private void ReorderMimeTypes(Tuple<int, int> indexesToSwap)
    {
       ArgumentNullException.ThrowIfNull(indexesToSwap);
       ArgumentOutOfRangeException.ThrowIfGreaterThan(indexesToSwap.Item1, SupportedMimeTypes.Count);
       ArgumentOutOfRangeException.ThrowIfGreaterThan(indexesToSwap.Item2, SupportedMimeTypes.Count);
       ArgumentOutOfRangeException.ThrowIfNegative(indexesToSwap.Item1);
       ArgumentOutOfRangeException.ThrowIfNegative(indexesToSwap.Item2);
       
       var item = SupportedMimeTypes[indexesToSwap.Item1];
       SupportedMimeTypes.RemoveAt(indexesToSwap.Item1);
       
       if (indexesToSwap.Item2 > indexesToSwap.Item1)
           SupportedMimeTypes.Insert(indexesToSwap.Item2 - 1, item);
       else
          SupportedMimeTypes.Insert(indexesToSwap.Item2, item);
       
    }
    
    private async Task UpdateDesktopFileAsync()
    {
        _mustUpdateDesktopFile = false;
        _desktopFile = new DesktopFile
        {
            Type = SelectedFileType,
            ShowTerminal = ShowTerminal,
            RequireSudo = RequireSudo,
            DisplayInMenu = DisplayInMenu,
            EnableLocalization = EnableLocalization,
            IsHidden = IsHidden,
            RunFromDBus = RunFromDBus,
            StartupNotifySupport = StartupNotifySupport,
            UseCustomExecCommand = UseCustomExecCommand,
            SupportedMimeTypes = SupportedMimeTypes.Select(stringViewModel => stringViewModel.Value)
        };
        await semaphoreSlim.WaitAsync();
        await Task.Run(async () =>
        {
            var result = desktopFileGenerator.Generate(_desktopFile);
            await Dispatcher.UIThread.InvokeAsync(() => Code = result);
        });
    }
}