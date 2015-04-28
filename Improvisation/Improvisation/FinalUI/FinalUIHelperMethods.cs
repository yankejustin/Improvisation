using System.Linq;
using Improvisation.Library;

namespace Improvisation.FinalUI
{
    internal static class FinalUIHelperMethods
    {
        public static string FileFriendlyString(string s)
        {
            s.NullCheck();
            return s.Split('\\').Last();
        }
    }
}
