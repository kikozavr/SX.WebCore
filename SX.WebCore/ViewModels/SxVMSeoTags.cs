using System.Linq;
using System.Text;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSeoTags
    {
        public SxVMSeoTags()
        {
            Keywords = new SxVMSeoKeyword[0];
        }

        public int Id { get; set; }
        public string RawUrl { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string H1 { get; set; }
        public string H1CssClass { get; set; }
        public SxVMSeoKeyword[] Keywords { get; set; }
        public string KeywordsString
        {
            get
            {
                if (!Keywords.Any()) return null;

                var sb = new StringBuilder();
                for (int i = 0; i < Keywords.Length; i++)
                {
                    sb.AppendFormat(", {0}", Keywords[i].Value);
                    sb.Remove(0, 2);
                }
                return sb.ToString();
            }
        }
    }
}
