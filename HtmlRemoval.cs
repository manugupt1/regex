using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexPageProcessor
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Methods to remove HTML from strings.
    /// </summary>
    public static class HtmlRemoval
    {
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _preHtmlRegex = new Regex(".*?>", RegexOptions.Compiled);
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {         
            source = _htmlRegex.Replace(source,string.Empty);
            int temp = source.IndexOf('>');
            if (temp != -1)
            {
                //Console.WriteLine(temp);
                source = source.Remove(0, temp+1);
            }
            temp = source.IndexOf('<');
            if (temp != -1)
            {
                //Console.WriteLine(temp);
                source = source.Remove(temp);
            }

            return source;
        }

    }
}
