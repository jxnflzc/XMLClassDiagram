using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Jxnflzc.XMLClassDiagram
{
    public class XMLUtil
    {
        public static XNamespace a = @"attribute";
        public static XNamespace c = @"collection";
        public static XNamespace o = @"object";

        public static List<ClassDiagram> Analyze(String filePath)
        {
            List<ClassDiagram> cdList = new List<ClassDiagram>();

            XElement xe = XElement.Load(filePath);

            var oModel = from ele in xe.Elements(o + "RootObject").Elements(c + "Children").Elements(o + "Model") select ele;

            IEnumerable<XElement> oClass = from ele in oModel.Elements(c + "Classes").Elements(o + "Class") select ele;
            cdList = XMLUtil.GetClass(oClass, cdList);

            IEnumerable<XElement> oGeneralization = from ele in oModel.Elements(c + "Generalizations").Elements(o + "Generalization") select ele;
            cdList = XMLUtil.GetDit(oGeneralization, cdList);

            IEnumerable<XElement> oAssociation = from ele in oModel.Elements(c + "Associations").Elements(o + "Association") select ele;
            cdList = XMLUtil.GetCbo(oAssociation, cdList);

            IEnumerable<XElement> oDependencies = from ele in oModel.Elements(c + "Dependencies").Elements(o + "Dependency") select ele;
            cdList = XMLUtil.GetCbo(oDependencies, cdList);

            cdList = XMLUtil.GetNoc(cdList);
            cdList = XMLUtil.GetWmc(cdList);

            return cdList;
        }

        public static void SaveResult(List<ClassDiagram> cdList, String savePath)
        {
            DateTime currentTime = DateTime.Now;
            String saveName;
            saveName = currentTime.Year.ToString() + "-" + currentTime.Month.ToString() + "-" + currentTime.Day.ToString() + "-" + currentTime.Hour.ToString() + "-" +
                currentTime.Minute.ToString() + "-" + currentTime.Second.ToString() + "-" + currentTime.Millisecond.ToString() + ".json";
            String jsonData = JsonConvert.SerializeObject(cdList, Formatting.Indented);
            if (!savePath.EndsWith(@"\"))
            {
                savePath += @"\";
            }
            File.WriteAllText(savePath + saveName, jsonData);
        }

        public static ClassDiagram GetClassDiagramById(List<ClassDiagram> classDiagramList, String id)
        {
            foreach (ClassDiagram cd in classDiagramList)
            {
                if (cd.Id == id)
                {
                    return cd;
                }
            }
            return null;
        }

        public static ClassDiagram GetClassDiagramByName(List<ClassDiagram> cdList, String name)
        {
            foreach (ClassDiagram cd in cdList)
            {
                if (cd.Name == name)
                {
                    return cd;
                }
            }
            return null;
        }

        public static Operation GetOperationByName(List<Operation> opList, String name)
        {
            foreach (Operation op in opList)
            {
                if (op.Name == name)
                {
                    return op;
                }
            }
            return null;
        }

        public static List<ClassDiagram> GetClass(IEnumerable<XElement> classes, List<ClassDiagram> classDiagramList)
        {
            if (classDiagramList == null)
            {
                classDiagramList = new List<ClassDiagram>();
            }

            foreach (var ele in classes)
            {
                ClassDiagram cd = new ClassDiagram();
                cd.Name = ele.Element(a + "Name").Value;
                cd.Id = ele.Attribute("Id").Value;
                cd = Attribute(ele.Elements(c + "Attributes").Elements(o + "Attribute"), cd);
                cd = Operation(ele.Elements(c + "Operations").Elements(o + "Operation"), cd);
                classDiagramList.Add(cd);
            }

            return classDiagramList;
        }

        public static ClassDiagram Attribute(IEnumerable<XElement> attributes, ClassDiagram cd)
        {
            if (cd == null)
            {
                cd = new ClassDiagram();
            }

            foreach (var attribute in attributes)
            {
                cd.AddAttribute(attribute.Element(a + "Name").Value);
            }

            return cd;
        }

        public static ClassDiagram Operation(IEnumerable<XElement> operations, ClassDiagram cd)
        {
            if (cd == null)
            {
                cd = new ClassDiagram();
            }

            foreach (var operation in operations)
            {
                Operation op = new Operation();
                op.Name = operation.Element(a + "Name").Value;
                op = Parameter(operation.Elements(c + "Parameters").Elements(o + "Parameter"), op);

                cd.AddOperation(op);
            }

            return cd;
        }

        public static Operation Parameter(IEnumerable<XElement> parameters, Operation operation)
        {
            if (operation == null)
            {
                operation = new Operation();
            }

            foreach (var parameter in parameters)
            {
                Parameter p = new Parameter();
                p.Name = parameter.Element(a + "Name").Value;
                p.Type = parameter.Element(a + "Parameter.DataType").Value;
                operation.AddParameters(p);
            }

            return operation;
        }

        public static List<ClassDiagram> GetDit(IEnumerable<XElement> generalizations, List<ClassDiagram> classDiagramList)
        {
            if (classDiagramList == null)
            {
                classDiagramList = new List<ClassDiagram>();
            }

            foreach (var generalization in generalizations)
            {
                String parentId = generalization.Element(c + "Object1").Element(o + "Class").Attribute("Ref").Value;
                String childId = generalization.Element(c + "Object2").Element(o + "Class").Attribute("Ref").Value;

                classDiagramList.ForEach(cd =>
                {
                    if (cd.Id == childId)
                    {
                        cd.ParentId = parentId;
                        ClassDiagram parent = GetClassDiagramById(classDiagramList, parentId);
                        cd.ParentName = parent.Name;
                        cd.Dit = parent.Dit + 1;
                    }
                });
            }

            return classDiagramList;
        }

        public static List<ClassDiagram> GetNoc(List<ClassDiagram> classDiagramList)
        {
            if (classDiagramList == null)
            {
                classDiagramList = new List<ClassDiagram>();
            }

            foreach (ClassDiagram cd in classDiagramList)
            {
                foreach (ClassDiagram cdNew in classDiagramList)
                {
                    if (cd.Id == cdNew.ParentId)
                        cd.Noc = cd.Noc + 1;
                }
            }

            return classDiagramList;
        }

        public static List<ClassDiagram> GetWmc(List<ClassDiagram> classDiagramList)
        {
            if (classDiagramList == null)
            {
                classDiagramList = new List<ClassDiagram>();
            }

            foreach (ClassDiagram cd in classDiagramList)
            {
                cd.Wmc = cd.Operations.ToArray().Length;
            }

            return classDiagramList;
        }

        public static List<ClassDiagram> GetCbo(IEnumerable<XElement> associations, List<ClassDiagram> classDiagramList)
        {
            foreach (var association in associations)
            {
                String class1Id = association.Element(c + "Object1").Element(o + "Class").Attribute("Ref").Value;
                String class2Id = association.Element(c + "Object2").Element(o + "Class").Attribute("Ref").Value;

                classDiagramList.ForEach(cd =>
                {
                    if (cd.Id == class1Id || cd.Id == class2Id)
                    {
                        cd.Cbo = cd.Cbo + 1;
                    }
                });
            }
            return classDiagramList;
        }
    }
}
