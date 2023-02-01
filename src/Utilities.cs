using SandcastleToDocFx.Sandcastle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
