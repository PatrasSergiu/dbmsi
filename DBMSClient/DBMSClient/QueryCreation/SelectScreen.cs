using DBMSClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSClient.QueryCreation
{
    public partial class SelectScreen : Form
    {

        List<Command> allTables;
        Command tableToBeJoin;
        TablesView _parentForm;
        public List<AtributTabel> selectedAttributes = new List<AtributTabel>();
        List<Command> tableFormSelect = new List<Command>();

        public List<Condition> conditions = new List<Condition>();
        public List<TableJoin> joins = new List<TableJoin>();

        string conditionText = "";
        string itemsQuery = "";
        string fromTable = "";
        string finalQuery = "";
        Boolean first = true;
        public SelectScreen(List<Command> tables, TablesView tablesView)
        {
            InitializeComponent();
            allTables = tables.ToList();
            this._parentForm = tablesView;
        }

        private void itemsButton_Click(object sender, EventArgs e)
        {
            QueryItems queryItems;
            List<string> tables = new List<string>();
            if (selectedAttributes.Count > 0)
            {
                
                foreach(var atr in tableToBeJoin.AttributesList)
                {
                    if(atr.FKeys != null)
                    {
                        foreach(var key in atr.FKeys)
                        {
                            if (tables.Contains(key.Key) == false ) tables.Add(key.Key);
                        }
                    }
                }
                //got all the tables, now its time to get then command list

                queryItems = new QueryItems(allTables.FindAll(e => tables.Contains(e.tableName)).ToList(), this);
            }
            else
            {
               queryItems = new QueryItems(allTables.ToList(), this);
            }
            queryItems.ShowDialog();
            if(queryItems.selectedTableName != null)
            {
                var items = queryItems.selectedAttributes;
                if (selectedAttributes.Count > 0) first = false;
                selectedAttributes.AddRange(items);
                var auxTableToBeJoin = allTables.Find(e => e.tableName == queryItems.selectedTableName);
                queryItems.Dispose();
                
                itemsQuery = "";
                    for (int i = 0; i < items.Count; i++)
                    {
                        itemsQuery += items[i].Name;
                        if (i != items.Count - 1)
                        {
                            itemsQuery += ",";
                        }
                    }
                
                fromTable = auxTableToBeJoin.tableName;
                if (first)
                {
                    queryLabel.Text = "SELECT " + itemsQuery + " FROM " + fromTable;
                }
                else
                {
                    queryLabel.Text += " JOIN " + queryItems.selectedTableName;
                    List<AtributTabel> joinCandidates = new List<AtributTabel>();
                    foreach (var atr in tableToBeJoin.AttributesList)
                    {
                        if (atr.FKeys != null)
                        {
                            foreach (var key in atr.FKeys)
                            {
                                if (key.Key == items[0].ParentTable) joinCandidates.Add(atr);
                            }
                        }
                    }

                    Join join = new Join(joinCandidates);
                    join.ShowDialog();
                    AtributTabel joinAttribute = join.atributTabel;
                    TableJoin tableJoin = new TableJoin();
                    tableJoin.initialTable = tableToBeJoin.tableName;
                    tableJoin.joinAttribute = joinAttribute.Name;
                    tableJoin.joinTable = fromTable;
                    this.joins.Add(tableJoin);
                }
                tableToBeJoin = allTables.Find(e => e.tableName == queryItems.selectedTableName);
                allTables.RemoveAll(e => e.tableName == queryItems.selectedTableName);
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
