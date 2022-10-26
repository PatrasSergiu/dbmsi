using DBMSClient.extras;
using DBMSClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBMSClient
{
    public partial class TablesView : Form
    {
        private Form1 _firstForm;
        List<Command> tables = new List<Command>();
        Command selectedTable = new Command();
        public TablesView()
        {
            InitializeComponent();
        }
        public TablesView(Form1 firstForm, List<Command> tbl)
        {
            InitializeComponent();
            tables = tbl;
            foreach (Command cmd in tables)
            {
                ListViewItem item = new ListViewItem(cmd.tableName);
                item.Tag = cmd;
                tablesListView.Items.Add(item);
            }
            _firstForm = firstForm;
            dbLabel.Text = "Using database " + firstForm.dbListView.SelectedItems[0].Text;
        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            CreateTable createTableView = new CreateTable(this);
            createTableView.Show();
        }

        private void dropTableButton_Click(object sender, EventArgs e)
        {
            if(tablesListView.SelectedItems.Count > 0)
            {
                Command command = new Command();
                ListViewItem item = tablesListView.SelectedItems[0];
                command = (Command)item.Tag;
                command.SqlQuery = String.Format("DROP TABLE " + item.Text);
                byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
                string response = Extras.cleanMessage(bytes);
                if (response == "OK")
                {
                    if (tablesListView.Items.Count > 0)
                    {
                        tablesListView.Items.Remove(tablesListView.SelectedItems[0]);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a table to delete",
                        "Oops",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning // for Warning  
                                               //MessageBoxIcon.Error // for Error 
                                               //MessageBoxIcon.Information  // for Information
                                               //MessageBoxIcon.Question // for Question
                           );
            }
            
        }

        private void viewTableList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tablesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = tablesListView.SelectedItems[0];
            Command command = (Command)item.Tag;
            selectedTable = command;
            viewTableList.Columns.Clear();
            indexComboBox.Items.Clear();

            foreach (AtributTabel atr in command.AttributesList)
            {
                viewTableList.Columns.Add(atr.Name);
                indexComboBox.Items.Add(atr.Name);
            }
            foreach (ColumnHeader column in viewTableList.Columns)
            {
                column.Width = viewTableList.Width / viewTableList.Columns.Count;
            }


        }

        private void viewTableList_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void createIndexButton_Click(object sender, EventArgs e)
        {
            if (indexComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a field to create an index for");
            }
            else
            {
                string selected = indexComboBox.SelectedItem.ToString();
                Command command = new Command();
                command = selectedTable;
                command.SqlQuery = String.Format("CREATE INDEX " + selected);
                byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
                string response = Extras.cleanMessage(bytes);
                if (response == "OK")
                {
                    MessageBox.Show("Creeat cu succes");
                }
                else
                {
                    MessageBox.Show("Exista deja un index cu acest nume sau a aparut o eroare");
                }
            }
        }
    }
}
