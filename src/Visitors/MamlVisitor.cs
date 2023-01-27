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
            MarkdownWriter.AppendAlert(alertType);

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
        }

        public override void Visit(TopicElement topic)
        {
            Console.WriteLine("TopicElement");
        }

        public override void Visit(CodeElement code)
        {
            var language = code.Element.Attribute("language")?.Value;
            var source = code.Element.Attribute("source")?.Value;
            
            if (source != null)
            {
                var sourceCodeFile = Path.Combine(Program.SourceCodeDirectory, source);

                MarkdownWriter.WriteCodeFromSourceFile(sourceCodeFile, language ?? "");   
            }
            else if (!string.IsNullOrWhiteSpace(code.Element.Value))
            {
                MarkdownWriter.WriteCodeFromText(code.Element.Value, language ?? "");   
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
                        mamlElementType = new SectionElement(element);
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
                        mamlElementType = new TitleElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Section>.");
                }

                mamlElementType?.Accept(this);

            }
        }

        public override void Visit(TitleElement title)
        {
            MarkdownWriter.WriteHeading2(title.Element.Value);

        }

        public override void Visit(IntroductionElement introduction)
        {
            foreach (var element in introduction.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;

                switch (type)
                {
                    case ElementType.Alert:
                        mamlElementType = new AlertElement(element);
                        break;
                    case ElementType.List:
                        mamlElementType = new ListElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    default:
                        
                        //throw new NotSupportedException($"Element type {type} not supported for <Introduction>.");
                        break;
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

            var referenceAppended = false;
            var otherResourcesAppended = false;
            
            foreach (var element in relatedTopics.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;

                switch (type)
                {
                    case ElementType.CodeEntityReference:
                        // TODO Remove code duplication.
                        if (!referenceAppended)
                        {
                            MarkdownWriter.WriteTextBold("Reference", true);
                            referenceAppended = true;
                        }
                        mamlElementType = new CodeEntityReferenceElement(element);
                        break;
                    case ElementType.ExternalLink:
                        // TODO 1/2 Remove code duplication.
                        if (!otherResourcesAppended)
                        {
                            MarkdownWriter.WriteTextBold("Other Resources", true);
                            referenceAppended = true;
                        }
                        mamlElementType = new ExternalLinkElement(element);
                        break;
                    case ElementType.Link:
                        // TODO 2/2 Remove code duplication.

                        if (!otherResourcesAppended)
                        {
                            MarkdownWriter.WriteTextBold("Other Resources", true);
                            referenceAppended = true;
                        }
                        mamlElementType = new LinkElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <RelatedTopics>.");
                        break;
                }

                mamlElementType?.Accept(this);
                MarkdownWriter.AppendLine();
                MarkdownWriter.AppendLine();

            }
        }

        public override void Visit(ContentElement content )
        {
            foreach (var element in content.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                
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
                    case ElementType.MediaLink:
                        mamlElementType = new MediaLinkElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Procedure:
                        mamlElementType = new ProcedureElement(element);
                        break;
                    case ElementType.Section:
                        mamlElementType = new SectionElement(element);
                        break;
                    case ElementType.Sections:
                        mamlElementType = new SectionsElement(element);
                        break;
                    case ElementType.Steps:
                        mamlElementType = new StepsElement(element);
                        break;
                    case ElementType.Table:
                        mamlElementType = new TableElement(element);
                        break;
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Content>.");

                        break;
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
                        var step = new StepElement(stepElement, true);
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
                        mamlElementType = new ContentElement(element);
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

                MamlElement? mamlElementType = null;

                switch (type)
                {
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element);
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
                        mamlElementType = new ImageElement(element);
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

                MamlElement mamlElementType;
                
                switch (type)
                {
                    case ElementType.TableHeader:
                        mamlElementType = new TableHeaderElement(element);
                        break;
                    case ElementType.Row:
                        mamlElementType = new RowElement(element);
                        break;
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Table>.");
                }
                
                mamlElementType.Accept(this);
            }
        }

        public override void Visit(ImageElement image)
        {
            var imageReference = image.Element.Attributes().SingleOrDefault(a => a.Name.LocalName == "href")?.Value;
            if (imageReference == null)
            {
                // TODO: Error handling.
                return;
            }

            MarkdownWriter.AppendImage(imageReference);
        }

        public override void Visit(TableHeaderElement content)
        {
            List<string> entryValues = new();

            foreach (var row in content.Element.Elements())
            {
                foreach (var entry in row.Elements())
                {
                    entryValues.Add(entry.Value);
                }
            }

            MarkdownWriter.AppendTableHeader(entryValues.ToArray());
        }

        public override void Visit(RowElement row)
        {
            MarkdownWriter.StartTableRow();

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
            if ( entry.Element.HasElements )
            {
                var element = entry.Element.Elements().First();

                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement mamlElementType;
                switch (type)
                {
                    case ElementType.Code:
                        mamlElementType = new CodeElement(element);
                        break;
                    case ElementType.Link:
                        mamlElementType = new LinkElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Entry>.");
                }

                mamlElementType.Accept(this);
            }
        }

        public override void Visit(LinkElement link)
        {
            MarkdownWriter.WriteXref(link.Element.Attributes().SingleOrDefault( a => a.Name.LocalName == "href")!.Value);
        }

        public override void Visit(ParaElement para)
        {
            // Para contains only text, so we add a space behind.
            if (!para.Element.Elements().Any())
            {
                MarkdownWriter.WriteParagraph(para.Element.Value);

                if (para.Element.Parent!.Name.LocalName != "entry")
                {
                    MarkdownWriter.AppendLine();
                    MarkdownWriter.AppendLine();
                }
            }
            
            // Para contains something more.
            if (para.Element.Elements().Any())
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
                        default:
                            throw new NotSupportedException($"Element type {type} not supported for <Para>.");
                    }

                    mamlElement?.Accept(this);
                }
            }
        }
        
        public override void Visit(RichParaElement para)
        {
            var nodesCount = para.Element.Nodes().Count();

            // Para contains an element (and possibly even text).
            foreach (var node in para.Element.Nodes())
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        var elementFromNode = XElement.Parse(node.ToString());

                        Utilities.ParseEnum(elementFromNode.Name.LocalName, out ElementType type);

                        MamlElement? mamlElementType = null;

                        switch (type)
                        {
                            case ElementType.Link:
                                mamlElementType = new LinkElement(elementFromNode);
                                break;
                            case ElementType.Command:
                                mamlElementType = new CommandElement(elementFromNode);
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
                                break;
                        }
                        
                        mamlElementType?.Accept(this);

                        break;
                    case XmlNodeType.Text:
                        MarkdownWriter.Append(node.ToString());
                        break;
                    default:
                        throw new NotSupportedException($"Node type {node.NodeType} not supported for <RichPara>.");
                }

                if (nodesCount > 1)
                {
                    MarkdownWriter.Append(' ');
                }
            }

            if (nodesCount > 1)
            {
                MarkdownWriter.AppendLine("\n");
            }

        }

        public override void Visit(TokenElement token)
        {
            var tokenName = token.Element.Value;

            // TODO: Move to separate method AND/OR fix how this is read.
            var tokenDocuments = Directory.EnumerateFiles(Program.DocumentationFilesDirectory, "*.tokens",
                SearchOption.AllDirectories);

            foreach (var document in tokenDocuments)
            {
                var doc = XDocument.Load(document);

                var element = doc.Root!.Elements().SingleOrDefault(e => e.Attribute("id")?.Value == tokenName);

                if (element != null)
                {
                    MamlElement mamlElement = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                    mamlElement.Accept(this);
                    break;
                }
            }
        }
        
        public override void Visit(CodeEntityReferenceElement codeEntityReferenceElement)
        {
            MarkdownWriter.AppendCodeEntityReference(codeEntityReferenceElement.Element.Value);
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
            MarkdownWriter.AppendCodeInline(codeInline.Element.Value);
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
            // TODO: Ordered list
            var bulletList = list.Element.Attribute("class")!.Value == "bullet";

            foreach (var listItem in list.Element.Elements())
            {
                Utilities.ParseEnum(listItem.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.ListItem:
                        MarkdownWriter.StartUnorderedListItem();
                        var listItemElement = new ListItemElement(listItem);
                        listItemElement.Accept(this);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <list>.");
                }

                MarkdownWriter.AppendLine();
            }
        }

        public override void Visit(ListItemElement listItem)
        {
            foreach (var element in listItem.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Para:
                        MamlElement para = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        para.Accept(this);
                        break;
                    case ElementType.Code:
                        var code = new CodeElement(element);
                        code.Accept(this);
                        break;
                    default:
                        break;
                        //throw new NotSupportedException($"Element type {type} not supported for <listItem>.");
                }

                MarkdownWriter.AppendLine();
            }
        }
    }
}
