using System.Xml.Linq;
using SandcastleToDocFx.Visitors;
using SandcastleToDocFx.Writers;

namespace SandcastleToDocFx.Sandcastle
{
    public abstract class MamlElement
    {
        public XElement Element { get; set; }
        public bool ShouldLineBreak { get; set; }

        public MamlElement(XElement element, bool shouldLineBreak)
        {
            Element = element;
            ShouldLineBreak = shouldLineBreak;
        }

        public abstract void Accept(Visitor visitor);
    }

    // Concrete elements implementations.

    public class AlertElement : MamlElement
    {
        public AlertElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class CaptionElement : MamlElement
    {
        public CaptionElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class TopicElement : MamlElement
    {
        public TopicElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }


    public class IntroductionElement : MamlElement
    {
        public IntroductionElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class SectionElement : MamlElement
    {
        public SectionElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class SectionsElement : MamlElement
    {
        public SectionsElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class RelatedTopicsElement : MamlElement
    {
        public RelatedTopicsElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ContentElement : MamlElement
    {
        public ContentElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class TableElement : MamlElement
    {
        public TableElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class RowElement : MamlElement
    {
        public RowElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class TableHeaderElement : MamlElement
    {
        public TableHeaderElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class EntryElement : MamlElement
    {
        public EntryElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ParaElement : MamlElement
    {
        public ParaElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class RichParaElement : MamlElement
    {
        public RichParaElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class LinkElement : MamlElement
    {
        public LinkElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ListElement : MamlElement
    {
        public ListElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ListItemElement : MamlElement
    {
        public ListItemElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }


    public class CodeInlineElement : MamlElement
    {
        public CodeInlineElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class LegacyBoldElement : MamlElement
    {
        public LegacyBoldElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class LegacyItalicElement : MamlElement
    {
        public LegacyItalicElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ExternalLinkElement : MamlElement
    {
        public ExternalLinkElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class CodeElement : MamlElement
    {
        public CodeElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ProcedureElement : MamlElement
    {
        public ProcedureElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class MediaLinkElement : MamlElement
    {
        public MediaLinkElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class ImageElement : MamlElement
    {
        public ImageElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class TitleElement : MamlElement
    {
        public TitleElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class TokenElement : MamlElement
    {
        public TokenElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class CommandElement : MamlElement
    {
        public CommandElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class CodeEntityReferenceElement : MamlElement
    {
        public CodeEntityReferenceElement(XElement element, bool shouldLineBreak = false) : base(element,
            shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class StepsElement : MamlElement
    {
        public StepsElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class StepElement : MamlElement
    {
        public StepElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            if (this.ShouldLineBreak)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }
}