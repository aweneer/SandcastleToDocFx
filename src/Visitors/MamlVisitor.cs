using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
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
                        MarkdownWriter.WriteHeading2(item.Value);
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
            foreach (var entry in row.Element.Elements())
            {
                Utilities.ParseEnum(entry.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Entry:
                        var element = new EntryElement(entry);
                        element.Accept(this);
                        break;
                    default:
                        break;

                }
            }
        }

        public override void Visit(EntryElement entry)
        {
            List<string> entries = new();

            var element = entry.Element.Elements().FirstOrDefault();
                
            Utilities.ParseEnum(element.Name.LocalName, out ElementType type);
            switch (type)
            {
                case ElementType.Para:
                    entries.Add(element.Value.Trim());
                    break;
                case ElementType.Link:
                    Console.WriteLine("Hi");
                    Console.WriteLine(element.Value);
                    Console.WriteLine("Hi");
                    entries.Add(element.Attribute("href").Value.Trim());
                    break;
                default:
                    break;

            }
            
            MarkdownWriter.AppendTableRow(entries.ToArray());

            //MarkdownWriter.AppendEntries(entries.ToArray());
        }

        public override void Visit(LinkElement link)
        {
            Console.WriteLine(link.Element.Attribute("href").Value);
        }

        public override void Visit(ParaElement para)
        {
            MarkdownWriter.WriteParagraph(para.Element.Value);

            // Para contains only text.
            if (!para.Element.Elements().Any())
            {
                MarkdownWriter.WriteLine();
                return;
            }

            // Para contains an element (and possibly even text).

            foreach (var paraElement in para.Element.Elements())
            {
                Utilities.ParseEnum(paraElement.Name.LocalName, out ElementType type);
            }

        }
        
        public override void Visit(RichParaElement para)
        {
            // Para contains only text.
            if (!para.Element.Elements().Any())
            {
                MarkdownWriter.WriteParagraph(para.Element.Value);
                return;
            }

            // Para contains an element (and possibly even text).

            foreach (var paraElement in para.Element.Elements())
            {
                Utilities.ParseEnum(paraElement.Name.LocalName, out ElementType type);

                
            }

        }

        public override void Visit(ListElement list)
        {
            var bulletList = list.Element.Attribute("class")!.Value == "bullet";

            foreach (var listItemType in list.Element.Elements().Elements())
            {
                Utilities.ParseEnum(listItemType.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Para:
                        MamlElement para = listItemType.HasElements ? new RichParaElement(listItemType) : new ParaElement(listItemType);
                        para.Accept(this);
                        break;
                    default:
                        Console.WriteLine(listItemType.Name.LocalName +" not supported in listElement");
                        break;

                }
            }
            var listItems = list.Element.Elements().Select( item => item.Value ).ToArray();
            if (bulletList)
            {
                foreach (var listItem in list.Element.Elements().Elements())
                {
                    MarkdownWriter.WriteUnorderedListItem(listItem.ToString());
                }
            }
            else
            {
                var position = 1;
                foreach (var listItem in list.Element.Elements())
                {
                    MarkdownWriter.WriteOrderedListItem(position, listItem.ToString());
                    position++;
                }
            }
        }
    }
}
