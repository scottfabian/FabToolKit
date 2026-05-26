using System.Diagnostics;

namespace FabToolKit.Tracing;

public class FormattedConsoleTraceListener : ConsoleTraceListener
{
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
