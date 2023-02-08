using SandcastleToDocFx.Sandcastle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SandcastleToDocFx
{
    public static class Utilities
    {
        public static bool ParseEnum(string elementName, out ElementType parsedElementName)
        {
            if (!Enum.TryParse(elementName, true, out parsedElementName))
            {
                return false;
            }

            return true;
        }

        public static string NormalizeTextSpaces(string originalText)
        {
            return string.Join(" ", originalText.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).ToList().Select(word => word));
        }

        public static string EscapeSpecialMarkdownCharactersAtStart(string text)
        {
            var trimmedText = text.TrimStart();
            var specialCharacters = new[] {'*', '>', '`'};
            if (specialCharacters.Any(character => character.Equals(trimmedText[0])) && !specialCharacters.Any(character => character.Equals(trimmedText[1])))
            {
                return @$"\{trimmedText}";
            }

            return text;
        }

        public static bool TextStartsWithPunctuation(string text)
        {
            var punctuationSymbols = new[] {',', '.', ';', ':'};
            return punctuationSymbols.Any(symbol => symbol.Equals(text[0]));
        }
        
        public static bool TextStartsWithEnclosingGlyphs(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            var enclosingGlyphs = new[] {')', ']', '>', '}'};
            return enclosingGlyphs.Any(symbol => symbol.Equals(text[0]));
        }
        
        public static bool TextEndsWithEnclosingGlyphs(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            var punctuationSymbols = new[] {'(', '[', '<', '{'};
            return punctuationSymbols.Any(symbol => symbol.Equals(text[^1]));
        }

        public static string NormalizeCodeEntityReference(string originalReference)
        {
            string cleanCodeEntityReference = originalReference.Trim();
            
            var types = new[] {"E:", "F:", "M:", "N:", "P:", "R:", "T:", "Overload:"};
            var type = types.FirstOrDefault(symbol => cleanCodeEntityReference.StartsWith(symbol));

            // Clean reference of type.
            if (!string.IsNullOrEmpty(type))
            {
                cleanCodeEntityReference = cleanCodeEntityReference.Remove(0, type!.Length).Trim();
            }

            // TODO: Verify this is needed.
            // Replace backtick with dash.
            //cleanCodeEntityReference = cleanCodeEntityReference.Replace("`", "-");

            // Remove method argument.
            var bracketsPosition = cleanCodeEntityReference.IndexOf('(');

            // if (bracketsPosition != -1)
            // {
            //     cleanCodeEntityReference = cleanCodeEntityReference.Remove(bracketsPosition);
            // }

            return cleanCodeEntityReference;
        }

        /// <summary>
        /// Replaces value of <paramref name="originalLink"/> XAttribute,
        /// looking for identical reference and replacing it with Markdown header value, normalized for header links.
        /// </summary>
        /// <param name="originalLink"></param>
        /// <returns></returns>
        public static (string Title, string Link) GetTitleAndLinkFromAddressedElement(string originalLink)
        {
            foreach (var element in Program.CurrentConceptualFileRootElement!.Descendants().Where( e => e.Attribute("address") != null))
            {
                var addressAttribute = element.Attribute("address");

                var titleElement = element.Elements().FirstOrDefault(e => e.Name.LocalName == "title");

                if (titleElement != null && originalLink.Contains(addressAttribute!.Value))
                {
                    var titleValue = titleElement.Value.Trim();
                    return (titleValue, titleValue.ToLowerInvariant().Replace(' ', '-'));
                }
            }

            return (originalLink, originalLink);
        }

        public static string CreateNewHeaderLink(string link, string? article = null)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(article))
            {
                sb.Append(article);
            }
            sb.Append('#');
            sb.Append(link);
            return sb.ToString();
        }

        public static string ReplaceLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return language;
            }

            var mamlByMarkdown = new Dictionary<string, string>()
            {
                //{"csharp", "cs"}, // TODO: Remove? csharp itself should be correct.
                {"c#", "csharp"},
                {"xaml", "xml"}
            };

            if (!mamlByMarkdown.TryGetValue(language, out var newLanguage))
            {
                return language;
            }

            return newLanguage;
        }

        public static string GetRelativePathOfReferencedImage( string imageReference, string extension )
        {
            var imageFile = imageReference + "." + extension;

            // TODO: There could be multiple files with the same name under different directories.
            var imageFilePath = Directory.GetFiles(
                    Program.DocumentationFilesDirectory.FullName,
                    imageFile,
                    SearchOption.AllDirectories)
                .FirstOrDefault();

            if (imageFilePath == null)
            {
                throw new NotSupportedException($"No image file '{imageFile}' was found in {Program.DocumentationFilesDirectory}.");
            }

            imageFilePath = Path.GetRelativePath(Program.CurrentConceptualFile.Directory.FullName, imageFilePath);
            imageFilePath = imageFilePath.Replace(@"\", "/");

            return imageFilePath;
        }

        
        public static string ReadCodeFromSourceFile(string sourceCodeFilePath)
        {
            return File.Exists(sourceCodeFilePath)
                ? File.ReadAllText(sourceCodeFilePath)
                : $"Missing source code at: '{sourceCodeFilePath}'.";
        }
    }
}
