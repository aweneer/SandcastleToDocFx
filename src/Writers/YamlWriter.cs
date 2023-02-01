using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SandcastleToDocFx.Writers;

public class YamlWriter
{
    public readonly StringBuilder StringBuilder;

    public YamlWriter()
    {
        this.StringBuilder = new();
    }

    public void ProcessTopic(XElement topic, string? indentation = null)
    {
        var topicId = topic.Attribute("id")?.Value;
        var topicTitle = topic.Attribute("title")?.Value;

        if (topicId == null || topicTitle == null)
        {
            throw new XmlException($"The element '<{topic.Name.LocalName}>' is missing both required attributes 'id' and 'title'.");
        }

        topicTitle = topicTitle.Contains(':') ? "\"" + topicTitle + "\"" : topicTitle;


        StringBuilder.AppendLine($"{indentation}items:");
        StringBuilder.AppendLine($"{indentation}- name: {topicTitle}");
        StringBuilder.AppendLine($"{indentation}  topicUid: {topicId}");
        indentation += "  ";
        foreach (var subTopic in topic.Elements())
        {
            ProcessTopic(subTopic, indentation);
        }
    }

    public void WriteYamlFile(string filePath )
    {
        File.WriteAllText(filePath, this.StringBuilder.ToString());
    }
}