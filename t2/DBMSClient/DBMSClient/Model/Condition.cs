using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBMSClient.Model
{
    public class Condition
    {

        public string attributeName;
        public string comparation;
        public string comparationValue;
        public string? ParentTable;


        public override string ToString()
        {
            return String.Format("{0} {1} {2}", attributeName, comparation, comparationValue);
        }
    }
}
