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
    public partial class AddConditionScreen : Form
    {
        List<Command> allTables;
        SelectScreen _parentForm;
        public Condition condition;

        public AddConditionScreen(List<Command> tables, SelectScreen selectScreen)
        {
            InitializeComponent();
            allTables = tables;
            _parentForm = selectScreen;
            fieldComboBox.IsAccessible = false;
            fieldComboBox.Enabled = false;
            conditionTextBox.Enabled = false;
            comboBox2.Enabled = false;

            foreach (var cmd in allTables)
            {
                ListViewItem item = new ListViewItem(cmd.tableName);
                item.Tag = cmd;
                tablesListView.Items.Add(item);
            }
        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            condition = new Condition();
            condition.comparation = comboBox2.Text;
            condition.comparationValue = conditionTextBox.Text;
            condition.attributeName = fieldComboBox.SelectedItem.ToString();
            condition.ParentTable = tablesListView.SelectedItems[0].Text;

            this.Close();
        }

        private void tablesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tablesListView.SelectedItems.Count > 0)
            {
                fieldComboBox.IsAccessible = true;
                comboBox2.Enabled = false;
                conditionTextBox.Enabled = false;
                fieldComboBox.Enabled = true;
                fieldComboBox.Items.Clear();
                Command cmd = (Command)tablesListView.SelectedItems[0].Tag;
                List<AtributTabel> atributes = cmd.AttributesList;
                foreach (AtributTabel atr in atributes)
                {
                    fieldComboBox.Items.Add(atr.Name);
                }

            }
        }

        private void fieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryLabel.Text = "WHERE " + fieldComboBox.SelectedItem;
            comboBox2.Enabled = true;
        }

        private void conditionTextBox_TextChanged(object sender, EventArgs e)
        {
            queryLabel.Text = "WHERE " + fieldComboBox.SelectedItem + " " + comboBox2.SelectedItem + " " + conditionTextBox.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            queryLabel.Text = "WHERE " + fieldComboBox.SelectedItem + " " + comboBox2.SelectedItem;
            conditionTextBox.Enabled = true;
        }
    }
}
