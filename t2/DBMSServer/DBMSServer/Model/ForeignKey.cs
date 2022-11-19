using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSServer.Model
{
    class ForeignKey
    {
        public ForeignKey(string attribute, string table, string referencedTable, string referencedAttribute)
        {
            this.attribute = attribute;
            this.referencedTable = referencedTable;
            this.referencedAttribute = referencedAttribute;
            this.table = table;
        }

        public string attribute { get; set; }
        public string table { get; set; }
        public string referencedTable { get; set; }
        public string referencedAttribute { get; set; }
    }
}
