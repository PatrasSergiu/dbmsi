using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBMSServer.Model
{
    public class Condition
    {

        public string attributeName;
        public string comparation;
        public string comparationValue;
        public string? ParentTable;

        public Condition()
        {

        }

        public Condition(string attributeName, string comparation, string comparationValue, string? parentTable)
        {
            this.attributeName = attributeName;
            this.comparation = comparation;
            this.comparationValue = comparationValue;
            ParentTable = parentTable;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", attributeName, comparation, comparationValue);
        }
    }
}
