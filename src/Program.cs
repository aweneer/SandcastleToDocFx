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
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace SandcastleToDocFx
{
    public class Program
    {
        public static string SourceCodeDirectory;
        public static string DocumentationFilesDirectory;
        static void Main(string[] args)
        {
            // TODO: Finish commandline app.
            //var app = new CommandApp();
            //app.Run(args);

            // TODO: Process args

            // TODO: Foreach *.aml file in args[1]

            var document = "";
            var destination = "C:\\Users\\JanHlavac\\Desktop\\SandcastleToDocFxExport";
            
            Directory.Delete(destination, true);

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var tocDocument = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\toc.content");
            var yamlWriter = new YamlWriter();

            foreach (var topic in tocDocument.Root.Elements())
            {
                yamlWriter.ProcessTopic(topic);
            }

            yamlWriter.WriteYamlFile(Path.Combine(destination, "toc.yml"));
            
            
            var visitor = new MamlVisitor();

            var amlFilesDirectory = "C:\\src\\PostSharp.Documentation\\Source";
            DocumentationFilesDirectory = amlFilesDirectory;
            var doc = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Overview.aml");
            var doc2 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Logging\\Console.aml");
            var doc3 = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\Introduction\\Introduction.aml");


            SourceCodeDirectory = "C:\\src\\PostSharp.Documentation\\Samples";
            var files = Directory.GetFiles(amlFilesDirectory, "*.aml", SearchOption.AllDirectories);
            files = files.ToArray();
            foreach (var file in files)
            {
                doc3 = XDocument.Load(file);
                Console.WriteLine(file);
                var documentId = doc3.Root.Attribute("id").Value;
                MarkdownWriter.AppendTopicId(documentId);
                
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
                        case ElementType.RelatedTopics:
                            var related = new RelatedTopicsElement(element);
                            related.Accept(visitor);
                            break;
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

                MarkdownWriter.WriteFile(destination, documentId);
            }
        }
    }
}