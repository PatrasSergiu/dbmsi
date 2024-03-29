﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSClient.Model
{
    public class Command
    {

        public string SqlQuery { get; set; } // CREATE/DROP/INSERT/DELETE   
        public List<AtributTabel>? AttributesList { get; set; }
        public string? dbName { get; set; }
        public string? tableName { get; set; }
        public Dictionary<string, string>? Values { get; set; }
        public List<Condition>? Conditions { get; set; }
        public List<TableJoin>? joins { get; set; }

    }
}
