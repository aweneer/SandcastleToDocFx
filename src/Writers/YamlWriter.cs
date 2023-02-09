using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace SandcastleToDocFx.Writers;

public class YamlWriter
{
    public readonly StringBuilder StringBuilder;
    public readonly string YamlIndentation = "  ";
    public readonly string YamlIndentationLong = "    ";
    public DirectoryInfo SourceDirectory { get; set; }

    public DirectoryInfo DestinationDirectory { get; set; }

    public YamlWriter(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
    {
        this.StringBuilder = new();
        this.SourceDirectory = sourceDirectory;
        this.DestinationDirectory = destinationDirectory;
    }

    public void ProcessTopic(XElement topic, string indentation, bool isRootElement)
    {
        var topicId = topic.Attribute("id")?.Value;
        var topicTitle = topic.Attribute("title")?.Value;
        
        if (topicId == null || topicTitle == null)
        {
            throw new XmlException($"The element '<{topic.Name.LocalName}>' is missing both required attributes 'id' and 'title'.");
        }
        
        topicTitle = topicTitle.Contains(':') ? "\"" + topicTitle + "\"" : topicTitle;

        if (isRootElement)
        {
            StringBuilder.AppendLine("items:");
        }

        StringBuilder.AppendLine($"{indentation}- name: {topicTitle}");
        StringBuilder.AppendLine($"{indentation}  topicUid: {(topicId == "postsharp" ? "index" : topicId)}");
        if (topic.HasElements)
        {
            StringBuilder.AppendLine($"{indentation}  items:");
        }

        indentation += YamlIndentationLong;
        foreach (var subTopic in topic.Elements())
        {
            ProcessTopic(subTopic, indentation, false);
        }
    }

    public void CreateRootToc( XElement topic, DirectoryInfo filePath, bool isRoot = false )
    {
        var topicId = topic.Attribute("id")?.Value;
        var topicTitle = topic.Attribute("title")?.Value;

        Console.WriteLine(topicId);

        if (topicId == null || topicTitle == null)
        {
            throw new XmlException($"The element '<{topic.Name.LocalName}>' is missing both required attributes 'id' and 'title'.");
        }

        topicTitle = topicTitle.Contains(':') ? "\"" + topicTitle + "\"" : topicTitle;

        StringBuilder.AppendLine("items:");
        StringBuilder.AppendLine($"- name: {topicTitle}");
        StringBuilder.AppendLine($"  topicUid: {topicId}");

        if (topic.HasElements)
        {
            StringBuilder.AppendLine($"{YamlIndentation}items:");
        }

        string? tocFile = Path.Combine(DestinationDirectory.FullName, "toc.yml");
        foreach (var subtopic in topic.Elements())
        {
            Console.WriteLine(subtopic.Attribute("id").Value);
            var subtopicId = subtopic.Attribute("id")?.Value;
            var subtopicTitle = subtopic.Attribute("title")?.Value;
            StringBuilder.AppendLine($"{YamlIndentationLong}- name: {subtopicTitle}");

            if (TryGetTocDirectoryBasedOnTopicId(filePath.FullName, subtopicId, out var directoryPath))
            {
                StringBuilder.AppendLine($"{YamlIndentationLong}  href: {Path.Combine(directoryPath, "toc.yml").Replace(@"\", "/")}");
            }
            else
            {
                StringBuilder.AppendLine($"{YamlIndentationLong}  topicUid: {subtopicId}");
            }
            
        }
        Console.WriteLine($"Writing '{tocFile}'.");
        WriteYamlFile(tocFile);

        // if (isRoot)
        // {
        //     foreach (var subtopic in topic.Elements())
        //     {
        //         var subtopicId = subtopic.Attribute("id")?.Value;
        //         TryGetTocDirectoryBasedOnTopicId(filePath.FullName, subtopicId, out var directoryPath);
        //         var subtopicDirectory = new DirectoryInfo(Path.Combine(SourceDirectory.FullName, directoryPath));
        //         CreateRootToc(subtopic, subtopicDirectory);
        //     }
        // }
    }

    // TODO: Move to Utilities.
    public bool TryGetTocDirectoryBasedOnTopicId(string location, string topicId, out string directoryPath)
    {
        var directory = Directory.EnumerateDirectories(location, "*").FirstOrDefault( d => d.Contains(topicId, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(directory))
        {
            directoryPath = location;
            return false;
        }

        directoryPath = Path.GetRelativePath(location, directory);
        return true;
    }

    public void WriteYamlFile(string filePath )
    {
        File.WriteAllText(filePath, this.StringBuilder.ToString());
        this.StringBuilder.Clear();
    }
}