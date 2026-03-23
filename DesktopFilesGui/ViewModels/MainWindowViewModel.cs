using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
using Serilog;

namespace DesktopFilesGui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isCodePopupVisible = false;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [FileNotExists(directoryProperty: nameof(PathToOutput), extension: ".desktop")]
    [Required(ErrorMessage =  "This parameter is required")]
    private string? _fileName;
    
    [ObservableProperty] private string? _applicationName;
    [ObservableProperty] private bool _enableLocalization;
    [ObservableProperty] private DesktopFileType _selectedFileType = StaticConfiguration.DEFAULT_DESKTOP_FILE_TYPE;

    [ObservableProperty] 
    [FileExists] 
    [NotifyDataErrorInfo]
    [Required(ErrorMessage =  "This parameter is required")]
    private string? _pathToRunFile;
   
    [ObservableProperty]
    [FileExists(requiredNotBeEmpty: false)]
    [NotifyDataErrorInfo] 
    private string? _pathToIcon;
    
    [ObservableProperty]
    [DirectoryExists]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage =  "This parameter is required")]
    private string? _pathToOutput = StaticConfiguration.DEFAULT_DESKTOP_FILE_PATH;
   
    [ObservableProperty] private bool _showTerminal;           
    [ObservableProperty] private bool _requireSudo;            
    [ObservableProperty] private bool _displayInMenu = true;   
    [ObservableProperty] private bool _isHidden;             
    [ObservableProperty] private bool _runFromDBus;           
    [ObservableProperty] private bool _startupNotifySupport;   
    [ObservableProperty] private bool _useCustomExecCommand;
    
    [ObservableProperty] 
    [NotifyDataErrorInfo]
    [Required(ErrorMessage =  "This parameter is required")]
    [Url(ErrorMessage = "This string must be a URL Address")]
    private string _url = string.Empty;
    
    [ObservableProperty] private ObservableCollection<StringViewModel> _supportedMimeTypes = new();
    [ObservableProperty] private string _code = string.Empty;
    
    [ObservableProperty] private bool _forceCreateDesktopFile = false;

    [ObservableProperty] private bool _canCreateDesktopFile;

    private bool _mustUpdateDesktopFile = true;
    private DesktopFile? _desktopFile;
    private SemaphoreSlim semaphoreSlim = new(1, 1);
    private readonly ILogger _logger;
    private readonly IGithubSourceOpener _githubSourceOpener;
    private readonly IDesktopFileSerializer _desktopFileSerializer;
    
    
    public MainWindowViewModel(ILogger logger,
        IGithubSourceOpener githubSourceOpener, 
        IDesktopFileSerializer desktopFileSerializer)
    {
        _logger = logger;
        _githubSourceOpener = githubSourceOpener;
        _desktopFileSerializer = desktopFileSerializer;

        this.ValidateAllProperties();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        _mustUpdateDesktopFile = true;
        if (e.PropertyName != nameof(CanCreateDesktopFile))
        {
            CanCreateDesktopFile = !HasErrors || ForceCreateDesktopFile;
        }
           
        base.OnPropertyChanged(e);
    }

    [RelayCommand]
    private async Task ChangeCodePopupVisibilityAsync()
    {
        
        if (!IsCodePopupVisible && _mustUpdateDesktopFile)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                if (!IsCodePopupVisible && _mustUpdateDesktopFile)
                    await UpdateDesktopFileAsync();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        IsCodePopupVisible = !IsCodePopupVisible;
    }

    [RelayCommand]
    private void AddMimeType()
    {
        _logger.Debug("Adding empty mime type");
        SupportedMimeTypes.Add(new StringViewModel()
        {
            Value = null,
            DynamicValidation = [new IsMimeAttribute()]
        });
    }

    [RelayCommand]
    private void OpenGithubSource()
    {
        Task.Run(_githubSourceOpener.Open);
    }
    
    
    [RelayCommand]
    private void ClearMimeTypes()
    {
        _logger.Debug("Clearing mime types");
        SupportedMimeTypes.Clear();
    }

    [RelayCommand(CanExecute = nameof(CanCreateDesktopFile))]
    private async Task CreateDesktopFileAsync()
    {
        
    }
    
    private async Task UpdateDesktopFileAsync()
    {
        _mustUpdateDesktopFile = false;
        _desktopFile = new DesktopFile
        {
            Type = SelectedFileType,
            ShowTerminal = ShowTerminal,
            RequireSudo = RequireSudo,
            HiddenInMenu = !DisplayInMenu,
            EnableLocalization = EnableLocalization,
            IsHidden = IsHidden,
            Name = ApplicationName,
            RunFromDBus = RunFromDBus,
            StartupNotifySupport = StartupNotifySupport,
            SupportedMimeTypes = SupportedMimeTypes
                .Where(stringViewModel => stringViewModel.Value is not null)
                .Select(stringViewModel => stringViewModel.Value)
                .Cast<string>()
        };
        await Task.Run(async () =>
        {
            var result = _desktopFileSerializer.Serialize(_desktopFile);
            await Dispatcher.UIThread.InvokeAsync(() => Code = result);
        });
    }
}