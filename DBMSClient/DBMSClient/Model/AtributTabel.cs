using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSClient.Model
{
    public class AtributTabel
    {
        public string Type { get; set; }
        public string? ParentTable { get; set; }
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimaryKey { get; set; }
        //table that it references, name of the attribute that it references
        public Dictionary<string, string> FKeys { get; set; }
    }
}
