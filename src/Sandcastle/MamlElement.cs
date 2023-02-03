﻿using System.Xml.Linq;
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
        public bool RequiresIndentation { get; set; }

        public AlertElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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
        public bool IsSubsection { get; set; }

        public SectionElement(XElement element, bool isSubsection = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
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
        public bool IsReferenceAppended { get; set; }

        public bool IsOtherResourcesAppended { get; set; }

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
        public bool RequiresIndentation { get; set; }
        public ContentElement(XElement element, bool requiresIndentation = false ,bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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
        public bool RequiresIndentation { get; set; }

        public ParaElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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
        public bool RequiresIndentation { get; set; }

        public RichParaElement(XElement element, bool isTableElement = false, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.IsTableElement = isTableElement;
            this.RequiresIndentation = requiresIndentation;
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

        public LinkElement(XElement element, bool isOnlyLink, bool shouldLineBreak = false) : base(element, shouldLineBreak)
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
        public bool RequiresIndentation { get; set; }

        public CodeElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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
        public bool RequiresIndentation { get; set; }

        public MediaLinkElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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
        public bool RequiresIndentation { get; set; }

        public ImageElement(XElement element, bool requiresIndentation = false, bool shouldLineBreak = false) : base(element, shouldLineBreak)
        {
            this.RequiresIndentation = requiresIndentation;
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

        public TitleElement(XElement element, Heading heading, bool shouldLineBreak = false) : base(element, shouldLineBreak)
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

    public class ConclusionElement : MamlElement
    {
        public ConclusionElement(XElement element, bool shouldLineBreak = false) : base(element, shouldLineBreak)
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