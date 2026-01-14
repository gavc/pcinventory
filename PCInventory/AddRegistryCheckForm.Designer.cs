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
            tableLayoutPanel = new TableLayoutPanel();
            lblFriendlyName = new Label();
            txtFriendlyName = new TextBox();
            lblKeyPath = new Label();
            txtKeyPath = new TextBox();
            lblValueName = new Label();
            txtValueName = new TextBox();
            chkEnabled = new CheckBox();
            btnOK = new Button();
            btnCancel = new Button();
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            
            // tableLayoutPanel
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(lblFriendlyName, 0, 0);
            tableLayoutPanel.Controls.Add(txtFriendlyName, 1, 0);
            tableLayoutPanel.Controls.Add(lblKeyPath, 0, 1);
            tableLayoutPanel.Controls.Add(txtKeyPath, 1, 1);
            tableLayoutPanel.Controls.Add(lblValueName, 0, 2);
            tableLayoutPanel.Controls.Add(txtValueName, 1, 2);
            tableLayoutPanel.Controls.Add(chkEnabled, 1, 3);
            tableLayoutPanel.Dock = DockStyle.Top;
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new Padding(5);
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.RowStyles.Add(new RowStyle());
            tableLayoutPanel.TabIndex = 0;
            
            // lblFriendlyName
            lblFriendlyName.Anchor = AnchorStyles.Left;
            lblFriendlyName.AutoSize = true;
            lblFriendlyName.Location = new Point(8, 11);
            lblFriendlyName.Margin = new Padding(3, 5, 3, 5);
            lblFriendlyName.Name = "lblFriendlyName";
            lblFriendlyName.Size = new Size(96, 20);
            lblFriendlyName.TabIndex = 0;
            lblFriendlyName.Text = "Friendly Name:";
            
            // txtFriendlyName
            txtFriendlyName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtFriendlyName.Location = new Point(110, 8);
            txtFriendlyName.Margin = new Padding(3, 5, 3, 5);
            txtFriendlyName.MinimumSize = new Size(300, 27);
            txtFriendlyName.Name = "txtFriendlyName";
            txtFriendlyName.Size = new Size(300, 27);
            txtFriendlyName.TabIndex = 1;
            
            // lblKeyPath
            lblKeyPath.Anchor = AnchorStyles.Left;
            lblKeyPath.AutoSize = true;
            lblKeyPath.Location = new Point(8, 51);
            lblKeyPath.Margin = new Padding(3, 5, 3, 5);
            lblKeyPath.Name = "lblKeyPath";
            lblKeyPath.Size = new Size(68, 20);
            lblKeyPath.TabIndex = 2;
            lblKeyPath.Text = "Key Path:";
            
            // txtKeyPath
            txtKeyPath.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtKeyPath.Location = new Point(110, 48);
            txtKeyPath.Margin = new Padding(3, 5, 3, 5);
            txtKeyPath.MinimumSize = new Size(300, 27);
            txtKeyPath.Name = "txtKeyPath";
            txtKeyPath.Size = new Size(300, 27);
            txtKeyPath.TabIndex = 3;
            txtKeyPath.Text = "HKEY_LOCAL_MACHINE\\";
            
            // lblValueName
            lblValueName.Anchor = AnchorStyles.Left;
            lblValueName.AutoSize = true;
            lblValueName.Location = new Point(8, 91);
            lblValueName.Margin = new Padding(3, 5, 3, 5);
            lblValueName.Name = "lblValueName";
            lblValueName.Size = new Size(91, 20);
            lblValueName.TabIndex = 4;
            lblValueName.Text = "Value Name:";
            
            // txtValueName
            txtValueName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtValueName.Location = new Point(110, 88);
            txtValueName.Margin = new Padding(3, 5, 3, 5);
            txtValueName.MinimumSize = new Size(300, 27);
            txtValueName.Name = "txtValueName";
            txtValueName.Size = new Size(300, 27);
            txtValueName.TabIndex = 5;
            
            // chkEnabled
            chkEnabled.Anchor = AnchorStyles.Left;
            chkEnabled.AutoSize = true;
            chkEnabled.Checked = true;
            chkEnabled.CheckState = CheckState.Checked;
            chkEnabled.Location = new Point(110, 128);
            chkEnabled.Margin = new Padding(3, 5, 3, 5);
            chkEnabled.Name = "chkEnabled";
            chkEnabled.Size = new Size(84, 24);
            chkEnabled.TabIndex = 6;
            chkEnabled.Text = "Enabled";
            chkEnabled.UseVisualStyleBackColor = true;
            
            // btnOK
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.AutoSize = true;
            btnOK.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOK.Location = new Point(275, 175);
            btnOK.MinimumSize = new Size(90, 30);
            btnOK.Name = "btnOK";
            btnOK.Padding = new Padding(10, 3, 10, 3);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            
            // btnCancel
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.AutoSize = true;
            btnCancel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnCancel.Location = new Point(370, 175);
            btnCancel.MinimumSize = new Size(90, 30);
            btnCancel.Name = "btnCancel";
            btnCancel.Padding = new Padding(10, 3, 10, 3);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            
            // AddRegistryCheckForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(472, 223);
            Controls.Add(tableLayoutPanel);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(450, 200);
            Name = "AddRegistryCheckForm";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registry Check";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
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