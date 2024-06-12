using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using DBMSServer.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DBMSServer.repo
{
    class Repo
    {
        public static IMongoClient client = new MongoClient();
        private IMongoDatabase database;
        public Repo()
        {

        }

        internal void CreateDatabase(string dbName)
        {
            database = client.GetDatabase(dbName);
        }

        internal void CreateCollection(string collectionName, string dbName)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(collectionName);
            string key = "aux";
            string value = "aux";
            collection.InsertOne(new Record(key, value));
            var filter = Builders<Record>.Filter.Eq("_id", key);
            collection.DeleteOne(filter);
        }

        internal void CreateIndex(string tableName, string dbName, int index)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            var originalTable = tableName.Split("_")[1];
            var records = this.loadCollection(originalTable, dbName);
            foreach(Record record in records)
            {
                string key = record.Value.Split("#")[index];
                string value = record.Key;
                if (checkExistence(tableName, dbName, key))
                {
                    var rec = loadRecordById(tableName, dbName, key);
                    var filter = Builders<Record>.Filter.Eq("_id", key);
                    var update = Builders<Record>.Update.Set(s => s.Value, String.Format("{0}#{1}", rec.Value, value));
                    collection.UpdateOne(filter, update);
                }
                else
                {
                    collection.InsertOne(new Record(key, value));
                }
            }
        }

        internal void InsertInIndex(string tableName, string dbName, string key, string value)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            if (checkExistence(tableName, dbName, key))
            {
                var rec = loadRecordById(tableName, dbName, key);
                var filter = Builders<Record>.Filter.Eq("_id", key);
                var update = Builders<Record>.Update.Set(s => s.Value, String.Format("{0}#{1}", rec.Value, value));
                collection.UpdateOne(filter, update);
            }
            else
            {
                collection.InsertOne(new Record(key, value));
            }
        }
        internal void testInsert()
        {
            Random random = new Random();
            database = client.GetDatabase("test");
            var collection = database.GetCollection<Record>("testTable");
            var collectionIndex = database.GetCollection<Record>("Index_testTable_c");
            string a, b, c;
            int mult = 10;
            for (int i = 1; i < 1000001; i++)
            {
                a = i.ToString();
                b = random.Next(1, 20).ToString();
                c = mult.ToString();
                if (i % 1000 == 0) mult = mult * 2;
                collection.InsertOne(new Record(a, b+"#"+c));

                if (checkExistence("Index_testTable_c", "test", c))
                {
                    var rec = loadRecordById("Index_testTable_c", "test", c);
                    var filter = Builders<Record>.Filter.Eq("_id", c);
                    var update = Builders<Record>.Update.Set(s => s.Value, String.Format("{0}#{1}", rec.Value, a));
                    collectionIndex.UpdateOne(filter, update);
                }
                else
                {
                    collectionIndex.InsertOne(new Record(c, a));
                }
            }
        }


        internal void Insert(string table, string dbName, string key, string value)
        {
            Console.WriteLine(table + " " + dbName);
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(table);
            if(checkExistence(table, dbName, key))
            {
                throw new Exception("There is already a record with that id");
            }
            else
            {
                collection.InsertOne(new Record(key, value));
            }
        }

        internal bool checkExistence(string table, string dbName, string key)
        {
            try
            {
                loadRecordById(table, dbName, key);
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        internal List<Record> loadCollection(string table, string dbName)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        internal async Task<List<Record>> getLessFilterAsync(IMongoCollection<Record> collection, string compValue)
        {
            var res = await collection.Aggregate()
                .AppendStage<Record>("{ $set : { _KeyInt : { $toInt : '$_id' } } }")
                .Match(Builders<Record>.Filter.Lt("_KeyInt", Convert.ToInt32(compValue)))
                .AppendStage<object>("{ $unset : '_KeyInt' }")
                .As<Record>()
                .ToListAsync();
            return res;
        }
        internal async Task<List<Record>> getGreaterFilterAsync(IMongoCollection<Record> collection, string compValue)
        {
            var res = await collection.Aggregate()
                .AppendStage<Record>("{ $set : { _KeyInt : { $toInt : '$_id' } } }")
                .Match(Builders<Record>.Filter.Gt("_KeyInt", Convert.ToInt32(compValue)))
                .AppendStage<object>("{ $unset : '_KeyInt' }")
                .As<Record>()
                .ToListAsync();
            return res;
        }

        internal List<Record> loadCollectionCompound(string table, string dbName, Condition condition1, Condition condition2)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(table);
            FilterDefinition<Record> filter;
            switch (condition1.comparation)
            {
                case "GREATER THAN":
                    var rez = getGreaterFilterAsync(collection, condition1.comparationValue);
                    return rez.Result;
                    break;
                case "LESSER THAN":
                    string compare = String.Format(condition1.comparationValue + "#" );
                    var filter1 = Builders<Record>.Filter.Lt(x => x.Key, condition1.comparationValue);
                    var filter2 = Builders<Record>.Filter.Eq(x => x.Key, condition1.comparationValue);
                    return collection.Find(filter1).ToList();
                    break;
                case "EQUAL":
                    if(condition2.comparation == "LESSER THAN")
                        filter = Builders<Record>.Filter.Lt(x => x.Key, String.Format("{0}#{1}",condition1.comparationValue, condition2.comparationValue));
                    else
                    {
                        filter = Builders<Record>.Filter.Lt(x => x.Key, String.Format("{0}#{1}", condition1.comparationValue, condition2.comparationValue));
                    }
                    break;
                default:
                    filter = Builders<Record>.Filter.Eq(x => x.Key, condition1.comparationValue);
                    break;
            }
            return collection.Find(filter).ToList();
        }

        internal List<Record> loadCollection(string table, string dbName, Condition condition)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(table);
            FilterDefinition<Record> filter;
            switch (condition.comparation)
            {
                case "GREATER THAN":
                    var rez = getGreaterFilterAsync(collection, condition.comparationValue);
                    return rez.Result;
                    break;
                case "LESSER THAN":
                    rez = getLessFilterAsync(collection, condition.comparationValue);
                    return rez.Result;
                    break;
                case "EQUAL":
                    filter = Builders<Record>.Filter.Eq(x => x.Key, condition.comparationValue);
                    break;
               default:
                    filter = Builders<Record>.Filter.Eq(x => x.Key, condition.comparationValue);
                    break;
            }
            return collection.Find(filter).ToList();
        }

        internal Record loadRecordById(string table, string dbName, string id)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(table);
            var filter = Builders<Record>.Filter.Eq("_id", id);

            return collection.Find(filter).First();
        }

        internal void DropDatabase(string dbName)
        {
            client.DropDatabase(dbName);
        }

        internal void DropCollection(string tableName, string dbName)
        {
            database = client.GetDatabase(dbName);
            database.DropCollection(tableName);
        }

        internal void DeleteRecord(string tableName, string dbName, string id)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            if(checkExistence(tableName, dbName, id))
            {
                var filter = Builders<Record>.Filter.Eq("_id", id);

                collection.DeleteOne(filter);
            }
            else
            {
                throw new Exception("There is no record with that id");
            }
        }

        internal void DeleteRecordWhere(string tableName, string dbName, string whereAttribute, string whereClause,int position)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            ///this is done without index for now, so we do it manually
            var records = loadCollection(tableName, dbName);
            Console.WriteLine("Delete where " + whereAttribute + " == " + whereClause);
            int nrDelete = 0;
            foreach(Record record in records)
            {
                var values = record.Value.Split("#");
                if (values[position] == whereClause)
                {
                    var filter = Builders<Record>.Filter.Lt("_id", record.Key);
                    collection.DeleteOne(filter);
                    nrDelete++;
                }
            }
            if(nrDelete == 0)
            {
                throw new Exception("No records matching were found.");
            }
        }

        internal void UpdateRecord(string tableName, string dbName, string key, string value)
        {
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            try
            {
                Record record = loadRecordById(tableName, dbName, key);
                var filter = Builders<Record>.Filter.Eq("_id", key);
                var update = Builders<Record>.Update.Set(s => s.Value, value);
                collection.UpdateOne(filter, update);
            }
            catch(Exception e)
            {
                throw new Exception("There was no record found with that id");
            }
        }

        internal void DeleteRecordIndex(string tableName, string dbName, string keyToDelete, string indexKey)
        {
            //id este primary key-ul care trebuie sters
            //noi trebuie pentru fiecare index sa stergem
            //a) tot randul care il contine pe id ca si valoare in cazul celor unique
            //b) sa eliminam cheia id din indexul non-unique
            database = client.GetDatabase(dbName);
            var collection = database.GetCollection<Record>(tableName);
            if (checkExistence(tableName, dbName, indexKey))
            {
                //am obtinut recordul
                Record record = loadRecordById(tableName, dbName, indexKey);
                //splituim valoarea dupa #, daca count > 1 atunci indexul este non-unique, altfel este unique
                List<string> splits = record.Value.Split("#").ToList();
                if(splits.Count > 1)
                {
                    //index non-unique
                    string value = "";
                    foreach(string pk in splits)
                    {
                        if(pk != keyToDelete)
                        {
                            if (value == "")
                                value = pk;
                            else
                                value += "#" + pk;
                        }
                    }
                    var filter = Builders<Record>.Filter.Eq("_id", indexKey);
                    var update = Builders<Record>.Update.Set(s => s.Value, value);
                    collection.UpdateOne(filter, update);
                }
                else
                {
                    //index unique;
                    //a) tot randul care il contine pe id ca si valoare in cazul celor unique
                    var filter = Builders<Record>.Filter.Eq("_id", indexKey);
                    collection.DeleteOne(filter);
                }
                
            }
            else
            {
                throw new Exception("There is no record with that id");
            }
        }
    }
}
