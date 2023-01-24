using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SandcastleToDocFx.Sandcastle;

namespace SandcastleToDocFx.Visitors
{
    public abstract class Visitor
    {
        public abstract void Visit(MamlElement element);
        public abstract void Visit(ContentElement element);
        public abstract void Visit(IntroductionElement element);
        public abstract void Visit(TopicElement element);
        public abstract void Visit(SectionElement element);
        public abstract void Visit(RelatedTopicsElement element);
        public abstract void Visit(TableElement element);
        public abstract void Visit(TableHeaderElement element);
        public abstract void Visit(RowElement element);
    }
}
