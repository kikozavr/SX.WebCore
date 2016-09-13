using SX.WebCore.ViewModels;
using System.Linq;
using System.Xml.Linq;

namespace SX.WebCore.Providers
{
    public class SxSiteMapProvider
    {
        public string GenerateSiteMap(ref SxVMSiteMapUrl[] urls)
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(ns + "urlset", new XAttribute("xmlns", ns.NamespaceName));

            SxVMSiteMapUrl item = null;

            if (urls != null && urls.Any())
            {
                for (int i = 0; i < urls.Length; i++)
                {
                    item = urls[i];

                    if (item.Loc == null) continue;

                    root.Add(new XElement(ns + "url",
                            new XElement(ns + "loc", item.Loc),
                            new XElement(ns + "lastmod", item.LasMod.ToString("yyyy-MM-dd"))
                        ));
                }
            }

            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return xml.ToString();
        }
    }
}
