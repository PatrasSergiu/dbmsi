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
            this.insertRecordButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.deleteTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.deleteWhereTextBox = new System.Windows.Forms.TextBox();
            this.deleteWhereButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
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
            this.viewTableList.Location = new System.Drawing.Point(478, 150);
            this.viewTableList.Name = "viewTableList";
            this.viewTableList.Size = new System.Drawing.Size(606, 406);
            this.viewTableList.TabIndex = 6;
            this.viewTableList.UseCompatibleStateImageBehavior = false;
            this.viewTableList.View = System.Windows.Forms.View.Details;
            this.viewTableList.SelectedIndexChanged += new System.EventHandler(this.viewTableList_SelectedIndexChanged_1);
            // 
            // createIndexButton
            // 
            this.createIndexButton.Location = new System.Drawing.Point(618, 97);
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
            this.indexComboBox.Location = new System.Drawing.Point(478, 102);
            this.indexComboBox.Name = "indexComboBox";
            this.indexComboBox.Size = new System.Drawing.Size(121, 24);
            this.indexComboBox.TabIndex = 8;
            // 
            // insertRecordButton
            // 
            this.insertRecordButton.Location = new System.Drawing.Point(1100, 587);
            this.insertRecordButton.Name = "insertRecordButton";
            this.insertRecordButton.Size = new System.Drawing.Size(82, 34);
            this.insertRecordButton.TabIndex = 9;
            this.insertRecordButton.Text = "Insert";
            this.insertRecordButton.UseVisualStyleBackColor = true;
            this.insertRecordButton.Click += new System.EventHandler(this.insertRecordButton_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(1100, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(289, 505);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(478, 571);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "ID to be deleted";
            // 
            // deleteTextBox
            // 
            this.deleteTextBox.Location = new System.Drawing.Point(478, 593);
            this.deleteTextBox.Name = "deleteTextBox";
            this.deleteTextBox.Size = new System.Drawing.Size(100, 23);
            this.deleteTextBox.TabIndex = 12;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(584, 574);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(79, 42);
            this.deleteButton.TabIndex = 13;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Variable Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(684, 560);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 21);
            this.label3.TabIndex = 14;
            this.label3.Text = "Delete where";
            // 
            // deleteComboBox
            // 
            this.deleteComboBox.FormattingEnabled = true;
            this.deleteComboBox.Location = new System.Drawing.Point(791, 563);
            this.deleteComboBox.Name = "deleteComboBox";
            this.deleteComboBox.Size = new System.Drawing.Size(61, 24);
            this.deleteComboBox.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Variable Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(684, 592);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 21);
            this.label4.TabIndex = 16;
            this.label4.Text = "is";
            // 
            // deleteWhereTextBox
            // 
            this.deleteWhereTextBox.Location = new System.Drawing.Point(711, 593);
            this.deleteWhereTextBox.Name = "deleteWhereTextBox";
            this.deleteWhereTextBox.Size = new System.Drawing.Size(74, 23);
            this.deleteWhereTextBox.TabIndex = 17;
            // 
            // deleteWhereButton
            // 
            this.deleteWhereButton.Location = new System.Drawing.Point(809, 593);
            this.deleteWhereButton.Name = "deleteWhereButton";
            this.deleteWhereButton.Size = new System.Drawing.Size(75, 23);
            this.deleteWhereButton.TabIndex = 18;
            this.deleteWhereButton.Text = "Confirm";
            this.deleteWhereButton.UseVisualStyleBackColor = true;
            this.deleteWhereButton.Click += new System.EventHandler(this.deleteWhereButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(1307, 582);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(82, 34);
            this.updateButton.TabIndex = 19;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // TablesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 639);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.deleteWhereButton);
            this.Controls.Add(this.deleteWhereTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.deleteComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.deleteTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.insertRecordButton);
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
            this.Load += new System.EventHandler(this.TablesView_Load);
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
        private Button insertRecordButton;
        private Panel panel1;
        private Label label1;
        private TextBox deleteTextBox;
        private Button deleteButton;
        private Label label3;
        private ComboBox deleteComboBox;
        private Label label4;
        private TextBox deleteWhereTextBox;
        private Button deleteWhereButton;
        private Button updateButton;
    }
}