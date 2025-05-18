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
            chkSerialNumber = new CheckBox();
            chkWindowsVersion = new CheckBox();
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
            btnSelectAll = new Button();
            btnDeselectAll = new Button();
            grpRegistryChecks = new GroupBox();
            lstRegistryChecks = new ListBox();
            btnAddRegistryCheck = new Button();
            btnEditRegistryCheck = new Button();
            btnRemoveRegistryCheck = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            
            grpStandardChecks.SuspendLayout();
            grpRegistryChecks.SuspendLayout();
            SuspendLayout();
            
            // grpStandardChecks
            grpStandardChecks.Controls.Add(chkSerialNumber);
            grpStandardChecks.Controls.Add(chkWindowsVersion);
            grpStandardChecks.Controls.Add(chkBIOSVersion);
            grpStandardChecks.Controls.Add(chkModel);
            grpStandardChecks.Controls.Add(chkMake);
            grpStandardChecks.Controls.Add(chkLastRebootTime);
            grpStandardChecks.Controls.Add(chkLoggedOnUser);
            grpStandardChecks.Controls.Add(chkMACAddress);
            grpStandardChecks.Controls.Add(chkIPAddress);
            grpStandardChecks.Controls.Add(chkTotalRAM);
            grpStandardChecks.Controls.Add(chkFreeHDDSpace);
            grpStandardChecks.Controls.Add(chkHDDSize);
            grpStandardChecks.Controls.Add(chkPendingReboot);
            grpStandardChecks.Controls.Add(btnSelectAll);
            grpStandardChecks.Controls.Add(btnDeselectAll);
            grpStandardChecks.Location = new Point(10, 10);
            grpStandardChecks.Name = "grpStandardChecks";
            grpStandardChecks.Size = new Size(380, 350);
            grpStandardChecks.TabIndex = 0;
            grpStandardChecks.TabStop = false;
            grpStandardChecks.Text = "Standard Health Checks";
            
            // chkSerialNumber
            chkSerialNumber.AutoSize = true;
            chkSerialNumber.Location = new Point(20, 290);
            chkSerialNumber.Name = "chkSerialNumber";
            chkSerialNumber.Size = new Size(129, 24);
            chkSerialNumber.TabIndex = 13;
            chkSerialNumber.Text = "Serial Number";
            chkSerialNumber.UseVisualStyleBackColor = true;
            
            // chkWindowsVersion
            chkWindowsVersion.AutoSize = true;
            chkWindowsVersion.Location = new Point(20, 260);
            chkWindowsVersion.Name = "chkWindowsVersion";
            chkWindowsVersion.Size = new Size(147, 24);
            chkWindowsVersion.TabIndex = 12;
            chkWindowsVersion.Text = "Windows Version";
            chkWindowsVersion.UseVisualStyleBackColor = true;
            
            // chkBIOSVersion
            chkBIOSVersion.AutoSize = true;
            chkBIOSVersion.Location = new Point(20, 230);
            chkBIOSVersion.Name = "chkBIOSVersion";
            chkBIOSVersion.Size = new Size(116, 24);
            chkBIOSVersion.TabIndex = 11;
            chkBIOSVersion.Text = "BIOS Version";
            chkBIOSVersion.UseVisualStyleBackColor = true;
            
            // chkModel
            chkModel.AutoSize = true;
            chkModel.Location = new Point(20, 200);
            chkModel.Name = "chkModel";
            chkModel.Size = new Size(74, 24);
            chkModel.TabIndex = 10;
            chkModel.Text = "Model";
            chkModel.UseVisualStyleBackColor = true;
            
            // chkMake
            chkMake.AutoSize = true;
            chkMake.Location = new Point(20, 170);
            chkMake.Name = "chkMake";
            chkMake.Size = new Size(69, 24);
            chkMake.TabIndex = 9;
            chkMake.Text = "Make";
            chkMake.UseVisualStyleBackColor = true;
            
            // chkLastRebootTime
            chkLastRebootTime.AutoSize = true;
            chkLastRebootTime.Location = new Point(20, 140);
            chkLastRebootTime.Name = "chkLastRebootTime";
            chkLastRebootTime.Size = new Size(144, 24);
            chkLastRebootTime.TabIndex = 8;
            chkLastRebootTime.Text = "Last Reboot Time";
            chkLastRebootTime.UseVisualStyleBackColor = true;
            
            // chkLoggedOnUser
            chkLoggedOnUser.AutoSize = true;
            chkLoggedOnUser.Location = new Point(20, 110);
            chkLoggedOnUser.Name = "chkLoggedOnUser";
            chkLoggedOnUser.Size = new Size(140, 24);
            chkLoggedOnUser.TabIndex = 7;
            chkLoggedOnUser.Text = "Logged-on User";
            chkLoggedOnUser.UseVisualStyleBackColor = true;
            
            // chkMACAddress
            chkMACAddress.AutoSize = true;
            chkMACAddress.Location = new Point(20, 80);
            chkMACAddress.Name = "chkMACAddress";
            chkMACAddress.Size = new Size(120, 24);
            chkMACAddress.TabIndex = 6;
            chkMACAddress.Text = "MAC Address";
            chkMACAddress.UseVisualStyleBackColor = true;
            
            // chkIPAddress
            chkIPAddress.AutoSize = true;
            chkIPAddress.Location = new Point(20, 50);
            chkIPAddress.Name = "chkIPAddress";
            chkIPAddress.Size = new Size(101, 24);
            chkIPAddress.TabIndex = 5;
            chkIPAddress.Text = "IP Address";
            chkIPAddress.UseVisualStyleBackColor = true;
            
            // chkTotalRAM
            chkTotalRAM.AutoSize = true;
            chkTotalRAM.Location = new Point(200, 50);
            chkTotalRAM.Name = "chkTotalRAM";
            chkTotalRAM.Size = new Size(103, 24);
            chkTotalRAM.TabIndex = 4;
            chkTotalRAM.Text = "Total RAM";
            chkTotalRAM.UseVisualStyleBackColor = true;
            
            // chkFreeHDDSpace
            chkFreeHDDSpace.AutoSize = true;
            chkFreeHDDSpace.Location = new Point(200, 80);
            chkFreeHDDSpace.Name = "chkFreeHDDSpace";
            chkFreeHDDSpace.Size = new Size(145, 24);
            chkFreeHDDSpace.TabIndex = 3;
            chkFreeHDDSpace.Text = "Free HDD Space";
            chkFreeHDDSpace.UseVisualStyleBackColor = true;
            
            // chkHDDSize
            chkHDDSize.AutoSize = true;
            chkHDDSize.Location = new Point(200, 110);
            chkHDDSize.Name = "chkHDDSize";
            chkHDDSize.Size = new Size(93, 24);
            chkHDDSize.TabIndex = 2;
            chkHDDSize.Text = "HDD Size";
            chkHDDSize.UseVisualStyleBackColor = true;

            // chkPendingReboot
            chkPendingReboot.AutoSize = true;
            chkPendingReboot.Location = new Point(200, 140);
            chkPendingReboot.Name = "chkPendingReboot";
            chkPendingReboot.Size = new Size(140, 24);
            chkPendingReboot.TabIndex = 13;
            chkPendingReboot.Text = "Pending Reboot";
            chkPendingReboot.UseVisualStyleBackColor = true;
            
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
            
            // btnSave
            btnSave.Location = new Point(620, 370);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            
            // btnCancel
            btnCancel.Location = new Point(705, 370);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // SettingsForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 410);
            Controls.Add(grpStandardChecks);
            Controls.Add(grpRegistryChecks);
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
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpStandardChecks;
        private CheckBox chkSerialNumber;
        private CheckBox chkWindowsVersion;
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
        private Button btnSelectAll;
        private Button btnDeselectAll;
        private GroupBox grpRegistryChecks;
        private ListBox lstRegistryChecks;
        private Button btnAddRegistryCheck;
        private Button btnEditRegistryCheck;
        private Button btnRemoveRegistryCheck;
        private Button btnSave;
        private Button btnCancel;
    }
}