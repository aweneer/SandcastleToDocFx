﻿using System;
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
        public static async void WriteFile(string fileName)
        {
            var file = Path.Combine("C:\\Users\\JanHlavac\\Desktop\\SandcastleToDocFxExport", $"{fileName}.md");

            using (StreamWriter writer = File.CreateText(file))
            {
                writer.Write(StringBuilder.ToString());
                StringBuilder.Clear();
                writer.Flush(); // TODO: Get rid of this logic.
            }
        }

        public static void WriteLine(string? value = null)
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
            StringBuilder.AppendLine();
            StringBuilder.AppendLine($"# {value}");
        }

        public static void WriteHeading2(string value)
        {
            StringBuilder.AppendLine();
            StringBuilder.AppendLine($"## {value}");
        }

        public static void WriteHeading3(string value)
        {
            StringBuilder.AppendLine();
            StringBuilder.AppendLine($"### {value}");
        }
        public static void WriteTextBold(string value)
        {
            StringBuilder.Append($"**{value}**");
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

        public static void WriteOrderedListItem(int position, string value)
        {
            StringBuilder.AppendLine($"{position}. {value}");
        }

        public static void AppendAlert(string? alertType = null)
        {
            MarkdownWriter.WriteLine();
            StringBuilder.AppendLine($">[!{alertType?.ToUpperInvariant()}]");
            StringBuilder.Append($">");
        }

        public static void StartUnorderedListItem()
        {
            StringBuilder.Append("* ");
        }
        public static void WriteUnorderedListItem(string value)
        {
            StringBuilder.AppendLine($"* {value}");
        }

        public static void WriteCodeFromSourceFile(string file, string language)
        {
            var code = File.ReadAllText(file);
            
            StringBuilder.AppendLine(@$"```{language}
{code}
```");
        }
        
        public static void WriteCode(string value)
        {
            StringBuilder.AppendLine($"`{value}`");
        }

        public static void AppendImage(string imageReference, string? imageName = null)
        {
            // TODO: Fix pathing towards the image, reference is only name of the file without extension.
            imageName = imageName ?? "Image";
            StringBuilder.AppendLine($"![{imageName}]({imageReference})");
        }

        public static void WriteHorizontalRule(string value)
        {
            StringBuilder.AppendLine($"---");
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
                StringBuilder.Append(value);
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
