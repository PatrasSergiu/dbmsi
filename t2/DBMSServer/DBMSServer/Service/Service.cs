﻿using DBMSServer.Model;
using DBMSServer.repo;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
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

            var indexes = getIndexes(command.tableName, command.dbName);
            for (int i = 1; i < indexes.Count; i++)
            {
                string numeTabel = String.Format("Index_{0}_{1}", command.tableName, indexes[i]);
                repository.DropCollection(numeTabel, command.dbName);
            }

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
                if (tableName+".kv" == node.Attributes[0].Value)
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

            string mainIndexFn = pkNames[0];
            for (int i = 1; i < pkNames.Count; i++)
                mainIndexFn += " " + pkNames[i];
            ///main IndexFile
            XmlElement mainIndex = catalog.CreateElement("IndexFile");
            XmlAttribute fname = catalog.CreateAttribute("fileName");
            XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
            isUnique.Value = "1";
            fname.Value = String.Format("{0}", tableName);
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
            string collectionName = String.Format("Index_{0}_{1}", command.tableName, indexName);

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
                if (collectionName == node.Attributes[0].Value)
                {
                    throw new Exception("Exista deja un index pentru acest camp");
                }
            }
            var myList = command.AttributesList;
            //myList.RemoveAll(a => a.IsPrimaryKey == true);
            //var myAttribute = myList.FirstOrDefault(i => i.Name == indexName);
            //int index = myList.IndexOf(myAttribute);
            var selectedIndexes = indexName.Split(" ");
            bool Unique = true;
            List<AtributTabel> indexList = new List<AtributTabel>();
            foreach(AtributTabel atr in myList)
            {
                if (selectedIndexes.Contains(atr.Name))
                {
                    indexList.Add(atr);
                    if (atr.IsUnique == false)
                        Unique = false;
                }
            }
            //pana aici am in indexList toate numele de atribute care fac parte din indexul meu
            //in unique daca e unique sau nu
            //mai departe trebuie sa modific xml sa aiba toate campurile nu doar unul
            //trebuie sa fac cumva sa verific in repo ce si cum



            ///nu exista index, il cream
            XmlElement newIndex = catalog.CreateElement("IndexFile");
            XmlElement indexAttributes = catalog.CreateElement("IndexAttributes");
            newIndex.AppendChild(indexAttributes);
            /// IndexFile -> IndexAttributes
            XmlAttribute fname = catalog.CreateAttribute("fileName");
            XmlAttribute isUnique = catalog.CreateAttribute("isUnique");
            fname.Value = collectionName;
            isUnique.Value = (Unique == true) ? "1" : "0";
            newIndex.Attributes.Append(fname);
            newIndex.Attributes.Append(isUnique);
            ///Index file atribut atribut -> IndexAttributes
            foreach(AtributTabel ind in indexList)
            {
                XmlElement indexAttr = catalog.CreateElement("IAttribute");
                indexAttr.InnerText = ind.Name;
                indexAttributes.AppendChild(indexAttr);
            }
            // IndexAttributes -> IAttribute text
            indexFiles.AppendChild(newIndex);
           
            var records = repository.loadCollection(command.tableName, command.dbName);
            foreach(Record record in records)
            {
                var keys = record.Key.Split("$");
                var values = record.Value.Split("#");
                string indexKey = "";
                string indexValue = "";
                int kInd = 0;
                int vInd = 0;
                foreach (AtributTabel atr in myList)
                {
                    string currentAttr = "";
                    if (atr.IsPrimaryKey)
                    {
                        currentAttr = keys[kInd];
                        kInd++;
                        indexValue = (indexValue == "") ? currentAttr : indexValue += "$" + currentAttr;
                    }
                    else
                    {
                        currentAttr = values[vInd];
                        vInd++;                      
                    }
                    if (indexList.Contains(atr))
                    {
                        indexKey = (indexKey == "") ? currentAttr : indexKey += "#" + currentAttr;
                    }

                }
                repository.InsertInIndex(collectionName, command.dbName, indexKey, indexValue);
            }

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
                var indexAttributes = node.SelectSingleNode("IndexAttributes");
                string val = "";
                foreach(XmlNode iattribute in indexAttributes)
                {
                    val = (val == "") ? iattribute.InnerText : val += "$" + iattribute.InnerText;
                }
                rez.Add(val);
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

        public void insertTestRows()
        {
            repository.testInsert();
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
            List<string> pkeys = getPrimaryKeys(command.tableName, command.dbName);

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
                        key += "$" + command.Values[at.Name];
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
            for (int i = 1; i < indexes.Count;i++)
            {
                Console.WriteLine(indexes[i]);
                var splits = indexes[i].Split("$");
                value = "";
                foreach(string s in splits)
                {
                    value = (value == "") ? command.Values[s] : value += "$" + command.Values[s];
                }
                if (value != key)
                    repository.InsertInIndex(String.Format("Index_{0}_{1}", command.tableName, indexes[i].Replace("$", " ")), command.dbName, value, key);
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
                        key += "$" + command.Values[at.Name];
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
            var originalRecord = createObject(command.tableName, command.dbName, repository.loadRecordById(command.tableName, command.dbName, key));
            repository.UpdateRecord(command.tableName, command.dbName, key, value);
            var recordValues = createObject(command.tableName, command.dbName, new Record(key, value));
            var indexes = this.getIndexes(command.tableName, command.dbName);
            for (int index = 1; index < indexes.Count; index++)
            {
                //id este primary key-ul care trebuie sters
                //noi trebuie pentru fiecare index sa stergem
                //a) tot randul care il contine pe id ca si valoare in cazul celor unique
                //b) sa eliminam cheia id din indexul non-unique

                var splits = indexes[index].Split("$");
                var indexKey = "";
                var originalKey = "";
                var indexValue = "";
                foreach (string s in splits)
                {
                    indexKey = (indexKey == "") ? recordValues[s] : indexKey += "$" + recordValues[s];
                    originalKey = (originalKey == "") ? originalRecord[s] : originalKey += "$" + originalRecord[s];
                }
                indexValue = key;
                if (indexKey != indexValue)
                {
                    string indexTable = String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " "));
                    Console.WriteLine(indexTable + " deleting " + indexKey);
                    repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")), command.dbName, key, originalKey);
                    repository.InsertInIndex(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")), command.dbName, indexKey, indexValue);
                }
            }
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
            Record record = repository.loadRecordById(command.tableName, command.dbName, id);
            var recordValues = createObject(command.tableName, command.dbName, record);
            repository.DeleteRecord(command.tableName, command.dbName, id);
            var indexes = getIndexes(command.tableName, command.dbName);
            for (int index = 1; index < indexes.Count; index++)
            {
                //id este primary key-ul care trebuie sters
                //noi trebuie pentru fiecare index sa stergem
                //a) tot randul care il contine pe id ca si valoare in cazul celor unique
                //b) sa eliminam cheia id din indexul non-unique

                var splits = indexes[index].Split("$");
                string value = "";
                foreach (string s in splits)
                {
                    value = (value == "") ? recordValues[s] : value += "$" + recordValues[s];
                }

                if (value != id)
                {
                    Console.WriteLine(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")) + " deleting " + value);
                    repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")), command.dbName, id, value);
                }
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
        public bool isEntireKey(string tableName, string dbName, List<string> attrNames)
        {
            var keys = getPrimaryKeys(tableName, dbName);
            if (keys.Count != attrNames.Count) return false;
            foreach(var key in keys)
            {
                if (attrNames.Contains(key) == false)
                    return false;
            }
            return true;
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

            List<ForeignKey> fkeys = getForeignKeys(tableName, dbName);

            foreach (XmlElement attribute in tableAttributes)
            {
                AtributTabel at = new AtributTabel();
                at.ParentTable = tableName;
                at.Name = attribute.GetAttribute("attributeName");
                at.Type = attribute.GetAttribute("attributeType");
                at.IsUnique = (attribute.GetAttribute("isUnique") == "1") ? true : false;
                if (pkeys.Contains(at.Name))
                {
                    at.IsPrimaryKey = true;
                }
                rezAttributes.Add(at);

                foreach(var fk in fkeys)
                {
                    if(fk.attribute == at.Name)
                    {
                        if(at.FKeys == null)
                        {
                            at.FKeys = new Dictionary<string, string>();
                        }
                        at.FKeys.Add(fk.referencedTable, fk.referencedAttribute);
                    }
                }

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
                throw new Exception("Cannot delete on a field there is no index on. Please create an index first");
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
                    var recordValues = createObject(command.tableName, command.dbName, record);
                    int position = 0;
                    for (int index = 1; index < indexes.Count; index++)
                    {
                        //id este primary key-ul care trebuie sters
                        //noi trebuie pentru fiecare index sa stergem
                        //a) tot randul care il contine pe id ca si valoare in cazul celor unique
                        //b) sa eliminam cheia id din indexul non-unique

                        splits = indexes[index].Split("$");
                        string value = "";
                        foreach (string s in splits)
                        {
                            value = (value == "") ? recordValues[s] : value += "$" + recordValues[s];
                        }

                        if (value != key)
                        {
                            Console.WriteLine(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")) + " deleting " + value);
                            repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, indexes[index].Replace("$", " ")), command.dbName, key, value);
                        }
                    }
                    //foreach (AtributTabel atr in command.AttributesList)
                    //{
                    //    if (atr.IsPrimaryKey == false)
                    //    {
                    //        if (indexes.Contains(atr.Name))
                    //        {
                    //            var indexKey = values[position];
                    //            repository.DeleteRecordIndex(String.Format("Index_{0}_{1}", command.tableName, atr.Name), command.dbName, key, indexKey);
                    //        }
                    //        position++;
                    //    }
                    //}
                }
            }
            //load record

            //delete main
            //delete in indexes

        }

        public Dictionary<string, string> createObject(string tableName, string dbName, Record record)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var keys = record.Key.Split("$");
            var values = record.Value.Split("#");
            int kInd = 0;
            int vInd = 0;
            var atribute = getTableAttributes(tableName, dbName);
            foreach (AtributTabel atr in atribute)
            {
                if (atr.IsPrimaryKey)
                {
                    result.Add(atr.Name, keys[kInd]);
                    kInd++;
                }
                else
                {
                    result.Add(atr.Name, values[vInd]);
                    vInd++;
                }
            }

            return result;
        }

        public Dictionary<string, string> createFilteredObject(List<AtributTabel> atributes, string dbName, Record record)
        {
            var result = new Dictionary<string, string>();

            var keys = record.Key.Split("$");
            var values = record.Value.Split("#");
            int kInd = 0;
            int vInd = 0;
            foreach (AtributTabel atr in atributes)
            {
                if (atr.IsPrimaryKey)
                {
                    result.Add(atr.Name, keys[kInd]);
                    kInd++;
                }
                else
                {
                    result.Add(atr.Name, values[vInd]);
                    vInd++;
                }
            }
            return result;
        }

        public Dictionary<string, string> createFilteredObject(List<AtributTabel> atributes, string dbName, Record record, List<Condition> conditions)
        {
            var result = new Dictionary<string, string>();

            var keys = record.Key.Split("$");
            var values = record.Value.Split("#");
            int kInd = 0;
            int vInd = 0;
            foreach (AtributTabel atr in atributes)
            {
                
                string value;
                if (atr.IsPrimaryKey)
                {
                    result.Add(atr.Name, keys[kInd]);
                    value = keys[kInd];
                    kInd++;
                }
                else
                {
                    result.Add(atr.Name, values[vInd]);
                    value = values[vInd];
                    vInd++;
                }
                foreach (Condition cond in conditions)
                    if (atr.ParentTable == cond.ParentTable && atr.Name == cond.attributeName)
                    {
                        //conditia se refera exact la campul acesta
                        //daca conditia nu este indeplinita, returnam null
                        if (cond.comparation == "EQUAL" && cond.comparationValue != value)
                            return null;
                        if (cond.comparation == "GREATER THAN")
                        {
                            try
                            {
                                if (Convert.ToInt32(cond.comparationValue) >= Convert.ToInt32(value))
                                    return null;

                            }
                            catch(Exception e)
                            {
                                throw new Exception("Attribute type mismatch on conditions");
                            }
                        }
                        if (cond.comparation == "LESSER THAN")
                        {
                            try
                            {
                                if (Convert.ToInt32(cond.comparationValue) <= Convert.ToInt32(value))
                                    return null;

                            }
                            catch (Exception e)
                            {
                                throw new Exception("Attribute type mismatch on conditions");
                            }
                        }
                    }
            }
            return result;
        }

        public Boolean isPart(List<Dictionary<string, string>> dictList, Dictionary<string, string> dict)
        {
            foreach(var d in dictList)
            {
                int t = 1;
                foreach(var k in d.Keys)
                {
                    if (dict[k] != d[k]) t = 0; 
                }
                if (t == 1)
                    return true;
            }

            return false;
        }

        public List<Dictionary<string, string>> eliminateDuplicates(List<Dictionary<string, string>> l1, List<Dictionary<string, string>> l2)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach(var a in l2)
            {
                if (isPart(l1, a)) result.Add(a);
            }

            return result;
        }
        public Boolean checkCondition(Dictionary<string, string> d, List<Condition> conditions, string tableName)
        {
            foreach(Condition c in conditions)
            {
                if (c.ParentTable != tableName) continue;
                var toBeCompared = d[c.attributeName];
                try
                { 
                    if (c.comparation == "EQUAL" && c.comparationValue != toBeCompared) return false;
                    if (c.comparation == "LESSER THAN" && Convert.ToInt32(toBeCompared) >= Convert.ToInt32(c.comparationValue)) return false;
                    if (c.comparation == "GREATER THAN" && Convert.ToInt32(toBeCompared) <= Convert.ToInt32(c.comparationValue)) return false;

                }
                catch(Exception e)
                {
                    throw new Exception("Attribute type mismatch");
                }
            }

            return true;
        }
        public List<Dictionary<string, string>> checkConditions(List<Dictionary<string, string>> l1, List<Condition> conditions, string tableName)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (var a in l1)
            {
                if (checkCondition(a, conditions, tableName)) result.Add(a);
            }

            return result;
        }
        public List<Dictionary<string,string>> loadTable(Command command)
        {
            List<Dictionary<string,string>> result = new List<Dictionary<string,string>>();

            var items = repository.loadCollection(command.tableName, command.dbName);
            foreach(var item in items)
            {
                result.Add(createObject(command.tableName, command.dbName, item));
            }

            return result;
        }

        public List<Dictionary<string, string>> loadTable(Command table, string dbName, List<Condition> conditions)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            var items = repository.loadCollection(table.tableName, dbName);
            foreach (var item in items)
            {
                var obj = createFilteredObject(table.AttributesList, dbName, item, conditions);
                if (obj != null) result.Add(obj);
            }

            return result;
        }

        public List<Dictionary<string, string>> loadFilteredTable(Command table, List<Condition> conditions, string tableName, string dbName)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
                var indexes = getIndexes(table.tableName,dbName);
                List<Record> allRecords = new List<Record>();
                bool searched = false;
                foreach (Condition condition in conditions)
                {
                    if (condition.ParentTable != table.tableName) continue;
                    foreach (var index in indexes)
                    {
                        var atrs = index.Split('$');
                        if (atrs[0] == condition.attributeName && atrs.Length == 1)
                        {
                            List<string> check = new List<string>();
                            check.Add(atrs[0]);
                            string collection;
                            if (isEntireKey(table.tableName, dbName, check) == true)
                            {
                                collection = String.Format("{0}", table.tableName);
                            }
                            else
                            {
                                collection = String.Format("Index_{0}_{1}", table.tableName, index);
                            }

                            var records = repository.loadCollection(collection, dbName, condition);
                            searched = true;
                            List<Dictionary<string, string>> aux = new List<Dictionary<string, string>>();
                            foreach (var record in records)
                            {
                                List<string> splits;
                                if (collection == table.tableName)
                                    splits = record.Key.Split('#').ToList();
                                else
                                {
                                    splits = record.Value.Split('#').ToList();
                                }
                                foreach (string key in splits)
                                {
                                    var originalRecord = repository.loadRecordById(table.tableName,dbName, key);
                                    //if(allRecords.Contains(originalRecord) ==false) allRecords.Add(originalRecord);
                                    aux.Add(createFilteredObject(table.AttributesList, dbName, originalRecord));
                                }
                            }
                            if (result.Count == 0)
                            {
                                result.AddRange(aux);
                            }
                            else
                            {
                                result = eliminateDuplicates(result, aux);
                            }
                        }
                        if (atrs[0] == condition.attributeName && atrs.Length > 1)
                        {
                            List<string> check = new List<string>();
                            check.Add(atrs[0]);
                            string collection;
                            if (isEntireKey(table.tableName, dbName, check) == true)
                            {
                                collection = String.Format("{0}", table.tableName);
                            }
                            else
                            {
                                string mes = "";
                                foreach (var atr in atrs)
                                {
                                    mes = (mes == "") ? atr : mes += " " + atr;
                                }
                                collection = String.Format("Index_{0}_{1}", table.tableName, mes);
                            }
                            var found = conditions.FirstOrDefault(e => e.attributeName == atrs[1]);
                            if (found == null)
                                continue;
                            var records = repository.loadCollectionCompound(collection, dbName, condition, found);
                            searched = true;
                        }
                    }
                
            }
            if (searched == false)
                result = loadTable(table, dbName, conditions);
            else
                result = checkConditions(result, conditions, table.tableName);

            return result;
        }

        public List<Dictionary<string, string>> loadFilteredSelection(Command command)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            List<Command> tableFormSelect = new List<Command>();
            foreach (AtributTabel atr in command.AttributesList)
            {
                var tabel = tableFormSelect.Find(a => a.tableName == atr.ParentTable);
                if (tabel == null)
                {
                    Command comm = new Command();
                    comm.AttributesList = new List<AtributTabel>();
                    comm.AttributesList.Add(atr);
                    comm.tableName = atr.ParentTable;
                    comm.dbName = command.dbName;
                    tableFormSelect.Add(comm);
                }
                else
                {
                    tabel.AttributesList.Add(atr);
                }
            }
            //i have the tables, and the conditions
            // I need to check for each table which conditions are his, and if they have an index
            //if they do, we search by the index, if they don't we table scan
            if(command.Conditions.Count > 0)
            {
                //get the indexes for each table, and then iterate through the conditions to find a useful index
                for (int tablePosition = 0; tablePosition < tableFormSelect.Count; tablePosition++)
                {
                    List<Dictionary<string, string>> auxResult = loadFilteredTable(tableFormSelect[tablePosition], command.Conditions, tableFormSelect[tablePosition].tableName, command.dbName);
                    if(tablePosition > 0)
                    {
                        TableJoin join = command.Joins[tablePosition - 1];
                        //auxResult = HashJoin(result, auxResult, join);
                        auxResult = mergeSortJoin(result, auxResult, join);
                    }
                    result = auxResult;
                }
            }
            //if there are no conditions, check if any fields have an index and use it, else scan
            //nevermind, if there are no conditions we should just take the primary table and use only the fields we need
            else
            {

                for (int tablePosition = 0; tablePosition < tableFormSelect.Count; tablePosition++)
                {
                    var table = tableFormSelect[tablePosition];
                    var records = repository.loadCollection(table.tableName, command.dbName);
                    List<Dictionary<string, string>> auxResult = new List<Dictionary<string, string>>();

                    foreach (var record in records)
                    {
                        auxResult.Add(createFilteredObject(table.AttributesList, command.dbName, record));
                    }
                    if (tablePosition > 0)
                    {
                        TableJoin join = command.Joins[tablePosition - 1];
                        auxResult = HashJoin(result, auxResult, join);
                        //auxResult = mergeSortJoin(result, auxResult, join);
                    }
                    result = auxResult;
                }
            }
            return result;
        }


        Dictionary<string, string> mergeDictionary(Dictionary<string, string> d1, Dictionary<string, string> d2)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach(var d in d1)
            {
                if (result.ContainsKey(d.Key) == false)
                    result.Add(d.Key, d.Value);
            }
            foreach(var d in d2)
            {
                if (result.ContainsKey(d.Key) == false)
                    result.Add(d.Key, d.Value);
            }

            return result;
        }

        List<Dictionary<string, string>> mergeSortJoin(List<Dictionary<string, string>> table1, List<Dictionary<string, string>> table2, TableJoin join)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            int index1 = 0, index2 = 0;
            int mark = -1;

            table1 = table1.OrderBy(dict => dict[join.joinAttribute]).ToList();
            table2 = table2.OrderBy(dict => dict[join.joinAttribute]).ToList();

            var joinAttr = join.joinAttribute;

            while ( index1 < table1.Count || index2 < table2.Count)
            {
                if(mark == -1)
                {
                    while (index1 < table1.Count && index2 < table2.Count && table1[index1][join.joinAttribute].CompareTo(table2[index2][join.joinAttribute]) < 0) index1++;
                    while (index1 < table1.Count && index2 < table2.Count && table1[index1][join.joinAttribute].CompareTo(table2[index2][join.joinAttribute]) > 0) index2++;

                    mark = index2;
                }
                if(index1 < table1.Count && index2 < table2.Count && table1[index1][join.joinAttribute].CompareTo(table2[index2][join.joinAttribute]) == 0)
                {
                    var fTable = table1[index1];
                    var sTable = table2[index2];
                    result.Add(mergeDictionary(fTable, sTable));
                    index2++;
                }
                else
                {
                    if (index1 > table1.Count) index2++;
                    else index2 = mark;
                    index1++;
                    mark = -1;
                }
            }

            return result;
        }

        List<Dictionary<string, string>> HashJoin(List<Dictionary<string, string>> table1, List<Dictionary<string, string>> table2, TableJoin join)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            //build phase
            //add all table1 to hash map
            Dictionary<string, List<Dictionary<string, string>>> hashmap = new Dictionary<string, List<Dictionary<string, string>>>();
            foreach(Dictionary<string, string> d in table1)
            {
                var hashCode = d[join.joinAttribute].GetHashCode().ToString();
                if (hashmap.ContainsKey(hashCode) == false)
                {
                    hashmap.Add(hashCode, new List<Dictionary<string, string>>());
                    hashmap[hashCode].Add(d);
                }
                else
                {
                    hashmap[hashCode].Add(d);
                }
            }

            //probe phase
            //iterate table2 and check for existing hashes

            foreach(Dictionary<string, string> d in table2)
            {
                var hashCode = d[join.joinAttribute].GetHashCode().ToString();
                if (hashmap.ContainsKey(hashCode) == false)
                {
                    continue;
                }
                else
                {
                    var relevantRecords = hashmap[hashCode];
                    foreach(var record in relevantRecords)
                    {
                        if (record[join.joinAttribute] == d[join.joinAttribute])
                            result.Add(mergeDictionary(record, d));
                    }
                }
            }

            return result;
        }

        List<Dictionary<string, string>> NestedJoin(List<Dictionary<string, string>> table1, Command table2, TableJoin join, List<Condition>? conditions)
        {
            var result = new List<Dictionary<string, string>>();

            foreach(Dictionary<string, string> d in table1)
            {
                var list = new List<string>();
                list.Add(join.joinAttribute);
                var searchedRecord = repository.loadRecordById(table2.tableName, table2.dbName, d[join.joinAttribute]);
                if(searchedRecord != null)
                {
                    if (conditions == null)
                    {
                        conditions = new List<Condition>();
                    }
                    var obj = createFilteredObject(table2.AttributesList, table2.dbName, searchedRecord, conditions);
                    if (obj != null) result.Add(mergeDictionary(d, obj));
                }
            }

            return result;
        }

        List<Dictionary<string, string>> IndexNestedLoop(Command command)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            List<Command> tableFormSelect = new List<Command>();
            foreach (AtributTabel atr in command.AttributesList)
            {
                var tabel = tableFormSelect.Find(a => a.tableName == atr.ParentTable);
                if (tabel == null)
                {
                    Command comm = new Command();
                    comm.AttributesList = new List<AtributTabel>();
                    comm.AttributesList.Add(atr);
                    comm.tableName = atr.ParentTable;
                    comm.dbName = command.dbName;
                    tableFormSelect.Add(comm);
                }
                else
                {
                    tabel.AttributesList.Add(atr);
                }
            }
            List<Dictionary<string, string>> auxResult;
            if (command.Conditions.Count > 0)
            {
                //get the indexes for each table, and then iterate through the conditions to find a useful index
                for (int tablePosition = 0; tablePosition < tableFormSelect.Count; tablePosition++)
                {
                    if (tablePosition > 0)
                    {
                        TableJoin join = command.Joins[tablePosition - 1];
                        auxResult = NestedJoin(result, tableFormSelect[tablePosition], join, command.Conditions);
                    }
                    else
                    {
                        auxResult = loadFilteredTable(tableFormSelect[tablePosition], command.Conditions, tableFormSelect[tablePosition].tableName, command.dbName);
                    }
                    result = auxResult;
                }
            }
            //if there are no conditions, check if any fields have an index and use it, else scan
            //nevermind, if there are no conditions we should just take the primary table and use only the fields we need
            else
            {

                for (int tablePosition = 0; tablePosition < tableFormSelect.Count; tablePosition++)
                {
                    var table = tableFormSelect[tablePosition];
                    auxResult = new List<Dictionary<string, string>>();

                    if(tablePosition > 0)
                    {
                        TableJoin join = command.Joins[tablePosition - 1];
                        auxResult = NestedJoin(result, table, join, null);
                    }
                    else
                    {
                        var records = repository.loadCollection(table.tableName, command.dbName);
                        foreach (var record in records)
                        {
                            auxResult.Add(createFilteredObject(table.AttributesList, command.dbName, record));
                        }
                    }
                    result = auxResult;
                }
            }


            return result;
        }


        public string ExecuteCommand(Command command)
        {
            try
            {
                string[] words = command.SqlQuery.Split(' ');
                switch (words[0])
                {
                    case "SELECT":
                        if (words[1] == "*")
                        {
                            List<Dictionary<string,string>> selection = loadTable(command).Distinct().ToList();
                            return cleanMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(selection)));
                        }
                        else
                        {
                            List<Dictionary<string, string>> selection = IndexNestedLoop(command).Distinct().ToList();
                            //List<Dictionary<string, string>> selection = loadFilteredSelection(command).Distinct().ToList();
                            return cleanMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(selection)));
                        }
                        break;
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
                            var myList = command.AttributesList;
                            //work here
                            foreach (AtributTabel atribut in myList)
                            {
                                if (atribut.FKeys != null || atribut.IsUnique == true)
                                {
                                    createIndex(atribut.Name, command);
                                }
                            }
                        }
                        if (words[1] == "INDEX")
                        {
                            Console.WriteLine("Creating  index on table" + command.tableName);
                            string indexName = words[2];
                            for (int i = 3; i < words.Length; i++)
                                indexName += " " + words[i];
                            createIndex(indexName, command);
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
                            List<Command> tables = getTables(command);
                            return cleanMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(tables)));
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

        internal void testSelectIndex()
        {
            var coll = repository.loadCollection("Index_testTable_c", "test", new Condition("c", "GREATER THAN", "80","testTable"));
        }

        internal void testSelectScan()
        {
            List<Condition> conditions = new List<Condition>();
            Command com = new Command();
            com.AttributesList = new List<AtributTabel>();
            AtributTabel atributTabel1 = new AtributTabel();
            AtributTabel atributTabel2 = new AtributTabel();
            AtributTabel atributTabel3 = new AtributTabel();
            atributTabel1.ParentTable = "testTable";
            atributTabel1.Name = "a";
            atributTabel1.IsPrimaryKey = true;
            com.AttributesList.Add(atributTabel1);
            atributTabel2.ParentTable = "testTable";
            atributTabel2.Name = "b";
            atributTabel2.IsPrimaryKey = false;
            com.AttributesList.Add(atributTabel2);
            atributTabel3.ParentTable = "testTable";
            atributTabel3.Name = "c";
            atributTabel3.IsPrimaryKey = false;
            com.AttributesList.Add(atributTabel3);


            com.dbName = "test";
            com.tableName = "testTable";
            conditions.Add(new Condition("c", "GREATER THAN", "80", "testTable"));
            var coll = loadTable(com, "test", conditions);
        }
    }
    
}
