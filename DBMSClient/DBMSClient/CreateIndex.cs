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

namespace DBMSClient
{
    public partial class CreateIndex : Form
    {

        public List<string> selected;

        public CreateIndex(List<string> options)
        {
            InitializeComponent();
            indexListView.Items.Clear();
            selected = new List<string>();
            indexListView.MultiSelect = true;
            foreach (string option in options)
            {
                ListViewItem item = new ListViewItem(option);
                indexListView.Items.Add(item);
            }
            
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (indexListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in indexListView.SelectedItems)
                {
                    selected.Add(item.Text);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select at least one field", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
