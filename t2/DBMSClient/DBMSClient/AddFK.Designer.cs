namespace DBMSClient
{
    partial class AddFK
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tablesComboBox = new System.Windows.Forms.ComboBox();
            this.attributesListView = new System.Windows.Forms.ListView();
            this.addFkButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose a table";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Variable Text", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(116, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Add a foreign key";
            // 
            // tablesComboBox
            // 
            this.tablesComboBox.FormattingEnabled = true;
            this.tablesComboBox.Location = new System.Drawing.Point(12, 146);
            this.tablesComboBox.Name = "tablesComboBox";
            this.tablesComboBox.Size = new System.Drawing.Size(121, 24);
            this.tablesComboBox.TabIndex = 2;
            this.tablesComboBox.SelectedIndexChanged += new System.EventHandler(this.tablesComboBox_SelectedIndexChanged);
            // 
            // attributesListView
            // 
            this.attributesListView.Location = new System.Drawing.Point(144, 59);
            this.attributesListView.Name = "attributesListView";
            this.attributesListView.Size = new System.Drawing.Size(149, 220);
            this.attributesListView.TabIndex = 3;
            this.attributesListView.UseCompatibleStateImageBehavior = false;
            this.attributesListView.View = System.Windows.Forms.View.Details;
            // 
            // addFkButton
            // 
            this.addFkButton.Location = new System.Drawing.Point(28, 227);
            this.addFkButton.Name = "addFkButton";
            this.addFkButton.Size = new System.Drawing.Size(75, 23);
            this.addFkButton.TabIndex = 4;
            this.addFkButton.Text = "Add";
            this.addFkButton.UseVisualStyleBackColor = true;
            this.addFkButton.Click += new System.EventHandler(this.addFkButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(343, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Finish";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AddFK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 320);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addFkButton);
            this.Controls.Add(this.attributesListView);
            this.Controls.Add(this.tablesComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddFK";
            this.Text = "AddFK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox tablesComboBox;
        private ListView attributesListView;
        private Button addFkButton;
        private Button button1;
    }
}