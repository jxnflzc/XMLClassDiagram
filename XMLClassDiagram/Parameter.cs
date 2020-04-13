using System;
using System.Collections.Generic;
using System.Text;

namespace Jxnflzc.XMLClassDiagram
{
    public class Parameter
    {
        public String Type
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Type + "   " + Name;
        }
    }
}
