namespace DBMSClient.QueryCreation
{
    partial class QueryItems
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
            this.allTablesListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.addButton = new System.Windows.Forms.Button();
            this.attributesListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // allTablesListView
            // 
            this.allTablesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.allTablesListView.Location = new System.Drawing.Point(43, 53);
            this.allTablesListView.Name = "allTablesListView";
            this.allTablesListView.Size = new System.Drawing.Size(126, 273);
            this.allTablesListView.TabIndex = 0;
            this.allTablesListView.UseCompatibleStateImageBehavior = false;
            this.allTablesListView.View = System.Windows.Forms.View.Details;
            this.allTablesListView.SelectedIndexChanged += new System.EventHandler(this.allTablesListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tables";
            this.columnHeader1.Width = 120;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(126, 332);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(124, 61);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add fields";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // attributesListView
            // 
            this.attributesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.attributesListView.Location = new System.Drawing.Point(215, 53);
            this.attributesListView.Name = "attributesListView";
            this.attributesListView.Size = new System.Drawing.Size(133, 273);
            this.attributesListView.TabIndex = 2;
            this.attributesListView.UseCompatibleStateImageBehavior = false;
            this.attributesListView.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 414);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "If no attributes are selected, the entire table is selected";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Attributes";
            this.columnHeader2.Width = 125;
            // 
            // QueryItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.attributesListView);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.allTablesListView);
            this.Name = "QueryItems";
            this.Text = "QueryItems";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView allTablesListView;
        private ColumnHeader columnHeader1;
        private Button addButton;
        private ListView attributesListView;
        private ColumnHeader columnHeader2;
        private Label label1;
    }
}