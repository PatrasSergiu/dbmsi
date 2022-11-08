using DBMSClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBMSClient
{
    public partial class CreateTable : Form
    {
        private TablesView _firstForm;
        public List<Command> tables = new List<Command>();

        public CreateTable()
        {
            InitializeComponent();
        }

        public CreateTable(TablesView firstForm)
        {
            InitializeComponent();
            _firstForm = firstForm;
            attributesGridView.RowCount = 2;
            

        }

        private void CreateTable_Load(object sender, EventArgs e)
        {

        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            List<AtributTabel> attributesList = new List<AtributTabel>();
            Command command = new Command();
            for (int r = 0; r < attributesGridView.Rows.Count - 1; r++)
            {
                DataGridViewRow row = attributesGridView.Rows[r];
                AtributTabel attribute = new AtributTabel();
                var name = row.Cells[0].Value as String; 
                if (name == null || tableNameTextBox.Text == "")
                {
                    MessageBox.Show("Every cell must be completed!",
                        "Oops",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error // for Warning  
                           //MessageBoxIcon.Error // for Error 
                           //MessageBoxIcon.Information  // for Information
                           //MessageBoxIcon.Question // for Question
                           );
                    return;
                }
                attribute.Name = name;
                var type = attributesGridView.Rows[r].Cells[1].Value;
                attribute.Type = (type == null) ? "int" : type.ToString();
                var aux = Convert.ToBoolean(row.Cells[2].Value);
                attribute.IsUnique = aux;
                aux = Convert.ToBoolean(row.Cells[3].Value);
                attribute.IsPrimaryKey = aux;
                ///foreign key check
                aux = Convert.ToBoolean(row.Cells[4].Value);
                if (aux)
                {
                    var auxTables = this.tables;
                    var itemToRemove = auxTables.SingleOrDefault(r => r.tableName == name);
                    if (itemToRemove != null)
                        auxTables.Remove(itemToRemove);
                    AddFK addFK = new AddFK(this);
                    addFK.selectedAttributeName = attribute.Name;
                    addFK.selectedAttributeType = attribute.Type;
                    if (attribute.FKeys != null)
                    {
                        addFK.fkeys = attribute.FKeys;
                    }
                    addFK.ShowDialog();
                    attribute.FKeys = addFK.fkeys;
                   
                }
                attributesList.Add(attribute);
            }
            command.SqlQuery = String.Format("CREATE TABLE " + tableNameTextBox.Text);
            command.AttributesList = attributesList;
            command.dbName = _firstForm.dbLabel.Text.Split(' ')[2];
            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
            string response = cleanMessage(bytes);
            if(response == "OK")
            {
                ListViewItem item = new ListViewItem(tableNameTextBox.Text);
                command.tableName = tableNameTextBox.Text;
                command.SqlQuery = "";
                tableNameTextBox.Text = "";
                item.Tag = command;
                _firstForm.tablesListView.Items.Add(item);
                this.Hide();
                this.Close();
            }
            else
            {
                MessageBox.Show(response, "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;
            try // Try connecting and send the message bytes  
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Create a new connection  
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  
                Console.WriteLine("================================");
                Console.WriteLine("=   Connected to the server    =");
                Console.WriteLine("================================");
                Console.WriteLine("Waiting for response...");

                messageBytes = new byte[bytesize]; // Clear the message   
                stream.Read(messageBytes, 0, messageBytes.Length);
                // Receive the stream of bytes  

                // Clean up  
                stream.Dispose();
                client.Close();
            }
            catch (Exception e) // Catch exceptions  
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes; // Return response  
        }
        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }
    }
}
