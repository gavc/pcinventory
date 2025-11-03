namespace PCInventory.Models
{
    public class SystemInformation
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string TotalRAM { get; set; } = string.Empty;
        public string LoggedOnUser { get; set; } = string.Empty;
        public double TotalRAMBytes { get; set; } = 0;
    }

    public class StorageInformation
    {
        public string HDDSize { get; set; } = string.Empty;
        public string FreeSpace { get; set; } = string.Empty;
        public double HDDSizeBytes { get; set; } = 0;
        public double FreeSpaceBytes { get; set; } = 0;
    }

    public class NetworkInformation
    {
        public string IPAddress { get; set; } = string.Empty;
        public string MACAddress { get; set; } = string.Empty;
        public string ConnectionType { get; set; } = string.Empty;
    }

    public class BIOSInformation
    {
        public string BIOSVersion { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
    }

    public class OperatingSystemInformation
    {
        public string WindowsVersion { get; set; } = string.Empty;
        public string LastRebootTime { get; set; } = string.Empty;
        public string InstallDate { get; set; } = string.Empty;
    }
}
