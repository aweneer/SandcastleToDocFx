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
                        throw new NotSupportedException($"Element type {type} not supported for <<alert>.");
                }
            }
        }

        public override void Visit(TopicElement topic)
        {
            Console.WriteLine("TopicElement");
        }

        public override void Visit(CodeElement code)
        {
            var language = code.Element.Attribute("language")?.Value;
            var sourceCode = code.Element.Attribute("source")?.Value;

               
            if (sourceCode != null)
            {
                var sourceCodeFile = Path.Combine(Program.SourceCodeDirectory, sourceCode);

                MarkdownWriter.WriteCodeFromSourceFile(sourceCodeFile, language);   
            }
            MarkdownWriter.WriteLine();
        }

        public override void Visit(SectionElement section)
        {
            foreach (var element in section.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;
                switch ( type )
                {
                    case ElementType.Title:
                        mamlElementType = new TitleElement(element);
                        break;
                    case ElementType.Content:
                        mamlElementType = new ContentElement(element);
                        break;
                    default:
                        break;
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
                Console.WriteLine(element.Name.LocalName);
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
                        mamlElementType = element.HasAttributes ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    
                    default:
                        Console.WriteLine(element.Name.LocalName +" not supported yet");

                        break;
                }
                
                mamlElementType?.Accept(this);
            }
        }

        public override void Visit(RelatedTopicsElement relatedTopics)
        {
            foreach (var element in relatedTopics.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                MamlElement? mamlElementType = null;

                switch (type)
                {
                    case ElementType.Link:
                        mamlElementType = new LinkElement(element);
                        break;
                    default:
                        Console.WriteLine(element.Name.LocalName + " unsupported yet.");
                        break;
                }

                mamlElementType?.Accept(this);
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
                        mamlElementType = element.HasAttributes ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    case ElementType.Procedure:
                        mamlElementType = new ProcedureElement(element);
                        break;
                    case ElementType.Section:
                        mamlElementType = new SectionElement(element);
                        break;
                    case ElementType.Steps:
                        Console.WriteLine(element.Name.LocalName + " NOT SUPPORTED YET");
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

        public override void Visit(ProcedureElement procedure)
        {
            // TODO
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

                        break;
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
                        break;
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
                        break;
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
                    case ElementType.Link:
                        mamlElementType = new LinkElement(element);
                        break;
                    case ElementType.Para:
                        mamlElementType = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <Entry>.");
                        break;
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
            MarkdownWriter.WriteParagraph(para.Element.Value);

            // Para contains only text, so we add a space behind.
            if (!para.Element.Elements().Any() && para.Element.Parent!.Name.LocalName != "entry" )
            {
                MarkdownWriter.WriteLine();
                MarkdownWriter.WriteLine();
            }
        }
        
        public override void Visit(RichParaElement para)
        {
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
                            case ElementType.CodeInline:
                            case ElementType.LanguageKeyword:
                                mamlElementType = new CodeInlineElement(elementFromNode);
                                break;
                            case ElementType.ExternalLink:
                                mamlElementType = new ExternalLinkElement(elementFromNode);
                                break;
                            case ElementType.LegacyBold:
                                mamlElementType = new CodeInlineElement(elementFromNode);
                                break;
                            case ElementType.LegacyItalic:
                                mamlElementType = new CodeInlineElement(elementFromNode);
                                break;
                            case ElementType.CodeEntityReference:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.LocalUri:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Literal:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Application:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Ui:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Markup:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Superscript:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.NewTerm:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.ParameterReference:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.QuoteInline:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.TableHeader:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Title:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            case ElementType.Token:
                                Console.WriteLine(elementFromNode.Name.LocalName + " NOT SUPPORTED YET");
                                break;
                            default:
                                Console.WriteLine(elementFromNode.Value);
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
                MarkdownWriter.Append(' ');
            }
            MarkdownWriter.WriteLine();
            MarkdownWriter.WriteLine();
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

                MarkdownWriter.WriteLine();
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

                MarkdownWriter.WriteLine();
            }
        }
    }
}
