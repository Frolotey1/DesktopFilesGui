using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopFilesGui.Models;
using DesktopFilesGui.Models.Enums;
using DesktopFilesGui.Services.Interfaces;

namespace DesktopFilesGui.ViewModels;

public sealed partial class DesktopFileListViewModel(IDesktopFilesListPresenter presenter) : ViewModelBase
{
    [ObservableProperty] private FilesViewType _filesViewType;
    [ObservableProperty] private string _path = StaticConfiguration.DEFAULT_DESKTOP_FILE_PATH;
    [ObservableProperty] private ObservableCollection<DesktopFileHeader> _files = [];

    [RelayCommand]
    private async Task LoadFilesAsync()
    {
        var token = new CancellationTokenSource();
        var files = await presenter.GetDesktopFilesAsync(Path, token);
        
        Files.AddRange(files);
    }
    
    [RelayCommand]
    private void UpdateFilesViewType(FilesViewType newType)
    {
        FilesViewType = newType;
    }
}