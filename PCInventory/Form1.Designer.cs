namespace PCInventory;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private ToolStripMenuItem pastePCListToolStripMenuItem = null!;

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
        pastePCListToolStripMenuItem = new ToolStripMenuItem();
        exportToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        scanSinglePCToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator3 = new ToolStripSeparator();
        settingsToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        exitToolStripMenuItem = new ToolStripMenuItem();
        helpToolStripMenuItem = new ToolStripMenuItem();
        aboutToolStripMenuItem = new ToolStripMenuItem();
        openLogFolderToolStripMenuItem = new ToolStripMenuItem();
        statusStrip = new StatusStrip();
        toolStripStatusLabel = new ToolStripStatusLabel();
        toolStripProgressBar = new ToolStripProgressBar();
        dataGridView = new DataGridView();
        pnlControls = new FlowLayoutPanel();
        contentLayout = new TableLayoutPanel();
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
        contentLayout.SuspendLayout();
        SuspendLayout();
        
        // menuStrip
        menuStrip.ImageScalingSize = new Size(20, 20);
        menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1100, 28);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip1";
        
        // fileToolStripMenuItem
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 
            importToolStripMenuItem,
            pastePCListToolStripMenuItem,
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
        importToolStripMenuItem.Text = "Import PCs from File...";
        importToolStripMenuItem.Click += importToolStripMenuItem_Click;
        
        // pastePCListToolStripMenuItem
        pastePCListToolStripMenuItem.Name = "pastePCListToolStripMenuItem";
        pastePCListToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
        pastePCListToolStripMenuItem.Size = new Size(270, 26);
        pastePCListToolStripMenuItem.Text = "Paste PC List...";
        pastePCListToolStripMenuItem.Click += pastePCListToolStripMenuItem_Click;
        
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

        // helpToolStripMenuItem
        helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            aboutToolStripMenuItem,
            openLogFolderToolStripMenuItem });
        helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        helpToolStripMenuItem.Size = new Size(55, 24);
        helpToolStripMenuItem.Text = "Help";

        // aboutToolStripMenuItem
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        aboutToolStripMenuItem.Size = new Size(200, 26);
        aboutToolStripMenuItem.Text = "About";
        aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;

        // openLogFolderToolStripMenuItem
        openLogFolderToolStripMenuItem.Name = "openLogFolderToolStripMenuItem";
        openLogFolderToolStripMenuItem.Size = new Size(200, 26);
        openLogFolderToolStripMenuItem.Text = "Open Log Folder";
        openLogFolderToolStripMenuItem.Click += openLogFolderToolStripMenuItem_Click;
        
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
        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView.Dock = DockStyle.Fill;
        dataGridView.Location = new Point(15, 15);
        dataGridView.Margin = new Padding(0, 0, 0, 6);
        dataGridView.Name = "dataGridView";
        dataGridView.ReadOnly = true;
        dataGridView.RowHeadersWidth = 51;
        dataGridView.RowTemplate.Height = 29;
        dataGridView.Size = new Size(1070, 442);
        dataGridView.TabIndex = 2;
        dataGridView.ScrollBars = ScrollBars.Both;

        // pnlControls
        pnlControls.Anchor = AnchorStyles.Right;
        pnlControls.AutoSize = true;
        pnlControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlControls.FlowDirection = FlowDirection.RightToLeft;
        pnlControls.WrapContents = false;
        pnlControls.Controls.Add(btnStop);
        pnlControls.Controls.Add(btnScan);
        pnlControls.Location = new Point(985, 463);
        pnlControls.Margin = new Padding(0, 0, 0, 0);
        pnlControls.Name = "pnlControls";
        pnlControls.Padding = new Padding(0);
        pnlControls.TabIndex = 3;

        // contentLayout
        contentLayout.AutoSize = false;
        contentLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        contentLayout.ColumnCount = 2;
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        contentLayout.Controls.Add(dataGridView, 0, 0);
        contentLayout.Controls.Add(pnlControls, 1, 1);
        contentLayout.Dock = DockStyle.Fill;
        contentLayout.Location = new Point(12, 40);
        contentLayout.Margin = new Padding(0, 0, 0, 0);
        contentLayout.Name = "contentLayout";
        contentLayout.Padding = new Padding(12, 12, 12, 12);
        contentLayout.RowCount = 2;
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contentLayout.Size = new Size(1076, 496);
        contentLayout.TabIndex = 4;
        contentLayout.SetColumnSpan(dataGridView, 2);

        // btnScan
        btnScan.AutoSize = true;
        btnScan.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnScan.MinimumSize = new Size(75, 32);
        btnScan.Name = "btnScan";
        btnScan.Padding = new Padding(12, 4, 12, 4);
        btnScan.Margin = new Padding(6, 0, 0, 0);
        btnScan.TabIndex = 0;
        btnScan.Text = "Scan";
        btnScan.UseVisualStyleBackColor = true;
        btnScan.Click += btnScan_Click;
        
        // btnStop
        btnStop.AutoSize = true;
        btnStop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        btnStop.Enabled = false;
        btnStop.MinimumSize = new Size(75, 32);
        btnStop.Name = "btnStop";
        btnStop.Padding = new Padding(12, 4, 12, 4);
        btnStop.Margin = new Padding(0, 0, 0, 0);
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
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1100, 600);
        Controls.Add(contentLayout);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(800, 500);
        Name = "Form1";
        Text = "PC Inventory";
        Load += Form1_Load;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
        pnlControls.ResumeLayout(false);
        pnlControls.PerformLayout();
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
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
    private FlowLayoutPanel pnlControls;
    private TableLayoutPanel contentLayout;
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
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private ToolStripMenuItem openLogFolderToolStripMenuItem;
}
