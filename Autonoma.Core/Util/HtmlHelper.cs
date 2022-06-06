using System;

namespace Autonoma.Core.Util
{
    public class HtmlHelper
    {
        public const string LineBreakTag = "<br/>";
        public const string LineBreakTagUnclosed = "<br>";
        public const string AnchorOpen = "<a ";
        public const string AnchorClose = "</a>";

        public static string ConvertNewLinesToHtmlLineBreaks(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            return content.Replace(Environment.NewLine, LineBreakTag)
                          .Replace(Environment.NewLine, LineBreakTagUnclosed)
                          .Replace("\r\n", LineBreakTag)
                          .Replace("\n", LineBreakTag);
        }
    }
}
