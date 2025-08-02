using System.Web;

namespace Fitessa.Helpers
{
    public static class HtmlHelper
    {
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return HttpUtility.HtmlEncode(input);
        }

        public static string EncodeAndPreserveLineBreaks(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var encoded = HttpUtility.HtmlEncode(input);
            return encoded.Replace("\n", "<br>").Replace("\r", "");
        }

        public static string SanitizeHtml(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var allowedTags = new[] { "b", "i", "u", "strong", "em", "br", "p" };
            var sanitized = input;

            foreach (var tag in allowedTags)
            {
                sanitized = sanitized.Replace($"<{tag}>", $"&lt;{tag}&gt;");
                sanitized = sanitized.Replace($"</{tag}>", $"&lt;/{tag}&gt;");
            }

            return HttpUtility.HtmlEncode(sanitized);
        }
    }
} 