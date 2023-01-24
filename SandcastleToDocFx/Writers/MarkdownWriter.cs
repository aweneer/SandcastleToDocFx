using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandcastleToDocFx.Writers
{
    public static class MarkdownWriter
    {
        public static StringBuilder StringBuilder = new();
        public static async void Write()
        {
            using (StreamWriter writer = File.CreateText(@"C:\Users\JanHlavac\Desktop\test.md"))
            {
                writer.Write(StringBuilder.ToString());
                writer.Flush(); // TODO: Get rid of this logic.
            }
        }

        public static void WriteParagraph(string value)
        {
            StringBuilder.AppendLine();
            StringBuilder.AppendLine($"{value.Trim()}");
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
            StringBuilder.AppendLine($"**{value}**");
        }

        public static void WriteTextItalic(string value)
        {
            StringBuilder.AppendLine($"*{value}*");
        }

        public static void WriteBlockQuote(string value)
        {
            StringBuilder.AppendLine($"> {value}");
        }

        public static void WriteOrderedList(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                StringBuilder.AppendLine($"{i+1}. {values[i]}");
            }
        }

        public static void WriteUnorderedList(string[] values)
        {
            foreach (var value in values)
            {
                StringBuilder.AppendLine($"- {value}");
            }
        }

        public static void WriteCode(string value)
        {
            StringBuilder.AppendLine($"`{value}`");
        }

        public static void WriteHorizontalRule(string value)
        {
            StringBuilder.AppendLine($"---");
        }

        public static void WriteLink(string value, string link)
        {
            StringBuilder.AppendLine($"[{value}]({link})");
        }
        public static void WriteLinkWithTitle(string value, string link, string title)
        {
            StringBuilder.AppendLine($"[{value}]({link} {title})");
        }

        public static void AppendTableHeader(string[] values)
        {
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
            StringBuilder.Append(" |"+Environment.NewLine);

            foreach (var length in headerValuesLengths)
            {
                StringBuilder.Append("|-");
                for (int i = 0; i < length; i++)
                {
                    StringBuilder.Append("-");
                }
                StringBuilder.Append("-|");
            }
        }

    }
}
