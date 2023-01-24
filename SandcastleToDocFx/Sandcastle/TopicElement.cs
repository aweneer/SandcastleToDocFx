using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SandcastleToDocFx.Visitors;

namespace SandcastleToDocFx.Sandcastle
{
    public class TopicElement : MamlElement
    {
        public TopicElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class IntroductionElement : MamlElement
    {
        public IntroductionElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
            // TODO Check for <para> and build the text
        }
    }
    public class SectionElement : MamlElement
    {
        public SectionElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public class RelatedTopicsElement : MamlElement
    {
        public RelatedTopicsElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
            // TODO Check for <para> and build the text
        }
    }

    public class ContentElement : MamlElement
    {
        public ContentElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TableElement : MamlElement
    {
        public TableElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RowElement : MamlElement
    {
        public RowElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TableHeaderElement : MamlElement
    {
        public TableHeaderElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
