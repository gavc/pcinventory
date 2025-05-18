using PCInventory.Models;
using PCInventory.Services;
using System.ComponentModel;
using System.Text;

namespace PCInventory;

public partial class Form1 : Form
{
    private List<string> _pcList = new List<string>();
    private List<PCInfo> _pcInfoList = new List<PCInfo>();
    private AppSettings _settings = new AppSettings();
    private FileService _fileService = new FileService();
    private PCHealthService? _healthService;
    private CancellationTokenSource? _cancellationTokenSource;
    private string _settingsFilePath;

    public Form1()
    {
        InitializeComponent();
        _settingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PCInventory",
            "settings.json");
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        try
        {
            // Ensure settings directory exists
            string? settingsDir = Path.GetDirectoryName(_settingsFilePath);
            if (!string.IsNullOrEmpty(settingsDir) && !Directory.Exists(settingsDir))
                Directory.CreateDirectory(settingsDir);

            // Load settings if available
            if (File.Exists(_settingsFilePath))
                _settings = _fileService.LoadSettings(_settingsFilePath);

            // Initialize health service with settings
            _healthService = new PCHealthService(_settings);
            
            // Configure DataGridView context menu
            dataGridView.ContextMenuStrip = gridContextMenu;
            dataGridView.CellMouseDown += DataGridView_CellMouseDown;
            
            // Setup initial DataGridView columns
            SetupDataGridViewColumns();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _settings = new AppSettings();
            _healthService = new PCHealthService(_settings);
        }
    }

    private void SetupDataGridViewColumns()
    {
        dataGridView.Columns.Clear();
        
        // Add the base columns
        dataGridView.Columns.Add("colPCName", "PC Name");
        dataGridView.Columns.Add("colStatus", "Status");
        
        // Add columns based on enabled health checks
        if (_settings.CheckHDDSize)
            dataGridView.Columns.Add("colHDDSize", "HDD Size");
            
        if (_settings.CheckFreeHDDSpace)
            dataGridView.Columns.Add("colFreeHDDSpace", "Free HDD Space");
            
        if (_settings.CheckTotalRAM)
            dataGridView.Columns.Add("colTotalRAM", "Total RAM");
            
        if (_settings.CheckIPAddress)
            dataGridView.Columns.Add("colIPAddress", "IP Address");
            
        if (_settings.CheckMACAddress)
            dataGridView.Columns.Add("colMACAddress", "MAC Address");
            
        if (_settings.CheckLoggedOnUser)
            dataGridView.Columns.Add("colLoggedOnUser", "Logged-on User");
            
        if (_settings.CheckLastRebootTime)
            dataGridView.Columns.Add("colLastRebootTime", "Last Reboot Time");
            
        if (_settings.CheckMake)
            dataGridView.Columns.Add("colMake", "Make");
            
        if (_settings.CheckModel)
            dataGridView.Columns.Add("colModel", "Model");
            
        if (_settings.CheckBIOSVersion)
            dataGridView.Columns.Add("colBIOSVersion", "BIOS Version");
            
        if (_settings.CheckWindowsVersion)
            dataGridView.Columns.Add("colWindowsVersion", "Windows Version");
            
        if (_settings.CheckSerialNumber)
            dataGridView.Columns.Add("colSerialNumber", "Serial Number");

        if (_settings.CheckPendingReboot)
        {
            dataGridView.Columns.Add("colPendingReboot", "Pending Reboot");
        }
            
        // Add registry check columns
        foreach (var regCheck in _settings.RegistryChecks.Where(rc => rc.Enabled))
        {
            string columnName = $"colReg_{regCheck.FriendlyName.Replace(" ", "_")}";
            dataGridView.Columns.Add(columnName, regCheck.FriendlyName);
        }

        // Set column properties
        foreach (DataGridViewColumn col in dataGridView.Columns)
        {
            col.ReadOnly = true;
        }
    }

    private void scanSinglePCToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Create an input dialog to get PC name
        string pcName = Microsoft.VisualBasic.Interaction.InputBox(
            "Enter the PC name or IP address to scan:", 
            "Scan Single PC", 
            Environment.MachineName, // Default to local machine name
            -1, -1);
        
        // Check if the user cancelled or entered an empty value
        if (string.IsNullOrWhiteSpace(pcName))
            return;
            
        // Setup the grid view if needed
        SetupDataGridViewColumns();
            
        // Clear existing data and add this PC
        dataGridView.Rows.Clear();
        _pcInfoList.Clear();
        _pcList = new List<string> { pcName };
        
        // Add row for the PC
        int rowIndex = dataGridView.Rows.Add();
        dataGridView.Rows[rowIndex].Cells["colPCName"].Value = pcName;
        dataGridView.Rows[rowIndex].Cells["colStatus"].Value = "Waiting...";
        
        // Start scanning
        ScanSelectedPCs();
    }
    
    private async void ScanSelectedPCs()
    {
        // Update UI for scanning
        SetUIForScanning(true);
        
        try
        {
            // Create cancellation token source
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Create tasks for all PCs
            var tasks = new List<Task<PCInfo>>();
            foreach (var pcName in _pcList)
            {
                tasks.Add(Task.Run(async () => 
                {
                    if (_healthService != null)
                    {
                        var pcInfo = await _healthService.GetPCHealthInfoAsync(pcName);
                        UpdatePCStatus(pcInfo);
                        return pcInfo;
                    }
                    else
                    {
                        var pcInfo = new PCInfo { PCName = pcName, Status = "Error: Health service not initialized" };
                        UpdatePCStatus(pcInfo);
                        return pcInfo;
                    }
                }, token));
            }

            // Wait for all tasks or cancellation
            try
            {
                // Use Task.WhenAll to wait for all tasks but handle each individually for better status reporting
                var completionTask = Task.WhenAll(tasks);
                
                try
                {
                    await completionTask;
                    _pcInfoList = tasks.Select(t => t.Result).ToList();
                }
                catch (Exception)
                {
                    // Some tasks failed, but we still want to process all completed ones
                    _pcInfoList = tasks.Where(t => t.IsCompleted && !t.IsFaulted).Select(t => t.Result).ToList();
                    
                    // Ensure all tasks get final status update
                    foreach (var task in tasks)
                    {
                        if (task.IsFaulted && task.Exception != null)
                        {
                            // Find the corresponding row and update status
                            var pcName = _pcList[tasks.IndexOf(task)];
                            UpdatePCStatus(new PCInfo { 
                                PCName = pcName, 
                                Status = "Error: Task failed" 
                            });
                        }
                    }
                }
                
                toolStripStatusLabel.Text = "Scan completed";
                exportToolStripMenuItem.Enabled = _pcInfoList.Count > 0;
            }
            catch (OperationCanceledException)
            {
                toolStripStatusLabel.Text = "Scan cancelled";
                
                // Make sure all remaining "Waiting..." statuses are updated
                foreach (var pcName in _pcList)
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        if (dataGridView.Rows[i].Cells["colPCName"].Value?.ToString() == pcName &&
                            dataGridView.Rows[i].Cells["colStatus"].Value?.ToString() == "Waiting...")
                        {
                            dataGridView.Rows[i].Cells["colStatus"].Value = "Cancelled";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error during scanning: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Ensure all statuses are updated before completing
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Cells["colStatus"].Value?.ToString() == "Waiting...")
                {
                    var pcName = dataGridView.Rows[i].Cells["colPCName"].Value?.ToString();
                    dataGridView.Rows[i].Cells["colStatus"].Value = "Not scanned";
                }
            }
            
            SetUIForScanning(false);
        }
    }

    private void btnScan_Click(object sender, EventArgs e)
    {
        if (_pcList.Count == 0)
        {
            MessageBox.Show("Please import a list of PCs first.", "No PC List", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Clear the grid and populate with PC names and waiting status
        dataGridView.Rows.Clear();
        foreach (var pcName in _pcList)
        {
            int rowIndex = dataGridView.Rows.Add();
            dataGridView.Rows[rowIndex].Cells["colPCName"].Value = pcName;
            dataGridView.Rows[rowIndex].Cells["colStatus"].Value = "Waiting...";
        }
        
        // Start scanning
        ScanSelectedPCs();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
        toolStripStatusLabel.Text = "Cancelling scan...";
    }

    private void importToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            _pcList = _fileService.ImportPCList(openFileDialog.FileName);
            
            // Update DataGridView with PC list
            dataGridView.Rows.Clear();
            foreach (var pcName in _pcList)
            {
                int rowIndex = dataGridView.Rows.Add();
                dataGridView.Rows[rowIndex].Cells["colPCName"].Value = pcName;
                dataGridView.Rows[rowIndex].Cells["colStatus"].Value = "Not Started";
            }

            toolStripStatusLabel.Text = $"Loaded {_pcList.Count} PC(s) from {Path.GetFileName(openFileDialog.FileName)}";
            btnScan.Enabled = _pcList.Count > 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error importing PC list: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void exportToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_pcInfoList.Count == 0)
        {
            MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (saveFileDialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            _fileService.ExportToCSV(_pcInfoList, saveFileDialog.FileName, _settings);
            toolStripStatusLabel.Text = $"Results exported to {Path.GetFileName(saveFileDialog.FileName)}";
            
            if (MessageBox.Show(
                "Results exported successfully. Would you like to open the file?", 
                "Export Successful", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("explorer.exe", saveFileDialog.FileName);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting results: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var settingsForm = new SettingsForm(_settings);
        if (settingsForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                // Save settings
                _fileService.SaveSettings(_settings, _settingsFilePath);
                
                // Update health service with new settings
                _healthService = new PCHealthService(_settings);
                
                // Update grid view columns based on new settings
                SetupDataGridViewColumns();
                
                // Reload data with new columns if we have any
                if (_pcInfoList.Count > 0)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pcInfo in _pcInfoList)
                    {
                        AddOrUpdateRow(pcInfo);
                    }
                }
                else if (_pcList.Count > 0)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pcName in _pcList)
                    {
                        int rowIndex = dataGridView.Rows.Add();
                        dataGridView.Rows[rowIndex].Cells["colPCName"].Value = pcName;
                        dataGridView.Rows[rowIndex].Cells["colStatus"].Value = "Not Started";
                    }
                }
                
                toolStripStatusLabel.Text = "Settings saved";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void SetUIForScanning(bool scanning)
    {
        btnScan.Enabled = !scanning;
        btnStop.Enabled = scanning;
        importToolStripMenuItem.Enabled = !scanning;
        settingsToolStripMenuItem.Enabled = !scanning;
        toolStripProgressBar.Visible = scanning;
        
        if (scanning)
        {
            toolStripStatusLabel.Text = "Scanning...";
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
        }
        else
        {
            toolStripProgressBar.Style = ProgressBarStyle.Blocks;
        }
    }

    private void UpdatePCStatus(PCInfo pcInfo)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action<PCInfo>(UpdatePCStatus), pcInfo);
            return;
        }

        AddOrUpdateRow(pcInfo);
    }
    
    private void AddOrUpdateRow(PCInfo pcInfo)
    {
        // Find existing row or add new one
        int rowIndex = -1;
        for (int i = 0; i < dataGridView.Rows.Count; i++)
        {
            if (dataGridView.Rows[i].Cells["colPCName"].Value?.ToString() == pcInfo.PCName)
            {
                rowIndex = i;
                break;
            }
        }
        
        if (rowIndex == -1)
        {
            rowIndex = dataGridView.Rows.Add();
        }
        
        // Update the row with PC info
        dataGridView.Rows[rowIndex].Cells["colPCName"].Value = pcInfo.PCName;
        dataGridView.Rows[rowIndex].Cells["colStatus"].Value = pcInfo.Status;
        
        if (pcInfo.Status == "Completed")
        {
            // Add data for all enabled columns
            if (_settings.CheckHDDSize && dataGridView.Columns.Contains("colHDDSize"))
                dataGridView.Rows[rowIndex].Cells["colHDDSize"].Value = pcInfo.HDDSize;
                
            if (_settings.CheckFreeHDDSpace && dataGridView.Columns.Contains("colFreeHDDSpace"))
                dataGridView.Rows[rowIndex].Cells["colFreeHDDSpace"].Value = pcInfo.FreeHDDSpace;
                
            if (_settings.CheckTotalRAM && dataGridView.Columns.Contains("colTotalRAM"))
                dataGridView.Rows[rowIndex].Cells["colTotalRAM"].Value = pcInfo.TotalRAM;
                
            if (_settings.CheckIPAddress && dataGridView.Columns.Contains("colIPAddress"))
                dataGridView.Rows[rowIndex].Cells["colIPAddress"].Value = pcInfo.IPAddress;
                
            if (_settings.CheckMACAddress && dataGridView.Columns.Contains("colMACAddress"))
                dataGridView.Rows[rowIndex].Cells["colMACAddress"].Value = pcInfo.MACAddress;
                
            if (_settings.CheckLoggedOnUser && dataGridView.Columns.Contains("colLoggedOnUser"))
                dataGridView.Rows[rowIndex].Cells["colLoggedOnUser"].Value = pcInfo.LoggedOnUser;
                
            if (_settings.CheckLastRebootTime && dataGridView.Columns.Contains("colLastRebootTime"))
                dataGridView.Rows[rowIndex].Cells["colLastRebootTime"].Value = pcInfo.LastRebootTime;
                
            if (_settings.CheckMake && dataGridView.Columns.Contains("colMake"))
                dataGridView.Rows[rowIndex].Cells["colMake"].Value = pcInfo.Make;
                
            if (_settings.CheckModel && dataGridView.Columns.Contains("colModel"))
                dataGridView.Rows[rowIndex].Cells["colModel"].Value = pcInfo.Model;
                
            if (_settings.CheckBIOSVersion && dataGridView.Columns.Contains("colBIOSVersion"))
                dataGridView.Rows[rowIndex].Cells["colBIOSVersion"].Value = pcInfo.BIOSVersion;
                
            if (_settings.CheckWindowsVersion && dataGridView.Columns.Contains("colWindowsVersion"))
                dataGridView.Rows[rowIndex].Cells["colWindowsVersion"].Value = pcInfo.WindowsVersion;
                
            if (_settings.CheckSerialNumber && dataGridView.Columns.Contains("colSerialNumber"))
                dataGridView.Rows[rowIndex].Cells["colSerialNumber"].Value = pcInfo.SerialNumber;

            if (_settings.CheckPendingReboot && dataGridView.Columns.Contains("colPendingReboot"))
            {
                dataGridView.Rows[rowIndex].Cells["colPendingReboot"].Value = pcInfo.PendingRebootStatus;
            }
                
            // Add registry check values
            foreach (var regCheck in _settings.RegistryChecks.Where(rc => rc.Enabled))
            {
                string columnName = $"colReg_{regCheck.FriendlyName.Replace(" ", "_")}";
                if (dataGridView.Columns.Contains(columnName) && 
                    pcInfo.CustomRegistryValues.TryGetValue(regCheck.FriendlyName, out string? value) && 
                    value != null)
                {
                    dataGridView.Rows[rowIndex].Cells[columnName].Value = value;
                }
            }
        }
        
        dataGridView.Refresh();
    }

    private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        // Handle right-click to properly select the row before showing context menu
        if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
        {
            // Select the row that was right-clicked
            dataGridView.ClearSelection();
            dataGridView.Rows[e.RowIndex].Selected = true;
            
            // Adjust context menu items based on the PC status
            string? pcStatus = dataGridView.Rows[e.RowIndex].Cells["colStatus"].Value?.ToString();
            rescanPCMenuItem.Enabled = true; // Always enable rescan
            restartPCMenuItem.Enabled = pcStatus == "Completed"; // Only enable restart for PCs that were successfully scanned
        }
    }

    private void rescanPCMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        // Update status to waiting
        dataGridView.SelectedRows[0].Cells["colStatus"].Value = "Waiting...";
        
        // Scan just this PC
        _pcList = new List<string> { pcName };
        
        // Start the scan
        ScanSinglePC(pcName);
    }

    private void restartPCMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        // Confirm restart
        if (MessageBox.Show(
            $"Are you sure you want to restart {pcName}?", 
            "Confirm Restart", 
            MessageBoxButtons.YesNo, 
            MessageBoxIcon.Question) != DialogResult.Yes)
            return;
            
        // Show progress
        toolStripStatusLabel.Text = $"Restarting {pcName}...";
        
        // Start the restart operation in a background task
        Task.Run(() =>
        {
            try 
            {
                // Execute remote restart
                bool success = RestartRemotePC(pcName);
                
                // Update UI with result
                BeginInvoke(() => 
                {
                    if (success)
                    {
                        toolStripStatusLabel.Text = $"Restart command sent to {pcName}";
                        // Update status in the grid
                        for (int i = 0; i < dataGridView.Rows.Count; i++)
                        {
                            if (dataGridView.Rows[i].Cells["colPCName"].Value?.ToString() == pcName)
                            {
                                dataGridView.Rows[i].Cells["colStatus"].Value = "Restarting...";
                                break;
                            }
                        }
                    }
                    else
                    {
                        toolStripStatusLabel.Text = $"Failed to restart {pcName}";
                        MessageBox.Show($"Failed to restart {pcName}. Make sure you have appropriate permissions.", 
                            "Restart Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                BeginInvoke(() => 
                {
                    toolStripStatusLabel.Text = $"Error restarting {pcName}";
                    MessageBox.Show($"Error restarting {pcName}: {ex.Message}", 
                        "Restart Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        });
    }

    private async void ScanSinglePC(string pcName)
    {
        // Similar to ScanSelectedPCs but just for one PC
        SetUIForScanning(true);
        
        try
        {
            if (_healthService != null)
            {
                var pcInfo = await _healthService.GetPCHealthInfoAsync(pcName);
                UpdatePCStatus(pcInfo);
                
                // Update the pcInfoList
                var existingIndex = _pcInfoList.FindIndex(p => p.PCName == pcName);
                if (existingIndex >= 0)
                    _pcInfoList[existingIndex] = pcInfo;
                else
                    _pcInfoList.Add(pcInfo);
                    
                toolStripStatusLabel.Text = $"Scan of {pcName} completed";
                exportToolStripMenuItem.Enabled = _pcInfoList.Count > 0;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error scanning {pcName}: {ex.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetUIForScanning(false);
        }
    }
    
    private bool RestartRemotePC(string pcName)
    {
        try
        {
            // Use the Windows Management Instrumentation (WMI) to restart the remote PC
            var scope = new System.Management.ManagementScope($"\\\\{pcName}\\root\\cimv2");
            scope.Connect();
            
            var query = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            var searcher = new System.Management.ManagementObjectSearcher(scope, query);
            
            using (var managementObjectCollection = searcher.Get())
            {
                foreach (System.Management.ManagementObject managementObject in managementObjectCollection)
                {
                    try
                    {
                        // Invoke the Win32Shutdown method
                        // 2 = Reboot
                        var inParams = managementObject.GetMethodParameters("Win32Shutdown");
                        inParams["Flags"] = 2; // Reboot flag
                        inParams["Reserved"] = 0;
                        
                        managementObject.InvokeMethod("Win32Shutdown", inParams, null);
                        return true;
                    }
                    finally
                    {
                        managementObject.Dispose();
                    }
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void copyMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;

        // Get the selected row
        DataGridViewRow row = dataGridView.SelectedRows[0];
        
        // Build string with all cell values in tab-separated format
        StringBuilder rowData = new StringBuilder();
        
        // First add column headers
        foreach (DataGridViewCell cell in row.Cells)
        {
            if (cell.OwningColumn.Visible)
            {
                rowData.Append(cell.OwningColumn.HeaderText);
                rowData.Append("\t");
            }
        }
        rowData.AppendLine();
        
        // Then add values
        foreach (DataGridViewCell cell in row.Cells)
        {
            if (cell.OwningColumn.Visible)
            {
                rowData.Append(cell.Value?.ToString() ?? string.Empty);
                rowData.Append("\t");
            }
        }
        
        try
        {
            // Copy to clipboard
            Clipboard.SetText(rowData.ToString());
            toolStripStatusLabel.Text = "Row data copied to clipboard";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error copying to clipboard: {ex.Message}", "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void rdpMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        try
        {
            // Launch Remote Desktop with the PC name
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "mstsc.exe",
                Arguments = $"/v:{pcName}",
                UseShellExecute = true
            };
            
            System.Diagnostics.Process.Start(startInfo);
            toolStripStatusLabel.Text = $"Launching RDP connection to {pcName}...";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching RDP: {ex.Message}", "RDP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void openCDriveMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        try
        {
            // Open File Explorer with path to the C$ share
            var path = $@"\\{pcName}\c$";
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = path,
                UseShellExecute = true
            };
            
            System.Diagnostics.Process.Start(startInfo);
            toolStripStatusLabel.Text = $"Opening C$ share on {pcName}...";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening C$ share: {ex.Message}", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void gpUpdateMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        // Confirm GPUpdate
        if (MessageBox.Show(
            $"Run GPUpdate /force on {pcName}?", 
            "Confirm GPUpdate", 
            MessageBoxButtons.YesNo, 
            MessageBoxIcon.Question) != DialogResult.Yes)
            return;
            
        // Show progress
        toolStripStatusLabel.Text = $"Running GPUpdate on {pcName}...";
        
        // Run the GPUpdate in a background task
        Task.Run(() =>
        {
            try
            {
                bool success = RunRemoteGPUpdate(pcName);
                
                BeginInvoke(() => 
                {
                    if (success)
                    {
                        toolStripStatusLabel.Text = $"GPUpdate command sent to {pcName}";
                    }
                    else
                    {
                        toolStripStatusLabel.Text = $"Failed to run GPUpdate on {pcName}";
                        MessageBox.Show($"Failed to run GPUpdate on {pcName}. Make sure you have appropriate permissions.", 
                            "GPUpdate Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                BeginInvoke(() => 
                {
                    toolStripStatusLabel.Text = $"Error running GPUpdate on {pcName}";
                    MessageBox.Show($"Error running GPUpdate on {pcName}: {ex.Message}", 
                        "GPUpdate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        });
    }

    private void pingMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        // Launch Command Prompt with ping command
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K ping {pcName} -t",
                UseShellExecute = true
            };
            
            System.Diagnostics.Process.Start(startInfo);
            toolStripStatusLabel.Text = $"Pinging {pcName}...";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching ping: {ex.Message}", "Ping Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void tracertMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
            return;
            
        // Get the PC name from the selected row
        string? pcName = dataGridView.SelectedRows[0].Cells["colPCName"].Value?.ToString();
        if (string.IsNullOrEmpty(pcName))
            return;
            
        // Launch Command Prompt with tracert command
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K tracert {pcName}",
                UseShellExecute = true
            };
            
            System.Diagnostics.Process.Start(startInfo);
            toolStripStatusLabel.Text = $"Running tracert to {pcName}...";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error launching tracert: {ex.Message}", "Tracert Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool RunRemoteGPUpdate(string pcName)
    {
        try
        {
            // Use WMI to run gpupdate /force on the remote PC
            var scope = new System.Management.ManagementScope($"\\\\{pcName}\\root\\cimv2");
            scope.Connect();
            
            var processToRun = new System.Management.ManagementClass(scope, new System.Management.ManagementPath("Win32_Process"), new System.Management.ObjectGetOptions());
            var inParams = processToRun.GetMethodParameters("Create");
            
            // Set up the command line for gpupdate /force
            inParams["CommandLine"] = "gpupdate /force";
            
            // Invoke the method
            var outParams = processToRun.InvokeMethod("Create", inParams, null);
            
            // Check the return value
            uint returnValue = Convert.ToUInt32(outParams["ReturnValue"]);
            return returnValue == 0;
        }
        catch
        {
            return false;
        }
    }
}
