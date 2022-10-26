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
            string dir = @"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\" + dbName;
            // If directory does not exist, don't even try.
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            catalog.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
        }
        public void dropTable(string tableName, Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetNode = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         command.tableName));
            Console.WriteLine("Deleting table " + command.tableName);
            allTables.RemoveChild(targetNode);

            //delete table files and index files
            string path = @"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\students\";
            string[] filePaths = Directory.GetFiles(path, String.Format("{0}_", command.tableName), SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                Console.WriteLine(filePath);
                File.Delete(filePath);
            }
            if(File.Exists(path + command.tableName + ".kv"))
            {
                File.Delete(path + command.tableName + ".kv");
            }
            if (File.Exists(path + command.tableName + ".ind"))
            {
                File.Delete(path + command.tableName + ".Ind");
            }



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

            var target = targetDb.FirstChild;
            foreach (XmlNode node in target)
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
                    indexAttr.InnerText = pkey;
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

        public void createIndex(string indexName, Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");

            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         command.tableName));
            var indexFiles = targetTable.SelectSingleNode(String.Format("IndexFiles"));
            XmlNodeList allIndexes = indexFiles.ChildNodes;
            //Console.WriteLine(indexFiles.OuterXml);
            foreach(XmlNode node in allIndexes)
            {
                Console.WriteLine(node.Attributes[0].Value);
                if(indexName+".ind" == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja un index pentru acest camp");
                }
                if(String.Format("{0}_{1}", command.tableName, indexName) == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja un index pentru acest camp");
                }
            }

            ///nu exista index, il cream
            XmlElement newIndex = catalog.CreateElement("IndexFile");
            XmlElement indexAttributes = catalog.CreateElement("IndexAttributes");
            newIndex.AppendChild(indexAttributes);
            /// IndexFile -> IndexAttributes
            XmlAttribute fname = catalog.CreateAttribute("fileName");
            XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
            fname.Value = String.Format("{0}_{1}", command.tableName, indexName);
            isUnique.Value = "1";
            newIndex.Attributes.Append(fname);
            newIndex.Attributes.Append(isUnique);
            ///Index file atribut atribut -> IndexAttributes
            XmlElement indexAttr = catalog.CreateElement("IAttribute");
            indexAttr.InnerText = indexName;
            indexAttributes.AppendChild(indexAttr);
            // IndexAttributes -> IAttribute text
            indexFiles.AppendChild(newIndex);
            catalog.Save(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");


        }

        public List<Command> getTables(Command command)
        {
            Console.WriteLine("Get tables from " + command.dbName);
            XmlDocument catalog = new XmlDocument();
            catalog.Load(@"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml");
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var target = targetDb.FirstChild;
            //Console.WriteLine(target.OuterXml);
            List<Command> tables    = new List<Command>(); 
            foreach (XmlNode node in target)
            {
                Command table = new Command();
                table.AttributesList = new List<AtributTabel>();
                table.tableName = node.Attributes[0].Value;
                XmlNodeList structure = node.SelectSingleNode("Structure").ChildNodes;
                foreach (XmlNode node2 in structure)
                {
                    AtributTabel atribut = new AtributTabel();
                    atribut.Name = node2.Attributes[0].Value;
                    atribut.Type = node2.Attributes[1].Value;
                    atribut.IsUnique = Convert.ToBoolean((Convert.ToInt32(node2.Attributes[2].Value)));
                    table.AttributesList.Add(atribut);
                }
                table.dbName = command.dbName;
                tables.Add(table);
            }
            return tables;
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
                        if (words[1] == "INDEX")
                        {
                            Console.WriteLine("Creating  index on table" + command.tableName);
                            createIndex(words[2], command);
                        }
                        
                        break;
                    case "DROP":
                        if (words[1] == "DATABASE")
                        {
                            dropDatabase(words[2]);
                        }
                        if (words[1] == "TABLE")
                        {
                            dropTable(words[2], command);
                        }
                        break;
                    case "GET":
                        if (words[1] == "TABLES")
                        {
                            getTables(command);
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
