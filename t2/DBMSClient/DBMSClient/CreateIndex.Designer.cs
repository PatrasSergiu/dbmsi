namespace DBMSClient
{
    partial class CreateIndex
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
            this.createButton = new System.Windows.Forms.Button();
            this.indexListView = new System.Windows.Forms.ListView();
            this.IndexName = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(92, 306);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 0;
            this.createButton.Text = "Finish";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // indexListView
            // 
            this.indexListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IndexName});
            this.indexListView.Location = new System.Drawing.Point(67, 51);
            this.indexListView.Name = "indexListView";
            this.indexListView.Size = new System.Drawing.Size(121, 249);
            this.indexListView.TabIndex = 1;
            this.indexListView.UseCompatibleStateImageBehavior = false;
            this.indexListView.View = System.Windows.Forms.View.Details;
            // 
            // IndexName
            // 
            this.IndexName.Text = "IndexName";
            this.IndexName.Width = 120;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Text", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(46, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select one or more fields";
            // 
            // CreateIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 379);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.indexListView);
            this.Controls.Add(this.createButton);
            this.Name = "CreateIndex";
            this.Text = "CreateIndex";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button createButton;
        private ListView indexListView;
        private Label label1;
        private ColumnHeader IndexName;
    }
}