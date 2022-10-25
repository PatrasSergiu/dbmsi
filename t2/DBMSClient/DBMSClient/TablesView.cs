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
        public TablesView()
        {
            InitializeComponent();
        }
        public TablesView(Form1 firstForm)
        {
            InitializeComponent();
            _firstForm = firstForm;
            dbLabel.Text = "Using database " + firstForm.dbListView.SelectedItems[0].Text;
        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            CreateTable createTableView = new CreateTable(this);
            createTableView.Show();
        }
    }
}
