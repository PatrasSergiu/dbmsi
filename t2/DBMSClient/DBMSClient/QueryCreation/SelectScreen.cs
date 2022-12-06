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
    public partial class SelectScreen : Form
    {

        List<Command> allTables;
        TablesView _parentForm;
        public List<AtributTabel> selectedAttributes = new List<AtributTabel>();
        List<Command> tableFormSelect = new List<Command>();

        public List<Condition> conditions = new List<Condition>();

        string conditionText = "";
        string itemsQuery = "";
        string fromTable = "";
        public SelectScreen(List<Command> tables, TablesView tablesView)
        {
            InitializeComponent();
            allTables = tables.ToList();
            this._parentForm = tablesView;
        }

        private void itemsButton_Click(object sender, EventArgs e)
        {
            QueryItems queryItems = new QueryItems(allTables.ToList(), this);
            queryItems.ShowDialog();
            if(queryItems.selectedTableName != null)
            {
                var items = queryItems.selectedAttributes;
                selectedAttributes.AddRange(items);
                Command found = allTables.Find(e => e.tableName == queryItems.selectedTableName);
                allTables.RemoveAll(e => e.tableName == queryItems.selectedTableName);
                queryItems.Dispose();
                
                itemsQuery = "";

                if (items.Count == found.AttributesList.Count)
                    itemsQuery = "*";
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        itemsQuery += items[i].Name;
                        if (i != items.Count - 1)
                        {
                            itemsQuery += ",";
                        }
                    }
                }
                fromTable = found.tableName;
                queryLabel.Text = "SELECT " + itemsQuery + " FROM " + fromTable;
            }
        }

        private void conditionButton_Click(object sender, EventArgs e)
        {
            foreach(AtributTabel atr in selectedAttributes)
            {
                var tabel = tableFormSelect.Find(a => a.tableName == atr.ParentTable);
                if (tabel == null)
                {
                    Command comm = new Command();
                    comm.AttributesList = new List<AtributTabel>();
                    comm.dbName = allTables[0].dbName;
                    comm.AttributesList.Add(atr);
                    comm.tableName = atr.ParentTable;
                    tableFormSelect.Add(comm);
                }
                else
                {
                    tabel.AttributesList.Add(atr);
                }
            }

            AddConditionScreen conditionScreen = new AddConditionScreen(tableFormSelect, this);
            conditionScreen.ShowDialog();
            if(conditionScreen.condition != null)
            {
                Condition cond = conditionScreen.condition;
                conditions.Add(cond);
                string txt = cond.ToString();
                if(conditionText == "")
                {
                    conditionText = txt;
                }
                else
                {
                    conditionText += " AND " + txt;
                }

                queryLabel.Text = "SELECT " + itemsQuery + " FROM " + fromTable + " WHERE " + conditionText;

            }
        }

        private void queryButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
