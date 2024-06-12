namespace DBMSClient
{
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
            this.button1 = new System.Windows.Forms.Button();
            this.dbListView = new System.Windows.Forms.ListView();
            this.headerName = new System.Windows.Forms.ColumnHeader();
            this.Nume = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.UseDbButton = new System.Windows.Forms.Button();
            this.DropDbButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.newDbTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(628, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dbListView
            // 
            this.dbListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName});
            this.dbListView.Location = new System.Drawing.Point(24, 85);
            this.dbListView.Name = "dbListView";
            this.dbListView.Size = new System.Drawing.Size(122, 252);
            this.dbListView.TabIndex = 7;
            this.dbListView.UseCompatibleStateImageBehavior = false;
            this.dbListView.View = System.Windows.Forms.View.Details;
            // 
            // headerName
            // 
            this.headerName.Text = "NumeDB";
            this.headerName.Width = 120;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Text", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(24, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Databases";
            // 
            // UseDbButton
            // 
            this.UseDbButton.Location = new System.Drawing.Point(180, 169);
            this.UseDbButton.Name = "UseDbButton";
            this.UseDbButton.Size = new System.Drawing.Size(75, 23);
            this.UseDbButton.TabIndex = 1;
            this.UseDbButton.Text = "Use";
            this.UseDbButton.Click += new System.EventHandler(this.UseDbButton_Click);
            // 
            // DropDbButton
            // 
            this.DropDbButton.Location = new System.Drawing.Point(180, 220);
            this.DropDbButton.Name = "DropDbButton";
            this.DropDbButton.Size = new System.Drawing.Size(75, 23);
            this.DropDbButton.TabIndex = 0;
            this.DropDbButton.Text = "Drop";
            this.DropDbButton.Click += new System.EventHandler(this.DropDbButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Text", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(399, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "New database name";
            // 
            // newDbTextBox
            // 
            this.newDbTextBox.Location = new System.Drawing.Point(399, 169);
            this.newDbTextBox.Name = "newDbTextBox";
            this.newDbTextBox.Size = new System.Drawing.Size(210, 23);
            this.newDbTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(478, 383);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(526, 383);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(179, 16);
            this.statusLabel.TabIndex = 6;
            this.statusLabel.Text = "No actions have been performed";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.newDbTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DropDbButton);
            this.Controls.Add(this.UseDbButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dbListView);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        public ListView dbListView;
        private Label label1;
        private Button UseDbButton;
        private Button DropDbButton;
        private Label label2;
        private TextBox newDbTextBox;
        private Label label3;
        private Label statusLabel;
        private ColumnHeader Nume;
        private ColumnHeader headerName;
    }
}