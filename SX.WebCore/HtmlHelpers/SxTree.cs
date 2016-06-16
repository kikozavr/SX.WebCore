using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public class SxTreeSettings<TModel>
        {
            public Func<TModel, TModel[]> FuncChildren { get; set; }
            public Func<TModel, string> FuncContent { get; set; }
        }

        public static MvcHtmlString SxTree<TModel>(this HtmlHelper htmlHelper, TModel[] data, SxTreeSettings<TModel> settings, object htmlAttributes = null)
        {
            var ul = new TagBuilder("ul");
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                ul.MergeAttributes(attributes, true);
            }

            for (int i = 0; i < data.Length; i++)
            {
                var node = data[i];
                var li = new TagBuilder("li");
                li.InnerHtml += settings.FuncContent != null ? settings.FuncContent(node) : null;
                li.InnerHtml+= getNodes(node, settings);
                ul.InnerHtml += li;
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        private static TagBuilder getNodes<TModel>(TModel node, SxTreeSettings<TModel> settings)
        {
            var children = settings.FuncChildren != null ? settings.FuncChildren(node) : new TModel[0];
            if (!children.Any()) return null;

            var ul = new TagBuilder("ul");
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                var li = new TagBuilder("li");
                li.InnerHtml += settings.FuncContent != null ? settings.FuncContent(child) : null;
                li.InnerHtml+= getNodes(child, settings);
                ul.InnerHtml += li;
            }
            return ul;
        }
    }
}
