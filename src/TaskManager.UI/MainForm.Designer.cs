#nullable enable
namespace TaskManager.UI;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        filterPanel = new Panel();
        lblStatus = new Label();
        cmbStatusFilter = new ComboBox();
        lblPriority = new Label();
        cmbPriorityFilter = new ComboBox();
        btnRefresh = new Button();
        gridTasks = new DataGridView();
        buttonPanel = new Panel();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnComplete = new Button();
        btnReport = new Button();
        filterPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridTasks).BeginInit();
        buttonPanel.SuspendLayout();
        SuspendLayout();
        //
        // filterPanel
        //
        filterPanel.Controls.Add(lblStatus);
        filterPanel.Controls.Add(cmbStatusFilter);
        filterPanel.Controls.Add(lblPriority);
        filterPanel.Controls.Add(cmbPriorityFilter);
        filterPanel.Controls.Add(btnRefresh);
        filterPanel.Dock = DockStyle.Top;
        filterPanel.Height = 48;
        filterPanel.Padding = new Padding(8);
        //
        // lblStatus
        //
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(12, 16);
        lblStatus.Text = "Status:";
        //
        // cmbStatusFilter
        //
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatusFilter.Location = new Point(68, 12);
        cmbStatusFilter.Size = new Size(140, 23);
        //
        // lblPriority
        //
        lblPriority.AutoSize = true;
        lblPriority.Location = new Point(224, 16);
        lblPriority.Text = "Prioritate:";
        //
        // cmbPriorityFilter
        //
        cmbPriorityFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbPriorityFilter.Location = new Point(300, 12);
        cmbPriorityFilter.Size = new Size(140, 23);
        //
        // btnRefresh
        //
        btnRefresh.Location = new Point(460, 10);
        btnRefresh.Size = new Size(100, 28);
        btnRefresh.Text = "Filtrează";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += BtnRefresh_Click;
        //
        // gridTasks
        //
        gridTasks.AllowUserToAddRows = false;
        gridTasks.AllowUserToDeleteRows = false;
        gridTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridTasks.Dock = DockStyle.Fill;
        gridTasks.MultiSelect = false;
        gridTasks.ReadOnly = true;
        gridTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //
        // buttonPanel
        //
        buttonPanel.Controls.Add(btnAdd);
        buttonPanel.Controls.Add(btnEdit);
        buttonPanel.Controls.Add(btnDelete);
        buttonPanel.Controls.Add(btnComplete);
        buttonPanel.Controls.Add(btnReport);
        buttonPanel.Dock = DockStyle.Bottom;
        buttonPanel.Height = 52;
        buttonPanel.Padding = new Padding(8);
        //
        // btnAdd
        //
        btnAdd.Location = new Point(12, 12);
        btnAdd.Size = new Size(100, 28);
        btnAdd.Text = "Adaugă";
        btnAdd.Click += BtnAdd_Click;
        //
        // btnEdit
        //
        btnEdit.Location = new Point(120, 12);
        btnEdit.Size = new Size(100, 28);
        btnEdit.Text = "Editează";
        btnEdit.Click += BtnEdit_Click;
        //
        // btnDelete
        //
        btnDelete.Location = new Point(228, 12);
        btnDelete.Size = new Size(100, 28);
        btnDelete.Text = "Șterge";
        btnDelete.Click += BtnDelete_Click;
        //
        // btnComplete
        //
        btnComplete.Location = new Point(336, 12);
        btnComplete.Size = new Size(120, 28);
        btnComplete.Text = "Finalizează";
        btnComplete.Click += BtnComplete_Click;
        //
        // btnReport
        //
        btnReport.Location = new Point(464, 12);
        btnReport.Size = new Size(100, 28);
        btnReport.Text = "Raport";
        btnReport.UseVisualStyleBackColor = true;
        btnReport.Click += BtnReport_Click;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 560);
        Controls.Add(gridTasks);
        Controls.Add(buttonPanel);
        Controls.Add(filterPanel);
        MinimumSize = new Size(800, 480);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Task Manager — Laborator 4";
        filterPanel.ResumeLayout(false);
        filterPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)gridTasks).EndInit();
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Panel filterPanel;
    private Label lblStatus;
    private ComboBox cmbStatusFilter;
    private Label lblPriority;
    private ComboBox cmbPriorityFilter;
    private Button btnRefresh;
    private DataGridView gridTasks;
    private Panel buttonPanel;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnComplete;
    private Button btnReport;
}
