using System.Diagnostics;

namespace FabToolKit.Tracing;

public class FormattedTextWriterTraceListener : TextWriterTraceListener
{
    // Primary constructor - takes directory and filename separately
    public FormattedTextWriterTraceListener(string logFileDirectory, string logFileName)
        : base(CreateLogFilePath(logFileDirectory, logFileName))
    {

    }

    // Secondary constructor - takes full path and extracts/defaults components
    public FormattedTextWriterTraceListener(string logFilePath)
        : this(GetDirectoryOrDefault(logFilePath), GetFileNameOrDefault(logFilePath))
    {
        // Constructor chaining handles everything
    }

    //tertiary constructor with default logfilepath
    public FormattedTextWriterTraceListener() : this(@"C:\ProgramData\Logs\log.txt")
    {
        // Constructor chaining handles everything
    }

    // Helper method to create the dated log file path
    private static string CreateLogFilePath(string directory, string fileName)
    {
        // Ensure directory exists
        Directory.CreateDirectory(directory);

        // Create dated filename
        string datedFileName = $"{fileName}{DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss")}.txt";
        return Path.Combine(directory, datedFileName);
    }

    // Helper method to extract directory or provide default
    private static string GetDirectoryOrDefault(string logFilePath)
    {
        string? directory = Path.GetDirectoryName(logFilePath);
        return !string.IsNullOrEmpty(directory) ? directory : @"C:\ProgramData\Logs";
    }

    // Helper method to extract filename or provide default
    private static string GetFileNameOrDefault(string logFilePath)
    {
        string? fileName = Path.GetFileNameWithoutExtension(logFilePath);
        return !string.IsNullOrEmpty(fileName) ? fileName : "logFile";
    }

    public override void Write(string? message)
    {
        base.Write(DateTime.Now.ToString("[yyyy-MM-dd:HH:mm:ss.fff] "));
        base.Write(message);
    }

    public override void WriteLine(string? message)
    {
        base.Write(DateTime.Now.ToString("[yyyy-MM-dd:HH:mm:ss.fff] "));
        base.WriteLine(message);
    }
}
