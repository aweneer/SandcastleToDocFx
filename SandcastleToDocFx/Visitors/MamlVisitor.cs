using System;
using System.Collections.Generic;
using System.Linq;
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
        public override void Visit(MamlElement element)
        {
            Console.WriteLine("HI");
            Console.WriteLine(element.Element.Attribute("id"));
        }

        public override void Visit(TopicElement element)
        {
            Console.WriteLine("HI");
            Console.WriteLine(element.Element.Attribute("id"));
            MarkdownWriter.Write();
        }

        public override void Visit(SectionElement element)
        {
            // TODO: Handle <title>
            // ...

            // TODO: Handle <content>
            // ...
            // Can  be either <table> or <list>

            foreach (var item in element.Element.Elements())
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

        public override void Visit(IntroductionElement element)
        {
            foreach (var paragraph in element.Element.Elements())
            {
                MarkdownWriter.WriteParagraph(paragraph.Value);
            }
        }

        public override void Visit(RelatedTopicsElement element)
        {
            Console.WriteLine("no");
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
                    case ElementType.Content:
                        var element = new ContentElement(item);
                        element.Accept(this);
                        MarkdownWriter.WriteHeading2(item.Value);
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
            foreach (var item in content.Element.Elements())
            {
                Utilities.ParseEnum(item.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Row:
                        var rowElement = new RowElement(item);
                        rowElement.Accept(this);
                        break;
                    default:
                        break;

                }
            }
        }

        public override void Visit(RowElement row)
        {

            List<string> entryValues = new();
            foreach (var entry in row.Element.Elements())
            {
                Utilities.ParseEnum(entry.Name.LocalName, out ElementType type);

                switch (type)
                {
                    case ElementType.Entry:
                        entryValues.Add(entry.Value);
                        break;
                    default:
                        break;

                }
            }
            MarkdownWriter.AppendTableHeader(entryValues.ToArray());
        }
    }
}
