# PC Inventory

A comprehensive Windows Forms application for gathering and managing PC inventory information across multiple computers in a network environment.

## 🎯 Overview

PC Inventory is a .NET 8.0 Windows application that collects detailed system information from multiple computers simultaneously, providing IT administrators and system managers with a centralized view of their hardware and software inventory.

## ✨ Features

### System Information Collection
- **Hardware Details**: CPU, RAM, storage capacity and free space, BIOS version
- **Network Information**: IP addresses, MAC addresses, connection types, WiFi details
- **System Status**: Current logged-on user, last reboot time, pending reboot status
- **Computer Identity**: Make, model, serial number, Windows version

### Advanced Capabilities
- **Batch Processing**: Query multiple computers simultaneously with progress tracking
- **Custom Registry Checks**: Add custom registry keys to monitor specific configurations
- **Export Functionality**: Save inventory data to CSV files for reporting and analysis
- **Smart Sorting**: Proper numeric sorting for storage and memory values
- **Real-time Status**: Live progress updates during data collection

### User Interface
- **Intuitive Design**: Clean Windows Forms interface with DataGridView for data display
- **Settings Management**: Configurable timeout settings and custom registry monitoring
- **Logging System**: Comprehensive logging with automatic cleanup of old log files
- **Error Handling**: Robust error handling with detailed status reporting

## 🚀 Getting Started

### Prerequisites
- Windows 10/11 or Windows Server 2016+
- .NET 8.0 Runtime
- Administrative privileges (recommended for full WMI access)
- Network access to target computers

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/gavc/pcinventory.git
   cd pcinventory
   ```

2. **Build the application:**
   ```bash
   dotnet build --configuration Release
   ```

3. **Run the application:**
   ```bash
   dotnet run --project PCInventory
   ```

   Or execute the built executable:
   ```bash
   .\PCInventory\bin\Release\net8.0-windows\PCInventory.exe
   ```

### Quick Start

1. **Launch the application** and you'll see the main inventory interface
2. **Add computers** by entering hostnames or IP addresses in the input field
3. **Configure settings** (optional) via the Settings menu for timeouts and custom registry checks
4. **Start inventory collection** by clicking "Get PC Info" to gather data from all added computers
5. **Export results** to CSV for reporting and analysis

## 🏗️ Architecture

### Project Structure
```
PCInventory/
├── Models/
│   ├── AppSettings.cs          # Application configuration
│   ├── BatchedHealthInfo.cs    # Batch processing data model
│   └── PCInfo.cs              # Core PC information model
├── Services/
│   ├── FileService.cs         # File I/O operations
│   ├── LoggingService.cs      # Application logging
│   └── PCHealthService.cs     # WMI data collection
├── Forms/
│   ├── Form1.cs              # Main application interface
│   ├── SettingsForm.cs       # Settings configuration
│   └── AddRegistryCheckForm.cs # Custom registry check setup
└── Program.cs                # Application entry point
```

### Key Components

- **PCHealthService**: Core service for WMI-based system information retrieval
- **FileService**: Handles settings persistence and CSV export functionality
- **LoggingService**: Manages application logging with automatic cleanup
- **PCInfo Model**: Comprehensive data model with both display and raw values for sorting

## 🔧 Configuration

### Application Settings
Settings are automatically saved to `%APPDATA%\PCInventory\settings.json`:

```json
{
  "TimeoutSeconds": 30,
  "CustomRegistryChecks": [
    {
      "Name": "Custom Setting",
      "RegistryPath": "HKEY_LOCAL_MACHINE\\SOFTWARE\\Company\\Product",
      "ValueName": "Version"
    }
  ]
}
```

### Custom Registry Monitoring
Add custom registry checks through the Settings menu to monitor:
- Software versions
- Configuration values
- License information
- Custom deployment settings

## 📊 Data Collection Details

### Collected Information
| Category | Data Points |
|----------|-------------|
| **Hardware** | CPU info, Total RAM, HDD size/free space, Make/Model, Serial number |
| **System** | Windows version, BIOS version, Last reboot, Pending reboot status |
| **Network** | IP address, MAC address, Connection type, WiFi details |
| **User** | Currently logged-on user |
| **Custom** | User-defined registry values |

### Performance
- **Concurrent Processing**: Queries multiple computers simultaneously
- **Timeout Handling**: Configurable timeouts prevent hanging on unresponsive systems
- **Progress Tracking**: Real-time status updates during data collection
- **Error Resilience**: Continues processing even if individual computers fail

## 💾 Export and Reporting

### CSV Export Features
- **Complete Data Export**: All collected information in structured format
- **Custom Columns**: Includes both display-friendly and raw values
- **UTF-8 Encoding**: Proper handling of special characters
- **Date Stamping**: Automatic timestamp inclusion for tracking

### Export Format
The CSV export includes columns for all collected data points, making it easy to:
- Import into Excel or database systems
- Generate reports and analytics
- Track changes over time
- Audit system configurations

## 🐛 Troubleshooting

### Common Issues

**"Access Denied" Errors**
- Ensure the application is run with administrative privileges
- Verify WMI access permissions on target computers
- Check Windows Firewall settings

**Network Connectivity Issues**
- Verify network connectivity to target computers
- Ensure WMI services are running on target systems
- Check for network firewall blocking WMI traffic

**Slow Performance**
- Reduce timeout values in settings for faster processing
- Process computers in smaller batches
- Ensure target computers are responsive

### Logging
Application logs are stored in `%APPDATA%\PCInventory\Logs\` with automatic cleanup of logs older than 7 days.

## 🛠️ Development

### Building from Source
```bash
# Clone the repository
git clone https://github.com/gavc/pcinventory.git
cd pcinventory

# Restore dependencies
dotnet restore

# Build the application
dotnet build

# Run in development mode
dotnet run --project PCInventory
```

### Technology Stack
- **.NET 8.0**: Modern .NET framework with Windows Forms
- **System.Management**: WMI access for system information retrieval
- **Windows Forms**: Native Windows UI framework
- **C# 12**: Latest C# language features

### Contributing
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -am 'Add new feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Create a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Support

For support, bug reports, or feature requests, please:
1. Check the [Issues](https://github.com/gavc/pcinventory/issues) section
2. Create a new issue with detailed information
3. Include log files and system information for bug reports

## 🔄 Version History

### Version 0.2.0 (Performance Release - October 9, 2025)
- **Major performance improvements: 30-40% faster PC scanning**
- Connection pooling implementation (single WMI connection per PC)
- Reduced ping timeout from 5s to 2s (60% faster offline detection)
- Removed 250 lines of dead code (18% smaller codebase)
- Enhanced error handling and logging
- Comprehensive performance optimization documentation

### Version 0.1.0 (Initial Release)
- Enhanced numeric sorting for storage and memory values
- Improved error handling and logging
- Custom registry check functionality
- Batch processing with progress tracking

---

**PC Inventory** - Streamlining IT asset management, one computer at a time.
