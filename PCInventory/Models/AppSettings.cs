namespace PCInventory.Models
{
    public class AppSettings
    {
        public bool CheckHDDSize { get; set; } = true;
        public bool CheckFreeHDDSpace { get; set; } = true;
        public bool CheckTotalRAM { get; set; } = true;
        public bool CheckIPAddress { get; set; } = true;
        public bool CheckMACAddress { get; set; } = true;
        public bool CheckLoggedOnUser { get; set; } = true;
        public bool CheckLastRebootTime { get; set; } = true;
        public bool CheckMake { get; set; } = true;
        public bool CheckModel { get; set; } = true;
        public bool CheckBIOSVersion { get; set; } = true;
        public bool CheckWindowsVersion { get; set; } = true;
        public bool CheckSerialNumber { get; set; } = true;
        public bool CheckPendingReboot { get; set; } = true;

        // Registry Check settings
        public List<RegistryCheckSetting> RegistryChecks { get; set; } = new List<RegistryCheckSetting>();
    }

    public class RegistryCheckSetting
    {
        public string FriendlyName { get; set; } = string.Empty;
        public string KeyPath { get; set; } = string.Empty;
        public string ValueName { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
    }
}