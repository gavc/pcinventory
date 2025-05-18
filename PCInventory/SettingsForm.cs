using PCInventory.Models;

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
            foreach (var check in _settings.RegistryChecks)
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
            chkSerialNumber.Checked = _settings.CheckSerialNumber;
            chkPendingReboot.Checked = _settings.CheckPendingReboot;
            
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
            chkSerialNumber.Checked = true;
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
            chkSerialNumber.Checked = false;
        }

        private void btnAddRegistryCheck_Click(object sender, EventArgs e)
        {
            var form = new AddRegistryCheckForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _registryChecks.Add(new RegistryCheckSetting
                {
                    FriendlyName = form.FriendlyName,
                    KeyPath = form.KeyPath,
                    ValueName = form.ValueName,
                    Enabled = true
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
                    selectedCheck.FriendlyName = form.FriendlyName;
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
            _settings.CheckSerialNumber = chkSerialNumber.Checked;
            _settings.CheckPendingReboot = chkPendingReboot.Checked;
            
            // Save registry check settings
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
    }
}