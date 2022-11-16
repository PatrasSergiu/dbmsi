using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        bool checkExistence(string table, string dbName, string key)
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
                    var filter = Builders<Record>.Filter.Eq("_id", record.Key);
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

       
    }

    public class Record
    {
        public Record(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [BsonId]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
