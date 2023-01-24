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
    
    // Concrete elements.

    public class AlertElement : MamlElement
    {
        public AlertElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
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

    public class EntryElement : MamlElement
    {
        public EntryElement(XElement element) : base(element) { }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ParaElement : MamlElement
    {
        public ParaElement(XElement element) : base(element) { }
        
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RichParaElement : MamlElement
    {
        public RichParaElement(XElement element) : base(element) { }
        
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class LinkElement : MamlElement
    {
        public LinkElement(XElement element) : base(element) { }
        
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class ListElement : MamlElement
    {
        public ListElement(XElement element) : base(element) { }
        
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
