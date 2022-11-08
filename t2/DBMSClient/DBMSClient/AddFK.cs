using DBMSClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSClient
{
    public partial class AddFK : Form
    {
        public List<Command> tables = new List<Command>();
        Command tableToReference = new Command();
        public string selectedAttributeName = "";
        public string selectedAttributeType = "";
        public Dictionary<string, string> fkeys = new Dictionary<string, string>();
        public AddFK(CreateTable createTableView)
        {
            this.tables.AddRange(createTableView.tables);
            InitializeComponent();
            foreach (Command c in tables)
            {
                tablesComboBox.Items.Add(c.tableName);
            }
            attributesListView.Columns.Add("Name");
            attributesListView.Columns.Add("Type");

        }

        private void addFkButton_Click(object sender, EventArgs e)
        {
            var referencedAttribute = attributesListView.SelectedItems[0].Text;
            MessageBox.Show(selectedAttributeName + " references table " + tableToReference.tableName + " , attribute: " + referencedAttribute);
            string value;
            var selectedtype = attributesListView.SelectedItems[0].SubItems[1].Text;
            if (selectedAttributeType != selectedtype)
            {
                MessageBox.Show("Attribute type mismatch", "ERROR");
                return;
            }
            bool hasValue = fkeys.TryGetValue(tableToReference.tableName, out value);
            if (hasValue)
            {
                MessageBox.Show("This attribute already references this table", "Error");
            }
            else
            {
                fkeys.Add(tableToReference.tableName, referencedAttribute);
            }
        }

        private void tablesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableToReference = tables.FirstOrDefault(r => r.tableName == tablesComboBox.SelectedItem);
            attributesListView.Items.Clear();
            foreach (AtributTabel atr in tableToReference.AttributesList)
            {
                ListViewItem item = new ListViewItem(atr.Name);
                item.SubItems.Add(atr.Type);
                attributesListView.Items.Add(item);
            }
            foreach (ColumnHeader column in attributesListView.Columns)
            {
                column.Width = attributesListView.Width / attributesListView.Columns.Count;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
