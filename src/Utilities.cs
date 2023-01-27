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
    }
}
