using PCInventory.Models;
using PCInventory.Utils;

namespace PCInventory
{
    public partial class SettingsForm : Form
    {
        private AppSettings _settings;
        private List<RegistryCheckSetting> _registryChecks;

        public SettingsForm(AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _registryChecks = new List<RegistryCheckSetting>();

            // Make a deep copy of registry checks to avoid modifying original until save
            var sourceChecks = _settings.RegistryChecks ?? new List<RegistryCheckSetting>();
            foreach (var check in sourceChecks)
            {
                _registryChecks.Add(new RegistryCheckSetting
                {
                    FriendlyName = check.FriendlyName,
                    KeyPath = check.KeyPath,
                    ValueName = check.ValueName,
                    Enabled = check.Enabled
                });
            }

            LoadSettings();
        }

        private void LoadSettings()
        {
            // Set standard check checkboxes based on settings
            chkHDDSize.Checked = _settings.CheckHDDSize;
            chkFreeHDDSpace.Checked = _settings.CheckFreeHDDSpace;
            chkTotalRAM.Checked = _settings.CheckTotalRAM;
            chkIPAddress.Checked = _settings.CheckIPAddress;
            chkMACAddress.Checked = _settings.CheckMACAddress;
            chkLoggedOnUser.Checked = _settings.CheckLoggedOnUser;
            chkLastRebootTime.Checked = _settings.CheckLastRebootTime;
            chkMake.Checked = _settings.CheckMake;
            chkModel.Checked = _settings.CheckModel;
            chkBIOSVersion.Checked = _settings.CheckBIOSVersion;
            chkWindowsVersion.Checked = _settings.CheckWindowsVersion;
            chkInstallDate.Checked = _settings.CheckInstallDate;
            chkSerialNumber.Checked = _settings.CheckSerialNumber;
            chkPendingReboot.Checked = _settings.CheckPendingReboot;
            chkNetworkConnectionType.Checked = _settings.CheckNetworkConnectionType;
            chkWiFiInfo.Checked = _settings.CheckWiFiInfo;
            
            // Load PC Name Validation settings
            chkEnablePCNameValidation.Checked = _settings.EnablePCNameValidation;
            txtPCNamePattern.Text = _settings.PCNamePattern;
            txtPCNamePattern.Enabled = _settings.EnablePCNameValidation;
            btnTestPattern.Enabled = _settings.EnablePCNameValidation;
            
            // Load registry checks into listbox
            RefreshRegistryChecksList();
        }

        private void RefreshRegistryChecksList()
        {
            lstRegistryChecks.Items.Clear();
            foreach (var check in _registryChecks)
            {
                string displayText = $"{check.FriendlyName} - {check.KeyPath}\\{check.ValueName}";
                if (!check.Enabled)
                    displayText = $"[DISABLED] {displayText}";
                
                lstRegistryChecks.Items.Add(displayText);
            }

            // Enable/disable buttons based on selection
            btnEditRegistryCheck.Enabled = lstRegistryChecks.SelectedIndex >= 0;
            btnRemoveRegistryCheck.Enabled = lstRegistryChecks.SelectedIndex >= 0;
        }

        private bool FriendlyNameExists(string friendlyName, int? ignoreIndex = null)
        {
            for (int i = 0; i < _registryChecks.Count; i++)
            {
                if (ignoreIndex.HasValue && i == ignoreIndex.Value)
                    continue;

                if (string.Equals(_registryChecks[i].FriendlyName, friendlyName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            chkHDDSize.Checked = true;
            chkFreeHDDSpace.Checked = true;
            chkTotalRAM.Checked = true;
            chkIPAddress.Checked = true;
            chkMACAddress.Checked = true;
            chkLoggedOnUser.Checked = true;
            chkLastRebootTime.Checked = true;
            chkMake.Checked = true;
            chkModel.Checked = true;
            chkBIOSVersion.Checked = true;
            chkWindowsVersion.Checked = true;
            chkInstallDate.Checked = true;
            chkSerialNumber.Checked = true;
            chkPendingReboot.Checked = true;
            chkNetworkConnectionType.Checked = true;
            chkWiFiInfo.Checked = true;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            chkHDDSize.Checked = false;
            chkFreeHDDSpace.Checked = false;
            chkTotalRAM.Checked = false;
            chkIPAddress.Checked = false;
            chkMACAddress.Checked = false;
            chkLoggedOnUser.Checked = false;
            chkLastRebootTime.Checked = false;
            chkMake.Checked = false;
            chkModel.Checked = false;
            chkBIOSVersion.Checked = false;
            chkWindowsVersion.Checked = false;
            chkInstallDate.Checked = false;
            chkSerialNumber.Checked = false;
            chkPendingReboot.Checked = false;
            chkNetworkConnectionType.Checked = false;
            chkWiFiInfo.Checked = false;
        }

        private void btnAddRegistryCheck_Click(object sender, EventArgs e)
        {
            var form = new AddRegistryCheckForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                string friendlyName = form.FriendlyName.Trim();
                if (FriendlyNameExists(friendlyName))
                {
                    MessageBox.Show(
                        $"A registry check named '{friendlyName}' already exists. Friendly names must be unique.",
                        "Duplicate Friendly Name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                _registryChecks.Add(new RegistryCheckSetting
                {
                    FriendlyName = friendlyName,
                    KeyPath = form.KeyPath,
                    ValueName = form.ValueName,
                    Enabled = form.IsEnabled
                });
                
                RefreshRegistryChecksList();
            }
        }

        private void btnEditRegistryCheck_Click(object sender, EventArgs e)
        {
            int selectedIndex = lstRegistryChecks.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _registryChecks.Count)
            {
                var selectedCheck = _registryChecks[selectedIndex];
                
                var form = new AddRegistryCheckForm
                {
                    FriendlyName = selectedCheck.FriendlyName,
                    KeyPath = selectedCheck.KeyPath,
                    ValueName = selectedCheck.ValueName,
                    IsEnabled = selectedCheck.Enabled
                };
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string friendlyName = form.FriendlyName.Trim();
                    if (FriendlyNameExists(friendlyName, selectedIndex))
                    {
                        MessageBox.Show(
                            $"A registry check named '{friendlyName}' already exists. Friendly names must be unique.",
                            "Duplicate Friendly Name",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    selectedCheck.FriendlyName = friendlyName;
                    selectedCheck.KeyPath = form.KeyPath;
                    selectedCheck.ValueName = form.ValueName;
                    selectedCheck.Enabled = form.IsEnabled;
                    
                    RefreshRegistryChecksList();
                }
            }
        }

        private void btnRemoveRegistryCheck_Click(object sender, EventArgs e)
        {
            int selectedIndex = lstRegistryChecks.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _registryChecks.Count)
            {
                if (MessageBox.Show(
                    $"Are you sure you want to remove '{_registryChecks[selectedIndex].FriendlyName}'?", 
                    "Confirm Removal", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _registryChecks.RemoveAt(selectedIndex);
                    RefreshRegistryChecksList();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save standard check settings
            _settings.CheckHDDSize = chkHDDSize.Checked;
            _settings.CheckFreeHDDSpace = chkFreeHDDSpace.Checked;
            _settings.CheckTotalRAM = chkTotalRAM.Checked;
            _settings.CheckIPAddress = chkIPAddress.Checked;
            _settings.CheckMACAddress = chkMACAddress.Checked;
            _settings.CheckLoggedOnUser = chkLoggedOnUser.Checked;
            _settings.CheckLastRebootTime = chkLastRebootTime.Checked;
            _settings.CheckMake = chkMake.Checked;
            _settings.CheckModel = chkModel.Checked;
            _settings.CheckBIOSVersion = chkBIOSVersion.Checked;
            _settings.CheckWindowsVersion = chkWindowsVersion.Checked;
            _settings.CheckInstallDate = chkInstallDate.Checked;
            _settings.CheckSerialNumber = chkSerialNumber.Checked;
            _settings.CheckPendingReboot = chkPendingReboot.Checked;
            _settings.CheckNetworkConnectionType = chkNetworkConnectionType.Checked;
            _settings.CheckWiFiInfo = chkWiFiInfo.Checked;
            
            // Save PC Name Validation settings
            _settings.EnablePCNameValidation = chkEnablePCNameValidation.Checked;
            _settings.PCNamePattern = txtPCNamePattern.Text.Trim();
            
            // Save registry check settings
            _settings.RegistryChecks ??= new List<RegistryCheckSetting>();
            _settings.RegistryChecks.Clear();
            foreach (var check in _registryChecks)
            {
                _settings.RegistryChecks.Add(new RegistryCheckSetting
                {
                    FriendlyName = check.FriendlyName,
                    KeyPath = check.KeyPath,
                    ValueName = check.ValueName,
                    Enabled = check.Enabled
                });
            }
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lstRegistryChecks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstRegistryChecks.SelectedIndex;
            btnEditRegistryCheck.Enabled = selectedIndex >= 0;
            btnRemoveRegistryCheck.Enabled = selectedIndex >= 0;
        }

        private void chkEnablePCNameValidation_CheckedChanged(object sender, EventArgs e)
        {
            txtPCNamePattern.Enabled = chkEnablePCNameValidation.Checked;
            btnTestPattern.Enabled = chkEnablePCNameValidation.Checked;
        }

        private void btnTestPattern_Click(object sender, EventArgs e)
        {
            // Create a dialog to test the pattern
            using var testForm = new Form
            {
                Text = "Test PC Name Pattern",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblInstruction = new Label
            {
                Text = "Enter sample PC names (one per line) to test the pattern:",
                Location = new Point(10, 10),
                Size = new Size(460, 20)
            };

            var txtInput = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(10, 35),
                Size = new Size(460, 150),
                Text = "PC123456\n PC112233\n\tPC 112234\n     PC112231\nPC 1121213   \nPC123321\n\t  PC998877    "
            };

            var lblResults = new Label
            {
                Text = "Results:",
                Location = new Point(10, 195),
                Size = new Size(460, 20)
            };

            var txtResults = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(10, 220),
                Size = new Size(460, 100),
                ReadOnly = true
            };

            var btnTest = new Button
            {
                Text = "Test",
                Location = new Point(310, 330),
                Size = new Size(75, 30)
            };

            var btnClose = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.OK,
                Location = new Point(395, 330),
                Size = new Size(75, 30)
            };

            btnTest.Click += (s, ev) =>
            {
                try
                {
                    var pattern = txtPCNamePattern.Text.Trim();
                    var lines = txtInput.Text
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .ToList();

                    var results = new System.Text.StringBuilder();
                    results.AppendLine($"Pattern: {(string.IsNullOrEmpty(pattern) ? "(empty)" : pattern)}");
                    results.AppendLine($"Total input lines: {lines.Count}");
                    results.AppendLine();

                    int validCount = 0;
                    int invalidCount = 0;

                    foreach (var line in lines)
                    {
                        var sanitized = PCNameValidator.SanitizePCName(line, pattern, chkEnablePCNameValidation.Checked);
                        if (!string.IsNullOrEmpty(sanitized))
                        {
                            results.AppendLine($"[OK] \"{line}\" -> \"{sanitized}\"");
                            validCount++;
                        }
                        else
                        {
                            results.AppendLine($"[X] \"{line}\" -> (rejected)");
                            invalidCount++;
                        }
                    }

                    results.AppendLine();
                    results.AppendLine($"Valid: {validCount}, Invalid: {invalidCount}");

                    txtResults.Text = results.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error testing pattern: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            testForm.Controls.Add(lblInstruction);
            testForm.Controls.Add(txtInput);
            testForm.Controls.Add(lblResults);
            testForm.Controls.Add(txtResults);
            testForm.Controls.Add(btnTest);
            testForm.Controls.Add(btnClose);
            testForm.AcceptButton = btnTest;

            testForm.ShowDialog();
        }
    }
}
