using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using PCInventory.Models;
using System.Diagnostics;

namespace PCInventory.Services
{
    public class PCHealthService
    {
        private readonly AppSettings _settings;
        private readonly bool _allowWmiFallback = true; // Always allow fallback since running as admin
        private readonly LoggingService _logger;

        public PCHealthService(AppSettings settings)
        {
            _settings = settings;
            _logger = new LoggingService();
        }

        public async Task<PCInfo> GetPCHealthInfoAsync(string pcName)
        {
            var pcInfo = new PCInfo
            {
                PCName = pcName,
                Status = "Connecting..."
            };

            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        // Basic connectivity check with timeout
                        using Ping ping = new Ping();
                        PingReply reply = ping.Send(pcName, 2000); // 2 second timeout - faster detection of offline PCs
                        if (reply.Status != IPStatus.Success)
                        {
                            pcInfo.Status = GetDetailedPingStatus(reply.Status);
                            _logger.LogWarning($"Ping failed: {reply.Status}", pcName);
                            return;
                        }

                        _logger.LogInfo($"Ping successful: {reply.RoundtripTime}ms", pcName);
                        pcInfo.Status = "Gathering data...";
                        
                        // Create single WMI connection scope for all queries (Connection Pooling)
                        var connectionOptions = new ConnectionOptions
                        {
                            Timeout = TimeSpan.FromSeconds(30),
                            EnablePrivileges = true,
                            Authentication = AuthenticationLevel.PacketPrivacy
                        };
                        
                        var wmiScope = new ManagementScope($"\\\\{pcName}\\root\\cimv2", connectionOptions);
                        
                        // Use batched WMI queries with shared connection for better performance
                        try
                        {
                            // Connect once and reuse for all queries
                            wmiScope.Connect();
                            _logger.LogInfo("WMI connection established", pcName);
                            
                            // Batch #1: System Information (Make, Model, TotalRAM, LoggedOnUser)
                            if (_settings.CheckMake || _settings.CheckModel || _settings.CheckTotalRAM || _settings.CheckLoggedOnUser)
                            {
                                var systemInfo = GetSystemInformationBatch(wmiScope);
                                if (_settings.CheckMake) pcInfo.Make = systemInfo.Make;
                                if (_settings.CheckModel) pcInfo.Model = systemInfo.Model;
                                if (_settings.CheckTotalRAM) 
                                {
                                    pcInfo.TotalRAM = systemInfo.TotalRAM;
                                    pcInfo.TotalRAMBytes = systemInfo.TotalRAMBytes;
                                }
                                if (_settings.CheckLoggedOnUser) pcInfo.LoggedOnUser = systemInfo.LoggedOnUser;
                            }
                            
                            // Batch #2: Storage Information (HDD size, free space)
                            if (_settings.CheckHDDSize || _settings.CheckFreeHDDSpace)
                            {
                                var storageInfo = GetStorageInformationBatch(wmiScope);
                                if (_settings.CheckHDDSize) 
                                {
                                    pcInfo.HDDSize = storageInfo.HDDSize;
                                    pcInfo.HDDSizeBytes = storageInfo.HDDSizeBytes;
                                }
                                if (_settings.CheckFreeHDDSpace) 
                                {
                                    pcInfo.FreeHDDSpace = storageInfo.FreeSpace;
                                    pcInfo.FreeHDDSpaceBytes = storageInfo.FreeSpaceBytes;
                                }
                            }
                            
                            // Batch #3: Network Information (IP, MAC, connection type)
                            if (_settings.CheckIPAddress || _settings.CheckMACAddress || _settings.CheckNetworkConnectionType)
                            {
                                var networkInfo = GetNetworkInformationBatch(wmiScope, pcName);
                                if (_settings.CheckIPAddress) pcInfo.IPAddress = networkInfo.IPAddress;
                                if (_settings.CheckMACAddress) pcInfo.MACAddress = networkInfo.MACAddress;
                                if (_settings.CheckNetworkConnectionType) pcInfo.NetworkConnectionType = networkInfo.ConnectionType;
                            }
                            
                            // Batch #4: BIOS/Hardware Information
                            if (_settings.CheckBIOSVersion || _settings.CheckSerialNumber)
                            {
                                var biosInfo = GetBIOSInformationBatch(wmiScope);
                                if (_settings.CheckBIOSVersion) pcInfo.BIOSVersion = biosInfo.BIOSVersion;
                                if (_settings.CheckSerialNumber) pcInfo.SerialNumber = biosInfo.SerialNumber;
                            }
                            
                            // Batch #5: Operating System Information
                            if (_settings.CheckWindowsVersion || _settings.CheckLastRebootTime || _settings.CheckInstallDate)
                            {
                                var osInfo = GetOperatingSystemInformationBatch(wmiScope, pcName);
                                if (_settings.CheckWindowsVersion) pcInfo.WindowsVersion = osInfo.WindowsVersion;
                                if (_settings.CheckLastRebootTime) pcInfo.LastRebootTime = osInfo.LastRebootTime;
                                if (_settings.CheckInstallDate) pcInfo.InstallDate = osInfo.InstallDate;
                            }
                            
                            // Individual checks that can't be easily batched
                            if (_settings.CheckPendingReboot)
                            {
                                pcInfo.PendingRebootStatus = CheckPendingReboot(pcName);
                            }
                            
                            if (_settings.CheckWiFiInfo)
                            {
                                pcInfo.WiFiInfo = GetWiFiInfo(pcName);
                            }
                            
                            // Get custom registry values
                            foreach (var regCheck in _settings.RegistryChecks.Where(rc => rc.Enabled))
                            {
                                try
                                {
                                    var value = GetRemoteRegistryValue(pcName, regCheck.KeyPath, regCheck.ValueName);
                                    pcInfo.CustomRegistryValues[regCheck.FriendlyName] = value;
                                }
                                catch (Exception ex)
                                {
                                    pcInfo.CustomRegistryValues[regCheck.FriendlyName] = $"Error: {ex.Message}";
                                }
                            }

                            pcInfo.Status = "Completed";
                            _logger.LogInfo("PC scan completed successfully", pcName);
                        }
                        catch (UnauthorizedAccessException uaEx)
                        {
                            pcInfo.Status = $"Access Denied: {uaEx.Message}";
                            _logger.LogError("Access denied during PC scan", uaEx, pcName);
                        }
                        catch (System.Management.ManagementException mgmtEx)
                        {
                            pcInfo.Status = $"WMI Error: {GetFriendlyWMIError(mgmtEx)}";
                            _logger.LogError("WMI error during PC scan", mgmtEx, pcName);
                        }
                        catch (System.Runtime.InteropServices.COMException comEx)
                        {
                            pcInfo.Status = $"COM Error: {GetFriendlyCOMError(comEx)}";
                            _logger.LogError("COM error during PC scan", comEx, pcName);
                        }
                        catch (TimeoutException timeEx)
                        {
                            pcInfo.Status = $"Timeout: {timeEx.Message}";
                            _logger.LogError("Timeout during PC scan", timeEx, pcName);
                        }
                        catch (Exception ex)
                        {
                            pcInfo.Status = $"Error: {ex.Message}";
                            _logger.LogError("Unexpected error during PC scan", ex, pcName);
                        }
                    }
                    catch (PingException pingEx)
                    {
                        pcInfo.Status = $"Network Error: {pingEx.Message}";
                        _logger.LogError("Ping error", pingEx, pcName);
                    }
                    catch (Exception ex)
                    {
                        pcInfo.Status = $"Unexpected Error: {ex.Message}";
                        _logger.LogError("Unexpected error in PC health check", ex, pcName);
                    }
                });

                return pcInfo;
            }
            catch (Exception ex)
            {
                pcInfo.Status = $"Error: {ex.Message}";
                return pcInfo;
            }
        }

        private string GetRemoteRegistryValue(string pcName, string keyPath, string valueName)
        {
            try
            {
                // First attempt: Standard Remote Registry method
                try
                {
                    string subKey = keyPath;
                    if (subKey.StartsWith("HKEY_LOCAL_MACHINE\\"))
                        subKey = subKey.Replace("HKEY_LOCAL_MACHINE\\", "");

                    using var baseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pcName);
                    using var key = baseKey.OpenSubKey(subKey);
                    
                    if (key == null)
                        return "Key not found";

                    var value = key.GetValue(valueName);
                    return value?.ToString() ?? "Value not found";
                }
                catch (Exception)
                {
                    // If standard method fails, try WMI method
                    if (_allowWmiFallback)
                    {
                        try
                        {
                            return GetRegistryValueViaWMI(pcName, keyPath, valueName);
                        }
                        catch (Exception wmiEx)
                        {
                            return $"Error: Registry access failed (tried both methods). {wmiEx.Message}";
                        }
                    }
                    
                    throw; // Re-throw if WMI fallback is not allowed
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string GetInstallDateFromRegistry(string pcName)
        {
            try
            {
                var rawValue = GetRemoteRegistryValue(pcName, @"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "InstallDate");

                if (string.IsNullOrWhiteSpace(rawValue))
                    return "Value not found";

                if (rawValue.Equals("Value not found", StringComparison.OrdinalIgnoreCase) ||
                    rawValue.Equals("Key not found", StringComparison.OrdinalIgnoreCase))
                {
                    return rawValue;
                }

                if (rawValue.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                    return rawValue;

                if (long.TryParse(rawValue, out long epochSeconds) && epochSeconds > 0)
                {
                    try
                    {
                        return DateTimeOffset.FromUnixTimeSeconds(epochSeconds)
                            .ToLocalTime()
                            .ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // Fall through to returning the raw value if conversion fails
                    }
                }

                return rawValue;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        
        private string GetRegistryValueViaWMI(string pcName, string keyPath, string valueName)
        {
            string hive = "HKLM";
            string subKeyPath = keyPath;
            
            if (keyPath.StartsWith("HKEY_LOCAL_MACHINE\\"))
            {
                subKeyPath = keyPath.Replace("HKEY_LOCAL_MACHINE\\", "");
            }
            
            // WMI query to get registry value
            var scope = new ManagementScope($"\\\\{pcName}\\root\\default");
            scope.Connect();
            
            using var classInstance = new ManagementClass(scope, new ManagementPath("StdRegProv"), new ObjectGetOptions());
            
            // First try to read as string value
            var result = ReadRegistryStringValue(classInstance, hive, subKeyPath, valueName);
            if (result != "WMI_NOT_FOUND")
                return result;
                
            // Then try DWORD
            result = ReadRegistryDWordValue(classInstance, hive, subKeyPath, valueName);
            if (result != "WMI_NOT_FOUND")
                return result;
                
            // Try Multi-String
            result = ReadRegistryMultiStringValue(classInstance, hive, subKeyPath, valueName);
            if (result != "WMI_NOT_FOUND")
                return result;
                
            // Try Binary
            result = ReadRegistryBinaryValue(classInstance, hive, subKeyPath, valueName);
            if (result != "WMI_NOT_FOUND")
                return result;
                
            return "Value not found (WMI)";
        }
        
        private string ReadRegistryStringValue(ManagementClass classInstance, string hive, string subKeyPath, string valueName)
        {
            using var inParams = classInstance.GetMethodParameters("GetStringValue");
            inParams["hDefKey"] = GetHiveValue(hive);
            inParams["sSubKeyName"] = subKeyPath;
            inParams["sValueName"] = valueName;
            
            using var outParams = classInstance.InvokeMethod("GetStringValue", inParams, null);
            
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue == 0)
            {
                var value = outParams["sValue"];
                return value?.ToString() ?? "Empty string value";
            }
            
            return "WMI_NOT_FOUND";
        }
        
        private string ReadRegistryDWordValue(ManagementClass classInstance, string hive, string subKeyPath, string valueName)
        {
            using var inParams = classInstance.GetMethodParameters("GetDWORDValue");
            inParams["hDefKey"] = GetHiveValue(hive);
            inParams["sSubKeyName"] = subKeyPath;
            inParams["sValueName"] = valueName;
            
            using var outParams = classInstance.InvokeMethod("GetDWORDValue", inParams, null);
            
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue == 0)
            {
                var value = outParams["uValue"];
                return value?.ToString() ?? "Empty DWORD value";
            }
            
            return "WMI_NOT_FOUND";
        }
        
        private string ReadRegistryMultiStringValue(ManagementClass classInstance, string hive, string subKeyPath, string valueName)
        {
            using var inParams = classInstance.GetMethodParameters("GetMultiStringValue");
            inParams["hDefKey"] = GetHiveValue(hive);
            inParams["sSubKeyName"] = subKeyPath;
            inParams["sValueName"] = valueName;
            
            using var outParams = classInstance.InvokeMethod("GetMultiStringValue", inParams, null);
            
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue == 0)
            {
                var values = outParams["sValue"] as string[];
                if (values != null && values.Length > 0)
                    return string.Join(", ", values);
                return "Empty multi-string value";
            }
            
            return "WMI_NOT_FOUND";
        }
        
        private string ReadRegistryBinaryValue(ManagementClass classInstance, string hive, string subKeyPath, string valueName)
        {
            using var inParams = classInstance.GetMethodParameters("GetBinaryValue");
            inParams["hDefKey"] = GetHiveValue(hive);
            inParams["sSubKeyName"] = subKeyPath;
            inParams["sValueName"] = valueName;
            
            using var outParams = classInstance.InvokeMethod("GetBinaryValue", inParams, null);
            
            var returnValue = Convert.ToInt32(outParams["ReturnValue"]);
            if (returnValue == 0)
            {
                var values = outParams["uValue"] as byte[];
                if (values != null && values.Length > 0)
                    return BitConverter.ToString(values).Replace("-", " ");
                return "Empty binary value";
            }
            
            return "WMI_NOT_FOUND";
        }
        
        private uint GetHiveValue(string hive)
        {
            return hive.ToUpper() switch
            {
                "HKCR" or "HKEY_CLASSES_ROOT" => 0x80000000u,
                "HKCU" or "HKEY_CURRENT_USER" => 0x80000001u,
                "HKLM" or "HKEY_LOCAL_MACHINE" => 0x80000002u,
                "HKU" or "HKEY_USERS" => 0x80000003u,
                "HKCC" or "HKEY_CURRENT_CONFIG" => 0x80000005u,
                _ => 0x80000002u // Default to HKLM
            };
        }

        private string FormatBytes(double bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }
            // Round to whole numbers for better display
            return $"{Math.Round(bytes)} {sizes[order]}";
        }

        private string CheckPendingReboot(string pcName)
        {
            try
            {
                List<string> reasons = new List<string>();

                // Check Windows Update reboot
                string wuPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update";
                string wuValue = GetRemoteRegistryValue(pcName, wuPath, "RebootRequired");
                if (wuValue != "Value not found" && wuValue != "Key not found" && !wuValue.StartsWith("Error:"))
                    reasons.Add("Windows Update");

                // Check SCCM reboot
                string sccmPath = @"SOFTWARE\Microsoft\SMS\Mobile Client\Reboot Management";
                string sccmValue = GetRemoteRegistryValue(pcName, sccmPath, "RebootPending");
                if (sccmValue != "Value not found" && sccmValue != "Key not found" && !sccmValue.StartsWith("Error:"))
                    reasons.Add("SCCM");

                // Check CBS reboot
                string cbsPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing";
                string cbsValue = GetRemoteRegistryValue(pcName, cbsPath, "RebootPending");
                if (cbsValue != "Value not found" && cbsValue != "Key not found" && !cbsValue.StartsWith("Error:"))
                    reasons.Add("CBS");

                // Check File Rename Operations
                string filePath = @"SYSTEM\CurrentControlSet\Control\Session Manager";
                string fileValue = GetRemoteRegistryValue(pcName, filePath, "PendingFileRenameOperations");
                if (fileValue != "Value not found" && fileValue != "Key not found" && !fileValue.StartsWith("Error:"))
                    reasons.Add("File Rename");

                if (reasons.Count > 0)
                    return $"Yes - {string.Join(", ", reasons)}";
                
                return "No";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string GetNetworkConnectionType(string pcName)
        {
            try
            {
                var connections = new List<string>();
                
                // Method 1: Check network adapter configurations with detailed wireless detection
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT Description, MACAddress, IPEnabled, ServiceName FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var description = obj["Description"]?.ToString() ?? "";
                    var macAddress = obj["MACAddress"]?.ToString() ?? "";
                    var serviceName = obj["ServiceName"]?.ToString() ?? "";
                    
                    if (!string.IsNullOrEmpty(description))
                    {
                        // Enhanced wireless detection
                        bool isWifi = description.ToLower().Contains("wireless") ||
                                     description.ToLower().Contains("wifi") ||
                                     description.ToLower().Contains("wi-fi") ||
                                     description.ToLower().Contains("802.11") ||
                                     description.ToLower().Contains("wlan") ||
                                     serviceName.ToLower().Contains("wlan") ||
                                     serviceName.ToLower().Contains("wifi");
                        
                        string connectionType = isWifi ? "WiFi" : "LAN";
                        
                        // Get connection name from Win32_NetworkAdapter
                        try
                        {
                            using var adapterSearcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                                $"SELECT NetConnectionID FROM Win32_NetworkAdapter WHERE MACAddress = '{macAddress}'");
                            using var adapterCollection = adapterSearcher.Get();
                            
                            string connectionName = "Unknown";
                            foreach (var adapter in adapterCollection)
                            {
                                connectionName = adapter["NetConnectionID"]?.ToString() ?? description;
                                break;
                            }
                            
                            connections.Add($"{connectionName} ({connectionType})");
                        }
                        catch
                        {
                            connections.Add($"{description} ({connectionType})");
                        }
                    }
                }
                
                // Method 2: If no connections found, try alternative approach
                if (connections.Count == 0)
                {
                    using var altSearcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                        "SELECT NetConnectionID, Name, AdapterType FROM Win32_NetworkAdapter WHERE NetEnabled = True AND PhysicalAdapter = True");
                    
                    using var altCollection = altSearcher.Get();
                    foreach (var obj in altCollection)
                    {
                        var connectionId = obj["NetConnectionID"]?.ToString();
                        var name = obj["Name"]?.ToString() ?? "";
                        var adapterType = obj["AdapterType"]?.ToString() ?? "";
                        
                        if (!string.IsNullOrEmpty(connectionId))
                        {
                            bool isWifi = name.ToLower().Contains("wireless") ||
                                         name.ToLower().Contains("wifi") ||
                                         name.ToLower().Contains("wi-fi") ||
                                         name.ToLower().Contains("802.11") ||
                                         adapterType.ToLower().Contains("wireless");
                            
                            string connectionType = isWifi ? "WiFi" : "LAN";
                            connections.Add($"{connectionId} ({connectionType})");
                        }
                    }
                }
                
                return connections.Count > 0 
                    ? string.Join(", ", connections) 
                    : "No active connections";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string GetWiFiInfo(string pcName)
        {
            try
            {
                // Method 1: Try modern WMI approach for Windows 10/11
                try
                {
                    using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                        "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionStatus = 2 AND (Name LIKE '%wireless%' OR Name LIKE '%wifi%' OR Name LIKE '%wi-fi%' OR Name LIKE '%802.11%')");
                    
                    using var collection = searcher.Get();
                    if (collection.Count > 0)
                    {
                        // Found active wireless adapter, try to get SSID via netsh
                        try
                        {
                            var wifiInfo = GetWiFiInfoViaNetsh(pcName);
                            if (!wifiInfo.StartsWith("Error") && !wifiInfo.Contains("No WiFi"))
                                return wifiInfo;
                        }
                        catch { }
                    }
                }
                catch { }

                // Method 2: Try WMI MSNdis approach (works on some systems)
                try
                {
                    using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\WMI", 
                        "SELECT * FROM MSNdis_80211_ServiceSetIdentifier WHERE Active=true");
                    
                    using var collection = searcher.Get();
                    foreach (var obj in collection)
                    {
                        try
                        {
                            byte[] ssidBytes = (byte[])obj["Ssid"];
                            if (ssidBytes != null && ssidBytes.Length > 0)
                            {
                                string ssid = System.Text.Encoding.UTF8.GetString(ssidBytes).Trim('\0').Trim();
                                if (!string.IsNullOrWhiteSpace(ssid))
                                {
                                    // Try to get BSSID
                                    string bssid = "Unknown";
                                    try
                                    {
                                        if (obj["Bssid"] != null)
                                        {
                                            byte[] bssidBytes = (byte[])obj["Bssid"];
                                            if (bssidBytes != null && bssidBytes.Length >= 6)
                                            {
                                                bssid = BitConverter.ToString(bssidBytes, 0, 6).Replace("-", ":");
                                            }
                                        }
                                    }
                                    catch { }
                                    
                                    return $"SSID: {ssid}, BSSID: {bssid}";
                                }
                            }
                        }
                        catch { continue; }
                    }
                }
                catch { }

                // Method 3: Registry approach for connected networks
                try
                {
                    // Check for current profile in registry
                    string networkListPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles";
                    
                    // Use WMI to enumerate registry subkeys
                    var scope = new ManagementScope($"\\\\{pcName}\\root\\default");
                    scope.Connect();
                    using var reg = new ManagementClass(scope, new ManagementPath("StdRegProv"), new ObjectGetOptions());
                    
                    // Enumerate profile subkeys
                    using var inParams = reg.GetMethodParameters("EnumKey");
                    inParams["hDefKey"] = 0x80000002u; // HKLM
                    inParams["sSubKeyName"] = networkListPath.Replace("HKEY_LOCAL_MACHINE\\", "");
                    
                    using var outParams = reg.InvokeMethod("EnumKey", inParams, null);
                    if (Convert.ToInt32(outParams["ReturnValue"]) == 0)
                    {
                        string[]? subKeys = outParams["sNames"] as string[];
                        if (subKeys != null)
                        {
                            foreach (string subKey in subKeys)
                            {
                                try
                                {
                                    string profileName = GetRemoteRegistryValue(pcName, $"{networkListPath}\\{subKey}", "ProfileName");
                                    string category = GetRemoteRegistryValue(pcName, $"{networkListPath}\\{subKey}", "Category");
                                    
                                    // Category 0 = Public, 1 = Private, 2 = Domain - WiFi profiles are usually 0 or 1
                                    if (!profileName.StartsWith("Error") && !profileName.Contains("not found") && 
                                        (category == "0" || category == "1"))
                                    {
                                        return $"WiFi Profile: {profileName}";
                                    }
                                }
                                catch { continue; }
                            }
                        }
                    }
                }
                catch { }

                // Method 4: Check if any wireless adapters are present
                try
                {
                    using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                        "SELECT Name, NetConnectionStatus FROM Win32_NetworkAdapter WHERE (Name LIKE '%wireless%' OR Name LIKE '%wifi%' OR Name LIKE '%wi-fi%' OR Name LIKE '%802.11%')");
                    
                    using var collection = searcher.Get();
                    if (collection.Count > 0)
                    {
                        foreach (var obj in collection)
                        {
                            var status = obj["NetConnectionStatus"];
                            if (status != null && Convert.ToInt32(status) == 2) // Connected
                            {
                                return "WiFi adapter connected (SSID unavailable)";
                            }
                        }
                        return "WiFi adapter detected (not connected)";
                    }
                }
                catch { }
                
                return "No WiFi connection detected";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string GetWiFiInfoViaNetsh(string pcName)
        {
            try
            {
                // Use a more reliable temp directory
                string tempDir = pcName == Environment.MachineName ? @"C:\temp" : $@"\\{pcName}\C$\temp";
                string outputFile = Path.Combine(tempDir, $"wifi_info_{DateTime.Now:yyyyMMddHHmmss}.txt");
                
                // Ensure temp directory exists
                try
                {
                    if (pcName == Environment.MachineName)
                    {
                        Directory.CreateDirectory(@"C:\temp");
                    }
                    else
                    {
                        // For remote machines, create via WMI
                        var scope = new ManagementScope($"\\\\{pcName}\\root\\cimv2");
                        scope.Connect();
                        using var processClass = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                        
                        using var inParams = processClass.GetMethodParameters("Create");
                        inParams["CommandLine"] = "cmd.exe /c if not exist C:\\temp mkdir C:\\temp";
                        processClass.InvokeMethod("Create", inParams, null);
                        
                        System.Threading.Thread.Sleep(500); // Wait for directory creation
                    }
                }
                catch { }
                
                // Execute netsh command
                var scope2 = new ManagementScope($"\\\\{pcName}\\root\\cimv2");
                scope2.Connect();
                using var processClass2 = new ManagementClass(scope2, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                
                using var inParams2 = processClass2.GetMethodParameters("Create");
                inParams2["CommandLine"] = $"cmd.exe /c netsh wlan show interfaces > C:\\temp\\wifi_info_{DateTime.Now:yyyyMMddHHmmss}.txt";
                
                using var outParams = processClass2.InvokeMethod("Create", inParams2, null);
                int processId = Convert.ToInt32(outParams["ProcessId"]);
                
                // Wait for process completion
                System.Threading.Thread.Sleep(2000);
                
                // Read the output file
                string networkPath = pcName == Environment.MachineName ? outputFile : outputFile.Replace(@"C:\temp", $@"\\{pcName}\C$\temp");
                
                if (File.Exists(networkPath))
                {
                    string content = File.ReadAllText(networkPath);
                    
                    // Parse the output
                    string ssid = "Unknown";
                    string bssid = "Unknown";
                    string state = "Unknown";
                    
                    foreach (string line in content.Split('\n'))
                    {
                        string trimmedLine = line.Trim();
                        if (trimmedLine.StartsWith("SSID") && trimmedLine.Contains(":"))
                        {
                            ssid = trimmedLine.Split(':')[1].Trim();
                        }
                        else if (trimmedLine.StartsWith("BSSID") && trimmedLine.Contains(":"))
                        {
                            bssid = trimmedLine.Split(':')[1].Trim();
                        }
                        else if (trimmedLine.StartsWith("State") && trimmedLine.Contains(":"))
                        {
                            state = trimmedLine.Split(':')[1].Trim();
                        }
                    }
                    
                    // Clean up the temp file
                    try { File.Delete(networkPath); } catch { }
                    
                    if (!string.IsNullOrEmpty(ssid) && ssid != "Unknown" && state.ToLower().Contains("connected"))
                    {
                        return $"SSID: {ssid}, BSSID: {bssid}";
                    }
                    else if (state.ToLower().Contains("disconnected"))
                    {
                        return "WiFi adapter present but disconnected";
                    }
                }
                
                return "WiFi command executed but no data retrieved";
            }
            catch (Exception ex)
            {
                return $"Error executing netsh: {ex.Message}";
            }
        }

        // Batched WMI Query Methods for Better Performance (with Connection Pooling)
        private Models.SystemInformation GetSystemInformationBatch(ManagementScope scope)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT Manufacturer, Model, TotalPhysicalMemory, UserName FROM Win32_ComputerSystem"));
                searcher.Options.Timeout = TimeSpan.FromSeconds(30);
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var totalRAMBytes = Convert.ToDouble(obj["TotalPhysicalMemory"] ?? 0);
                    return new Models.SystemInformation
                    {
                        Make = obj["Manufacturer"]?.ToString() ?? "N/A",
                        Model = obj["Model"]?.ToString() ?? "N/A",
                        TotalRAM = FormatBytes(totalRAMBytes),
                        TotalRAMBytes = totalRAMBytes,
                        LoggedOnUser = obj["UserName"]?.ToString() ?? "N/A"
                    };
                }
                return new Models.SystemInformation();
            }
            catch (UnauthorizedAccessException)
            {
                return new Models.SystemInformation
                {
                    Make = "Access Denied", 
                    Model = "Access Denied", 
                    TotalRAM = "Access Denied",
                    TotalRAMBytes = -1,
                    LoggedOnUser = "Access Denied"
                };
            }
            catch (System.Management.ManagementException mgmtEx)
            {
                var errorMsg = GetFriendlyWMIError(mgmtEx);
                return new Models.SystemInformation
                {
                    Make = errorMsg, 
                    Model = errorMsg, 
                    TotalRAM = errorMsg,
                    TotalRAMBytes = -1,
                    LoggedOnUser = errorMsg
                };
            }
            catch (TimeoutException)
            {
                return new Models.SystemInformation
                {
                    Make = "Timeout", 
                    Model = "Timeout", 
                    TotalRAM = "Timeout",
                    TotalRAMBytes = -1,
                    LoggedOnUser = "Timeout"
                };
            }
            catch (Exception ex)
            {
                return new Models.SystemInformation
                {
                    Make = $"Error: {ex.Message}", 
                    Model = $"Error: {ex.Message}", 
                    TotalRAM = $"Error: {ex.Message}",
                    TotalRAMBytes = -1,
                    LoggedOnUser = $"Error: {ex.Message}"
                };
            }
        }
        
        private Models.StorageInformation GetStorageInformationBatch(ManagementScope scope)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT Size, FreeSpace FROM Win32_LogicalDisk WHERE DeviceID = 'C:'"));
                searcher.Options.Timeout = TimeSpan.FromSeconds(30);
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var sizeBytes = Convert.ToDouble(obj["Size"] ?? 0);
                    var freeSpaceBytes = Convert.ToDouble(obj["FreeSpace"] ?? 0);
                    return new Models.StorageInformation
                    {
                        HDDSize = FormatBytes(sizeBytes),
                        HDDSizeBytes = sizeBytes,
                        FreeSpace = FormatBytes(freeSpaceBytes),
                        FreeSpaceBytes = freeSpaceBytes
                    };
                }
                return new Models.StorageInformation { HDDSize = "N/A", FreeSpace = "N/A", HDDSizeBytes = -1, FreeSpaceBytes = -1 };
            }
            catch (UnauthorizedAccessException)
            {
                return new Models.StorageInformation
                {
                    HDDSize = "Access Denied",
                    FreeSpace = "Access Denied",
                    HDDSizeBytes = -1,
                    FreeSpaceBytes = -1
                };
            }
            catch (System.Management.ManagementException mgmtEx)
            {
                var errorMsg = GetFriendlyWMIError(mgmtEx);
                return new Models.StorageInformation
                {
                    HDDSize = errorMsg,
                    FreeSpace = errorMsg,
                    HDDSizeBytes = -1,
                    FreeSpaceBytes = -1
                };
            }
            catch (TimeoutException)
            {
                return new Models.StorageInformation
                {
                    HDDSize = "Timeout",
                    FreeSpace = "Timeout",
                    HDDSizeBytes = -1,
                    FreeSpaceBytes = -1
                };
            }
            catch (Exception ex)
            {
                return new Models.StorageInformation
                {
                    HDDSize = $"Error: {ex.Message}",
                    FreeSpace = $"Error: {ex.Message}",
                    HDDSizeBytes = -1,
                    FreeSpaceBytes = -1
                };
            }
        }

        private Models.NetworkInformation GetNetworkInformationBatch(ManagementScope scope, string pcName)
        {
            try
            {
                var result = new Models.NetworkInformation();
                
                // Get IP and MAC from network adapter configuration
                using var searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT IPAddress, MACAddress, Description FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True"));
                searcher.Options.Timeout = TimeSpan.FromSeconds(30);
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var ipAddresses = (string[])obj["IPAddress"];
                    if (ipAddresses != null && ipAddresses.Length > 0 && string.IsNullOrEmpty(result.IPAddress))
                    {
                        result.IPAddress = string.Join(", ", ipAddresses);
                    }
                    
                    var macAddress = obj["MACAddress"]?.ToString();
                    if (!string.IsNullOrEmpty(macAddress) && string.IsNullOrEmpty(result.MACAddress))
                    {
                        result.MACAddress = macAddress;
                    }
                }
                
                // Get connection type information
                if (_settings.CheckNetworkConnectionType)
                {
                    try
                    {
                        result.ConnectionType = GetNetworkConnectionType(pcName);
                    }
                    catch (Exception ex)
                    {
                        result.ConnectionType = $"Error determining connection type: {ex.Message}";
                    }
                }
                
                // Set defaults if nothing found
                if (string.IsNullOrEmpty(result.IPAddress)) result.IPAddress = "N/A";
                if (string.IsNullOrEmpty(result.MACAddress)) result.MACAddress = "N/A";
                if (string.IsNullOrEmpty(result.ConnectionType)) result.ConnectionType = "N/A";
                
                return result;
            }
            catch (UnauthorizedAccessException)
            {
                return new Models.NetworkInformation
                {
                    IPAddress = "Access Denied",
                    MACAddress = "Access Denied",
                    ConnectionType = "Access Denied"
                };
            }
            catch (System.Management.ManagementException mgmtEx)
            {
                var errorMsg = GetFriendlyWMIError(mgmtEx);
                return new Models.NetworkInformation
                {
                    IPAddress = errorMsg,
                    MACAddress = errorMsg,
                    ConnectionType = errorMsg
                };
            }
            catch (TimeoutException)
            {
                return new Models.NetworkInformation
                {
                    IPAddress = "Timeout",
                    MACAddress = "Timeout",
                    ConnectionType = "Timeout"
                };
            }
            catch (Exception ex)
            {
                return new Models.NetworkInformation
                {
                    IPAddress = $"Error: {ex.Message}",
                    MACAddress = $"Error: {ex.Message}",
                    ConnectionType = $"Error: {ex.Message}"
                };
            }
        }

        private Models.BIOSInformation GetBIOSInformationBatch(ManagementScope scope)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT SMBIOSBIOSVersion, SerialNumber FROM Win32_BIOS"));
                searcher.Options.Timeout = TimeSpan.FromSeconds(30);
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return new Models.BIOSInformation
                    {
                        BIOSVersion = obj["SMBIOSBIOSVersion"]?.ToString() ?? "N/A",
                        SerialNumber = obj["SerialNumber"]?.ToString() ?? "N/A"
                    };
                }
                return new Models.BIOSInformation { BIOSVersion = "N/A", SerialNumber = "N/A" };
            }
            catch (UnauthorizedAccessException)
            {
                return new Models.BIOSInformation
                {
                    BIOSVersion = "Access Denied",
                    SerialNumber = "Access Denied"
                };
            }
            catch (System.Management.ManagementException mgmtEx)
            {
                var errorMsg = GetFriendlyWMIError(mgmtEx);
                return new Models.BIOSInformation
                {
                    BIOSVersion = errorMsg,
                    SerialNumber = errorMsg
                };
            }
            catch (TimeoutException)
            {
                return new Models.BIOSInformation
                {
                    BIOSVersion = "Timeout",
                    SerialNumber = "Timeout"
                };
            }
            catch (Exception ex)
            {
                return new Models.BIOSInformation
                {
                    BIOSVersion = $"Error: {ex.Message}",
                    SerialNumber = $"Error: {ex.Message}"
                };
            }
        }

    private Models.OperatingSystemInformation GetOperatingSystemInformationBatch(ManagementScope scope, string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT Caption, Version, LastBootUpTime, InstallDate FROM Win32_OperatingSystem"));
                searcher.Options.Timeout = TimeSpan.FromSeconds(30);
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var caption = obj["Caption"]?.ToString() ?? string.Empty;
                    var version = obj["Version"]?.ToString() ?? string.Empty;
                    var windowsVersion = $"{caption} ({version})";

                    var lastRebootTime = "N/A";
                    try
                    {
                        if (obj["LastBootUpTime"] != null)
                        {
                            var lastBootUpTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                            var daysAgo = (DateTime.Now - lastBootUpTime).Days;
                            lastRebootTime = $"{daysAgo} days ago ({lastBootUpTime})";
                        }
                    }
                    catch (Exception ex)
                    {
                        lastRebootTime = $"Error parsing last boot time: {ex.Message}";
                    }

                    var installDate = string.Empty;
                    try
                    {
                        var rawInstall = obj["InstallDate"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(rawInstall))
                        {
                            var installDateTime = ManagementDateTimeConverter.ToDateTime(rawInstall);
                            installDate = installDateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    catch (Exception ex)
                    {
                        installDate = $"Error parsing install date: {ex.Message}";
                    }

                    if (string.IsNullOrWhiteSpace(installDate) || installDate.Equals("N/A", StringComparison.OrdinalIgnoreCase) || installDate.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                    {
                        installDate = GetInstallDateFromRegistry(pcName);
                    }

                    var info = new Models.OperatingSystemInformation
                    {
                        WindowsVersion = windowsVersion,
                        LastRebootTime = lastRebootTime,
                        InstallDate = installDate
                    };

                    if (string.IsNullOrWhiteSpace(info.InstallDate))
                    {
                        info.InstallDate = GetInstallDateFromRegistry(pcName);
                    }

                    return info;
                }
                var fallbackInfo = new Models.OperatingSystemInformation
                {
                    WindowsVersion = "N/A",
                    LastRebootTime = "N/A",
                    InstallDate = GetInstallDateFromRegistry(pcName)
                };
                return fallbackInfo;
            }
            catch (UnauthorizedAccessException)
            {
                return new Models.OperatingSystemInformation
                {
                    WindowsVersion = "Access Denied",
                    LastRebootTime = "Access Denied",
                    InstallDate = "Access Denied"
                };
            }
            catch (System.Management.ManagementException mgmtEx)
            {
                var errorMsg = GetFriendlyWMIError(mgmtEx);
                return new Models.OperatingSystemInformation
                {
                    WindowsVersion = errorMsg,
                    LastRebootTime = errorMsg,
                    InstallDate = errorMsg
                };
            }
            catch (TimeoutException)
            {
                return new Models.OperatingSystemInformation
                {
                    WindowsVersion = "Timeout",
                    LastRebootTime = "Timeout",
                    InstallDate = "Timeout"
                };
            }
            catch (Exception ex)
            {
                return new Models.OperatingSystemInformation
                {
                    WindowsVersion = $"Error: {ex.Message}",
                    LastRebootTime = $"Error: {ex.Message}",
                    InstallDate = $"Error: {ex.Message}"
                };
            }
        }

        // Error handling helper methods
        private string GetDetailedPingStatus(IPStatus status)
        {
            return status switch
            {
                IPStatus.TimedOut => "Unreachable: Request timed out",
                IPStatus.DestinationHostUnreachable => "Unreachable: Destination host unreachable",
                IPStatus.DestinationNetworkUnreachable => "Unreachable: Destination network unreachable",
                IPStatus.DestinationUnreachable => "Unreachable: Destination unreachable",
                IPStatus.PacketTooBig => "Unreachable: Packet too big",
                IPStatus.TtlExpired => "Unreachable: TTL expired",
                IPStatus.BadRoute => "Unreachable: Bad route",
                IPStatus.BadHeader => "Unreachable: Bad header",
                IPStatus.BadOption => "Unreachable: Bad option",
                IPStatus.HardwareError => "Unreachable: Hardware error",
                IPStatus.IcmpError => "Unreachable: ICMP error",
                _ => $"Unreachable: {status}"
            };
        }

        private string GetFriendlyWMIError(System.Management.ManagementException mgmtEx)
        {
            return mgmtEx.ErrorCode switch
            {
                ManagementStatus.AccessDenied => "Access denied - check permissions",
                ManagementStatus.InvalidNamespace => "Invalid WMI namespace",
                ManagementStatus.InvalidClass => "Invalid WMI class",
                ManagementStatus.InvalidQuery => "Invalid WMI query",
                ManagementStatus.ProviderFailure => "WMI provider failure",
                ManagementStatus.NotSupported => "Operation not supported",
                ManagementStatus.OutOfMemory => "Out of memory",
                ManagementStatus.TransportFailure => "Network transport failure",
                _ => $"WMI Error: {mgmtEx.Message} (Code: {mgmtEx.ErrorCode})"
            };
        }

        private string GetFriendlyCOMError(System.Runtime.InteropServices.COMException comEx)
        {
            return comEx.HResult switch
            {
                unchecked((int)0x800706BA) => "RPC server unavailable - check if target PC is accessible",
                unchecked((int)0x80070005) => "Access denied - check credentials and permissions",
                unchecked((int)0x80041003) => "Access denied to WMI namespace",
                unchecked((int)0x80041010) => "Invalid WMI class",
                unchecked((int)0x80041017) => "Invalid WMI query syntax",
                unchecked((int)0x80041006) => "WMI service not available",
                unchecked((int)0x800401E4) => "COM object not registered",
                unchecked((int)0x80070002) => "System cannot find the specified file",
                unchecked((int)0x8007052E) => "Logon failure - check credentials",
                _ => $"COM Error: {comEx.Message} (HRESULT: 0x{comEx.HResult:X8})"
            };
        }
    }
}