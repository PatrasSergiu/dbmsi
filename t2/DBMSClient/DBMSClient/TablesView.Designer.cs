namespace DBMSClient
{
    partial class TablesView
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
            this.dbLabel = new System.Windows.Forms.Label();
            this.tablesListView = new System.Windows.Forms.ListView();
            this.tablesHeader1 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.dropTableButton = new System.Windows.Forms.Button();
            this.createTableButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dbLabel
            // 
            this.dbLabel.AutoSize = true;
            this.dbLabel.Font = new System.Drawing.Font("Segoe UI Variable Text", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dbLabel.Location = new System.Drawing.Point(428, -8);
            this.dbLabel.Name = "dbLabel";
            this.dbLabel.Size = new System.Drawing.Size(488, 57);
            this.dbLabel.TabIndex = 0;
            this.dbLabel.Text = "Using database students";
            // 
            // tablesListView
            // 
            this.tablesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tablesHeader1});
            this.tablesListView.Location = new System.Drawing.Point(27, 82);
            this.tablesListView.Name = "tablesListView";
            this.tablesListView.Size = new System.Drawing.Size(247, 450);
            this.tablesListView.TabIndex = 1;
            this.tablesListView.UseCompatibleStateImageBehavior = false;
            this.tablesListView.View = System.Windows.Forms.View.List;
            // 
            // tablesHeader1
            // 
            this.tablesHeader1.Text = "Nume tabel";
            this.tablesHeader1.Width = 300;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Text", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(95, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 43);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tables";
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(724, 167);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(600, 365);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // dropTableButton
            // 
            this.dropTableButton.Location = new System.Drawing.Point(313, 324);
            this.dropTableButton.Name = "dropTableButton";
            this.dropTableButton.Size = new System.Drawing.Size(101, 58);
            this.dropTableButton.TabIndex = 4;
            this.dropTableButton.Text = "Drop Table";
            this.dropTableButton.UseVisualStyleBackColor = true;
            // 
            // createTableButton
            // 
            this.createTableButton.Location = new System.Drawing.Point(313, 235);
            this.createTableButton.Name = "createTableButton";
            this.createTableButton.Size = new System.Drawing.Size(101, 53);
            this.createTableButton.TabIndex = 5;
            this.createTableButton.Text = "Create table";
            this.createTableButton.UseVisualStyleBackColor = true;
            this.createTableButton.Click += new System.EventHandler(this.createTableButton_Click);
            // 
            // TablesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 639);
            this.Controls.Add(this.createTableButton);
            this.Controls.Add(this.dropTableButton);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tablesListView);
            this.Controls.Add(this.dbLabel);
            this.Name = "TablesView";
            this.Text = "TablesView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Label dbLabel;
        public ListView tablesListView;
        private Label label2;
        private ColumnHeader tablesHeader1;
        private ListView listView2;
        private Button dropTableButton;
        private Button createTableButton;
    }
}