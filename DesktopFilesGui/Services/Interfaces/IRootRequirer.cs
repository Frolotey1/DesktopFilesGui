using System.Threading.Tasks;

namespace DesktopFilesGui.Services.Interfaces;

public interface IRootRequirer
{
    public Task RequireRootAsync();
}