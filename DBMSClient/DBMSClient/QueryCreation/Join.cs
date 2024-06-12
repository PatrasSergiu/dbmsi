using DBMSClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSClient.QueryCreation
{
    public partial class Join : Form
    {

        public AtributTabel atributTabel;


        public Join(List<AtributTabel> atrs)
        {
            InitializeComponent();
            
            
            foreach (AtributTabel atr in atrs)
            {
                ListViewItem item = new ListViewItem(atr.Name);
                item.Tag = atr;
                listView1.Items.Add(item);
            }
            MessageBox.Show(listView1.Items.Count.ToString());
        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                atributTabel = (AtributTabel) listView1.SelectedItems[0].Tag;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an attribute from the list", "oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
