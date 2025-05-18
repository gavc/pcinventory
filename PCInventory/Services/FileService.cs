using System.Text;
using PCInventory.Models;

namespace PCInventory.Services
{
    public class FileService
    {
        public List<string> ImportPCList(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file was not found.", filePath);

            var pcNames = new List<string>();
            using var reader = new StreamReader(filePath);
            string? line;
            
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    pcNames.Add(line.Trim());
            }

            return pcNames;
        }

        public void ExportToCSV(List<PCInfo> pcInfoList, string filePath, AppSettings settings)
        {
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
            
            // Add custom registry check headers
            foreach (var regCheck in settings.RegistryChecks.Where(rc => rc.Enabled))
            {
                headers.Add(regCheck.FriendlyName);
            }
            
            writer.WriteLine(string.Join(",", headers.Select(EscapeCSV)));
            
            // Write data rows
            foreach (var pcInfo in pcInfoList)
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
            var json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, json);
        }

        public AppSettings LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
                return new AppSettings();
                
            var json = File.ReadAllText(filePath);
            return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
    }
}