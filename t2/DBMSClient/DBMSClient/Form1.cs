using DBMSClient.Model;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Xml;

namespace DBMSClient
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");
            foreach (XmlNode node in nodes)
            {
                ListViewItem item = new ListViewItem(node.Attributes[0].Value);
                dbListView.Items.Add(item);                            
            }

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

        private void button1_Click(object sender, EventArgs e)
        {
            string databaseName = newDbTextBox.Text;
            Command command = new Command();
            command.SqlQuery = "CREATE DATABASE " + databaseName;
            newDbTextBox.Text = "";


            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
            string response = cleanMessage(bytes);
            statusLabel.Text = response;

            if(response == "OK")
            {
                ListViewItem item = new ListViewItem(databaseName);
                dbListView.Items.Add(item);
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

        private void DropDbButton_Click(object sender, EventArgs e)
        {
            string toBeDeleted = dbListView.SelectedItems[0].Text;
            Command command = new Command();
            command.SqlQuery = "DROP DATABASE " + toBeDeleted;

            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(command)));
            string response = cleanMessage(bytes);
            statusLabel.Text = response.ToString();

            if(response == "OK")
            {
                if(dbListView.Items.Count > 0)
                {
                    dbListView.Items.Remove(dbListView.SelectedItems[0]);
                }
            }
        }

        private void UseDbButton_Click(object sender, EventArgs e)
        {
            if (dbListView.SelectedItems.Count > 0)
            {
                string dbName = dbListView.SelectedItems[0].Text;
                TablesView tablesView = new TablesView(this);
                tablesView.Show();
            }
            else
            {
               
                    MessageBox.Show("Please select a database",
                        "Oops",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning // for Warning  
                                             //MessageBoxIcon.Error // for Error 
                                             //MessageBoxIcon.Information  // for Information
                                             //MessageBoxIcon.Question // for Question
                           );
                  
                
            }
        }

 
    }
}