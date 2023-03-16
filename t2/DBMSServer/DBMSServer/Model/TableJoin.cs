using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSServer.Model
{
    public class TableJoin
    {
        public string initialTable { get; set; }
        public string joinTable { get; set; }
        public string joinAttribute { get; set; }

    }
}
