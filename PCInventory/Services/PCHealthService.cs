using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using PCInventory.Models;

namespace PCInventory.Services
{
    public class PCHealthService
    {
        private readonly AppSettings _settings;
        private readonly bool _allowWmiFallback = true; // Always allow fallback since running as admin

        public PCHealthService(AppSettings settings)
        {
            _settings = settings;
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
                    // Basic connectivity check
                    using Ping ping = new Ping();
                    PingReply reply = ping.Send(pcName);
                    if (reply.Status != IPStatus.Success)
                    {
                        pcInfo.Status = "Unreachable";
                        return;
                    }

                    pcInfo.Status = "Gathering data...";
                    
                    // Only run the checks that are enabled in settings
                    try
                    {
                        if (_settings.CheckIPAddress)
                            pcInfo.IPAddress = GetIPAddress(pcName);
                        
                        if (_settings.CheckMACAddress)
                            pcInfo.MACAddress = GetMACAddress(pcName);
                        
                        if (_settings.CheckHDDSize)
                            pcInfo.HDDSize = GetHDDSize(pcName);
                        
                        if (_settings.CheckFreeHDDSpace)
                            pcInfo.FreeHDDSpace = GetFreeHDDSpace(pcName);
                        
                        if (_settings.CheckTotalRAM)
                            pcInfo.TotalRAM = GetTotalRAM(pcName);
                        
                        if (_settings.CheckLoggedOnUser)
                            pcInfo.LoggedOnUser = GetLoggedOnUser(pcName);
                        
                        if (_settings.CheckLastRebootTime)
                            pcInfo.LastRebootTime = GetLastRebootTime(pcName);
                        
                        if (_settings.CheckMake)
                            pcInfo.Make = GetMake(pcName);
                        
                        if (_settings.CheckModel)
                            pcInfo.Model = GetModel(pcName);
                        
                        if (_settings.CheckBIOSVersion)
                            pcInfo.BIOSVersion = GetBIOSVersion(pcName);
                        
                        if (_settings.CheckWindowsVersion)
                            pcInfo.WindowsVersion = GetWindowsVersion(pcName);
                        
                        if (_settings.CheckSerialNumber)
                            pcInfo.SerialNumber = GetSerialNumber(pcName);
                        
                        if (_settings.CheckPendingReboot)
                        {
                            pcInfo.PendingRebootStatus = CheckPendingReboot(pcName);
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
                    }
                    catch (Exception ex)
                    {
                        pcInfo.Status = $"Error: {ex.Message}";
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

        private string GetIPAddress(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT IPAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var ipAddresses = (string[])obj["IPAddress"];
                    if (ipAddresses != null && ipAddresses.Length > 0)
                        return string.Join(", ", ipAddresses);
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetMACAddress(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT MACAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var macAddress = obj["MACAddress"]?.ToString();
                    if (!string.IsNullOrEmpty(macAddress))
                        return macAddress;
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetHDDSize(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT Size FROM Win32_LogicalDisk WHERE DeviceID = 'C:'");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var size = Convert.ToDouble(obj["Size"]);
                    return FormatBytes(size);
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetFreeHDDSpace(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT FreeSpace FROM Win32_LogicalDisk WHERE DeviceID = 'C:'");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var freeSpace = Convert.ToDouble(obj["FreeSpace"]);
                    return FormatBytes(freeSpace);
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetTotalRAM(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var totalRam = Convert.ToDouble(obj["TotalPhysicalMemory"]);
                    return FormatBytes(totalRam);
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetLoggedOnUser(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT UserName FROM Win32_ComputerSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return obj["UserName"]?.ToString() ?? "N/A";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetLastRebootTime(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT LastBootUpTime FROM Win32_OperatingSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var lastBootUpTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    var daysAgo = (DateTime.Now - lastBootUpTime).Days;
                    return $"{daysAgo} days ago ({lastBootUpTime})";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetMake(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT Manufacturer FROM Win32_ComputerSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return obj["Manufacturer"]?.ToString() ?? "N/A";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetModel(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT Model FROM Win32_ComputerSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return obj["Model"]?.ToString() ?? "N/A";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetBIOSVersion(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT SMBIOSBIOSVersion FROM Win32_BIOS");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return obj["SMBIOSBIOSVersion"]?.ToString() ?? "N/A";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetWindowsVersion(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT Caption, Version FROM Win32_OperatingSystem");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    var caption = obj["Caption"]?.ToString() ?? string.Empty;
                    var version = obj["Version"]?.ToString() ?? string.Empty;
                    return $"{caption} ({version})";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
            }
        }

        private string GetSerialNumber(string pcName)
        {
            try
            {
                using var searcher = new ManagementObjectSearcher($"\\\\{pcName}\\root\\cimv2", 
                    "SELECT SerialNumber FROM Win32_BIOS");
                
                using var collection = searcher.Get();
                foreach (var obj in collection)
                {
                    return obj["SerialNumber"]?.ToString() ?? "N/A";
                }
                return "N/A";
            }
            catch
            {
                return "Error";
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
                catch (Exception regEx)
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
            // Round to whole numbers for better sorting
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
    }
}