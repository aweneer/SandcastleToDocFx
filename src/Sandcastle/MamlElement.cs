using System.Xml.Linq;
using SandcastleToDocFx.Visitors;
using SandcastleToDocFx.Writers;

namespace SandcastleToDocFx.Sandcastle
{
    public abstract class MamlElement
    {
        public XElement Element { get; set; }
        public bool RequiresIndentation { get; set; }
        public bool ShouldLineBreak { get; set; }

        public MamlElement(XElement element, bool requiresIndentation, bool shouldLineBreak)
        {
            Element = element;
            RequiresIndentation = requiresIndentation;
            ShouldLineBreak = shouldLineBreak;
        }

        public abstract void Accept(Visitor visitor);
    }

    // Concrete elements implementations.

    public class AlertElement : MamlElement
    {

        public AlertElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation, shouldLineBreak)
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
        public CaptionElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public TopicElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public IntroductionElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public bool IsSubsection { get; set; }

        public SectionElement(
            XElement element,
            bool isSubsection = false,
            bool requiresIndentation = false,
            bool shouldLineBreak = false)
            : base(element, requiresIndentation,  shouldLineBreak)
        {
            this.IsSubsection = isSubsection;
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
        public SectionsElement(
            XElement element,
            bool requiresIndentation = false,
            bool shouldLineBreak = false)
            : base(element, requiresIndentation,  shouldLineBreak)
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
        public bool IsReferenceAppended { get; set; }

        public bool IsOtherResourcesAppended { get; set; }

        public RelatedTopicsElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public ContentElement(XElement element, bool requiresIndentation = false ,bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public TableElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public RowElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public TableHeaderElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public EntryElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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

        public ParaElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public bool IsTableElement { get; set; }

        public RichParaElement(XElement element, bool isTableElement = false, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
        {
            this.IsTableElement = isTableElement;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            // TODO: Fix this logic, for steps it is broken
            if (this.ShouldLineBreak && !IsTableElement)
            {
                MarkdownWriter.AppendLine();
            }
        }
    }

    public class LinkElement : MamlElement
    {
        public bool IsOnlyLink { get; set; }

        public LinkElement(XElement element, bool isOnlyLink, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
        {
            this.IsOnlyLink = isOnlyLink;
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
        public ListElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public bool IsOrderedListItem { get; set; }

        public ListItemElement(XElement element, bool isOrderedListItem, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
        {
            this.IsOrderedListItem = isOrderedListItem;
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
        public CodeInlineElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public LegacyBoldElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public LegacyItalicElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public ExternalLinkElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public CodeElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public ProcedureElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public MediaLinkElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public ImageElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public Heading Heading { get; set; }

        public TitleElement(XElement element, Heading heading, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
        {
            this.Heading = heading;
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
        public TokenElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public CommandElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public CodeEntityReferenceElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation, shouldLineBreak)
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
        public StepsElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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
        public StepElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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

    public class ConclusionElement : MamlElement
    {
        public ConclusionElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, requiresIndentation,  shouldLineBreak)
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