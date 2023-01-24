using System;
using System.Collections.Generic;
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
            
        }
        public override void Visit(TopicElement topic)
        {
            Console.WriteLine("TopicElement");
        }

        public override void Visit(SectionElement section)
        {
            foreach (var item in section.Element.Elements())
            {
                Utilities.ParseEnum(item.Name.LocalName, out ElementType type);

                switch ( type )
                {
                    case ElementType.Title:
                        MarkdownWriter.WriteHeading2(item.Value);
                        break;
                    case ElementType.Content:
                        var content = new ContentElement(item);
                        content.Accept(this);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Visit(IntroductionElement introduction)
        {
            foreach (var element in introduction.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Alert:
                        Console.WriteLine("alert not supported yet");
                        break;
                    case ElementType.List:
                        var list = new ListElement(element);
                        list.Accept(this);
                        break;
                    case ElementType.Para:
                        var para = new ParaElement(element);
                        para.Accept(this);
                        break;
                    
                    default:
                        Console.WriteLine(element.Name.LocalName +" not supported yet");

                        break;
                }
            }
        }

        public override void Visit(RelatedTopicsElement relatedTopics)
        {
            foreach (var element in relatedTopics.Element.Elements())
            {
                Utilities.ParseEnum(element.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Link:
                        var link = new LinkElement(element);
                        link.Accept(this);
                        break;
                    default:
                        Console.WriteLine(element.Name.LocalName + " unsupported yet.");
                        break;
                }
            }
        }

        public override void Visit(ContentElement content )
        {
            foreach (var item in content.Element.Elements())
            {
                Utilities.ParseEnum(item.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Title:
                        MarkdownWriter.WriteHeading2(item.Value);
                        break;
                    case ElementType.Table:
                        var element = new TableElement(item);
                        element.Accept(this);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Visit(TableElement content)
        {
            foreach (var item in content.Element.Elements())
            {
                Utilities.ParseEnum(item.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.TableHeader:
                        var tableHeader = new TableHeaderElement(item);
                        tableHeader.Accept(this);
                        break;
                    case ElementType.Row:
                        var rowElement = new RowElement(item);
                        rowElement.Accept(this);
                        break;
                    default:
                        break;
                }
            }
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

                switch (type)
                {
                    case ElementType.Entry:
                        var element = new EntryElement(entries[i]);
                        element.Accept(this);
                        break;
                    default:
                        break;
                }

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
                switch (type)
                {    
                    case ElementType.Link:
                        var link = new LinkElement(element);
                        link.Accept(this);
                        break;
                    case ElementType.Para:
                        MamlElement para = element.HasElements ? new RichParaElement(element) : new ParaElement(element);
                        para.Accept(this);
                        break;
                    default:
                        
                        break;
                }
            }
        }

        public override void Visit(LinkElement link)
        {
            MarkdownWriter.WriteXref(link.Element.Attributes().SingleOrDefault( a => a.Name.LocalName == "href")!.Value);
        }

        public override void Visit(ParaElement para)
        {
            MarkdownWriter.WriteParagraph(para.Element.Value);

            // Para contains only text.
            if (!para.Element.Elements().Any() && para.Element.Parent!.Name.LocalName != "entry" )
            {
                MarkdownWriter.WriteLine();
                return;
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
                        switch (type)
                        {
                            case ElementType.Link:
                                var link = new LinkElement(elementFromNode);
                                link.Accept(this);
                                break;
                            case ElementType.CodeInline:
                            case ElementType.LanguageKeyword:
                                var codeInline = new CodeInlineElement(elementFromNode);
                                codeInline.Accept(this);
                                break;
                            case ElementType.ExternalLink:
                                var externalLink = new ExternalLinkElement(elementFromNode);
                                externalLink.Accept(this);
                                break;
                            case ElementType.LegacyBold:
                                var bold = new CodeInlineElement(elementFromNode);
                                bold.Accept(this);
                                break;
                            case ElementType.LegacyItalic:
                                var italic = new CodeInlineElement(elementFromNode);
                                italic.Accept(this);
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
                            default:
                                Console.WriteLine(elementFromNode.Value);
                                throw new NotSupportedException($"Element type {type} not supported for <RichPara>.");
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        MarkdownWriter.Append(node.ToString());
                        break;
                    default:
                        throw new NotSupportedException($"Node type {node.NodeType} not supported for <RichPara>.");
                }
                MarkdownWriter.Append(' ');
            }
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
            foreach (var item in listItem.Element.Elements())
            {
                Utilities.ParseEnum(item.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Para:
                        MamlElement para = item.HasElements ? new RichParaElement(item) : new ParaElement(item);
                        para.Accept(this);
                        break;
                    default:
                        throw new NotSupportedException($"Element type {type} not supported for <listItem>.");
                        break;
                }

                MarkdownWriter.WriteLine();
            }
        }
    }
}
