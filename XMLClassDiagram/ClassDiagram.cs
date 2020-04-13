using System;
using System.Collections.Generic;

namespace Jxnflzc.XMLClassDiagram
{
    [Serializable]
    public class ClassDiagram
    {
        public String Id
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String ParentId
        {
            get;
            set;
        }

        public String ParentName
        {
            get;
            set;
        }

        public int Dit
        {
            get;
            set;
        }

        public int Cbo
        {
            get;
            set;
        }

        public int Noc
        {
            get;
            set;
        }

        public int Wmc
        {
            get;
            set;
        }

        public List<String> Attributes
        {
            get;
            set;
        }

        public List<Operation> Operations
        {
            get;
            set;
        }

        public ClassDiagram()
        {
            this.Dit = 0;
            this.Cbo = 0;
            this.Noc = 0;
            this.Attributes = new List<String>();
            this.Operations = new List<Operation>();
        }

        public void AddAttribute(String attribute)
        {
            this.Attributes.Add(attribute);
        }

        public void SetAttributes(String attributeOld, String attributeNew)
        {
            this.Attributes.ForEach(i =>
            {
                if (i == attributeOld)
                {
                    i = attributeNew;
                }
            }
            );
        }

        public void AddOperation(Operation operation)
        {
            this.Operations.Add(operation);
        }

        public void SetOperations(Operation operationOld, Operation operationNew)
        {
            this.Operations.ForEach(i =>
            {
                if (i.Name == operationOld.Name)
                {
                    i = operationNew;
                }
            }
            );
        }

        public override String ToString()
        {
            String result = "======\t" + this.Name + "\t======";
            result += "\n\tId: " + this.Id;
            result += "\n\tDIT: " + this.Dit;
            result += "\n\tCBO: " + this.Cbo;
            result += "\n\tNOC: " + this.Noc;
            result += "\n\tParentId: " + this.ParentId;
            result += "\n\tParentName: " + this.ParentName;

            for (int i = 0; i < this.Attributes.ToArray().Length; i++)
            {
                result += "\n\tAttribute: " + this.Attributes[i];
            }

            for (int i = 0; i < this.Operations.ToArray().Length; i++)
            {
                result += "\n\tOperation: " + this.Operations[i].ToString();
            }
            return result;
        }
    }
}
