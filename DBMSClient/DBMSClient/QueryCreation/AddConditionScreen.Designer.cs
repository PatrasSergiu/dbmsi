namespace DBMSClient.QueryCreation
{
    partial class AddConditionScreen
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
            this.tablesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.fieldComboBox = new System.Windows.Forms.ComboBox();
            this.queryLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.conditionTextBox = new System.Windows.Forms.TextBox();
            this.finishButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tablesListView
            // 
            this.tablesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.tablesListView.Location = new System.Drawing.Point(25, 38);
            this.tablesListView.Name = "tablesListView";
            this.tablesListView.Size = new System.Drawing.Size(126, 267);
            this.tablesListView.TabIndex = 0;
            this.tablesListView.UseCompatibleStateImageBehavior = false;
            this.tablesListView.View = System.Windows.Forms.View.Details;
            this.tablesListView.SelectedIndexChanged += new System.EventHandler(this.tablesListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table";
            this.columnHeader1.Width = 120;
            // 
            // fieldComboBox
            // 
            this.fieldComboBox.FormattingEnabled = true;
            this.fieldComboBox.Location = new System.Drawing.Point(25, 332);
            this.fieldComboBox.Name = "fieldComboBox";
            this.fieldComboBox.Size = new System.Drawing.Size(126, 24);
            this.fieldComboBox.TabIndex = 1;
            this.fieldComboBox.SelectedIndexChanged += new System.EventHandler(this.fieldComboBox_SelectedIndexChanged);
            // 
            // queryLabel
            // 
            this.queryLabel.AutoSize = true;
            this.queryLabel.Location = new System.Drawing.Point(25, 372);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(46, 16);
            this.queryLabel.TabIndex = 2;
            this.queryLabel.Text = "WHERE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select condition";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "GREATER THAN",
            "LESSER THAN",
            "EQUAL"});
            this.comboBox2.Location = new System.Drawing.Point(172, 116);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 24);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Condition Value";
            // 
            // conditionTextBox
            // 
            this.conditionTextBox.Location = new System.Drawing.Point(172, 187);
            this.conditionTextBox.Name = "conditionTextBox";
            this.conditionTextBox.Size = new System.Drawing.Size(121, 23);
            this.conditionTextBox.TabIndex = 6;
            this.conditionTextBox.TextChanged += new System.EventHandler(this.conditionTextBox_TextChanged);
            // 
            // finishButton
            // 
            this.finishButton.Location = new System.Drawing.Point(184, 238);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(90, 26);
            this.finishButton.TabIndex = 7;
            this.finishButton.Text = "finish";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
            // 
            // AddConditionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 410);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.conditionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.queryLabel);
            this.Controls.Add(this.fieldComboBox);
            this.Controls.Add(this.tablesListView);
            this.Name = "AddConditionScreen";
            this.Text = "AddConditionScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView tablesListView;
        private ColumnHeader columnHeader1;
        private ComboBox fieldComboBox;
        private Label queryLabel;
        private Label label1;
        private ComboBox comboBox2;
        private Label label2;
        private TextBox conditionTextBox;
        private Button finishButton;
    }
}