using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SandcastleToDocFx.Visitors;

namespace SandcastleToDocFx.Sandcastle
{
    public abstract class MamlElement
    {
        public XElement Element { get; set; }

        public MamlElement(XElement element)
        {
            Element = element;
        }

        public abstract void Accept(Visitor visitor);
    }
}
