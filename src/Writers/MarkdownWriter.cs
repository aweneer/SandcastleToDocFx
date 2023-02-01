using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SandcastleToDocFx.Writers
{
    public static class MarkdownWriter
    {
        public static StringBuilder StringBuilder = new();
        public static void WriteFile(string destination, string fileName)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var file = Path.Combine(destination, $"{fileName}.md");

            using (StreamWriter writer = File.CreateText(file))
            {
                writer.Write(StringBuilder.ToString());
                StringBuilder.Clear();
                writer.Flush(); // TODO: Get rid of this logic.
            }
        }

        public static void AppendLine(string? value = null)
        {
            StringBuilder.AppendLine(value);
        }

        public static void AppendCode(string code)
        {
            StringBuilder.AppendLine("HERE SHOULD BE CODE, BUT IT IS NOT, BECAUSE IT's NOT WORKING YET");
        }
        public static void Append(char character)
        {
            StringBuilder.Append(character);
        }

        public static void WriteParagraph(string value)
        {
            StringBuilder.Append(value.Trim());
        }

        public static void Append(string value)
        {
            StringBuilder.Append(value.Trim());
        }

        public static void WriteHeading1(string value)
        {
            StringBuilder.AppendLine($"# {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteHeading2(string value)
        {
            StringBuilder.AppendLine($"## {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteHeading3(string value)
        {
            StringBuilder.AppendLine($"### {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteHeading4(string value)
        {
            StringBuilder.AppendLine($"#### {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteHeading5(string value)
        {
            StringBuilder.AppendLine($"##### {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteHeading6(string value)
        {
            StringBuilder.AppendLine($"###### {value.Trim()}{Environment.NewLine}");
        }

        public static void WriteTextBold(string value, bool lineBreak = false)
        {
            StringBuilder.Append($"**{value}**");

            if (lineBreak)
            {
                StringBuilder.AppendLine("\n");
            }
        }

        public static void WriteTextItalic(string value)
        {
            StringBuilder.Append($"*{value}*");
        }

        public static void WriteBlockQuote(string value)
        {
            StringBuilder.AppendLine($"> {value}");
        }

        public static void AppendCodeInline(string value)
        {
            StringBuilder.Append($"`{value}`");
        }

        public static void StartCodeInline()
        {
            StringBuilder.Append("<code>");
        }

        public static void EndCodeInline()
        {
            StringBuilder.Append("</code>");
        }

        public static void WriteOrderedListItem(int position, string value)
        {
            StringBuilder.AppendLine($"{position}. {value}");
        }

        public static void AppendAlert(string? alertType = null)
        {
            StringBuilder.AppendLine($">[!{alertType?.ToUpperInvariant()}]");
            StringBuilder.Append(">");
        }

        public static void StartUnorderedListItem()
        {
            StringBuilder.Append("* ");
        }
        public static void StartOrderedListItem(int position)
        {
            StringBuilder.Append($"{position}. ");
        }

        public static void WriteCodeFromSourceFile(string sourceCodeFilePath, string language)
        {
            var code = File.Exists(sourceCodeFilePath)
                ? File.ReadAllText(sourceCodeFilePath)
                : $"Missing source code at: '{sourceCodeFilePath}'.";
            
            StringBuilder.AppendLine(@$"```{language}
{code}
```");
        }

        public static void WriteCodeFromText(string sourceCodeText, string language)
        {

            StringBuilder.AppendLine(@$"```{language}
{sourceCodeText.Trim()}
```");
        }

        public static void StartMarkdownMetadata()
        {
            WriteHorizontalRule();
        }

        public static void EndMarkdownMetadata()
        {
            // TODO: Better handling.
            if (!StringBuilder.ToString().Contains("---"))
            {
                throw new IOException($"{typeof(MarkdownWriter)} did not call append start of markdown metadata yet.");
            }

            WriteHorizontalRule();
        }

        public static void AppendMetadataUid(string uid)
        {
            StringBuilder.AppendLine($"uid: {uid}");
        }

        public static void AppendMetadataTitle(string title)
        {
            StringBuilder.AppendLine($"title: \"{title}\"");
        }
        
        

        public static void WriteCode(string value)
        {
            StringBuilder.AppendLine($"`{value}`");
        }

        public static void AppendImage(
            string filePath,
            string imageName = "Image")
        {
            StringBuilder.AppendLine($"![{imageName}]({filePath})");
        }

        public static void AppendCodeEntityReference(string codeEntityReference)
        {
            StringBuilder.Append($"<xref:{codeEntityReference}>");
        }


        public static void AppendQuoteInline(string quotedText)
        {
            StringBuilder.Append($"\"{quotedText}\"");
        }
        public static void WriteHorizontalRule()
        {
            StringBuilder.AppendLine("---");
        }

        public static void WriteXref(string value)
        {
            StringBuilder.Append($"<xref:{value}>");
        }
        
        public static void WriteLink(string value, string link)
        {
            StringBuilder.Append($"[{value}]({link})");
        }
        public static void WriteLinkWithTitle(string value, string link, string title)
        {
            StringBuilder.AppendLine($"[{value}]({link} {title})");
        }

        public static void AppendTableHeader(string[] values)
        {
            // Header
            int[] headerValuesLengths = new int[values.Length];
            
            StringBuilder.Append("| ");
            
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                headerValuesLengths[i] = value.Length;
                StringBuilder.Append(value.Trim());
                if ( i + 1 < values.Length )
                {
                    StringBuilder.Append(" | ");
                }

            }

            StringBuilder.Append(" |" + Environment.NewLine);

            // Header separator
            foreach (var length in headerValuesLengths)
            {
                StringBuilder.Append("|-");
                for (int j = 0; j < length; j++)
                {
                    StringBuilder.Append('-');
                }
                StringBuilder.Append('-');
            }

            StringBuilder.AppendLine("|");
        }

        public static void AppendTableRow(string[] values)
        {
            // Header
            int[] headerValuesLengths = new int[values.Length];
            
            StringBuilder.Append("| ");
            
            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                headerValuesLengths[i] = value.Length;
                StringBuilder.Append(value);
                if ( i + 1 < values.Length )
                {
                    StringBuilder.Append(" | ");
                }

            }

            StringBuilder.Append(" |" + Environment.NewLine);
        }

        public static void StartTableRow()
        {
            StringBuilder.Append("| ");
        }
        
        public static void AddTableRowSeparator()
        {
            StringBuilder.Append(" | ");
        }
        
        public static void EndTableRow()
        {
            StringBuilder.AppendLine(" |");
        }

        public static void AppendEntries(string[] entries)
        {
            
        }
    }
}
