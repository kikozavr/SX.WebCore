using System.Web.Mvc;
using System;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public class SxGridLookupSettings
        {
            public string DataAjaxUrl { get; set; }
            public bool IsSingleMode { get; set; } = true;
            public string DefaulText { get; set; }
        }

        public static MvcHtmlString SxGridLookup<TModel>(this HtmlHelper htmlHelper, SxGridLookupSettings settings, TModel[] collection=null)
        {
            if (settings == null)
                throw new ArgumentNullException("SxGridLookupSettings");
            if(settings!=null && settings.DataAjaxUrl==null)
                throw new ArgumentNullException("SxGridLookupSettings.DataAjaxUrl");

            var div = new TagBuilder("div");
            div.MergeAttribute("data-is-loaded", "false");
            div.MergeAttribute("data-is-single-mode", settings.IsSingleMode.ToString().ToLower());
            div.MergeAttribute("data-ajax-url", settings.DataAjaxUrl);
            div.AddCssClass("sx-gvl");

            var group = new TagBuilder("div");
            group.AddCssClass("input-group");

            var input = new TagBuilder("input");
            input.AddCssClass("form-control sx-gvl__input");
            input.MergeAttribute("value", settings.DefaulText);
            input.MergeAttribute("type", "text");
            group.InnerHtml += input;

            var btnSpan = new TagBuilder("span");
            btnSpan.AddCssClass("input-group-btn");

            var btn = new TagBuilder("button");
            btn.AddCssClass("btn btn-default sx-gvl__button");
            btn.MergeAttribute("type", "button");
            btn.InnerHtml += "<i class=\"fa fa-spinner fa-spin\" style=\"display: none;\"></i> Выбрать";


            btnSpan.InnerHtml += btn;

            group.InnerHtml += btnSpan;
            div.InnerHtml += group;

            var dropdown = new TagBuilder("div");
            dropdown.AddCssClass("sx-gvl__dropdown");

            var content = new TagBuilder("div");
            content.AddCssClass("sx-gvl__content");
            dropdown.InnerHtml += content;

            div.InnerHtml += dropdown;


            return MvcHtmlString.Create(div.ToString());  
        }
    }
}
