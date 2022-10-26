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
            this.dropTableButton = new System.Windows.Forms.Button();
            this.createTableButton = new System.Windows.Forms.Button();
            this.viewTableList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.createIndexButton = new System.Windows.Forms.Button();
            this.indexComboBox = new System.Windows.Forms.ComboBox();
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
            this.tablesListView.View = System.Windows.Forms.View.Details;
            this.tablesListView.SelectedIndexChanged += new System.EventHandler(this.tablesListView_SelectedIndexChanged);
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
            // dropTableButton
            // 
            this.dropTableButton.Location = new System.Drawing.Point(313, 324);
            this.dropTableButton.Name = "dropTableButton";
            this.dropTableButton.Size = new System.Drawing.Size(101, 58);
            this.dropTableButton.TabIndex = 4;
            this.dropTableButton.Text = "Drop Table";
            this.dropTableButton.UseVisualStyleBackColor = true;
            this.dropTableButton.Click += new System.EventHandler(this.dropTableButton_Click);
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
            // viewTableList
            // 
            this.viewTableList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.viewTableList.Location = new System.Drawing.Point(738, 154);
            this.viewTableList.Name = "viewTableList";
            this.viewTableList.Size = new System.Drawing.Size(606, 406);
            this.viewTableList.TabIndex = 6;
            this.viewTableList.UseCompatibleStateImageBehavior = false;
            this.viewTableList.View = System.Windows.Forms.View.Details;
            this.viewTableList.SelectedIndexChanged += new System.EventHandler(this.viewTableList_SelectedIndexChanged_1);
            // 
            // createIndexButton
            // 
            this.createIndexButton.Location = new System.Drawing.Point(945, 94);
            this.createIndexButton.Name = "createIndexButton";
            this.createIndexButton.Size = new System.Drawing.Size(106, 32);
            this.createIndexButton.TabIndex = 7;
            this.createIndexButton.Text = "Create Index";
            this.createIndexButton.UseVisualStyleBackColor = true;
            this.createIndexButton.Click += new System.EventHandler(this.createIndexButton_Click);
            // 
            // indexComboBox
            // 
            this.indexComboBox.FormattingEnabled = true;
            this.indexComboBox.Location = new System.Drawing.Point(795, 99);
            this.indexComboBox.Name = "indexComboBox";
            this.indexComboBox.Size = new System.Drawing.Size(121, 24);
            this.indexComboBox.TabIndex = 8;
            // 
            // TablesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 639);
            this.Controls.Add(this.indexComboBox);
            this.Controls.Add(this.createIndexButton);
            this.Controls.Add(this.viewTableList);
            this.Controls.Add(this.createTableButton);
            this.Controls.Add(this.dropTableButton);
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
        private Button dropTableButton;
        private Button createTableButton;
        private ListView viewTableList;
        private ColumnHeader columnHeader1;
        private Button createIndexButton;
        private ComboBox indexComboBox;
    }
}