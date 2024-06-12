using DBMSClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSClient.QueryCreation
{
    public partial class QueryItems : Form
    {
        List<Command> allTables;
        SelectScreen _parentForm;
        Command selectedTable;
        public string selectedTableName;
        public List<AtributTabel> selectedAttributes;

        public QueryItems(List<Command> tables, SelectScreen parentForm)
        {
            this.Controls.Clear();
            InitializeComponent();
            allTables = tables.ToList();
            _parentForm = parentForm;
            allTablesListView.FullRowSelect = true;
            allTablesListView.MultiSelect = false;
            attributesListView.FullRowSelect = true;
            allTablesListView.Items.Clear();
            attributesListView.Items.Clear();
            selectedAttributes = new List<AtributTabel>();
            foreach (Command cmd in allTables)
            {
                ListViewItem item = new ListViewItem(cmd.tableName);
                item.Tag = cmd;
                tables.Add(cmd);
                allTablesListView.Items.Add(item);
            }

        }

        private void allTablesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(allTablesListView.SelectedItems.Count > 0)
            {
                ListViewItem item = allTablesListView.SelectedItems[0];
                selectedTable = (Command)item.Tag;

                attributesListView.Items.Clear();

                foreach(AtributTabel atr in selectedTable.AttributesList)
                {
                    item = new ListViewItem(atr.Name);
                    atr.ParentTable = selectedTable.tableName;
                    item.Tag = atr;
                    attributesListView.Items.Add(item);
                }

            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if(allTablesListView.SelectedItems.Count > 0)
            {
                if(attributesListView.SelectedItems.Count == 0)
                {
                    selectedTableName = selectedTable.tableName;
                    selectedAttributes = selectedTable.AttributesList;
                }
                else
                {
                    selectedTableName = selectedTable.tableName;
                    foreach(ListViewItem item in attributesListView.SelectedItems)
                    {
                        selectedAttributes.Add((AtributTabel)item.Tag);
                    }
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a table first", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
