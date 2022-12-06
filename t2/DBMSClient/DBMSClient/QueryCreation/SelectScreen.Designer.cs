namespace DBMSClient.QueryCreation
{
    partial class SelectScreen
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
            this.itemsButton = new System.Windows.Forms.Button();
            this.conditionButton = new System.Windows.Forms.Button();
            this.queryLabel = new System.Windows.Forms.Label();
            this.queryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // itemsButton
            // 
            this.itemsButton.Location = new System.Drawing.Point(57, 39);
            this.itemsButton.Name = "itemsButton";
            this.itemsButton.Size = new System.Drawing.Size(129, 67);
            this.itemsButton.TabIndex = 0;
            this.itemsButton.Text = "Add items";
            this.itemsButton.UseVisualStyleBackColor = true;
            this.itemsButton.Click += new System.EventHandler(this.itemsButton_Click);
            // 
            // conditionButton
            // 
            this.conditionButton.Location = new System.Drawing.Point(347, 39);
            this.conditionButton.Name = "conditionButton";
            this.conditionButton.Size = new System.Drawing.Size(129, 67);
            this.conditionButton.TabIndex = 1;
            this.conditionButton.Text = "Add Condition";
            this.conditionButton.UseVisualStyleBackColor = true;
            this.conditionButton.Click += new System.EventHandler(this.conditionButton_Click);
            // 
            // queryLabel
            // 
            this.queryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryLabel.Location = new System.Drawing.Point(12, 109);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(525, 61);
            this.queryLabel.TabIndex = 2;
            this.queryLabel.Text = "Query";
            this.queryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // queryButton
            // 
            this.queryButton.Location = new System.Drawing.Point(226, 173);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(98, 36);
            this.queryButton.TabIndex = 3;
            this.queryButton.Text = "Execute Select";
            this.queryButton.UseVisualStyleBackColor = true;
            this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
            // 
            // SelectScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 234);
            this.Controls.Add(this.queryButton);
            this.Controls.Add(this.queryLabel);
            this.Controls.Add(this.conditionButton);
            this.Controls.Add(this.itemsButton);
            this.Name = "SelectScreen";
            this.Text = "SelectScreen";
            this.ResumeLayout(false);

        }

        #endregion

        private Button itemsButton;
        private Button conditionButton;
        public  Label queryLabel;
        private Button queryButton;
    }
}