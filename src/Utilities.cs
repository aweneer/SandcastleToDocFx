using SandcastleToDocFx.Sandcastle;
using System;
using System.Collections.Generic;
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

        public static bool CheckTextForStartsWithPunctuation(string text)
        {
            var punctuationSymbols = new[] {',', '.', ';', ':'};
            return punctuationSymbols.Any(symbol => symbol.Equals(text[0]));
        }

        public static string NormalizeCodeEntityReference(string originalReference)
        {
            string cleanCodeEntityReference = originalReference;
            
            var types = new[] {"E:", "M:", "N:", "P:", "R:", "T:"};
            var hasType = types.Any(symbol => symbol.Contains(cleanCodeEntityReference[0]));
            var type = types.FirstOrDefault(symbol => symbol.Contains(cleanCodeEntityReference[0]));

            // Clean reference of type.
            if (hasType)
            {
                cleanCodeEntityReference = originalReference.Remove(0, type!.Length).Trim();
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
            foreach (var element in Program.RootElement!.Descendants().Where( e => e.Attribute("address") != null))
            {
                var addressAttribute = element.Attribute("address");

                var titleElement = element.Elements().FirstOrDefault(e => e.Name.LocalName == "title");

                if (titleElement != null && originalLink.Contains(addressAttribute!.Value))
                {
                    var titleValue = titleElement.Value;
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
    }
}
