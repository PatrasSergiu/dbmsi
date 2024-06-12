namespace DBMSClient
{
    partial class CreateTable
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.tableNameTextBox = new System.Windows.Forms.TextBox();
            this.attributesGridView = new System.Windows.Forms.DataGridView();
            this.createTableButton = new System.Windows.Forms.Button();
            this.attributeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attributeType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.attributeUnique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attributePK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.foreignKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Variable Text", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(214, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nume tabel";
            // 
            // tableNameTextBox
            // 
            this.tableNameTextBox.Location = new System.Drawing.Point(195, 85);
            this.tableNameTextBox.Name = "tableNameTextBox";
            this.tableNameTextBox.Size = new System.Drawing.Size(144, 23);
            this.tableNameTextBox.TabIndex = 1;
            // 
            // attributesGridView
            // 
            this.attributesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attributesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attributeName,
            this.attributeType,
            this.attributeUnique,
            this.attributePK,
            this.foreignKey});
            this.attributesGridView.Location = new System.Drawing.Point(33, 145);
            this.attributesGridView.Name = "attributesGridView";
            this.attributesGridView.RowTemplate.Height = 25;
            this.attributesGridView.Size = new System.Drawing.Size(556, 350);
            this.attributesGridView.TabIndex = 2;
            // 
            // createTableButton
            // 
            this.createTableButton.Location = new System.Drawing.Point(181, 519);
            this.createTableButton.Name = "createTableButton";
            this.createTableButton.Size = new System.Drawing.Size(118, 55);
            this.createTableButton.TabIndex = 3;
            this.createTableButton.Text = "Create table";
            this.createTableButton.UseVisualStyleBackColor = true;
            this.createTableButton.Click += new System.EventHandler(this.createTableButton_Click);
            // 
            // attributeName
            // 
            this.attributeName.HeaderText = "Attribute name";
            this.attributeName.MaxInputLength = 20;
            this.attributeName.Name = "attributeName";
            // 
            // attributeType
            // 
            dataGridViewCellStyle1.NullValue = "int";
            this.attributeType.DefaultCellStyle = dataGridViewCellStyle1;
            this.attributeType.HeaderText = "Type";
            this.attributeType.Items.AddRange(new object[] {
            "int",
            "double",
            "bool",
            "string"});
            this.attributeType.MaxDropDownItems = 4;
            this.attributeType.Name = "attributeType";
            // 
            // attributeUnique
            // 
            this.attributeUnique.HeaderText = "Unique";
            this.attributeUnique.Name = "attributeUnique";
            // 
            // attributePK
            // 
            this.attributePK.HeaderText = "Primary Key";
            this.attributePK.Name = "attributePK";
            // 
            // foreignKey
            // 
            this.foreignKey.HeaderText = "Foreign Key";
            this.foreignKey.Name = "foreignKey";
            // 
            // CreateTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 586);
            this.Controls.Add(this.createTableButton);
            this.Controls.Add(this.attributesGridView);
            this.Controls.Add(this.tableNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "CreateTable";
            this.Text = "CreateTable";
            this.Load += new System.EventHandler(this.CreateTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.attributesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox tableNameTextBox;
        private DataGridView attributesGridView;
        private Button createTableButton;
        private DataGridViewTextBoxColumn attributeName;
        private DataGridViewComboBoxColumn attributeType;
        private DataGridViewCheckBoxColumn attributeUnique;
        private DataGridViewCheckBoxColumn attributePK;
        private DataGridViewCheckBoxColumn foreignKey;
    }
}