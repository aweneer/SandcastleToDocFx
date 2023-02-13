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
using System.Xml.Linq;

namespace SandcastleToDocFx
{
    public class Program
    {
        public static string? SourceCodeDirectory;
        public static DirectoryInfo? DocumentationFilesDirectory;
        public static XElement? CurrentConceptualFileRootElement;
        public static FileInfo? CurrentConceptualFile;
        public static DirectoryInfo? ExportDestinationDirectory;
        static void Main(string[] args)
        {
            // TODO: Finish commandline app.
            //var app = new CommandApp();
            //app.Run(args);
            // TODO: Process args
            // TODO: Foreach *.aml file in args[1]


            // Setup directories.
            var exportDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "SandCastleToDocFx_Export");
            
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }
            
            ExportDestinationDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SandCastleToDocFx_Export",
                "conceptual"));
            
            if (Directory.Exists(ExportDestinationDirectory.FullName))
            {
                Directory.Delete(ExportDestinationDirectory.FullName, true);
            }

            Directory.CreateDirectory(ExportDestinationDirectory.FullName);
            
            
            var destinationDirectoryInfo = new DirectoryInfo(ExportDestinationDirectory.FullName);
            var sourceDirectoryInfo = new DirectoryInfo("C:\\src\\PostSharp.Documentation\\Source");
            DocumentationFilesDirectory = sourceDirectoryInfo;
            SourceCodeDirectory = "C:\\src\\PostSharp.Documentation\\Samples";

            // Create toc.yml
            var tocDocument = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\toc.content");
            
            var yamlWriter = new YamlWriter( sourceDirectoryInfo, destinationDirectoryInfo );

            foreach (var topic in tocDocument.Root.Elements())
            {
                yamlWriter.ProcessTopic(topic, "", true);
                //yamlWriter.CreateRootToc(topic, yamlWriter.SourceDirectory, true);
            }

            yamlWriter.WriteYamlFile(Path.Combine(ExportDestinationDirectory.FullName, "toc.yml"));
            
            
            // Start processing .aml files with visitor.
            var visitor = new MamlVisitor();
            TransformMamlFilesToMarkdown(visitor, sourceDirectoryInfo, ExportDestinationDirectory.FullName);
            Console.WriteLine($"Exporting .aml files to .md completed in '{ExportDestinationDirectory.FullName}'.");

            var shfbProjectFile = Path.Combine(DocumentationFilesDirectory.FullName, "PostSharp.shfbproj");
            Console.WriteLine($"Exporting .shfbproj file '{shfbProjectFile}' to .md.");
            TransformSandcastleHelpFileBuilderProject(shfbProjectFile);
            
        }

        public static void TransformSandcastleHelpFileBuilderProject(string projectFile)
        {
            var document = XDocument.Load(projectFile);
            var rootNamespaceTitle = document.Descendants().FirstOrDefault( e => e.Name.LocalName == "RootNamespaceTitle")?.Value;
            var projectSummary = document.Descendants().FirstOrDefault( e => e.Name.LocalName == "ProjectSummary")?.Value;

            var namespaceSummariesElement = document.Descendants().FirstOrDefault( e => e.Name.LocalName == "NamespaceSummaries");

            MarkdownWriter.StartMarkdownMetadata();
            MarkdownWriter.AppendMetadataUid("Project_PostSharp");
            MarkdownWriter.AppendMetadataTitle(rootNamespaceTitle);
            MarkdownWriter.AppendMetadata("product", "postsharp");
            MarkdownWriter.AppendMetadata("categories", new[] { "PostSharp", "AOP", "PostSharp API" });
            MarkdownWriter.EndMarkdownMetadata();
            
            MarkdownWriter.WriteHeading1(rootNamespaceTitle);
            MarkdownWriter.AppendLine(projectSummary);
            MarkdownWriter.WriteHeading2("Namespaces");
            MarkdownWriter.AppendTableHeader(new[]{"Namespace", "Description"});

            var documentedNamespaces = namespaceSummariesElement.Elements().Where(e => e.Attribute("isDocumented").Value == "True");
            
            foreach (var namespaceSummaryElement in documentedNamespaces)
            {
                var namespaceName = namespaceSummaryElement.Attribute("name").Value;
                MarkdownWriter.StartTableRow();
                MarkdownWriter.WriteXref(namespaceName);
                MarkdownWriter.AddTableRowSeparator();
                MarkdownWriter.Append(namespaceSummaryElement.Value);
                MarkdownWriter.EndTableRow();
            }
            
            MarkdownWriter.WriteFile(ExportDestinationDirectory.FullName, "Project_PostSharp");
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
                var document = XDocument.Load(file.FullName);
                CurrentConceptualFile = file;
                CurrentConceptualFileRootElement = document.Root;
                //Console.WriteLine(file);

                // Markdown Metadata
                var documentId = CurrentConceptualFileRootElement.Attribute("id");
                MarkdownWriter.StartMarkdownMetadata();
                MarkdownWriter.AppendMetadataUid(documentId.Value);
                var tocDocument = XDocument.Load("C:\\src\\PostSharp.Documentation\\Source\\TOC.content");
                var fileTitle = tocDocument
                    .Descendants()
                    .Where(e => e.Attribute("id")?.Value == documentId.Value)
                    .Select(e => e.Attribute("title")?.Value ?? "null")
                    .SingleOrDefault();

                MarkdownWriter.AppendMetadata("title", fileTitle);
                MarkdownWriter.AppendMetadata("product", "postsharp");
                MarkdownWriter.AppendMetadata("categories", new[] { "PostSharp", "AOP", "Metaprogramming" });
                //MarkdownWriter.AppendMetadataTitle(fileTitle);
                MarkdownWriter.EndMarkdownMetadata();
                
                // Add heading for the article based on title from .aml file.
                if ( !string.IsNullOrEmpty(fileTitle)) {
                    MarkdownWriter.WriteHeading1(fileTitle);
                }

                // Process content elements in .aml file.
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

                MarkdownWriter.WriteFile(destinationDirectory, documentId.Value == "postsharp" ? "index" : documentId.Value);
            }

            foreach (var subdirectory in sourceDirectory.GetDirectories())
            {
                var newDestinationDirectory = Path.Combine(destinationDirectory, subdirectory.Name);
                TransformMamlFilesToMarkdown(visitor, subdirectory, newDestinationDirectory  );
            }
        }
    }
}