using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopFilesGui.Models;

namespace DesktopFilesGui.Services.Interfaces;

public interface IDesktopFileDeserializer {
    Task<DesktopFile> DeserializeAsync(IEnumerable<string> lines);
    DesktopFile Deserialize(IEnumerable<string> lines);
}
