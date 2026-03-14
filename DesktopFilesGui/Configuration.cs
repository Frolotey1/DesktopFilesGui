using System;
using System.IO;

namespace DesktopFilesGui;

public static class Configuration
{
    public const string SERILOG_OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level}] [Thread: {Thread}] {Message:lj}{NewLine}{Exception}";
    public static readonly string APPLICATION_DATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopFilesGui");

}