using DBMSServer.Model;
using DBMSServer.repo;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DBMSServer.Service
{
    class Service
    {
        private string catalogPath = @"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\Catalog.xml";
        Repo repository;
        public Service()
        {
            repository = new Repo();
        }

        public void readCatalog()
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine(node.Attributes[0].Value);
            }
        }

        public List<ForeignKey> getRefferredBy(string tableName, string dbName)
        {
            List<ForeignKey> tables = new List<ForeignKey>();

            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));

            var allTables = targetDb.FirstChild;

            foreach(XmlNode table in allTables)
            {
                Console.WriteLine(table.InnerXml);
                XmlNode foreignKeys = table.SelectSingleNode("foreignKeys");
                if (foreignKeys.HasChildNodes == false)
                    continue;
                foreach(XmlNode key in foreignKeys.ChildNodes)
                {
                    XmlNode references = key.SelectSingleNode("references");
                    string refTable = references.SelectSingleNode("refTable").InnerText;
                    string refAttribute = references.SelectSingleNode("refAttribute").InnerText;
                    if (refTable == tableName)
                    {
                        tables.Add(new ForeignKey(key.SelectSingleNode("fkAttribute").InnerText, table.Attributes[0].Value, refTable, refAttribute));
                        break;
                    }
                }
            }
            return tables;
        } 

        public void dropDatabase(string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            //XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");

            var root = catalog.SelectSingleNode("Databases");
            Console.WriteLine(String.Format("DataBase[@dbName={0}]",
                         dbName));
            var targetNode = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));
            Console.WriteLine(targetNode.OuterXml);
            root.RemoveChild(targetNode);

            repository.DropDatabase(dbName);
            catalog.Save(catalogPath);
        }
        public void dropTable(string tableName, Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetNode = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         command.tableName));

            var referencedIn = getRefferredBy(command.tableName, command.dbName);
            if(referencedIn.Count > 0)
            {
                string tables = "";
                foreach(var table in referencedIn)
                {
                    if (tables == "")
                        tables = table.referencedTable;
                    else
                        tables += " " + table.referencedTable;
                }
                throw new Exception(String.Format("Unable to drop table because it is a parent to: {0}", tables));
            }

            Console.WriteLine("Deleting table " + command.tableName);
            allTables.RemoveChild(targetNode);

            repository.DropCollection(command.tableName, command.dbName);
            catalog.Save(catalogPath);
        }

        //public void createIndex(string tableName, string dbName, string fileName, AtributTabel atribut)
        //{
        //    XmlDocument catalog = new XmlDocument();
        //    catalog.Load(catalogPath);
        //    var root = catalog.SelectSingleNode("Databases");
        //    var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
        //                 dbName));

        //    var allTables = targetDb.FirstChild;
        //    XmlNode indexFiles = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
        //                tableName)).SelectSingleNode("IndexFiles");
        //    // am ajuns la tabelul potrivit, acum creez index
        //    XmlElement mainIndex = catalog.CreateElement("IndexFile");
        //    XmlAttribute fname = catalog.CreateAttribute("fileName");
        //    XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
        //    isUnique.Value = atribut.IsUnique.ToString();
        //    fname.Value = fileName;
        //    mainIndex.Attributes.Append(fname);
        //    mainIndex.Attributes.Append(isUnique);
        //    ///atasez IndexFile la indexfiles
        //    indexFiles.AppendChild(mainIndex);
        //    ///acum creez IndexAttributes pe care il atasez la IndexFile
        //    XmlElement IndexAttributes = catalog.CreateElement("IndexAttributes");
        //    mainIndex.AppendChild(IndexAttributes);
        //    //in continuare trebuie sa creez IAttribute unde salvez atributul
        //    XmlElement indexAttr = catalog.CreateElement("IAttribute");
        //    indexAttr.InnerText = atribut.Name;
        //    IndexAttributes.AppendChild(indexAttr);

        //    catalog.Save(catalogPath);

        //}

        public void createTable(string tableName, Command command)
        {
            command.tableName = tableName;
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
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
            XmlElement fk = catalog.CreateElement("foreignKeys");
            XmlElement indexFiles = catalog.CreateElement("IndexFiles");
            table.AppendChild(structure);
            table.AppendChild(pk);
            table.AppendChild(fk);
            table.AppendChild(indexFiles);

            //Iterate through atributes, if one is pk we save it
            List<String> pkNames = new List<String>();
            List<AtributTabel> fkeys = new List<AtributTabel>();
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
                if (atribut.FKeys != null && atribut.FKeys.Count > 0)
                {
                    fkeys.Add(atribut);
                }
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
            XmlNodeList existingKeys = fk.ChildNodes;

            //ar trebuii pentru fiecare tabel sa vedem toate campurile catre care este fk
            Dictionary<string, List<AtributTabel>> tables = new Dictionary<string, List<AtributTabel>>();
            var myList = command.AttributesList.ToList();
            myList.RemoveAll(a => a.IsPrimaryKey == true);
            foreach (AtributTabel key in fkeys)
            {
                foreach (var item in key.FKeys)
                {
                    //parcurg fiecare atribut
                    if (tables.ContainsKey(item.Key))
                    {
                        //daca tabelul a aparut deja de la alt atribut
                        //atunci doresc sa adaug la acelasi tabel atributul curent
                        tables[item.Key].Add(key);
                    }
                    else
                    {
                        tables.Add(item.Key, new List<AtributTabel>());
                        tables[item.Key].Add(key);
                    }
                }
                var myAttribute = myList.FirstOrDefault(i => i.Name == key.Name);
                int index = myList.IndexOf(myAttribute);
                repository.CreateIndex(String.Format("Index_{0}_{1}", command.tableName, key.Name), command.dbName, index);
            }
            foreach (var auxTable in tables)
            {
                XmlElement foreignKey = catalog.CreateElement("foreignKey");
                fk.AppendChild(foreignKey);

                XmlElement references = catalog.CreateElement("references");
                foreignKey.AppendChild(references);
                XmlElement refTable = catalog.CreateElement("refTable");
                refTable.InnerText = auxTable.Key;

                foreach (var item in auxTable.Value)
                {
                    XmlElement fkAttribute = catalog.CreateElement("fkAttribute");
                    fkAttribute.InnerText = item.Name;
                    foreignKey.AppendChild(fkAttribute);

                    XmlElement refAttribute = catalog.CreateElement("refAttribute");
                    refAttribute.InnerText = item.FKeys[auxTable.Key];
                    references.AppendChild(refAttribute);
                }
                references.AppendChild(refTable);

            }
            repository.CreateCollection(tableName, command.dbName);
            catalog.Save(catalogPath);
        }

        public void createDatabase(string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);

            ///verifica daca db nu exista deja cu acelasi nume
            XmlNodeList nodes = catalog.SelectNodes("Databases/DataBase");
            foreach (XmlNode node in nodes)
            {
                if (dbName == node.Attributes[0].Value)
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


            repository.CreateDatabase(dbName);
            catalog.Save(catalogPath);



            string dir = @"C:\Users\patra\Documents\GitHub\dbmsi\t2\DBMSServer\DBMSServer\" + dbName;
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public void createIndex(string indexName, Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);

            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         command.tableName));
            var indexFiles = targetTable.SelectSingleNode(String.Format("IndexFiles"));
            XmlNodeList allIndexes = indexFiles.ChildNodes;
            //Console.WriteLine(indexFiles.OuterXml);
            foreach (XmlNode node in allIndexes)
            {
                if (indexName + ".ind" == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja un index pentru acest camp");
                }
                if (String.Format("Index_{0}_{1}", command.tableName, indexName) == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja un index pentru acest camp");
                }
            }
            var myList = command.AttributesList;
            myList.RemoveAll(a => a.IsPrimaryKey == true);
            var myAttribute = myList.FirstOrDefault(i => i.Name == indexName);
            int index = myList.IndexOf(myAttribute);
            ///nu exista index, il cream
            XmlElement newIndex = catalog.CreateElement("IndexFile");
            XmlElement indexAttributes = catalog.CreateElement("IndexAttributes");
            newIndex.AppendChild(indexAttributes);
            /// IndexFile -> IndexAttributes
            XmlAttribute fname = catalog.CreateAttribute("fileName");
            XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
            fname.Value = String.Format("Index_{0}_{1}", command.tableName, indexName);
            isUnique.Value = myAttribute.IsUnique.ToString();
            newIndex.Attributes.Append(fname);
            newIndex.Attributes.Append(isUnique);
            ///Index file atribut atribut -> IndexAttributes
            XmlElement indexAttr = catalog.CreateElement("IAttribute");
            indexAttr.InnerText = indexName;
            indexAttributes.AppendChild(indexAttr);
            // IndexAttributes -> IAttribute text
            indexFiles.AppendChild(newIndex);


            repository.CreateIndex(String.Format("Index_{0}_{1}", command.tableName, indexName), command.dbName, index);
            catalog.Save(catalogPath);
        }

        public List<string> getIndexes(string tableName, string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         tableName));
            //now we need to get to the IndexFile tag so we can iterate through its children
            var indexFiles = targetTable.SelectSingleNode("IndexFiles");
            List<string> rez = new List<string>();
            foreach (XmlNode node in indexFiles)
            {
                rez.Add(node.SelectSingleNode("IndexAttributes/IAttribute").InnerText);
            }

            return rez;
        }

        public List<ForeignKey> getForeignKeys(string tableName, string dbName)
        {
            List<ForeignKey> foreignKeys = new List<ForeignKey>();
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         tableName));
            //we need to get to all foreign keys
            var allKeys = targetTable.SelectSingleNode("foreignKeys");

            foreach (XmlNode key in allKeys)
            {
                XmlNode reference = key.SelectSingleNode("references");
                string refTable = reference.SelectSingleNode("refTable").InnerText;
                string refAttribute = reference.SelectSingleNode("refAttribute").InnerText;
                string tableAttr = key.SelectSingleNode("fkAttribute").InnerText;
                foreignKeys.Add(new ForeignKey(tableAttr, tableName, refTable, refAttribute));
            }

            return foreignKeys;
        }

        public void insertInTable(Command command)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         command.tableName));
            var targetStructure = targetTable.SelectSingleNode("Structure");
            List<AtributTabel> atribute = new List<AtributTabel>();
            string key = "", value = "";

            
            //get all pkeys
            List<string> pkeys = new List<string>();
            XmlNodeList keys = targetTable.SelectSingleNode("primaryKey").ChildNodes;
            foreach (XmlNode k in keys)
            {
                Console.WriteLine("Primary key: " + k.InnerText);
                pkeys.Add(k.InnerText);
            }
            foreach (XmlElement atribut in targetStructure.ChildNodes)
            {
                AtributTabel at = new AtributTabel();
                at.Name = atribut.GetAttribute("attributeName");
                at.Type = atribut.GetAttribute("attributeType");
                at.IsUnique = (atribut.GetAttribute("isUnique") == "1") ? true : false;
                atribute.Add(at);
                if (pkeys.Contains(at.Name))
                {
                    if (key == "")
                        key = command.Values[at.Name];
                    else
                        key += "#" + command.Values[at.Name];
                }
                else
                {
                    if (value == "")
                    {
                        value = command.Values[at.Name];
                    }
                    else
                    {
                        value += "#" + command.Values[at.Name];
                    }
                }
            }
            Console.WriteLine("Key: " + key);
            Console.WriteLine("Value:" + value);
            checkValues(atribute, command.Values);
            //values are now checked, should be sent to repo for inserting
            //before sending, we should check if foreign key exists
            List<ForeignKey> foreignKeys = getForeignKeys(command.tableName, command.dbName);
            //i now have the foreign keys, i have to verify for each value that it exists in the table it references
            //check in the referenced table, if the referenced attribute index contains the atribute value i got
            foreach (ForeignKey foreignKey in foreignKeys)
            {
                string atributeValue = command.Values[foreignKey.attribute];
                var pks = getPrimaryKeys(foreignKey.referencedTable, command.dbName);
                string indexToBeChecked = "";
                if (pks.Contains(foreignKey.referencedAttribute))
                    indexToBeChecked = foreignKey.referencedTable;
                else
                    indexToBeChecked = String.Format("Index_{0}_{1}", foreignKey.referencedTable, foreignKey.referencedAttribute);
                if (repository.checkExistence(indexToBeChecked, command.dbName, atributeValue) == false)
                    throw new Exception(String.Format("Campul {0} cu valoarea {1} nu exista in tabelul {2}", foreignKey.attribute, atributeValue, foreignKey.referencedTable));
            }
            
            repository.Insert(command.tableName, command.dbName, key, value);
            var indexes = this.getIndexes(command.tableName, command.dbName);
            foreach (var index in indexes)
            {
                Console.WriteLine(index);
                value = command.Values[index];
                if (value != key)
                    repository.InsertInIndex(String.Format("Index_{0}_{1}", command.tableName, index), command.dbName, value, key);
            }
            //foreach index get value and insert (indexV, key)
        }

        public void updateRecord(Command command, string tableName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         tableName));
            var targetStructure = targetTable.SelectSingleNode("Structure");
            List<AtributTabel> atribute = new List<AtributTabel>();
            string key = "", value = "";
            //get all pkeys
            List<string> pkeys = new List<string>();
            pkeys = getPrimaryKeys(tableName, command.dbName);
            foreach (XmlElement atribut in targetStructure.ChildNodes)
            {
                AtributTabel at = new AtributTabel();
                at.Name = atribut.GetAttribute("attributeName");
                at.Type = atribut.GetAttribute("attributeType");
                at.IsUnique = (atribut.GetAttribute("isUnique") == "1") ? true : false;
                atribute.Add(at);
                if (pkeys.Contains(at.Name))
                {
                    if (key == "")
                        key = command.Values[at.Name];
                    else
                        key += "#" + command.Values[at.Name];
                }
                else
                {
                    if (value == "")
                    {
                        value = command.Values[at.Name];
                    }
                    else
                    {
                        value += "#" + command.Values[at.Name];
                    }
                }
            }
            Console.WriteLine("Key: " + key);
            Console.WriteLine("Value:" + value);
            checkValues(atribute, command.Values);
            //values are now checked, should be sent to repo for inserting
            //before sending, we should create the key-value pairs
            repository.UpdateRecord(command.tableName, command.dbName, key, value);
        }

        public void checkValues(List<AtributTabel> atribute, Dictionary<string, string> values)
        {
            foreach (AtributTabel at in atribute)
            {
                if (at.Type == "string")
                {
                    if (values[at.Name] == "")
                    {
                        throw new Exception("Attributes cannot be empty");
                    }
                    continue;
                }
                if (at.Type == "boolean")
                {
                    if (values[at.Name] != "1" && values[at.Name] != "0")
                    {
                        throw new Exception("Campurile booleane trebuie completate cu 0 sau 1");
                    }
                }
                if (at.Type == "int")
                {
                    try
                    {
                        int a = Convert.ToInt32(values[at.Name]);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Campul " + at.Name + " trebuie sa contina un numar intreg");
                    }
                }
                if (at.Type == "double")
                {
                    try
                    {
                        double a = Convert.ToDouble(values[at.Name]);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Campul " + at.Name + " trebuie sa contina un numar real");
                    }
                }
            }
        }

        public List<Command> getTables(Command command)
        {
            Console.WriteLine("Get tables from " + command.dbName);
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         command.dbName));

            var target = targetDb.FirstChild;
            //Console.WriteLine(target.OuterXml);
            List<Command> tables = new List<Command>();
            foreach (XmlNode node in target)
            {
                Command table = new Command();
                table.AttributesList = new List<AtributTabel>();
                table.tableName = node.Attributes[0].Value;
                table.AttributesList = getTableAttributes(table.tableName, command.dbName);
                //XmlNodeList structure = node.SelectSingleNode("Structure").ChildNodes;
                //foreach (XmlNode node2 in structure)
                //{
                //    AtributTabel atribut = new AtributTabel();
                //    atribut.Name = node2.Attributes[0].Value;
                //    atribut.Type = node2.Attributes[1].Value;
                //    atribut.IsUnique = Convert.ToBoolean((Convert.ToInt32(node2.Attributes[2].Value)));
                //    table.AttributesList.Add(atribut);
                //}
                table.dbName = command.dbName;
                tables.Add(table);
            }
            return tables;
        }


        public void deleteFromTable(Command command, string id)
        {

            //before deleting we should check if there is a child table using this
            //ex GroupId 531 used by student in the group 531
            //suppose we are trying to delete the group 531, we need to

            //get all the tables that reference the table groups
            var fkeys = getRefferredBy(command.tableName, command.dbName);
            //we have a list of foreign keys that reference the table groups
            //since it is a reference, every table in this list should have an index for easy searching
            //then we need to iterate through each table, and check if the index_table_refAttribute is equal to the id we're trying to delete
            // if yes, throw errror, if not, proceed with the deletion
            foreach(var fk in fkeys)
            {
                string indexTable = String.Format("Index_{0}_{1}", fk.table, fk.attribute);
                if (repository.checkExistence(indexTable, command.dbName, id))
                {
                    throw new Exception("Cannot delete the record with the id " + id + " because it is referenced in table " + fk.table);
                }
            }

            repository.DeleteRecord(command.tableName, command.dbName, id);
            var indexes = getIndexes(command.tableName, command.dbName);
            foreach (var index in indexes)
            {
                //id este primary key-ul care trebuie sters
                //noi trebuie pentru fiecare index sa stergem
                //a) tot randul care il contine pe id ca si valoare in cazul celor unique
                //b) sa eliminam cheia id din indexul non-unique
                var indexValue = command.Values[index];
                if (indexValue != id)
                    repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, index), command.dbName, id, indexValue);
            }
        }
        public List<string> getPrimaryKeys(string tableName, string dbName)
        {
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));
            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         tableName));
            XmlNodeList pkAttributes = targetTable.SelectSingleNode("primaryKey").ChildNodes;
            List<string> primarykeys = new List<string>();
            foreach (XmlNode pkAttribute in pkAttributes)
            {
                primarykeys.Add(pkAttribute.InnerText);
                Console.WriteLine(pkAttribute.InnerText);
            }
            return primarykeys;
        }

        public List<AtributTabel> getTableAttributes(string tableName, string dbName)
        {
            var rezAttributes = new List<AtributTabel>();
            XmlDocument catalog = new XmlDocument();
            catalog.Load(catalogPath);
            var root = catalog.SelectSingleNode("Databases");
            var targetDb = root.SelectSingleNode(String.Format("DataBase[@dbName='{0}']",
                         dbName));
            var allTables = targetDb.FirstChild;
            var targetTable = allTables.SelectSingleNode(String.Format("Table[@tableName='{0}']",
                         tableName));

            XmlNodeList tableAttributes = targetTable.SelectSingleNode("Structure").ChildNodes;
            List<string> pkeys = getPrimaryKeys(tableName, dbName);

            foreach (XmlElement attribute in tableAttributes)
            {
                AtributTabel at = new AtributTabel();
                at.Name = attribute.GetAttribute("attributeName");
                at.Type = attribute.GetAttribute("attributeType");
                at.IsUnique = (attribute.GetAttribute("isUnique") == "1") ? true : false;
                if (pkeys.Contains(at.Name))
                {
                    at.IsPrimaryKey = true;
                }
                rezAttributes.Add(at);
            }

            return rezAttributes;
        }


        public void deleteFromTableWhere(Command command)
        {
            var words = command.SqlQuery.Split(" ");
            string whereAttribute = words[2];
            string whereClause = words[3];
            //get position in the structure
            List<string> primaryKeys = getPrimaryKeys(command.tableName, command.dbName);
            List<AtributTabel> atribute = getTableAttributes(command.tableName, command.dbName);

            int position = 0;
            foreach (var at in atribute)
            {
                if (at.Name == whereAttribute) break;
                if (at.IsPrimaryKey == false)
                {
                    position++;
                }
            }
            repository.DeleteRecordWhere(command.tableName, command.dbName, whereAttribute, whereClause, position);
        }

        public void deleteWhereIndex(Command command)
        {
            //get key
            var splits = command.SqlQuery.Split(' ');
            var myAttribute = splits[2];
            var myAttributeValue = splits[3];
            var indexes = getIndexes(command.tableName, command.dbName);
            if (indexes.Contains(myAttribute) == false) { 
                deleteFromTableWhere(command);
                return;
            }
            else
            {
                Record indexedRecord = repository.loadRecordById(String.Format("Index_{0}_{1}", command.tableName, myAttribute), command.dbName, myAttributeValue);
                //i now have the primary key in record.Value, so I proceed to remove it from the main and the rest of
                List<string> keys = indexedRecord.Value.Split('#').ToList();
                //key can be MPP or something like MPP#AI
                foreach(string key in keys)
                {
                    Record record = repository.loadRecordById(command.tableName, command.dbName, key);

                    //delete the main index
                    repository.DeleteRecord(command.tableName, command.dbName, key);
                    //now how do i know which part of the value is the index im trying to delete
                    List<string> values = record.Value.Split("#").ToList();
                    int position = 0;
                    foreach (AtributTabel atr in command.AttributesList)
                    {
                        if (atr.IsPrimaryKey == false)
                        {
                            if (indexes.Contains(atr.Name))
                            {
                                var indexKey = values[position];
                                repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, atr.Name), command.dbName, key, indexKey);
                            }
                            position++;
                        }
                    }
                }
            }
            //load record

            //delete main
            //delete in indexes

        }



        public string ExecuteCommand(Command command)
        {
            try
            {
                string[] words = command.SqlQuery.Split(' ');
                switch (words[0])
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
                            var myList = command.AttributesList.ToList();
                            foreach (AtributTabel atribut in myList)
                            {
                                if (atribut.IsPrimaryKey == false && (atribut.FKeys != null || atribut.IsUnique == true))
                                {
                                    createIndex(atribut.Name, command);
                                }
                            }
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
                    case "INSERT":
                        if (words[1] == "INTO")
                        {
                            insertInTable(command);
                        }
                        break;
                    case "DELETE":
                        {
                            if (words[1] == "WHERE")
                            {
                                //useless after lab3
                                //deleteFromTableWhere(command);
                                deleteWhereIndex(command);
                            }
                            else
                            {
                                deleteFromTable(command, words[1]);
                            }
                        }
                        break;
                    case "UPDATE":
                        updateRecord(command, words[1]);
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
