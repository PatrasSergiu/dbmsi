﻿// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Xml;
using DBMSServer.Model;
using DBMSServer.Service;
using System.Reflection;
using System.Windows.Input;
using System.Diagnostics;

namespace TcpServer
{
    class Program
    {
        static readonly Stopwatch timer = new Stopwatch();
        static void testSelect(Service srvDBMS)
        {
            timer.Start();
            //srvDBMS.insertTestRows();
            Console.WriteLine("250k rows inserted " );
            timer.Restart();
            srvDBMS.testSelectIndex();
            Console.WriteLine("250k rows select with index: " + timer.Elapsed.ToString());
            timer.Restart();
            srvDBMS.testSelectScan();
            Console.WriteLine("250k rows select without index: " + timer.Elapsed.ToString());
            timer.Stop();

        }

        static void Main(string[] args)
        {
            string path = @"C:\\Users\\patra\\Documents\\GitHub\\dbmsi\\t2\\DBMSServer\\DBMSServer\\Catalog.xml";
            if (File.Exists(path))
            {
                Console.WriteLine("checked for catalog");
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("Databases");
                doc.AppendChild(root);
                doc.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
                Console.WriteLine("Created catalog");
            }
            Service srvDBMS = new Service();
            srvDBMS.readCatalog();
            //testSelect(srvDBMS);
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
            TcpListener listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine(@"  
            ===================================================  
                   Started listening requests at: {0}:{1}  
            ===================================================",
            ep.Address, ep.Port);

            // Run the loop continuously; this is the server.  
            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                // Read the message and perform different actions  
                message = cleanMessage(buffer);

                // Save the data sent by the client;  
                Command command = JsonConvert.DeserializeObject<Command>(message); // Deserialize  
                string text = srvDBMS.ExecuteCommand(command);
                byte[] bytes = System.Text.Encoding.Unicode.GetBytes(text);
                sender.GetStream().Write(bytes, 0, bytes.Length); // Send the response
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

        // Sends the message string using the bytes provided.  
        private static void sendMessage(byte[] bytes, TcpClient client)
        {
            client.GetStream()
                .Write(bytes, 0,
                bytes.Length); // Send the stream  
        }
    }
}
