using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SX.WebCore
{
    public static class UrlHelperExtensions
    {
        public static string ContentAbsUrl(this UrlHelper url, string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
               contextUri.Host, contextUri.Port == 80 ? string.Empty : ":" + contextUri.Port);

            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }

        public static string SeoFriendlyUrl(this UrlHelper url, string title)
        {
            if (title == null)
                throw new ArgumentNullException("Входящий параметр должен иметь значение");

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }
        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            switch (s)
            {
                case "а": return "a";
                case "б": return "b";
                case "в": return "v";
                case "г": return "g";
                case "д": return "d";
                case "е": return "e";
                case "ё": return "io";
                case "ж": return "j";
                case "з": return "z";
                case "и": return "i";
                case "й": return "iy";
                case "к": return "k";
                case "л": return "l";
                case "м": return "m";
                case "н": return "n";
                case "о": return "o";
                case "п": return "p";
                case "р": return "r";
                case "с": return "s";
                case "т": return "t";
                case "у": return "u";
                case "ф": return "ph";
                case "х": return "h";
                case "ц": return "c";
                case "ч": return "ch";
                case "ш": return "sh";
                case "щ": return "sch";
                case "ъ": return null;
                case "ы": return "i";
                case "ь": return null;
                case "э": return "e";
                case "ю": return "iu";
                case "я": return "ia";
                default: return null;
            }
        }
    }
}
