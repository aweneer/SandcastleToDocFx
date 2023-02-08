using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SandcastleToDocFx;
using SandcastleToDocFx.Writers;
using SandcastleToDocFx.Sandcastle;

namespace SandcastleToDocFx.Visitors
{
    public class MamlVisitor : Visitor
    {
        public override void Visit(MamlElement mamlElement)
        {
            Console.WriteLine("MamlElement");
        }

        public override void Visit(AlertElement alert)
        {
            var alertType = alert.Element.Attribute("class")?.Value;
            MarkdownWriter.AppendAlert(alertType, alert.RequiresIndentation);

            foreach (var element in alert.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElement;
                switch (type)
                {
                    case ElementType.Para:
                        mamlElement = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Code:
                        mamlElement = new CodeElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Alert>.");
                }

                mamlElement.Accept(this);
            }

            MarkdownWriter.AppendLine();
        }

        public override void Visit(TopicElement topic)
        {
            Console.WriteLine("TopicElement");
        }

        public override void Visit(CodeElement code)
        {
            var language = Utilities.ReplaceLanguage(code.Element.Attribute("language")?.Value!);
            var source = code.Element.Attribute("source")?.Value;
            
            if (source != null)
            {
                var sourceCodeFile = Path.Combine(Program.SourceCodeDirectory, source);
                
                // TODO: Generate .cs file.
                MarkdownWriter.WriteCodeFromSourceFile(sourceCodeFile, language ?? "", code.RequiresIndentation);   
            }
            else if (!string.IsNullOrWhiteSpace(code.Element.Value))
            {
                var codeText = code.Element.Value;
                MarkdownWriter.WriteCodeFromText(codeText, language ?? "", code.RequiresIndentation);   
            }

            MarkdownWriter.AppendLine();
        }

        public override void Visit(SectionsElement sections)
        {
            foreach (var element in sections.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                switch ( type )
                {
                    case ElementType.Section:
                        mamlElementType = new SectionElement(element, true);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Sections>.");
                }

                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(SectionElement section)
        {
            foreach (var element in section.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType;
                switch ( type )
                {
                    case ElementType.Content:
                        mamlElementType = new ContentElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Sections:
                        mamlElementType = new SectionsElement(element);
                        break;
                    case ElementType.Title:
                        mamlElementType = section.IsSubsection ? new TitleElement(element, Heading.H3) : new TitleElement(element, Heading.H2);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Section>.");
                }

                mamlElementType?.Accept(this);

            }
        }

        public override void Visit(TitleElement title)
        {
            MarkdownWriter.AppendLine();
            switch (title.Heading)
            {
                case Heading.H1:
                    MarkdownWriter.WriteHeading1(title.Element.Value);
                    break;
                case Heading.H2:
                    MarkdownWriter.WriteHeading2(title.Element.Value);
                    break;
                case Heading.H3:
                    MarkdownWriter.WriteHeading3(title.Element.Value);
                    break;
                case Heading.H4:
                    MarkdownWriter.WriteHeading4(title.Element.Value);
                    break;
                case Heading.H5:
                    MarkdownWriter.WriteHeading5(title.Element.Value);
                    break;
                case Heading.H6:
                    MarkdownWriter.WriteHeading6(title.Element.Value);
                    break;
                default:
                    throw new NotSupportedException($"Heading type '{title.Heading}' not supported for <Title> element.");
            }
        }

        public override void Visit(IntroductionElement introduction)
        {
            foreach (var element in introduction.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                var requiresIndentation =
                    introduction.RequiresIndentation && element != introduction.Element.Elements().First();
                switch (type)
                {
                    case ElementType.Alert:
                        mamlElementType = new AlertElement(element);
                        break;
                    case ElementType.Code:
                        mamlElementType = new CodeElement(element);
                        break;
                    case ElementType.List:
                        mamlElementType = new ListElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element, false, requiresIndentation, true) : new ParaElement(element, requiresIndentation, true);
                        break;
                    case ElementType.Procedure:
                        mamlElementType = new ProcedureElement(element);
                        break;
                    case ElementType.Table:
                        mamlElementType = new TableElement(element);
                        break;
                    case ElementType.AutoOutline:
                        // Nothing to generate. DocFx automatically creates outline from heading elements.
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Introduction>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(RelatedTopicsElement relatedTopics)
        {
            if (relatedTopics.Element.HasElements)
            {
                MarkdownWriter.WriteHeading2("See Also");
            }
            
            foreach (var element in relatedTopics.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType;
                switch (type)
                {
                    case ElementType.CodeEntityReference:
                        // TODO: naming of these 3 methods.
                        WriteRelatedTopicsReferenceHeader(ref relatedTopics);
                        mamlElementType = new CodeEntityReferenceElement(element, false, true);
                        break;
                    case ElementType.ExternalLink:
                        WriteRelatedTopicsResourcesHeader(ref relatedTopics);
                        mamlElementType = new ExternalLinkElement(element, false, true);
                        break;
                    case ElementType.Link:
                        WriteRelatedTopicsResourcesHeader(ref relatedTopics);
                        mamlElementType = new LinkElement(element, false, false, true);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <RelatedTopics>.");
                }

                mamlElementType?.Accept(this);
                if (relatedTopics.IsReferenceAppended || relatedTopics.IsOtherResourcesAppended)
                {
                    MarkdownWriter.Append("<br>");
                }

            }
        }

        public void WriteRelatedTopicsReferenceHeader( ref RelatedTopicsElement relatedTopics )
        {
            if (!relatedTopics.IsReferenceAppended)
            {
                MarkdownWriter.WriteTextBold("Reference", true);
                relatedTopics.IsReferenceAppended = true;
            }
        }
        
        public void WriteRelatedTopicsResourcesHeader(ref RelatedTopicsElement relatedTopics)
        {
            if (!relatedTopics.IsOtherResourcesAppended)
            {
                MarkdownWriter.WriteTextBold("Other Resources", true);
                relatedTopics.IsOtherResourcesAppended = true;
            }
        }

        public override void Visit(ContentElement content )
        {
            foreach (var element in content.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                var requiresIndentation =
                    content.RequiresIndentation && element != content.Element.Elements().First();
                switch (type)
                {
                    case ElementType.Alert:
                        // TODO: Move to separate method.
                        var previousSiblingElement = element.PreviousNode;
                        if (previousSiblingElement != null)
                        {
                            var previousElementIsAlert = XElement.Parse(previousSiblingElement.ToString()).Name.LocalName == "alert";
                            if (requiresIndentation && previousElementIsAlert)
                            {
                                MarkdownWriter.AppendLine("---", true, true);
                            }
                        }
                        mamlElementType = new AlertElement(element, requiresIndentation);
                        break;
                    case ElementType.Code:
                        mamlElementType = new CodeElement(element, requiresIndentation);
                        break;
                    case ElementType.List:
                        mamlElementType = new ListElement(element, requiresIndentation);
                        break;
                    case ElementType.MediaLink:
                        mamlElementType = new MediaLinkElement(element, requiresIndentation, true);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element, false, requiresIndentation, true) : new ParaElement(element, requiresIndentation, true);
                        break;
                    case ElementType.Procedure:
                        mamlElementType = new ProcedureElement(element, requiresIndentation);
                        break;
                    case ElementType.Section:
                        mamlElementType = new SectionElement(element, requiresIndentation);
                        break;
                    case ElementType.Sections:
                        mamlElementType = new SectionsElement(element, requiresIndentation);
                        break;
                    case ElementType.Steps:
                        mamlElementType = new StepsElement(element, requiresIndentation);
                        break;
                    case ElementType.Table:
                        mamlElementType = new TableElement(element, requiresIndentation, true);
                        break;
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element, Heading.H2, requiresIndentation);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Content>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(StepsElement stepsElement)
        {
            var steps = stepsElement.Element.Elements().ToArray();

            for (int i = 0; i < steps.Length; i++)
            {
                MarkdownWriter.StartOrderedListItem( i+1 );

                var stepElement = steps[i];
                Utilities.ParseEnum(stepElement.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Step:
                        var step = new StepElement(stepElement, stepsElement.RequiresIndentation, true);
                        step.Accept(this);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Steps>.");
                }
            }
        }

        public override void Visit(StepElement step)
        {
            foreach (var element in step.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType;

                switch (type)
                {
                    case ElementType.Content:
                        mamlElementType = new ContentElement(element, true);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Section:
                        mamlElementType = new SectionElement(element);
                        break;
                    case ElementType.Step:
                        mamlElementType = new StepElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Step>.");
                }
                
                mamlElementType.Accept(this);
            }
        }
        
        public override void Visit(ProcedureElement procedure)
        {
            foreach (var element in procedure.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType;

                switch (type)
                {
                    case ElementType.Conclusion:
                        mamlElementType = new ConclusionElement(element);
                        break;
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element, Heading.H3);
                        break;
                    case ElementType.Steps:
                        mamlElementType = new StepsElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Procedure>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }
        
        public override void Visit(ConclusionElement procedure)
        {
            foreach (var element in procedure.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType;

                switch (type)
                {
                    case ElementType.Content:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Conclusion>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(MediaLinkElement mediaLink)
        {
            foreach (var element in mediaLink.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;

                switch (type)
                {
                    case ElementType.Caption:
                        mamlElementType = new CaptionElement(element);
                        break;
                    case ElementType.Image:
                        mamlElementType = new ImageElement(element, mediaLink.RequiresIndentation);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <MediaLink>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(CaptionElement caption)
        {
            
        }
        
        public override void Visit(TableElement table)
        {
            foreach (var element in table.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                
                switch (type)
                {
                    case ElementType.TableHeader:
                        mamlElementType = new TableHeaderElement(element, table.RequiresIndentation);
                        break;
                    case ElementType.Row:
                        mamlElementType = new RowElement(element, table.RequiresIndentation);
                        break;
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element, Heading.H4);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Table>.");
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(ImageElement image)
        {
            var imageReference = image.Element.Attributes().SingleOrDefault(a => a.Name.LocalName == "href")?.Value;
            if (imageReference == null)
            {
                throw new NotSupportedException($"<Image> element in {Program.CurrentConceptualFile} is missing a 'href' attribute.");
            }

            var imageFilePath = Utilities.GetRelativePathOfReferencedImage(imageReference, "png");

            MarkdownWriter.AppendImage(imageFilePath, image.RequiresIndentation);
        }

        public override void Visit(TableHeaderElement tableHeader)
        {
            List<string> entryValues = new();

            foreach (var row in tableHeader.Element.Elements())
            {
                foreach (var entry in row.Elements())
                {
                    entryValues.Add(entry.Value);
                }
            }

            MarkdownWriter.AppendTableHeader(entryValues.ToArray(), tableHeader.RequiresIndentation);
        }

        public override void Visit(RowElement row)
        {
            MarkdownWriter.StartTableRow(row.RequiresIndentation);

            var entries = row.Element.Elements().ToArray();
            for (int i = 0; i < entries.Count(); i++)
            {
                Utilities.ParseEnum(entries[i].Name.LocalName, out ElementType type);

                MamlElement mamlElementType;
                switch (type)
                {
                    case ElementType.Entry:
                        mamlElementType = new EntryElement(entries[i]);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Row>.");
                }

                mamlElementType.Accept(this);

                if ( i + 1 < entries.Count() ) {
                    MarkdownWriter.AddTableRowSeparator();
                }
            }
            
            MarkdownWriter.EndTableRow();

        }

        public override void Visit(EntryElement entry)
        {
            var elements = entry.Element.Elements().ToArray();
            var elementsCount = elements.Length;
            bool requiresLineBreak = elementsCount > 1;
                
            for ( var i = 0; i < elementsCount; i++)
            {
                var element = elements[i];
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement mamlElementType;
                switch (type)
                {
                    case ElementType.Code:
                        mamlElementType = new CodeElement(element);
                        break;
                    case ElementType.Link:
                        mamlElementType = new LinkElement(element, false);
                        break;
                    case ElementType.List:
                        mamlElementType = new ListElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element, true) : new ParaElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Entry>.");
                }

                mamlElementType.Accept(this);
                if ( i + 1 < elementsCount ) {
                    MarkdownWriter.Append("<br>");
                }
            }
        }

        public override void Visit(LinkElement link)
        {
            var linkHref = link.Element.Attributes().SingleOrDefault(a => a.Name.LocalName == "href");

            if (linkHref != null && linkHref.Value.Contains('#'))
            {
                var splitLink = linkHref.Value.Split('#');

                if (splitLink.Length != 2)
                {
                    throw new NotSupportedException($"Split link has illegal length of '{splitLink.Length}'.");
                }
                
                var linkData = Utilities.GetTitleAndLinkFromAddressedElement(splitLink[1]);
                var newLink = Utilities.CreateNewHeaderLink(linkData.Link, splitLink[0]);
                MarkdownWriter.WriteLink(linkData.Title, newLink, link.IsOnlyLink);
            }
            else
            {
                MarkdownWriter.WriteXref(linkHref.Value);
            }
        }

        public override void Visit(ParaElement para)
        {
            if (para.RequiresIndentation)
            {
                MarkdownWriter.Append(string.Empty, true, para.RequiresIndentation);
            }

            var paraElementValue = para.Element.Value;
            
            if (para.Element.Parent != null && paraElementValue.Length > 1)
            {
                Utilities.ParseEnum(para.Element.Parent.Name.LocalName, out ElementType parentType);
                if (parentType == ElementType.Alert)
                {
                    paraElementValue = Utilities.EscapeSpecialMarkdownCharactersAtStart(paraElementValue);
                }
            }

            // Para contains only text, so we add a space behind.
            if (!para.Element.HasElements)
            {
                var normalizedText = Utilities.NormalizeTextSpaces(paraElementValue);
                MarkdownWriter.Append(normalizedText);

                if (para.Element.Parent != null && para.Element.Parent.Name.LocalName != "entry")
                {
                    MarkdownWriter.AppendLine(Environment.NewLine);
                }
            }

            // Para contains something more.
            if (para.Element.HasElements)
            {
                foreach (var element in para.Element.Elements())
                {
                    Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                    MamlElement? mamlElement;
                    
                    switch (type)
                    {
                        case ElementType.Para:
                            mamlElement = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                            break;
                        case ElementType.Code:
                            mamlElement = new CodeElement(element);
                            break;
                        case ElementType.CodeInline:
                            mamlElement = new CodeInlineElement(element);
                            break;
                        default:
                            throw new NotSupportedException($"Element type {type} not supported for <Para>.");
                    }

                    mamlElement?.Accept(this);
                }
            }
        }
        
        public override void Visit(RichParaElement richPara)
        {
            if (richPara.RequiresIndentation)
            {
                MarkdownWriter.Append(string.Empty, true, richPara.RequiresIndentation);
            }
            
            var nodesCount = richPara.Element.Nodes().Count();

            // Para contains an element (and possibly even text).
            foreach (var node in richPara.Element.Nodes())
            {
                bool skipSpace = false;
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementFromNode = XElement.Parse(node.ToString());

                        Utilities.ParseEnum(elementFromNode.Name.LocalName, out ElementType type);

                        MamlElement? mamlElementType = null;

                        switch (type)
                        {
                            case ElementType.Link:
                                var isOnlyLink = richPara.Element.Nodes().All(n => n.NodeType == XmlNodeType.Element);
                                //var shouldLineBreak = richPara.Element.Parent!.Name.LocalName != "entry"; // TODO: Check if this needs to work
                                mamlElementType = new LinkElement(elementFromNode, isOnlyLink);
                                break;
                            case ElementType.Command:
                                mamlElementType = new CommandElement(elementFromNode, false, true);
                                break;
                            case ElementType.Code:
                                mamlElementType = new CodeElement(elementFromNode);
                                break;
                            case ElementType.CodeEntityReference:
                                mamlElementType = new CodeEntityReferenceElement(elementFromNode);
                                break;
                            case ElementType.CodeInline:
                            case ElementType.LanguageKeyword:
                            case ElementType.Literal:
                                mamlElementType = new CodeInlineElement(elementFromNode);
                                break;
                            case ElementType.ExternalLink:
                                mamlElementType = new ExternalLinkElement(elementFromNode);
                                break;
                            case ElementType.LegacyBold:
                            case ElementType.Application:
                            case ElementType.Ui:
                                mamlElementType = new LegacyBoldElement(elementFromNode);
                                break;
                            case ElementType.LegacyItalic:
                            case ElementType.FictitiousUri:
                            case ElementType.NewTerm:
                            case ElementType.ParameterReference:
                            case ElementType.Placeholder:
                            case ElementType.Phrase:
                                mamlElementType = new LegacyItalicElement(elementFromNode);
                                break;
                            case ElementType.LocalUri:
                                mamlElementType = new LegacyItalicElement(elementFromNode);
                                break;
                            case ElementType.Markup:
                                MarkdownWriter.Append(node.ToString());
                                break;
                            case ElementType.Para:
                                mamlElementType = new ParaElement(elementFromNode);
                                break;
                            case ElementType.Superscript:
                                MarkdownWriter.Append(node.ToString());
                                break;
                            case ElementType.QuoteInline:
                                MarkdownWriter.AppendQuoteInline(elementFromNode.Name.LocalName);
                                break;
                            case ElementType.Token:
                                mamlElementType = new TokenElement(elementFromNode);
                                break;
                            default:
                                throw new NotSupportedException($"Element type {type} not supported for <RichPara>.");
                        }
                        
                        mamlElementType?.Accept(this);

                        break;
                    case XmlNodeType.Text:
                        var normalizedText = Utilities.NormalizeTextSpaces(node.ToString());
                        skipSpace = Utilities.TextEndsWithEnclosingGlyphs(normalizedText);
                        MarkdownWriter.Append(normalizedText);
                        break;
                    default:
                        throw new NotSupportedException($"Node type {node.NodeType} not supported for <RichPara>.");
                }

                if (node.NextNode?.NodeType == XmlNodeType.Text)
                {
                    skipSpace = Utilities.TextStartsWithPunctuation(node.NextNode.ToString()) || Utilities.TextStartsWithEnclosingGlyphs(node.NextNode.ToString());
                }

                if (nodesCount > 1 && !skipSpace)
                {
                    MarkdownWriter.Append(' ');
                }
            }

            if ( !richPara.IsTableElement && (richPara.ShouldLineBreak || nodesCount > 1 ))
            {
                MarkdownWriter.AppendLine("\n");
            }

        }

        public override void Visit(CommandElement command)
        {
            MarkdownWriter.StartCodeInline();

            foreach (var node in command.Element.Nodes())
            {
                var noSpace = false;
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementFromNode = XElement.Parse(node.ToString());

                        Utilities.ParseEnum(elementFromNode.Name.LocalName, out ElementType type);

                        MamlElement? mamlElementType;

                        switch (type)
                        {
                            // Replaceable element is visually identical to italic.
                            case ElementType.Replaceable:
                                mamlElementType = new LegacyItalicElement(elementFromNode);
                                noSpace = node.NextNode?.ToString().StartsWith('"') ?? false;
                                break;
                            default:
                                throw new NotSupportedException($"Element type {type} not supported for <Command>.");
                        }

                        mamlElementType?.Accept(this);

                        break;
                    case XmlNodeType.Text:
                        var normalizedText = Utilities.NormalizeTextSpaces(node.ToString());
                        noSpace = normalizedText.EndsWith('"');
                        MarkdownWriter.Append(normalizedText);
                        break;
                    default:
                        throw new NotSupportedException($"Node type {node.NodeType} not supported for <Command>.");
                }
                if (!noSpace && command.Element.Nodes().Count() > 1) 
                {
                    MarkdownWriter.Append(' ');
                }
            }

            MarkdownWriter.EndCodeInline();
        }

        public override void Visit(TokenElement token)
        {
            var tokenName = token.Element.Value;

            // TODO: Move to separate method AND/OR fix how this is read.
            var tokenDocuments = Directory.EnumerateFiles(Program.DocumentationFilesDirectory.FullName, "*.tokens",
                SearchOption.AllDirectories);

            foreach (var document in tokenDocuments)
            {
                var doc = XDocument.Load(document);

                var element = doc.Root!.Elements().SingleOrDefault(e => e.Attribute("id")?.Value == tokenName);
                
                if (element != null)
                {
                    MamlElement mamlElement = element.HasElements ? new RichParaElement(element, true) : new ParaElement(element);
                    mamlElement.Accept(this);
                    break;
                }
            }
        }
        
        public override void Visit(CodeEntityReferenceElement codeEntityReference)
        {
            var cleanCodeEntityReference = Utilities.NormalizeCodeEntityReference(codeEntityReference.Element.Value);
            MarkdownWriter.AppendCodeEntityReference(cleanCodeEntityReference);
        }

        public override void Visit(ExternalLinkElement externalLink)
        {
            string? linkText = null;
            string? linkUri = null;
            
            foreach (var linkElement in externalLink.Element.Elements())
            {
                switch (linkElement.Name.LocalName)
                {
                    case "linkText":
                        linkText = linkElement.Value;
                        break;
                    case "linkUri":
                        linkUri = linkElement.Value;
                        break;
                }
            }

            if (linkText != null && linkUri != null)
            {
                MarkdownWriter.WriteLink(linkText, linkUri);
            }
        }

        public override void Visit(CodeInlineElement codeInline)
        {
            MarkdownWriter.AppendCodeInline(codeInline.Element.Value, true);
        }

        public override void Visit(LegacyBoldElement bold)
        {
            MarkdownWriter.WriteTextBold(bold.Element.Value);
        }
        public override void Visit(LegacyItalicElement bold)
        {
            MarkdownWriter.WriteTextItalic(bold.Element.Value);
        }

        public override void Visit(ListElement list)
        {
            var bulletList = list.Element.Attribute("class")!.Value == "bullet";
            var orderedList = list.Element.Attribute("class")!.Value == "ordered";

            if (orderedList && list.RequiresIndentation)
            {
                AddIndentedOrderedList(list);
                return;
            }

            foreach (var listItem in list.Element.Elements())
            {
                Utilities.ParseEnum(listItem.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.ListItem:
                        MarkdownWriter.StartUnorderedListItem(list.RequiresIndentation);
                        var listItemElement = new ListItemElement(listItem, false, list.RequiresIndentation, true);
                        listItemElement.Accept(this);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <list>.");
                }
            }
        }

        public void AddIndentedOrderedList(ListElement list)
        {
            var elementsCount = list.Element.Elements().Count();
            
            for (int i = 0; i < elementsCount; i++)
            {
                var listItem = list.Element.Elements().ToArray()[i];
                
                Utilities.ParseEnum(listItem.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.ListItem:
                        var listItemEntry = (char) (i + 97) + ". ";
                        MarkdownWriter.Append(listItemEntry, false, list.RequiresIndentation);
                        var listItemElement = new ListItemElement(listItem, true, list.RequiresIndentation, true);
                        listItemElement.Accept(this);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <list>.");
                }
            }
        }

        public override void Visit(ListItemElement listItem)
        {
            MamlElement? mamlElement;
            
            // If the first <ListItem> node starts with a text outside any element type, we use the same handling as of <Para> element.
            var listItemElement = listItem.Element;
            if (listItemElement.Nodes().First().NodeType == XmlNodeType.Text)
            {
                mamlElement = listItemElement.HasElements ? new RichParaElement(listItemElement) : new ParaElement(listItemElement);
                mamlElement.Accept(this);
                return;
            }

            foreach (var element in listItem.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Para:
                        mamlElement = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Code:
                        mamlElement = new CodeElement(element);
                        break;
                    case ElementType.List:
                        mamlElement = new ListElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <listItem>.");
                }

                mamlElement?.Accept(this);

                if (listItem.IsOrderedListItem && element == listItemElement.Elements().First() && listItemElement.Elements().Count() > 1)
                {
                    MarkdownWriter.Append("<br>");
                }
            }
        }
    }
}
