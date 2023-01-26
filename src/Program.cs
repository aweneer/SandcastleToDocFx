using SandcastleToDocFx.Sandcastle;
using SandcastleToDocFx.Visitors;
using SandcastleToDocFx.Writers;
using Spectre.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SandcastleToDocFx
{
    public class Program
    {
        public static string SourceCodeDirectory;
        static void Main(string[] args)
        {
            // TODO: Finish commandline app.
            //var app = new CommandApp();
            //app.Run(args);

            // TODO: Process args

            // TODO: Foreach *.aml file in args[1]

            var document = "";

            var visitor = new MamlVisitor();

            var doc = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Overview.aml");
            var doc2 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Logging\\Console.aml");
            var doc3 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Introduction\\Introduction.aml");

            SourceCodeDirectory = "C:\\src\\PostSharp.Documentation\\Samples";
            var files = Directory.GetFiles("C:\\src\\PostSharp.Documentation\\Source", "*.aml", SearchOption.AllDirectories);
            files = files.ToArray();
            foreach (var file in files)
            {
                //Console.WriteLine("===");
                doc3 = XDocument.Load(file);
                Console.WriteLine(file);
                var fileName = doc3.Root.Attribute("id").Value;
                foreach (var element in doc3.Root.Elements().FirstOrDefault().Elements())
                {
                    if (!Utilities.ParseEnum(element.Name.LocalName, out ElementType parsedElementName))
                    {
                        throw new NotSupportedException(element.Name.LocalName.ToString() + " not supported");
                    }

                    switch (parsedElementName)
                    {
                        case ElementType.Glossary:
                            // Not supported.
                            break;
                        case ElementType.Introduction:
                            var introduction = new IntroductionElement(element);
                            introduction.Accept(visitor);
                            break;
                        // case ElementType.RelatedTopics:
                        //     var related = new RelatedTopicsElement(element);
                        //     related.Accept(visitor);
                        //     break;
                        case ElementType.Section:
                            var section = new SectionElement(element);
                            section.Accept(visitor);
                            break;
                        // case ElementType.Summary:
                        //     break;
                        default:
                            //Console.WriteLine(element.Name.LocalName);
                            // throw new NotImplementedException(
                            //     $"Parsing of element <{element.Name}> as first-level element is not implemented .");
                            break;
                    }

                }
                MarkdownWriter.WriteFile(fileName);
            }
        }
    }
}