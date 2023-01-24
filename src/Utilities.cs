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
    }
}
