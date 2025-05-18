namespace PCInventory.Models
{
    public class PCInfo
    {
        public string PCName { get; set; } = string.Empty;
        public string HDDSize { get; set; } = string.Empty;
        public string FreeHDDSpace { get; set; } = string.Empty;
        public string TotalRAM { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string MACAddress { get; set; } = string.Empty;
        public string LoggedOnUser { get; set; } = string.Empty;
        public string LastRebootTime { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string BIOSVersion { get; set; } = string.Empty;
        public string WindowsVersion { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string PendingRebootStatus { get; set; } = string.Empty;
        public Dictionary<string, string> CustomRegistryValues { get; set; } = new Dictionary<string, string>();

        // Status for UI updates
        public string Status { get; set; } = "Not Started";
    }
}