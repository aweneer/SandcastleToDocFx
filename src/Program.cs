using SandcastleToDocFx.Sandcastle;
using SandcastleToDocFx.Visitors;
using SandcastleToDocFx.Writers;
using Spectre.Cli;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
        public static string? SourceCodeDirectory;
        public static DirectoryInfo? DocumentationFilesDirectory;
        public static XElement? CurrentConceptualFileRootElement;
        public static FileInfo? CurrentConceptualFile;
        static void Main(string[] args)
        {
            // TODO: Finish commandline app.
            //var app = new CommandApp();
            //app.Run(args);

            // TODO: Process args

            // TODO: Foreach *.aml file in args[1]

            // SandcastleToDocFxExport\conceptual == PostSharp.Documentation\Source
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

            // Replace with command argument.
            var sourceDirectory = "C:\\src\\PostSharp.Documentation\\Source";
            var sourceDirectoryInfo = new DirectoryInfo(sourceDirectory);

            DocumentationFilesDirectory = sourceDirectoryInfo;
            SourceCodeDirectory = "C:\\src\\PostSharp.Documentation\\Samples";
            TransformMamlFilesToMarkdown(visitor, sourceDirectoryInfo, destination);
        }

        public static void TransformMamlFilesToMarkdown(MamlVisitor visitor, DirectoryInfo sourceDirectory, string destinationDirectory)
        {

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            
            var filesToCopy = sourceDirectory.GetFiles("*.png", SearchOption.TopDirectoryOnly);

            // Copy all required files.
            foreach (var file in filesToCopy)
            {
                var targetFilePath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(targetFilePath);
            }

            var filesToTransform = sourceDirectory.GetFiles("*.aml", SearchOption.TopDirectoryOnly);

            foreach (var file in filesToTransform)
            {
                CurrentConceptualFile = file;
                var document = XDocument.Load(file.FullName);
                CurrentConceptualFileRootElement = document.Root;
                Console.WriteLine(file);
                var documentId = CurrentConceptualFileRootElement.Attribute("id");

                MarkdownWriter.StartMarkdownMetadata();
                MarkdownWriter.AppendMetadataUid(documentId.Value);
                //Console.WriteLine(documentId);
                var tocDocument = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\TOC.content");
                var fileTitle = tocDocument
                    .Descendants()
                    .Where(e => e.Attribute("id")?.Value == documentId.Value)
                    .Select(e => e.Attribute("title")?.Value ?? "null")
                    .SingleOrDefault();

                // TODO: Better null/error handling.
                MarkdownWriter.AppendMetadataTitle(fileTitle);
                MarkdownWriter.EndMarkdownMetadata();
                
                if ( !string.IsNullOrEmpty(fileTitle)) {
                    MarkdownWriter.WriteHeading1(fileTitle);
                }

                foreach (var element in document.Root.Elements().FirstOrDefault().Elements())
                {
                    if (!Utilities.ParseEnum(element.Name.LocalName, out ElementType parsedElementName))
                    {
                        throw new NotSupportedException($"Unexpected element type <{element.Name.LocalName}> not supported.");
                    }

                    switch (parsedElementName)
                    {
                        case ElementType.Glossary:
                            // Currently not supported.
                            break;
                        case ElementType.Introduction:
                            var introduction = new IntroductionElement(element);
                            introduction.Accept(visitor);
                            break;
                        case ElementType.Procedure:
                            var procedure = new ProcedureElement(element);
                            procedure.Accept(visitor);
                            break;
                        case ElementType.RelatedTopics:
                            var related = new RelatedTopicsElement(element);
                            related.Accept(visitor);
                            break;
                        case ElementType.Section:
                            var section = new SectionElement(element);
                            section.Accept(visitor);
                            break;
                        case ElementType.Summary:
                            var summary = new ParaElement(element);
                            summary.Accept(visitor);
                            break;
                        default:
                            throw new NotImplementedException($"Parsing of <{element.Name.LocalName}> element is not implemented.");
                    }

                }

                MarkdownWriter.WriteFile(destinationDirectory, documentId.Value);
            }

            foreach (var subdirectory in sourceDirectory.GetDirectories())
            {
                var newDestinationDirectory = Path.Combine(destinationDirectory, subdirectory.Name);
                TransformMamlFilesToMarkdown(visitor, subdirectory, newDestinationDirectory  );
            }
        }
    }
}