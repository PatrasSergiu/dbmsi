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
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public bool IsPrimaryKey { get; set; }

    }
}
