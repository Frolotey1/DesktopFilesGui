using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DesktopFilesGui.Models;

namespace DesktopFilesGui.Services.Interfaces;

public interface IDesktopFilesListPresenter
{
    public Task<IReadOnlyCollection<DesktopFileHeader>> GetDesktopFilesAsync(string path, CancellationTokenSource cancellationTokenSource);
}