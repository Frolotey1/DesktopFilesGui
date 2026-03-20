using System.Threading.Tasks;

namespace DesktopFilesGui.Services.Interfaces;

public interface IExecutionTriggersJsonCreator
{
    public ValueTask EnsureCreatedAsync();
}