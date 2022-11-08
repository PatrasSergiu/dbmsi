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
using TextBox = System.Windows.Forms.TextBox;
using Label = System.Windows.Forms.Label;
using System.Reflection;
using System.Data.Common;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace DBMSClient
{
    public partial class TablesView : Form
    {
        private Form1 _firstForm;
        List<Command> tables = new List<Command>();
        Command selectedTable = new Command();
        List<TextBox> textBoxes = new List<TextBox>();
        public TablesView()
        {
            InitializeComponent();
        }
        public TablesView(Form1 firstForm, List<Command> tbl)
        {
            InitializeComponent();
            this.Controls.Add(panel1);
            foreach (Command cmd in tbl)
            {
                ListViewItem item = new ListViewItem(cmd.tableName);
                item.Tag = cmd;
                tables.Add(cmd);
                tablesListView.Items.Add(item);
            }
            _firstForm = firstForm;
            dbLabel.Text = "Using database " + firstForm.dbListView.SelectedItems[0].Text;
        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            CreateTable createTableView = new CreateTable(this);
            createTableView.tables.AddRange(tables);
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
            deleteComboBox.Items.Clear();
            textBoxes.Clear();
            int pointX = 20;
            int pointY = 25;
            pointX += 20;
            pointY += 10;
            panel1.Visible = true;
            panel1.Location = new Point(1100, 20);
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Clear();
            foreach (AtributTabel atr in command.AttributesList)
            {
                viewTableList.Columns.Add(atr.Name);    
                indexComboBox.Items.Add(atr.Name);
                TextBox a = new TextBox();
                Label l = new Label();
                l.Text = atr.Name;
                l.Name = atr.Name + "Label";
                l.Location = new Point(pointX, pointY);
                l.Visible = true;
                l.Parent = panel1;
                pointY += 20;
                a.Text = "";
                a.Name = atr.Name + "TextBox";
                a.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                a.Height = 10;
                a.Width = 100;
                a.Location = new Point(pointX, pointY);
                a.Visible = true;
                a.Parent = panel1;
                textBoxes.Add(a);
                panel1.Controls.Add(a);
                panel1.Controls.Add(l);
                panel1.Show();
                pointY += 30;

                if(atr.IsPrimaryKey == false)
                {
                    deleteComboBox.Items.Add(atr.Name);
                }
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
                    foreach (TextBox tb in textBoxes)
                    {
                        tb.Clear();
                    }
                    MessageBox.Show("Creeat cu succes");
                }
                else
                {
                    MessageBox.Show("Exista deja un index cu acest nume sau a aparut o eroare");
                }
            }
        }

        private void insertRecordButton_Click(object sender, EventArgs e)
        {
            int textBoxIndex = 0;
            Dictionary<string, string> values = new Dictionary<string, string>();
           foreach(AtributTabel atr in selectedTable.AttributesList)
            {
                //MessageBox.Show(atr.Name + ": " + textBoxes[textBoxIndex].Text);
                values.Add(atr.Name, textBoxes[textBoxIndex].Text);
                textBoxIndex++;
            }
            Command command = new Command();
            command = selectedTable;
            command.SqlQuery = String.Format("INSERT INTO " + selectedTable.tableName);
            command.Values = values;
            byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
            string response = Extras.cleanMessage(bytes);
            if (response == "OK")
            {
                MessageBox.Show("Inserted succesfully");
                foreach (TextBox tb in textBoxes)
                {
                    tb.Clear();
                }
            }
            else
            {
                MessageBox.Show(response);
            }
        }

        private void TablesView_Load(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if(tablesListView.SelectedItems.Count > 0)
            {
                string key = deleteTextBox.Text;
                Command command = new Command();
                command = selectedTable;
                command.SqlQuery = String.Format("DELETE " + key);
                byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
                string response = Extras.cleanMessage(bytes);
                if (response == "OK")
                {
                    MessageBox.Show("Deleted succesfully");
                    deleteTextBox.Clear();
                }
                else
                {
                    MessageBox.Show(response);
                }
            }
            else
            {
                MessageBox.Show("Please select a table from the left side", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void deleteWhereButton_Click(object sender, EventArgs e)
        {
            if (deleteComboBox.SelectedIndex > -1 && deleteWhereTextBox.Text != ""){
                if (tablesListView.SelectedItems.Count <= 0)
                {
                    MessageBox.Show("Please select a table from the left side", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string whereAttribute = deleteComboBox.SelectedItem.ToString();
                string whereClause = deleteWhereTextBox.Text;
                Command command = new Command();
                command = selectedTable;
                command.SqlQuery = String.Format("DELETE WHERE {0} {1}", whereAttribute, whereClause);
                byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
                string response = Extras.cleanMessage(bytes);
                if (response == "OK")
                {
                    MessageBox.Show("One or more records were deleted succesfully.");
                    deleteTextBox.Clear();
                }
                else
                {
                    MessageBox.Show(response);
                }
            }
            else
            {
                MessageBox.Show("Please complete both fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            int textBoxIndex = 0;
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (AtributTabel atr in selectedTable.AttributesList)
            {
                //MessageBox.Show(atr.Name + ": " + textBoxes[textBoxIndex].Text);
                values.Add(atr.Name, textBoxes[textBoxIndex].Text);
                textBoxIndex++;
            }
            Command command = new Command();
            command = selectedTable;
            command.SqlQuery = String.Format("UPDATE " + selectedTable.tableName);
            command.Values = values;

            byte[] bytes = Extras.sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
            string response = Extras.cleanMessage(bytes);
            if (response == "OK")
            {
                MessageBox.Show("Updated succesfully");
                foreach (TextBox tb in textBoxes)
                {
                    tb.Clear();
                }
            }
            else
            {
                MessageBox.Show(response);
            }
        }
    }
}
