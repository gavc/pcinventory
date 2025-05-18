namespace PCInventory
{
    public partial class AddRegistryCheckForm : Form
    {
        public string FriendlyName { get; set; } = string.Empty;
        public string KeyPath { get; set; } = "HKEY_LOCAL_MACHINE\\";
        public string ValueName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;

        public AddRegistryCheckForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Initialize form fields from properties
            txtFriendlyName.Text = FriendlyName;
            txtKeyPath.Text = KeyPath;
            txtValueName.Text = ValueName;
            chkEnabled.Checked = IsEnabled;
            
            // Set focus on the first field
            txtFriendlyName.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtFriendlyName.Text))
            {
                MessageBox.Show("Friendly name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFriendlyName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtKeyPath.Text))
            {
                MessageBox.Show("Registry key path is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKeyPath.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtValueName.Text))
            {
                MessageBox.Show("Value name is required.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValueName.Focus();
                return;
            }

            // Save values back to properties
            FriendlyName = txtFriendlyName.Text.Trim();
            KeyPath = txtKeyPath.Text.Trim();
            ValueName = txtValueName.Text.Trim();
            IsEnabled = chkEnabled.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}