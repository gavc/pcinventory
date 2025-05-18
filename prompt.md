# Windows Forms C# .NET 8 - Remote PC Health Checker

## Overview
A simple Windows Forms application to remotely check the health and system values of multiple PCs, using multi-threaded, non-blocking operations.

## Features
1. **Remote System Health Check**: Gather system information from remote PCs.
2. **Batch Processing**: Read multiple PC names/IP addresses from a file.
3. **Multi-threaded Non-blocking Execution**: Ensure responsiveness during data retrieval.
4. **Enumerated Data**:
   - PC Name
   - HDD Size
   - Free HDD Space
   - Total RAM
   - IP Address
   - MAC Address
   - Logged-on User
   - Last Reboot Time (Days Ago)
   - Make
   - Model
   - BIOS Version
   - Windows Version
   - Serial Number
5. **Remote Registry Check**:
   - User-defined friendly name for results (Column Header)
   - Custom Key Path & Value Name input for scanning registry values
6. **Simple Menu**:
   - File
   - Import (Load PC List for Scanning)
   - Export (Save Results to CSV)
   - Settings
   - Exit
7. **Settings Page**:
   - Enable/Disable specific checks via checkboxes
   - Input section for registry keys to be scanned
8. **Status Strip**:
   - Status Strip to show current status/progress and any other necessary messages

## Technical Requirements
- **Framework**: .NET 8
- **UI Framework**: Windows Forms
- **Concurrency**: Multi-threading with Task Parallel Library (TPL)
- **File Handling**: Read/write operations for import/export
- **Networking**: Remote PC health data retrieval using WMI
- **Registry Access**: Query registry values remotely

## Expected Behavior
- Users should be able to load a list of PCs from a file.
- The app should process each machine asynchronously to avoid UI freezing.
- Retrieved results should be displayed in a grid view.
- Users should have the ability to export results as a CSV file.
- The settings page should allow custom configuration for registry scans and health check toggling.

## Development Guidelines
- Use **async/await** for non-blocking operations.
- Implement exception handling to manage remote connectivity failures.
- Utilize **DataGridView** for displaying results dynamically.
- Leverage **BackgroundWorker** or **Task.Run** for executing tasks efficiently.

---

### Notes:
- Consider providing logging for better diagnostics.
- Ensure error handling for inaccessible remote PCs.
- Provide a user-friendly way to modify registry paths.