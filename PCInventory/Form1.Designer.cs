namespace PCInventory;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        importToolStripMenuItem = new ToolStripMenuItem();
        exportToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        scanSinglePCToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator3 = new ToolStripSeparator();
        settingsToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        statusStrip = new StatusStrip();
        toolStripStatusLabel = new ToolStripStatusLabel();
        toolStripProgressBar = new ToolStripProgressBar();
        dataGridView = new DataGridView();
        pnlControls = new Panel();
        btnScan = new Button();
        btnStop = new Button();
        openFileDialog = new OpenFileDialog();
        saveFileDialog = new SaveFileDialog();
        gridContextMenu = new ContextMenuStrip(components);
        rescanPCMenuItem = new ToolStripMenuItem();
        restartPCMenuItem = new ToolStripMenuItem();
        copyMenuItem = new ToolStripMenuItem();
        rdpMenuItem = new ToolStripMenuItem();
        openCDriveMenuItem = new ToolStripMenuItem();
        gpUpdateMenuItem = new ToolStripMenuItem();
        pingMenuItem = new ToolStripMenuItem();
        tracertMenuItem = new ToolStripMenuItem();
        
        menuStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
        pnlControls.SuspendLayout();
        SuspendLayout();
        
        // menuStrip
        menuStrip.ImageScalingSize = new Size(20, 20);
        menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1100, 28);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip1";
        
        // fileToolStripMenuItem
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 
            importToolStripMenuItem, 
            exportToolStripMenuItem, 
            toolStripSeparator1, 
            scanSinglePCToolStripMenuItem, 
            toolStripSeparator3,
            settingsToolStripMenuItem, 
            toolStripSeparator2, 
            exitToolStripMenuItem 
        });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        
        // importToolStripMenuItem
        importToolStripMenuItem.Name = "importToolStripMenuItem";
        importToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        importToolStripMenuItem.Size = new Size(224, 26);
        importToolStripMenuItem.Text = "Import PCs...";
        importToolStripMenuItem.Click += importToolStripMenuItem_Click;
        
        // exportToolStripMenuItem
        exportToolStripMenuItem.Enabled = false;
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        exportToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        exportToolStripMenuItem.Size = new Size(224, 26);
        exportToolStripMenuItem.Text = "Export Results...";
        exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
        
        // toolStripSeparator1
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(221, 6);
        
        // scanSinglePCToolStripMenuItem
        scanSinglePCToolStripMenuItem.Name = "scanSinglePCToolStripMenuItem";
        scanSinglePCToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
        scanSinglePCToolStripMenuItem.Size = new Size(224, 26);
        scanSinglePCToolStripMenuItem.Text = "Scan PC...";
        scanSinglePCToolStripMenuItem.Click += scanSinglePCToolStripMenuItem_Click;
        
        // toolStripSeparator3
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new Size(221, 6);
        
        // settingsToolStripMenuItem
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        settingsToolStripMenuItem.Size = new Size(224, 26);
        settingsToolStripMenuItem.Text = "Settings...";
        settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
        
        // toolStripSeparator2
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(221, 6);
        
        // exitToolStripMenuItem
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
        exitToolStripMenuItem.Size = new Size(224, 26);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        
        // statusStrip
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
        statusStrip.Location = new Point(0, 528);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1100, 26);
        statusStrip.TabIndex = 1;
        statusStrip.Text = "statusStrip1";
        
        // toolStripStatusLabel
        toolStripStatusLabel.Name = "toolStripStatusLabel";
        toolStripStatusLabel.Size = new Size(50, 20);
        toolStripStatusLabel.Text = "Ready";
        
        // toolStripProgressBar
        toolStripProgressBar.Name = "toolStripProgressBar";
        toolStripProgressBar.Size = new Size(150, 18);
        toolStripProgressBar.Visible = false;
        
        // dataGridView
        dataGridView.AllowUserToAddRows = false;
        dataGridView.AllowUserToDeleteRows = false;
        dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView.Location = new Point(12, 40);
        dataGridView.Name = "dataGridView";
        dataGridView.ReadOnly = true;
        dataGridView.RowHeadersWidth = 51;
        dataGridView.RowTemplate.Height = 29;
        dataGridView.Size = new Size(1076, 428);
        dataGridView.TabIndex = 2;
        dataGridView.ScrollBars = ScrollBars.Both;
        
        // pnlControls
        pnlControls.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        pnlControls.Controls.Add(btnScan);
        pnlControls.Controls.Add(btnStop);
        pnlControls.Location = new Point(828, 474);
        pnlControls.Name = "pnlControls";
        pnlControls.Size = new Size(260, 51);
        pnlControls.TabIndex = 3;
        
        // btnScan
        btnScan.Location = new Point(79, 3);
        btnScan.Name = "btnScan";
        btnScan.Size = new Size(80, 40);
        btnScan.TabIndex = 0;
        btnScan.Text = "Scan";
        btnScan.UseVisualStyleBackColor = true;
        btnScan.Click += btnScan_Click;
        
        // btnStop
        btnStop.Enabled = false;
        btnStop.Location = new Point(165, 3);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(80, 40);
        btnStop.TabIndex = 1;
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = true;
        btnStop.Click += btnStop_Click;
        
        // openFileDialog
        openFileDialog.DefaultExt = "txt";
        openFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
        openFileDialog.Title = "Import PC List";
        
        // saveFileDialog
        saveFileDialog.DefaultExt = "csv";
        saveFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";
        saveFileDialog.Title = "Export Results";
        
        // gridContextMenu
        gridContextMenu.ImageScalingSize = new Size(20, 20);
        gridContextMenu.Items.AddRange(new ToolStripItem[] { copyMenuItem, new ToolStripSeparator(), rescanPCMenuItem, restartPCMenuItem, rdpMenuItem, openCDriveMenuItem, gpUpdateMenuItem, pingMenuItem, tracertMenuItem });
        gridContextMenu.Name = "gridContextMenu";
        gridContextMenu.Size = new Size(181, 82);
        
        // copyMenuItem
        copyMenuItem.Name = "copyMenuItem";
        copyMenuItem.Size = new Size(180, 24);
        copyMenuItem.Text = "Copy";
        copyMenuItem.ShortcutKeys = Keys.Control | Keys.C;
        copyMenuItem.Click += copyMenuItem_Click;
        
        // rescanPCMenuItem
        rescanPCMenuItem.Name = "rescanPCMenuItem";
        rescanPCMenuItem.Size = new Size(180, 24);
        rescanPCMenuItem.Text = "Rescan PC";
        rescanPCMenuItem.Click += rescanPCMenuItem_Click;
        
        // restartPCMenuItem
        restartPCMenuItem.Name = "restartPCMenuItem";
        restartPCMenuItem.Size = new Size(180, 24);
        restartPCMenuItem.Text = "Restart PC";
        restartPCMenuItem.Click += restartPCMenuItem_Click;

        // rdpMenuItem
        rdpMenuItem.Name = "rdpMenuItem";
        rdpMenuItem.Size = new Size(180, 24);
        rdpMenuItem.Text = "RDP";
        rdpMenuItem.Click += rdpMenuItem_Click;

        // openCDriveMenuItem
        openCDriveMenuItem.Name = "openCDriveMenuItem";
        openCDriveMenuItem.Size = new Size(180, 24);
        openCDriveMenuItem.Text = "Open C Drive";
        openCDriveMenuItem.Click += openCDriveMenuItem_Click;

        // gpUpdateMenuItem
        gpUpdateMenuItem.Name = "gpUpdateMenuItem";
        gpUpdateMenuItem.Size = new Size(180, 24);
        gpUpdateMenuItem.Text = "GP Update";
        gpUpdateMenuItem.Click += gpUpdateMenuItem_Click;

        // pingMenuItem
        pingMenuItem.Name = "pingMenuItem";
        pingMenuItem.Size = new Size(180, 24);
        pingMenuItem.Text = "Ping";
        pingMenuItem.Click += pingMenuItem_Click;

        // tracertMenuItem
        tracertMenuItem.Name = "tracertMenuItem";
        tracertMenuItem.Size = new Size(180, 24);
        tracertMenuItem.Text = "Tracert";
        tracertMenuItem.Click += tracertMenuItem_Click;
        
        // Form1
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 554);
        Controls.Add(pnlControls);
        Controls.Add(dataGridView);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        Name = "Form1";
        Text = "PC Inventory";
        Load += Form1_Load;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        pnlControls.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem importToolStripMenuItem;
    private ToolStripMenuItem exportToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem scanSinglePCToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem settingsToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem exitToolStripMenuItem;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel toolStripStatusLabel;
    private ToolStripProgressBar toolStripProgressBar;
    private DataGridView dataGridView;
    private Panel pnlControls;
    private Button btnScan;
    private Button btnStop;
    private OpenFileDialog openFileDialog;
    private SaveFileDialog saveFileDialog;
    private ContextMenuStrip gridContextMenu;
    private ToolStripMenuItem rescanPCMenuItem;
    private ToolStripMenuItem restartPCMenuItem;
    private ToolStripMenuItem copyMenuItem;
    private ToolStripMenuItem rdpMenuItem;
    private ToolStripMenuItem openCDriveMenuItem;
    private ToolStripMenuItem gpUpdateMenuItem;
    private ToolStripMenuItem pingMenuItem;
    private ToolStripMenuItem tracertMenuItem;
}
