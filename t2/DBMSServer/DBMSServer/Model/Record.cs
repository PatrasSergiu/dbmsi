using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBMSServer.Model
{
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
        public override string ToString()
        {
            return String.Format("Key: {0} Value: {1}", Key, Value);
        }
    }
}
