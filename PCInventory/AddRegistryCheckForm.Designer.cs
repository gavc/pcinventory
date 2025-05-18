namespace PCInventory
{
    partial class AddRegistryCheckForm
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
            lblFriendlyName = new Label();
            txtFriendlyName = new TextBox();
            lblKeyPath = new Label();
            txtKeyPath = new TextBox();
            lblValueName = new Label();
            txtValueName = new TextBox();
            chkEnabled = new CheckBox();
            btnOK = new Button();
            btnCancel = new Button();
            SuspendLayout();
            
            // lblFriendlyName
            lblFriendlyName.AutoSize = true;
            lblFriendlyName.Location = new Point(12, 15);
            lblFriendlyName.Name = "lblFriendlyName";
            lblFriendlyName.Size = new Size(96, 20);
            lblFriendlyName.TabIndex = 0;
            lblFriendlyName.Text = "Friendly Name:";
            
            // txtFriendlyName
            txtFriendlyName.Location = new Point(140, 12);
            txtFriendlyName.Name = "txtFriendlyName";
            txtFriendlyName.Size = new Size(320, 27);
            txtFriendlyName.TabIndex = 1;
            
            // lblKeyPath
            lblKeyPath.AutoSize = true;
            lblKeyPath.Location = new Point(12, 55);
            lblKeyPath.Name = "lblKeyPath";
            lblKeyPath.Size = new Size(68, 20);
            lblKeyPath.TabIndex = 2;
            lblKeyPath.Text = "Key Path:";
            
            // txtKeyPath
            txtKeyPath.Location = new Point(140, 52);
            txtKeyPath.Name = "txtKeyPath";
            txtKeyPath.Size = new Size(320, 27);
            txtKeyPath.TabIndex = 3;
            txtKeyPath.Text = "HKEY_LOCAL_MACHINE\\";
            
            // lblValueName
            lblValueName.AutoSize = true;
            lblValueName.Location = new Point(12, 95);
            lblValueName.Name = "lblValueName";
            lblValueName.Size = new Size(91, 20);
            lblValueName.TabIndex = 4;
            lblValueName.Text = "Value Name:";
            
            // txtValueName
            txtValueName.Location = new Point(140, 92);
            txtValueName.Name = "txtValueName";
            txtValueName.Size = new Size(320, 27);
            txtValueName.TabIndex = 5;
            
            // chkEnabled
            chkEnabled.AutoSize = true;
            chkEnabled.Checked = true;
            chkEnabled.CheckState = CheckState.Checked;
            chkEnabled.Location = new Point(140, 135);
            chkEnabled.Name = "chkEnabled";
            chkEnabled.Size = new Size(84, 24);
            chkEnabled.TabIndex = 6;
            chkEnabled.Text = "Enabled";
            chkEnabled.UseVisualStyleBackColor = true;
            
            // btnOK
            btnOK.Location = new Point(275, 175);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(90, 30);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            
            // btnCancel
            btnCancel.Location = new Point(370, 175);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 30);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // AddRegistryCheckForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(472, 223);
            Controls.Add(lblFriendlyName);
            Controls.Add(txtFriendlyName);
            Controls.Add(lblKeyPath);
            Controls.Add(txtKeyPath);
            Controls.Add(lblValueName);
            Controls.Add(txtValueName);
            Controls.Add(chkEnabled);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddRegistryCheckForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registry Check";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFriendlyName;
        private TextBox txtFriendlyName;
        private Label lblKeyPath;
        private TextBox txtKeyPath;
        private Label lblValueName;
        private TextBox txtValueName;
        private CheckBox chkEnabled;
        private Button btnOK;
        private Button btnCancel;
    }
}