using SandcastleToDocFx.Sandcastle;
using SandcastleToDocFx.Visitors;
using SandcastleToDocFx.Writers;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SandcastleToDocFx
{
    public class Program
    {
        static void Main(string[] args)
        {
            // TODO: Process args

            // TODO: Foreach *.aml file in args[1]

            var document = "";

            var visitor = new MamlVisitor();

            var doc = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Overview.aml");
            var doc2 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Logging\\Console.aml");
            var doc3 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Introduction\\Introduction.aml");

            foreach ( var element in doc3.Root.Elements().FirstOrDefault().Elements() )
            {
                if ( !Utilities.ParseEnum(element.Name.LocalName, out ElementType parsedElementName) )
                {
                    throw new NotSupportedException(element.Name.LocalName.ToString() + " not supported");
                }

                switch (parsedElementName)
                {
                    case ElementType.Introduction:
                        var introduction = new IntroductionElement(element);
                        introduction.Accept(visitor);
                        break;
                    case ElementType.Topic:
                        var topic = new TopicElement(element);
                        topic.Accept(visitor);
                        break;
                    case ElementType.Section:
                        var section = new SectionElement(element);
                        section.Accept(visitor);
                        break;
                    case ElementType.RelatedTopics:
                        var related = new RelatedTopicsElement(element);
                        related.Accept(visitor);
                        break;
                    default:
                        throw new NotImplementedException($"Parsing of element <{element.Name}> as first-level element is not implemented .");
                }

                MarkdownWriter.Write();
                
            }
        }
    }
}