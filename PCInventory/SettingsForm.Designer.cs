namespace PCInventory
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            grpStandardChecks = new GroupBox();
            flpStandardChecks = new FlowLayoutPanel();
            chkSerialNumber = new CheckBox();
            chkWindowsVersion = new CheckBox();
            chkInstallDate = new CheckBox();
            chkBIOSVersion = new CheckBox();
            chkModel = new CheckBox();
            chkMake = new CheckBox();
            chkLastRebootTime = new CheckBox();
            chkLoggedOnUser = new CheckBox();
            chkMACAddress = new CheckBox();
            chkIPAddress = new CheckBox();
            chkTotalRAM = new CheckBox();
            chkFreeHDDSpace = new CheckBox();
            chkHDDSize = new CheckBox();
            chkPendingReboot = new CheckBox();
            chkNetworkConnectionType = new CheckBox();
            chkWiFiInfo = new CheckBox();
            btnSelectAll = new Button();
            btnDeselectAll = new Button();
            grpRegistryChecks = new GroupBox();
            lstRegistryChecks = new ListBox();
            btnAddRegistryCheck = new Button();
            btnEditRegistryCheck = new Button();
            btnRemoveRegistryCheck = new Button();
            grpPCNameValidation = new GroupBox();
            chkEnablePCNameValidation = new CheckBox();
            txtPCNamePattern = new TextBox();
            lblPCNamePattern = new Label();
            lblPatternHelp = new Label();
            btnTestPattern = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            
            grpStandardChecks.SuspendLayout();
            grpRegistryChecks.SuspendLayout();
            grpPCNameValidation.SuspendLayout();
            SuspendLayout();
            
            // grpStandardChecks
            grpStandardChecks.Controls.Add(flpStandardChecks);
            grpStandardChecks.Controls.Add(btnSelectAll);
            grpStandardChecks.Controls.Add(btnDeselectAll);
            grpStandardChecks.Location = new Point(10, 10);
            grpStandardChecks.Name = "grpStandardChecks";
            grpStandardChecks.Size = new Size(380, 350);
            grpStandardChecks.TabIndex = 0;
            grpStandardChecks.TabStop = false;
            grpStandardChecks.Text = "Standard Health Checks";
            
            // flpStandardChecks
            flpStandardChecks.AutoScroll = true;
            flpStandardChecks.FlowDirection = FlowDirection.TopDown;
            flpStandardChecks.Location = new Point(10, 25);
            flpStandardChecks.Name = "flpStandardChecks";
            flpStandardChecks.Size = new Size(360, 285);
            flpStandardChecks.TabIndex = 0;
            flpStandardChecks.WrapContents = false;
            flpStandardChecks.Controls.Add(chkIPAddress);
            flpStandardChecks.Controls.Add(chkMACAddress);
            flpStandardChecks.Controls.Add(chkLoggedOnUser);
            flpStandardChecks.Controls.Add(chkLastRebootTime);
            flpStandardChecks.Controls.Add(chkMake);
            flpStandardChecks.Controls.Add(chkModel);
            flpStandardChecks.Controls.Add(chkBIOSVersion);
            flpStandardChecks.Controls.Add(chkWindowsVersion);
            flpStandardChecks.Controls.Add(chkSerialNumber);
            flpStandardChecks.Controls.Add(chkInstallDate);
            flpStandardChecks.Controls.Add(chkTotalRAM);
            flpStandardChecks.Controls.Add(chkFreeHDDSpace);
            flpStandardChecks.Controls.Add(chkHDDSize);
            flpStandardChecks.Controls.Add(chkPendingReboot);
            flpStandardChecks.Controls.Add(chkNetworkConnectionType);
            flpStandardChecks.Controls.Add(chkWiFiInfo);
            
            // chkSerialNumber
            chkSerialNumber.AutoSize = true;
            chkSerialNumber.Margin = new Padding(3, 2, 3, 2);
            chkSerialNumber.Name = "chkSerialNumber";
            chkSerialNumber.TabIndex = 13;
            chkSerialNumber.Text = "Serial Number";
            chkSerialNumber.UseVisualStyleBackColor = true;
            
            // chkWindowsVersion
            chkWindowsVersion.AutoSize = true;
            chkWindowsVersion.Margin = new Padding(3, 2, 3, 2);
            chkWindowsVersion.Name = "chkWindowsVersion";
            chkWindowsVersion.TabIndex = 12;
            chkWindowsVersion.Text = "Windows Version";
            chkWindowsVersion.UseVisualStyleBackColor = true;
            
            // chkBIOSVersion
            chkBIOSVersion.AutoSize = true;
            chkBIOSVersion.Margin = new Padding(3, 2, 3, 2);
            chkBIOSVersion.Name = "chkBIOSVersion";
            chkBIOSVersion.TabIndex = 11;
            chkBIOSVersion.Text = "BIOS Version";
            chkBIOSVersion.UseVisualStyleBackColor = true;
            
            // chkModel
            chkModel.AutoSize = true;
            chkModel.Margin = new Padding(3, 2, 3, 2);
            chkModel.Name = "chkModel";
            chkModel.TabIndex = 10;
            chkModel.Text = "Model";
            chkModel.UseVisualStyleBackColor = true;
            
            // chkMake
            chkMake.AutoSize = true;
            chkMake.Margin = new Padding(3, 2, 3, 2);
            chkMake.Name = "chkMake";
            chkMake.TabIndex = 9;
            chkMake.Text = "Make";
            chkMake.UseVisualStyleBackColor = true;
            
            // chkLastRebootTime
            chkLastRebootTime.AutoSize = true;
            chkLastRebootTime.Margin = new Padding(3, 2, 3, 2);
            chkLastRebootTime.Name = "chkLastRebootTime";
            chkLastRebootTime.TabIndex = 8;
            chkLastRebootTime.Text = "Last Reboot Time";
            chkLastRebootTime.UseVisualStyleBackColor = true;
            
            // chkLoggedOnUser
            chkLoggedOnUser.AutoSize = true;
            chkLoggedOnUser.Margin = new Padding(3, 2, 3, 2);
            chkLoggedOnUser.Name = "chkLoggedOnUser";
            chkLoggedOnUser.TabIndex = 7;
            chkLoggedOnUser.Text = "Logged-on User";
            chkLoggedOnUser.UseVisualStyleBackColor = true;
            
            // chkMACAddress
            chkMACAddress.AutoSize = true;
            chkMACAddress.Margin = new Padding(3, 2, 3, 2);
            chkMACAddress.Name = "chkMACAddress";
            chkMACAddress.TabIndex = 6;
            chkMACAddress.Text = "MAC Address";
            chkMACAddress.UseVisualStyleBackColor = true;
            
            // chkIPAddress
            chkIPAddress.AutoSize = true;
            chkIPAddress.Margin = new Padding(3, 2, 3, 2);
            chkIPAddress.Name = "chkIPAddress";
            chkIPAddress.TabIndex = 5;
            chkIPAddress.Text = "IP Address";
            chkIPAddress.UseVisualStyleBackColor = true;
            
            // chkTotalRAM
            chkTotalRAM.AutoSize = true;
            chkTotalRAM.Margin = new Padding(3, 2, 3, 2);
            chkTotalRAM.Name = "chkTotalRAM";
            chkTotalRAM.TabIndex = 4;
            chkTotalRAM.Text = "Total RAM";
            chkTotalRAM.UseVisualStyleBackColor = true;
            
            // chkFreeHDDSpace
            chkFreeHDDSpace.AutoSize = true;
            chkFreeHDDSpace.Margin = new Padding(3, 2, 3, 2);
            chkFreeHDDSpace.Name = "chkFreeHDDSpace";
            chkFreeHDDSpace.TabIndex = 3;
            chkFreeHDDSpace.Text = "Free HDD Space";
            chkFreeHDDSpace.UseVisualStyleBackColor = true;
            
            // chkHDDSize
            chkHDDSize.AutoSize = true;
            chkHDDSize.Margin = new Padding(3, 2, 3, 2);
            chkHDDSize.Name = "chkHDDSize";
            chkHDDSize.TabIndex = 2;
            chkHDDSize.Text = "HDD Size";
            chkHDDSize.UseVisualStyleBackColor = true;

            // chkPendingReboot
            chkPendingReboot.AutoSize = true;
            chkPendingReboot.Margin = new Padding(3, 2, 3, 2);
            chkPendingReboot.Name = "chkPendingReboot";
            chkPendingReboot.TabIndex = 13;
            chkPendingReboot.Text = "Pending Reboot";
            chkPendingReboot.UseVisualStyleBackColor = true;
            
            // chkNetworkConnectionType
            chkNetworkConnectionType.AutoSize = true;
            chkNetworkConnectionType.Margin = new Padding(3, 2, 3, 2);
            chkNetworkConnectionType.Name = "chkNetworkConnectionType";
            chkNetworkConnectionType.TabIndex = 14;
            chkNetworkConnectionType.Text = "Network Connection";
            chkNetworkConnectionType.UseVisualStyleBackColor = true;
            
            // chkWiFiInfo
            chkWiFiInfo.AutoSize = true;
            chkWiFiInfo.Margin = new Padding(3, 2, 3, 2);
            chkWiFiInfo.Name = "chkWiFiInfo";
            chkWiFiInfo.TabIndex = 15;
            chkWiFiInfo.Text = "WiFi Info";
            chkWiFiInfo.UseVisualStyleBackColor = true;

            // chkInstallDate (moved below WiFi Info)
            chkInstallDate.AutoSize = true;
            chkInstallDate.Margin = new Padding(3, 2, 3, 2);
            chkInstallDate.Name = "chkInstallDate";
            chkInstallDate.TabIndex = 16;
            chkInstallDate.Text = "OS Install Date";
            chkInstallDate.UseVisualStyleBackColor = true;
            
            // btnSelectAll
            btnSelectAll.Location = new Point(80, 320);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(120, 25);
            btnSelectAll.TabIndex = 1;
            btnSelectAll.Text = "Select All";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            
            // btnDeselectAll
            btnDeselectAll.Location = new Point(210, 320);
            btnDeselectAll.Name = "btnDeselectAll";
            btnDeselectAll.Size = new Size(120, 25);
            btnDeselectAll.TabIndex = 0;
            btnDeselectAll.Text = "Deselect All";
            btnDeselectAll.UseVisualStyleBackColor = true;
            btnDeselectAll.Click += btnDeselectAll_Click;
            
            // grpRegistryChecks
            grpRegistryChecks.Controls.Add(lstRegistryChecks);
            grpRegistryChecks.Controls.Add(btnAddRegistryCheck);
            grpRegistryChecks.Controls.Add(btnEditRegistryCheck);
            grpRegistryChecks.Controls.Add(btnRemoveRegistryCheck);
            grpRegistryChecks.Location = new Point(400, 10);
            grpRegistryChecks.Name = "grpRegistryChecks";
            grpRegistryChecks.Size = new Size(380, 350);
            grpRegistryChecks.TabIndex = 1;
            grpRegistryChecks.TabStop = false;
            grpRegistryChecks.Text = "Registry Value Checks";
            
            // lstRegistryChecks
            lstRegistryChecks.FormattingEnabled = true;
            lstRegistryChecks.ItemHeight = 20;
            lstRegistryChecks.Location = new Point(10, 30);
            lstRegistryChecks.Name = "lstRegistryChecks";
            lstRegistryChecks.Size = new Size(360, 280);
            lstRegistryChecks.TabIndex = 2;
            lstRegistryChecks.SelectedIndexChanged += lstRegistryChecks_SelectedIndexChanged;
            
            // btnAddRegistryCheck
            btnAddRegistryCheck.Location = new Point(10, 320);
            btnAddRegistryCheck.Name = "btnAddRegistryCheck";
            btnAddRegistryCheck.Size = new Size(100, 25);
            btnAddRegistryCheck.TabIndex = 3;
            btnAddRegistryCheck.Text = "Add";
            btnAddRegistryCheck.UseVisualStyleBackColor = true;
            btnAddRegistryCheck.Click += btnAddRegistryCheck_Click;
            
            // btnEditRegistryCheck
            btnEditRegistryCheck.Location = new Point(120, 320);
            btnEditRegistryCheck.Name = "btnEditRegistryCheck";
            btnEditRegistryCheck.Size = new Size(100, 25);
            btnEditRegistryCheck.TabIndex = 4;
            btnEditRegistryCheck.Text = "Edit";
            btnEditRegistryCheck.UseVisualStyleBackColor = true;
            btnEditRegistryCheck.Click += btnEditRegistryCheck_Click;
            
            // btnRemoveRegistryCheck
            btnRemoveRegistryCheck.Location = new Point(230, 320);
            btnRemoveRegistryCheck.Name = "btnRemoveRegistryCheck";
            btnRemoveRegistryCheck.Size = new Size(140, 25);
            btnRemoveRegistryCheck.TabIndex = 5;
            btnRemoveRegistryCheck.Text = "Remove";
            btnRemoveRegistryCheck.UseVisualStyleBackColor = true;
            btnRemoveRegistryCheck.Click += btnRemoveRegistryCheck_Click;
            
            // grpPCNameValidation
            grpPCNameValidation.Controls.Add(chkEnablePCNameValidation);
            grpPCNameValidation.Controls.Add(lblPCNamePattern);
            grpPCNameValidation.Controls.Add(txtPCNamePattern);
            grpPCNameValidation.Controls.Add(lblPatternHelp);
            grpPCNameValidation.Controls.Add(btnTestPattern);
            grpPCNameValidation.Location = new Point(10, 370);
            grpPCNameValidation.Name = "grpPCNameValidation";
            grpPCNameValidation.Size = new Size(770, 120);
            grpPCNameValidation.TabIndex = 4;
            grpPCNameValidation.TabStop = false;
            grpPCNameValidation.Text = "PC Name Validation";
            
            // chkEnablePCNameValidation
            chkEnablePCNameValidation.AutoSize = true;
            chkEnablePCNameValidation.Location = new Point(15, 25);
            chkEnablePCNameValidation.Name = "chkEnablePCNameValidation";
            chkEnablePCNameValidation.Size = new Size(200, 24);
            chkEnablePCNameValidation.TabIndex = 0;
            chkEnablePCNameValidation.Text = "Enable PC Name Validation";
            chkEnablePCNameValidation.UseVisualStyleBackColor = true;
            chkEnablePCNameValidation.CheckedChanged += chkEnablePCNameValidation_CheckedChanged;
            
            // lblPCNamePattern
            lblPCNamePattern.AutoSize = true;
            lblPCNamePattern.Location = new Point(15, 55);
            lblPCNamePattern.Name = "lblPCNamePattern";
            lblPCNamePattern.Size = new Size(120, 20);
            lblPCNamePattern.TabIndex = 1;
            lblPCNamePattern.Text = "PC Name Pattern:";
            
            // txtPCNamePattern
            txtPCNamePattern.Location = new Point(140, 52);
            txtPCNamePattern.Name = "txtPCNamePattern";
            txtPCNamePattern.Size = new Size(200, 27);
            txtPCNamePattern.TabIndex = 2;
            txtPCNamePattern.PlaceholderText = "e.g., AA######";
            
            // lblPatternHelp
            lblPatternHelp.AutoSize = true;
            lblPatternHelp.Location = new Point(15, 85);
            lblPatternHelp.Name = "lblPatternHelp";
            lblPatternHelp.Size = new Size(450, 20);
            lblPatternHelp.TabIndex = 3;
            lblPatternHelp.Text = "Pattern format: A = Letter (A-Z), # = Digit (0-9). Example: AA###### = PC123456";
            
            // btnTestPattern
            btnTestPattern.Location = new Point(350, 50);
            btnTestPattern.Name = "btnTestPattern";
            btnTestPattern.Size = new Size(100, 30);
            btnTestPattern.TabIndex = 4;
            btnTestPattern.Text = "Test Pattern";
            btnTestPattern.UseVisualStyleBackColor = true;
            btnTestPattern.Click += btnTestPattern_Click;
            
            // btnSave
            btnSave.Location = new Point(620, 500);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            
            // btnCancel
            btnCancel.Location = new Point(705, 500);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // SettingsForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 540);
            Controls.Add(grpStandardChecks);
            Controls.Add(grpRegistryChecks);
            Controls.Add(grpPCNameValidation);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "PC Inventory Settings";
            grpStandardChecks.ResumeLayout(false);
            grpStandardChecks.PerformLayout();
            grpRegistryChecks.ResumeLayout(false);
            grpPCNameValidation.ResumeLayout(false);
            grpPCNameValidation.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpStandardChecks;
        private FlowLayoutPanel flpStandardChecks;
        private CheckBox chkSerialNumber;
        private CheckBox chkWindowsVersion;
    private CheckBox chkInstallDate;
        private CheckBox chkBIOSVersion;
        private CheckBox chkModel;
        private CheckBox chkMake;
        private CheckBox chkLastRebootTime;
        private CheckBox chkLoggedOnUser;
        private CheckBox chkMACAddress;
        private CheckBox chkIPAddress;
        private CheckBox chkTotalRAM;
        private CheckBox chkFreeHDDSpace;
        private CheckBox chkHDDSize;
        private CheckBox chkPendingReboot;
        private CheckBox chkNetworkConnectionType;
        private CheckBox chkWiFiInfo;
        private Button btnSelectAll;
        private Button btnDeselectAll;
        private GroupBox grpRegistryChecks;
        private ListBox lstRegistryChecks;
        private Button btnAddRegistryCheck;
        private Button btnEditRegistryCheck;
        private Button btnRemoveRegistryCheck;
        private GroupBox grpPCNameValidation;
        private CheckBox chkEnablePCNameValidation;
        private TextBox txtPCNamePattern;
        private Label lblPCNamePattern;
        private Label lblPatternHelp;
        private Button btnTestPattern;
        private Button btnSave;
        private Button btnCancel;
    }
}