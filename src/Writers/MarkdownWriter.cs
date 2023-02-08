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

        public static void AppendLine(string value = "", bool trim = true, bool requiresIndentation = false)
        {
            var indent = requiresIndentation ? Indentation : null;
            var trimmedValue = trim ? value.Trim() : value;
            StringBuilder.AppendLine($"{indent}{trimmedValue}");
        }

        public static void AppendCode(string code)
        {
            StringBuilder.AppendLine("HERE SHOULD BE CODE, BUT IT IS NOT, BECAUSE IT's NOT WORKING YET");
        }
        public static void Append(char character)
        {
            StringBuilder.Append(character);
        }

        public static void Append(string value, bool trim = true, bool requiresIndentation = false)
        {
            var indent = requiresIndentation ? Indentation : null;
            var trimmedValue = trim ? value.Trim() : value;
            StringBuilder.Append($"{indent}{trimmedValue}");
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

        public static void AppendCodeInline(string value, bool trim = false)
        {
            StringBuilder.Append($"`{value.Trim()}`");
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

        public static void AppendAlert(string? alertType = null, bool requiresIndentation = false)
        {
            var indent = requiresIndentation ? Indentation : null;
            StringBuilder.AppendLine($"{indent}> [!{alertType?.ToUpperInvariant()}]");
            StringBuilder.Append($"{indent}> ");
        }

        public static void StartUnorderedListItem(bool requiresIndentation = false)
        {
            var indent = requiresIndentation ? Indentation : null;
            StringBuilder.Append($"{indent}* ");
        }
        public static void StartOrderedListItem(int position)
        {
            StringBuilder.Append($"{position}. ");
        }

        public static void WriteCsodeFromSourceFile(string sourceCodeFilePath, string language, bool indent)
        {
            var code = Utilities.ReadCodeFromSourceFile(sourceCodeFilePath);
            
            StringBuilder.AppendLine(@$"```{language}
{code}
```");
        }

        public static void WriteCsodeFromText(string sourceCodeText, string language, bool indent)
        {

            StringBuilder.AppendLine(@$"```{language}
{sourceCodeText.Trim()}
```");
        }

        public static void WriteCodeFromSourceFile(string sourceCodeFilePath, string language, bool shouldIndent)
        {
            var indent = shouldIndent ? Indentation : null;
            var code = Utilities.ReadCodeFromSourceFile(sourceCodeFilePath).TrimStart();

            StringBuilder.AppendLine($"{indent}```{language}");

            using (StringReader reader = new StringReader(code))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    StringBuilder.AppendLine($"{indent}{line}");
                }
            }

            StringBuilder.AppendLine($"{indent}```");
        }

        // TODO: Remove redundancy of this method.
        public static void WriteCodeFromText(string sourceCode, string language, bool shouldIndent)
        {
            string indent = shouldIndent ? "    " : "";
            sourceCode = sourceCode.TrimStart().TrimEnd();

            StringBuilder.AppendLine($"{indent}```{language}");

            using (StringReader reader = new StringReader(sourceCode))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    StringBuilder.AppendLine($"{indent}{line}");
                }
            }

            StringBuilder.AppendLine($"{indent}```");
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
            bool requiresIndentation = false)
        {
            var indent = requiresIndentation ? Indentation : null;
            StringBuilder.AppendLine($"{indent}![{filePath}]({filePath})");
        }

        public static string Indentation { get; set; } = "    ";

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
        
        public static void WriteLink(string value, string link, bool lineBreaks = false)
        {
            if (lineBreaks)
            {
                StringBuilder.AppendLine($"[{value.Trim()}]({link})");
            }
            else
            {
                StringBuilder.Append($"[{value.Trim()}]({link})");
            }
        }
        public static void WriteLinkWithTitle(string value, string link, string title)
        {
            StringBuilder.AppendLine($"[{value}]({link} {title})");
        }

        public static void AppendTableHeader(string[] values, bool requiresIndentation = false)
        {
            if (requiresIndentation)
            {
                Append("", false, requiresIndentation);
            }
            
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

            if (requiresIndentation)
            {
                Append("", false, requiresIndentation);
            }
            
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

        public static void StartTableRow(bool requiresIndentation = false)
        {
            if (requiresIndentation)
            {
                Append("", false, requiresIndentation);
            }
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
