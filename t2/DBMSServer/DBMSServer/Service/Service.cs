using DBMSServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DBMSServer.Service
{
    class Service
    {

        public Service()
        {

        }

        public void readCatalog()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine(node.Attributes[0].Value);
            }
        }

        public void dropDatabase(string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            //XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");

            var root = catalog.SelectSingleNode("Databases");
            Console.WriteLine(String.Format("DataBase[@dbName={0}]",
                         dbName));
            var targetNode = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));
            Console.WriteLine(targetNode.OuterXml);
            root.RemoveChild(targetNode);

            catalog.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
        }

        public void createTable(string tableName, Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            //getting the right db
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));
           
            XmlNodeList allTables = targetDb.SelectNodes("/Tables");
            var target = targetDb.FirstChild;
            foreach (XmlNode node in allTables)
            {
                if (tableName == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja tabel cu acest nume");
                }
            }

            //Create table with the attributes
            XmlElement table = catalog.CreateElement("Table");
            target.AppendChild(table);

            //create attributes tableName and fileName
            XmlAttribute tName = catalog.CreateAttribute("tableName");
            tName.Value = tableName;
            XmlAttribute fileName = catalog.CreateAttribute("fileName");
            fileName.Value = String.Format("{0}.kv", tableName);
            table.Attributes.Append(tName);
            table.Attributes.Append(fileName);

            XmlElement structure = catalog.CreateElement("Structure");
            XmlElement pk = catalog.CreateElement("primaryKey");
            XmlElement indexFiles = catalog.CreateElement("IndexFiles");
            table.AppendChild(indexFiles);
            table.AppendChild(structure);
            table.AppendChild(pk);

            //Iterate through atributes, if one is pk we save it
            List<String> pkNames = new List<String>();
            foreach (AtributTabel atribut in command.AttributesList)
            {
                XmlElement attribute = catalog.CreateElement("Attribute");
                structure.AppendChild(attribute);

                XmlAttribute attributeName = catalog.CreateAttribute("attributeName");
                XmlAttribute attributeType = catalog.CreateAttribute("attributeType");
                XmlAttribute attributeUnique = catalog.CreateAttribute("isUnique");
                attribute.Attributes.Append(attributeName);
                attribute.Attributes.Append(attributeType);
                attribute.Attributes.Append(attributeUnique);
                attributeName.Value = atribut.Name;
                attributeType.Value = atribut.Type;
                attributeUnique.Value = (atribut.IsUnique == true) ? 1.ToString() : 0.ToString();

                if (atribut.IsPrimaryKey)
                    pkNames.Add(atribut.Name);
            }
            ///main IndexFile
            XmlElement mainIndex = catalog.CreateElement("IndexFile");
            XmlAttribute fname = catalog.CreateAttribute("fileName");
            XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
            isUnique.Value = "1";
            fname.Value = String.Format("{0}.ind", tableName);
            mainIndex.Attributes.Append(fname);
            mainIndex.Attributes.Append(isUnique);

            ///IndexAttributes
            XmlElement IndexAttributes = catalog.CreateElement("IndexAttributes");
            mainIndex.AppendChild(IndexAttributes);

            if (pkNames.Count > 0)
            {
                foreach (string pkey in pkNames)
                {
                    XmlElement pkAttribute = catalog.CreateElement("pkAttribute");
                    pkAttribute.InnerText = pkey;
                    pk.AppendChild(pkAttribute);

                    XmlElement indexAttr = catalog.CreateElement("IAttribute");
                    IndexAttributes.InnerText = pkey;
                    IndexAttributes.AppendChild(indexAttr);
                }
            }
            indexFiles.AppendChild(mainIndex);


            catalog.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            var path = String.Format(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\{0}\{1}.kv", command.dbName, tableName);
            FileStream fs = File.Create(path);
            path = String.Format(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\{0}\{1}.ind", command.dbName, tableName);
            fs = File.Create(path);


        }

        public void createDatabase(string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");

            ///verifica daca db nu exista deja cu acelasi nume
            XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");
            foreach (XmlNode node in nodes)
            {
                if(dbName == node.Attributes[0].Value)
                {
                    throw new Exception("Exista db cu acest nume");
                }
            }


            ////adauga db nou
            Console.WriteLine("New dbName = " + dbName);
            XmlNode root = catalog.SelectSingleNode("Databases");
            XmlElement database = catalog.CreateElement("DataBase");
            root.AppendChild(database);

            XmlAttribute databaseName = catalog.CreateAttribute("dbName");
            database.Attributes.Append(databaseName);
            databaseName.Value = dbName;
            Console.WriteLine(root.OuterXml);

            XmlElement tables = catalog.CreateElement("Tables");
            database.AppendChild(tables);
            catalog.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");

            string dir = @"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\"+dbName;
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public string ExecuteCommand(Command command)
        {
            try
            {
                string[] words = command.SqlQuery.Split(' ');
                switch(words[0])
                {
                    case "CREATE":
                        if (words[1] == "DATABASE")
                        {
                            Console.WriteLine("Creating database");
                            createDatabase(words[2]); 
                        }
                        if (words[1] == "TABLE")
                        {
                            Console.WriteLine("Creating table");
                            createTable(words[2], command);
                        }
                        
                        break;
                    case "DROP":
                        if (words[1] == "DATABASE")
                        {
                            dropDatabase(words[2]);
                        }
                        break;
                }
                return "OK";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }
    }
}
