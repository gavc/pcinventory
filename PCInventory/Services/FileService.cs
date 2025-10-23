using System.Text;
using System.Text.Json;
using System.Threading;
using PCInventory.Models;

namespace PCInventory.Services
{
    public class FileService
    {
        public List<string> ImportPCList(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The specified file was not found: {filePath}", filePath);

            try
            {
                var pcNames = new List<string>();
                using var reader = new StreamReader(filePath);
                string? line;
                int lineNumber = 0;
                
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var trimmedLine = line.Trim();
                            // Basic validation for PC names
                            if (IsValidPCName(trimmedLine))
                            {
                                pcNames.Add(trimmedLine);
                            }
                            else
                            {
                                // Log invalid PC name but continue processing
                                System.Diagnostics.Debug.WriteLine($"Skipping invalid PC name on line {lineNumber}: {trimmedLine}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing line {lineNumber}: {ex.Message}");
                        // Continue processing other lines
                    }
                }

                if (pcNames.Count == 0)
                    throw new InvalidOperationException("No valid PC names found in the file.");

                return pcNames;
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException($"Access denied when trying to read file: {filePath}");
            }
            catch (IOException ioEx)
            {
                throw new IOException($"IO error when reading file: {filePath}. {ioEx.Message}", ioEx);
            }
            catch (Exception ex) when (!(ex is FileNotFoundException || ex is ArgumentException || ex is InvalidOperationException))
            {
                throw new Exception($"Unexpected error reading file: {filePath}. {ex.Message}", ex);
            }
        }

        private bool IsValidPCName(string pcName)
        {
            if (string.IsNullOrWhiteSpace(pcName) || pcName.Length > 255)
                return false;

            // Check for invalid characters (Windows computer name rules)
            char[] invalidChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|', ' ' };
            
            // Allow spaces but check for other invalid characters
            char[] restrictedChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };
            
            return !pcName.Any(c => restrictedChars.Contains(c));
        }

        public void ExportToCSV(List<PCInfo> pcInfoList, string filePath, AppSettings settings)
        {
            if (pcInfoList == null)
                throw new ArgumentNullException(nameof(pcInfoList));
            
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            try
            {
                // Ensure directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
                
                // Write header row
                var headers = new List<string> { "PC Name", "Status" };
                
                if (settings.CheckHDDSize) headers.Add("HDD Size");
                if (settings.CheckFreeHDDSpace) headers.Add("Free HDD Space");
                if (settings.CheckTotalRAM) headers.Add("Total RAM");
                if (settings.CheckIPAddress) headers.Add("IP Address");
                if (settings.CheckMACAddress) headers.Add("MAC Address");
                if (settings.CheckLoggedOnUser) headers.Add("Logged-on User");
                if (settings.CheckLastRebootTime) headers.Add("Last Reboot");
                if (settings.CheckMake) headers.Add("Make");
                if (settings.CheckModel) headers.Add("Model");
                if (settings.CheckBIOSVersion) headers.Add("BIOS Version");
                if (settings.CheckWindowsVersion) headers.Add("Windows Version");
                if (settings.CheckSerialNumber) headers.Add("Serial Number");
                if (settings.CheckNetworkConnectionType) headers.Add("Network Connection Type");
                if (settings.CheckWiFiInfo) headers.Add("WiFi Info");
                
                // Add custom registry check headers
                foreach (var regCheck in settings.RegistryChecks.Where(rc => rc.Enabled))
                {
                    headers.Add(regCheck.FriendlyName);
                }
                
                writer.WriteLine(string.Join(",", headers.Select(EscapeCSV)));
                
                // Write data rows
                foreach (var pcInfo in pcInfoList)
                {
                    try
                    {
                        var values = new List<string> { EscapeCSV(pcInfo.PCName), EscapeCSV(pcInfo.Status) };
                        
                        if (settings.CheckHDDSize) values.Add(EscapeCSV(pcInfo.HDDSize));
                        if (settings.CheckFreeHDDSpace) values.Add(EscapeCSV(pcInfo.FreeHDDSpace));
                        if (settings.CheckTotalRAM) values.Add(EscapeCSV(pcInfo.TotalRAM));
                        if (settings.CheckIPAddress) values.Add(EscapeCSV(pcInfo.IPAddress));
                        if (settings.CheckMACAddress) values.Add(EscapeCSV(pcInfo.MACAddress));
                        if (settings.CheckLoggedOnUser) values.Add(EscapeCSV(pcInfo.LoggedOnUser));
                        if (settings.CheckLastRebootTime) values.Add(EscapeCSV(pcInfo.LastRebootTime));
                        if (settings.CheckMake) values.Add(EscapeCSV(pcInfo.Make));
                        if (settings.CheckModel) values.Add(EscapeCSV(pcInfo.Model));
                        if (settings.CheckBIOSVersion) values.Add(EscapeCSV(pcInfo.BIOSVersion));
                        if (settings.CheckWindowsVersion) values.Add(EscapeCSV(pcInfo.WindowsVersion));
                        if (settings.CheckSerialNumber) values.Add(EscapeCSV(pcInfo.SerialNumber));
                        if (settings.CheckNetworkConnectionType) values.Add(EscapeCSV(pcInfo.NetworkConnectionType));
                        if (settings.CheckWiFiInfo) values.Add(EscapeCSV(pcInfo.WiFiInfo));
                        
                        // Add custom registry values
                        foreach (var regCheck in settings.RegistryChecks.Where(rc => rc.Enabled))
                        {
                            if (pcInfo.CustomRegistryValues.TryGetValue(regCheck.FriendlyName, out string? value))
                            {
                                values.Add(EscapeCSV(value ?? string.Empty));
                            }
                            else
                            {
                                values.Add(string.Empty);
                            }
                        }
                        
                        writer.WriteLine(string.Join(",", values));
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other entries
                        System.Diagnostics.Debug.WriteLine($"Error writing data for PC {pcInfo.PCName}: {ex.Message}");
                        writer.WriteLine($"{EscapeCSV(pcInfo.PCName)},Error writing data: {EscapeCSV(ex.Message)}");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException($"Access denied when trying to write to file: {filePath}");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException($"Directory not found for file path: {filePath}");
            }
            catch (IOException ioEx)
            {
                throw new IOException($"IO error when writing to file: {filePath}. {ioEx.Message}", ioEx);
            }
            catch (Exception ex) when (!(ex is ArgumentNullException || ex is ArgumentException))
            {
                throw new Exception($"Unexpected error writing to file: {filePath}. {ex.Message}", ex);
            }
        }
        
        private string EscapeCSV(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
                
            bool needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n');
            if (!needsQuotes)
                return value;
                
            // Escape quotes by doubling them and wrap in quotes
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        public void SaveSettings(AppSettings settings, string filePath)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            string fullPath = Path.GetFullPath(filePath);
            string? directory = Path.GetDirectoryName(fullPath);

            try
            {
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json;
                try
                {
                    json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                }
                catch (JsonException jsonEx)
                {
                    throw new InvalidOperationException($"Error serializing settings: {jsonEx.Message}", jsonEx);
                }

                string fileName = Path.GetFileName(fullPath);
                string backupPath = Path.Combine(directory ?? string.Empty, $".{fileName}.bak");

                for (int attempt = 1; attempt <= 3; attempt++)
                {
                    string tempPath = Path.Combine(directory ?? string.Empty, $".{fileName}.{Guid.NewGuid():N}.tmp");
                    try
                    {
                        File.WriteAllText(tempPath, json, Encoding.UTF8);

                        if (File.Exists(fullPath))
                        {
                            File.Replace(tempPath, fullPath, backupPath, ignoreMetadataErrors: true);
                            try
                            {
                                if (File.Exists(backupPath))
                                {
                                    File.Delete(backupPath);
                                }
                            }
                            catch
                            {
                                // Best-effort cleanup of backup file
                            }
                        }
                        else
                        {
                            File.Move(tempPath, fullPath);
                        }

                        return;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new UnauthorizedAccessException($"Access denied when trying to save settings to: {fullPath}");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw new DirectoryNotFoundException($"Directory not found for settings file: {fullPath}");
                    }
                    catch (IOException) when (attempt < 3)
                    {
                        Thread.Sleep(150);
                        continue;
                    }
                    catch (IOException ioEx)
                    {
                        throw new IOException($"IO error when saving settings to: {fullPath}. {ioEx.Message}", ioEx);
                    }
                    finally
                    {
                        try
                        {
                            if (File.Exists(tempPath))
                            {
                                File.Delete(tempPath);
                            }
                        }
                        catch
                        {
                            // Swallow cleanup exceptions
                        }
                    }
                }

                throw new IOException($"Failed to save settings to: {fullPath}. File may be locked.");
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception ex) when (!(ex is ArgumentNullException || ex is ArgumentException || ex is InvalidOperationException))
            {
                throw new Exception($"Unexpected error saving settings to: {fullPath}. {ex.Message}", ex);
            }
        }

        public AppSettings LoadSettings(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                return new AppSettings();

            try
            {
                var json = File.ReadAllText(filePath, Encoding.UTF8);
                
                if (string.IsNullOrWhiteSpace(json))
                    return new AppSettings();

                var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
                return settings ?? new AppSettings();
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException($"Access denied when trying to load settings from: {filePath}");
            }
            catch (IOException ioEx)
            {
                throw new IOException($"IO error when loading settings from: {filePath}. {ioEx.Message}", ioEx);
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                // Try to load backup if available
                var backupFile = filePath + ".backup";
                if (File.Exists(backupFile))
                {
                    try
                    {
                        var backupJson = File.ReadAllText(backupFile, Encoding.UTF8);
                        var backupSettings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(backupJson);
                        return backupSettings ?? new AppSettings();
                    }
                    catch
                    {
                        // If backup also fails, return default settings
                    }
                }
                
                throw new InvalidOperationException($"Error parsing settings file: {filePath}. {jsonEx.Message}. File may be corrupted.", jsonEx);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                throw new Exception($"Unexpected error loading settings from: {filePath}. {ex.Message}", ex);
            }
        }
    }
}