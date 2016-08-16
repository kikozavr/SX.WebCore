using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public class SxShareButtonsSettings
        {
            /// <summary>
            /// URL текущей страницы
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// Заголовок текущей страницы
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Описание текущей страницы
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Изображение текущей страницы
            /// </summary>
            public string Image { get; set; }
        }

        public class SxLikeButtonsSettings
        {
            public Func<int> LikeUpCount { get; set; }

            public Func<int> LikeDownCount { get; set; }

            public Func<string> UrlUp { get; set; }

            public Func<string> UrlDown { get; set; }
        }

        public static MvcHtmlString SxShareButtons(this HtmlHelper htmlHelper, SxShareButton[] buttons, Dictionary<string, SxShareButtonsSettings> settings = null, SxLikeButtonsSettings lbSettings=null )
        {
            if (buttons == null || !buttons.Any()) return null;

            var ul = new TagBuilder("ul");
            ul.AddCssClass("share-buttons");

            SxShareButton button;
            TagBuilder li;
            TagBuilder btn;

            if(lbSettings!=null)
            {
                if (lbSettings.LikeUpCount == null)
                    throw new ArgumentNullException("Не задана функция получения количества положительных лайков");
                if (lbSettings.LikeDownCount == null)
                    throw new ArgumentNullException("Не задана функция получения количества отрицательных лайков");
                if (lbSettings.UrlUp == null)
                    throw new ArgumentNullException("Не задана функция получения Url положительных лайков материала");
                if (lbSettings.UrlDown == null)
                    throw new ArgumentNullException("Не задана функция получения Url отрицательных лайков материала");

                li = new TagBuilder("li");

                li.MergeAttribute("data-url", lbSettings.UrlUp());
                li.MergeAttribute("onclick", "sendLike(this)");
                li.AddCssClass("share-buttons__like-btn");
                btn = new TagBuilder("button");
                btn.AddCssClass("btn btn-sm btn-info");
                btn.InnerHtml += "<span class=\"share-buttons__icon\"><i class=\"fa fa-thumbs-o-up text-success\"></i></span>";
                btn.InnerHtml += "<span class=\"badge share-buttons__counter\">"+ lbSettings.LikeUpCount() + "</span>";
                li.InnerHtml += btn;
                ul.InnerHtml += li;

                li = new TagBuilder("li");
                li.MergeAttribute("data-url", lbSettings.UrlDown());
                li.MergeAttribute("onclick", "sendLike(this)");
                li.AddCssClass("share-buttons__like-btn");
                btn = new TagBuilder("button");
                btn.AddCssClass("btn btn-sm btn-info");
                btn.InnerHtml += "<span class=\"share-buttons__icon\"><i class=\"fa fa-thumbs-o-down text-danger\"></i></span>";
                btn.InnerHtml += "<span class=\"badge share-buttons__counter\">"+ lbSettings.LikeDownCount() + "</span>";
                li.InnerHtml += btn;
                ul.InnerHtml += li;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                button = buttons[i];

                li = new TagBuilder("li");
                li.MergeAttribute("data-type", button.Net.Code);
                if (settings != null && settings.ContainsKey(button.Net.Code))
                {
                    var set = settings[button.Net.Code];
                    if (!string.IsNullOrEmpty(set.Url))
                        li.MergeAttribute("data-url", set.Url);
                    if (!string.IsNullOrEmpty(set.Title))
                        li.MergeAttribute("data-title", set.Title);
                    if (!string.IsNullOrEmpty(set.Text))
                        li.MergeAttribute("data-text", set.Text);
                    if (!string.IsNullOrEmpty(set.Image))
                        li.MergeAttribute("data-image", set.Image);
                }
                li.MergeAttribute("tabindex", i.ToString());
                li.AddCssClass("goodshare");

                btn = new TagBuilder("button");
                btn.MergeAttribute("style", "background-color:" + button.Net.Color);
                btn.AddCssClass("btn btn-sm btn-default");
                btn.InnerHtml += "<span class=\"share-buttons__icon\"><i class=\"" + button.Net.LogoCssClass + "\"></i></span>";
                btn.InnerHtml += "<span class=\"share-buttons__title\">" + button.Net.Name + "</span>";
                if (button.Net.HasCounter && button.ShowCounter)
                    btn.InnerHtml += "<span class=\"badge share-buttons__counter\" data-counter=\"" + button.Net.Code + "\"></span>";
                li.InnerHtml += btn;

                ul.InnerHtml += li;
            }

            return MvcHtmlString.Create(ul.ToString());
        }
    }
}
