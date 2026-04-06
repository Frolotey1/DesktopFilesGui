using System.Threading.Tasks;
using DesktopFilesGui.Models;

namespace DesktopFilesGui.Services.Interfaces;

public interface IDesktopFileReader
{
    public Task<DesktopFile> ReadAsync();
}