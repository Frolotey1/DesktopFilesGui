using System.Threading;
using System.Threading.Tasks;

namespace DesktopFilesGui.Services.Interfaces;

public interface IRootRequirer
{
    public Task RequireRootAsync(CancellationTokenSource cancellationTokenSource);
}