using LittleKingdom.Resources;
using System.Collections.Generic;
using System.Linq;

namespace ResourceRequestsTests
{
    public static class ResourceRequestsTestsUtilities
    {
        public static string ResourcesToString(Resources resources)
        {
            IDictionary<ResourceType, int> dict = resources.GetAll();
            IEnumerable<string> kvps = dict.Select(p => string.Join(": ", p.Key, p.Value));
            return string.Join(", ", kvps);
        }
    }
}