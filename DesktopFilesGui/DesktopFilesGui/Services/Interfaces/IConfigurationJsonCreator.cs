using System.Threading.Tasks;

namespace DesktopFilesGui.Services.Interfaces;

public interface IConfigurationJsonCreator
{
    public ValueTask EnsureCreatedAsync();
}