using System;
using System.Collections.Generic;
using System.Text;

namespace Jxnflzc.XMLClassDiagram
{
    [Serializable]
    public class Operation
    {
        public String Name
        {
            get;
            set;
        }

        public List<Parameter> Parameters
        {
            get;
            set;
        }

        public Operation()
        {
            this.Parameters = new List<Parameter>();
        }

        public void AddParameters(Parameter parameter)
        {
            this.Parameters.Add(parameter);
        }

        public void SetParameters(Parameter parameterOld, Parameter parameterNew)
        {
            this.Parameters.ForEach(i =>
            {
                if (i.Name == parameterOld.Name)
                {
                    i = parameterNew;
                }
            }
            );
        }

        public override String ToString()
        {
            String result = this.Name + "\tParameters: ";
            for (int i = 0; i < Parameters.ToArray().Length; i++)
            {
                result += this.Parameters[i] + "\t";
            }
            return result;
        }
    }
}
