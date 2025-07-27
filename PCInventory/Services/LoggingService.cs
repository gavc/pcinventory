using System.Text;

namespace PCInventory.Services
{
    public class LoggingService
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public LoggingService()
        {
            var logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PCInventory", 
                "Logs");
            
            Directory.CreateDirectory(logDirectory);
            _logFilePath = Path.Combine(logDirectory, $"PCInventory_{DateTime.Now:yyyyMMdd}.log");
        }

        public void LogError(string message, Exception? exception = null, string? pcName = null)
        {
            var logEntry = CreateLogEntry("ERROR", message, exception, pcName);
            WriteToLog(logEntry);
        }

        public void LogWarning(string message, string? pcName = null)
        {
            var logEntry = CreateLogEntry("WARNING", message, null, pcName);
            WriteToLog(logEntry);
        }

        public void LogInfo(string message, string? pcName = null)
        {
            var logEntry = CreateLogEntry("INFO", message, null, pcName);
            WriteToLog(logEntry);
        }

        public void LogDebug(string message, string? pcName = null)
        {
#if DEBUG
            var logEntry = CreateLogEntry("DEBUG", message, null, pcName);
            WriteToLog(logEntry);
#endif
        }

        private string CreateLogEntry(string level, string message, Exception? exception, string? pcName)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}]");
            
            if (!string.IsNullOrEmpty(pcName))
                sb.AppendLine($"PC: {pcName}");
            
            sb.AppendLine($"Message: {message}");
            
            if (exception != null)
            {
                sb.AppendLine($"Exception: {exception.GetType().Name}");
                sb.AppendLine($"Exception Message: {exception.Message}");
                sb.AppendLine($"Stack Trace: {exception.StackTrace}");
                
                if (exception.InnerException != null)
                {
                    sb.AppendLine($"Inner Exception: {exception.InnerException.GetType().Name}");
                    sb.AppendLine($"Inner Message: {exception.InnerException.Message}");
                }
            }
            
            sb.AppendLine(new string('-', 80));
            return sb.ToString();
        }

        private void WriteToLog(string logEntry)
        {
            try
            {
                lock (_lockObject)
                {
                    File.AppendAllText(_logFilePath, logEntry, Encoding.UTF8);
                }
            }
            catch (Exception)
            {
                // Silently fail if we can't write to log - don't throw exceptions from logging
                // This prevents infinite loops or crashes due to logging failures
            }
        }

        public void CleanupOldLogs(int daysToKeep = 30)
        {
            try
            {
                var logDirectory = Path.GetDirectoryName(_logFilePath);
                if (string.IsNullOrEmpty(logDirectory) || !Directory.Exists(logDirectory))
                    return;

                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var logFiles = Directory.GetFiles(logDirectory, "PCInventory_*.log");

                foreach (var logFile in logFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(logFile);
                        if (fileInfo.CreationTime < cutoffDate)
                        {
                            File.Delete(logFile);
                        }
                    }
                    catch
                    {
                        // Continue with other files if one fails to delete
                    }
                }
            }
            catch
            {
                // Silently fail if cleanup doesn't work
            }
        }

        public string GetLogFilePath() => _logFilePath;
    }
}
