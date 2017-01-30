namespace CountDown
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tile4TextBox = new System.Windows.Forms.TextBox();
            this.Tile3TextBox = new System.Windows.Forms.TextBox();
            this.Tile2TextBox = new System.Windows.Forms.TextBox();
            this.Tile1TextBox = new System.Windows.Forms.TextBox();
            this.TileOptionsComboBox = new System.Windows.Forms.ComboBox();
            this.ChooseButton = new System.Windows.Forms.Button();
            this.SolveButton = new System.Windows.Forms.Button();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TargetTextBox = new System.Windows.Forms.TextBox();
            this.TimerButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox2.SuspendLayout();
            this.ListContextMenuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Tile4TextBox);
            this.groupBox2.Controls.Add(this.Tile3TextBox);
            this.groupBox2.Controls.Add(this.Tile2TextBox);
            this.groupBox2.Controls.Add(this.Tile1TextBox);
            this.groupBox2.Controls.Add(this.TileOptionsComboBox);
            this.groupBox2.Location = new System.Drawing.Point(11, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 119);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "T&iles";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "&Options:";
            // 
            // Tile4TextBox
            // 
            this.Tile4TextBox.Location = new System.Drawing.Point(129, 33);
            this.Tile4TextBox.MaxLength = 3;
            this.Tile4TextBox.Name = "Tile4TextBox";
            this.Tile4TextBox.Size = new System.Drawing.Size(33, 20);
            this.Tile4TextBox.TabIndex = 5;
            this.Tile4TextBox.Text = "100";
            this.Tile4TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Tile4TextBox.TextChanged += new System.EventHandler(this.Card4TextBox_TextChanged);
            this.Tile4TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // Tile3TextBox
            // 
            this.Tile3TextBox.Location = new System.Drawing.Point(91, 33);
            this.Tile3TextBox.MaxLength = 3;
            this.Tile3TextBox.Name = "Tile3TextBox";
            this.Tile3TextBox.Size = new System.Drawing.Size(33, 20);
            this.Tile3TextBox.TabIndex = 4;
            this.Tile3TextBox.Text = "100";
            this.Tile3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Tile3TextBox.TextChanged += new System.EventHandler(this.Card3TextBox_TextChanged);
            this.Tile3TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // Tile2TextBox
            // 
            this.Tile2TextBox.Location = new System.Drawing.Point(53, 33);
            this.Tile2TextBox.MaxLength = 3;
            this.Tile2TextBox.Name = "Tile2TextBox";
            this.Tile2TextBox.Size = new System.Drawing.Size(33, 20);
            this.Tile2TextBox.TabIndex = 3;
            this.Tile2TextBox.Text = "100";
            this.Tile2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Tile2TextBox.TextChanged += new System.EventHandler(this.Card2TextBox_TextChanged);
            this.Tile2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // Tile1TextBox
            // 
            this.Tile1TextBox.Location = new System.Drawing.Point(15, 33);
            this.Tile1TextBox.MaxLength = 3;
            this.Tile1TextBox.Name = "Tile1TextBox";
            this.Tile1TextBox.Size = new System.Drawing.Size(33, 20);
            this.Tile1TextBox.TabIndex = 2;
            this.Tile1TextBox.Text = "100";
            this.Tile1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Tile1TextBox.TextChanged += new System.EventHandler(this.Card1TextBox_TextChanged);
            this.Tile1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // TileOptionsComboBox
            // 
            this.TileOptionsComboBox.CausesValidation = false;
            this.TileOptionsComboBox.Items.AddRange(new object[] {
            "6 small tiles",
            "1 large and 5 small tiles",
            "2 large and 4 small tiles",
            "3 large and 3 small tiles",
            "4 large and 2 small tiles"});
            this.TileOptionsComboBox.Location = new System.Drawing.Point(70, 76);
            this.TileOptionsComboBox.Name = "TileOptionsComboBox";
            this.TileOptionsComboBox.Size = new System.Drawing.Size(148, 21);
            this.TileOptionsComboBox.TabIndex = 9;
            // 
            // ChooseButton
            // 
            this.ChooseButton.Location = new System.Drawing.Point(12, 279);
            this.ChooseButton.Name = "ChooseButton";
            this.ChooseButton.Size = new System.Drawing.Size(65, 23);
            this.ChooseButton.TabIndex = 14;
            this.ChooseButton.Text = "&Choose";
            this.ChooseButton.UseVisualStyleBackColor = true;
            this.ChooseButton.Click += new System.EventHandler(this.ChooseButton_Click);
            // 
            // SolveButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(200, 279);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(60, 23);
            this.SolveButton.TabIndex = 16;
            this.SolveButton.Text = "&Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // ListContextMenuStrip
            // 
            this.ListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.selectAllStripMenuItem});
            this.ListContextMenuStrip.Name = "contextMenuStrip1";
            this.ListContextMenuStrip.Size = new System.Drawing.Size(165, 54);
            this.ListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ListContextMenuStrip_Opening);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllStripMenuItem
            // 
            this.selectAllStripMenuItem.Name = "selectAllStripMenuItem";
            this.selectAllStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllStripMenuItem.Text = "Select All";
            this.selectAllStripMenuItem.Click += new System.EventHandler(this.selectAllStripMenuItem_Click);
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsListBox.ContextMenuStrip = this.ListContextMenuStrip;
            this.ResultsListBox.Location = new System.Drawing.Point(279, 16);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.ScrollAlwaysVisible = true;
            this.ResultsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ResultsListBox.Size = new System.Drawing.Size(246, 290);
            this.ResultsListBox.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TargetTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 139);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 54);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tar&get";
            // 
            // TargetTextBox
            // 
            this.TargetTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetTextBox.Location = new System.Drawing.Point(103, 20);
            this.TargetTextBox.MaxLength = 3;
            this.TargetTextBox.Name = "TargetTextBox";
            this.TargetTextBox.Size = new System.Drawing.Size(45, 20);
            this.TargetTextBox.TabIndex = 11;
            this.TargetTextBox.Text = "999";
            this.TargetTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TargetTextBox.TextChanged += new System.EventHandler(this.TargetTextBox_TextChanged);
            this.TargetTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericTextBox_KeyPress);
            // 
            // TimerButton
            // 
            this.TimerButton.Location = new System.Drawing.Point(96, 279);
            this.TimerButton.Name = "TimerButton";
            this.TimerButton.Size = new System.Drawing.Size(85, 23);
            this.TimerButton.TabIndex = 15;
            this.TimerButton.Text = "Start &Timer";
            this.TimerButton.UseVisualStyleBackColor = true;
            this.TimerButton.Click += new System.EventHandler(this.TimerButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar);
            this.groupBox3.Location = new System.Drawing.Point(11, 203);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(250, 53);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Timer";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 24);
            this.progressBar.Maximum = 30000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(228, 13);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 320);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TimerButton);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ChooseButton);
            this.MinimumSize = new System.Drawing.Size(557, 356);
            this.Name = "MainForm";
            this.Text = "Countdown Number Puzzle";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ListContextMenuStrip.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TileOptionsComboBox;
        private System.Windows.Forms.Button ChooseButton;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ListContextMenuStrip;
        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TargetTextBox;
        private System.Windows.Forms.Button TimerButton;
        private System.Windows.Forms.TextBox Tile4TextBox;
        private System.Windows.Forms.TextBox Tile3TextBox;
        private System.Windows.Forms.TextBox Tile2TextBox;
        private System.Windows.Forms.TextBox Tile1TextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectAllStripMenuItem;

    }
}

