namespace InfectionVet.Utilities;

/// <summary>
/// Provides basic file logging for application events and errors.
/// The log can help technical support identify when an error occured, what operation caused it, and what message was generated.
/// </summary>
public class Logger
{
    private static readonly string ProjectDirectory =
        Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "../../../"));
    
    private static readonly string LogDirectory =
        Path.Combine(ProjectDirectory, "Logs");
    
    private static readonly string LogFile =
        Path.Combine(LogDirectory, "infectionvet.log");

    /// <summary>
    /// Writes an informational message to the log file.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    /// <summary>
    /// Writes an error message to the log file.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    public static void LogError(string message)
    {
        WriteLog("ERROR", message);
    }

    /// <summary>
    /// Writes a formatted log entry to the log file.
    /// </summary>
    /// <param name="level">The severity level of the log entry.</param>
    /// <param name="message">The message to log.</param>
    private static void WriteLog(string level, string message)
    {
        Directory.CreateDirectory(LogDirectory);

        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string logEntry = $"[{timeStamp}] [{level}] {message}";
        
        File.AppendAllText(
            LogFile,
            logEntry + Environment.NewLine);
    }
}