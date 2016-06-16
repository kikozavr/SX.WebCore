using System;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public sealed class SxIndicatorGroupSettings<TModel>
        {
            /// <summary>
            /// Название итема
            /// </summary>
            public Func<TModel, string> FuncGetLabel { get; set; }
        }

        public static MvcHtmlString SxIndicatorGroup<TModel>(this HtmlHelper htmlHelper, TModel[] collection, SxIndicatorGroupSettings<TModel> settings)
        {
            checkIndicatorGroupSettings(settings);
            if (!collection.Any()) return null;

            TModel item;
            TagBuilder li;
            TagBuilder span;
            TagBuilder spanIndicator;
            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-unstyled sx-indicator-list");
            for (int i = 0; i < collection.Length; i++)
            {
                item = collection[i];

                li = new TagBuilder("li");

                spanIndicator = new TagBuilder("div");
                spanIndicator.AddCssClass("sx-indicator");
                spanIndicator.InnerHtml += "<span data-dir=\"true\">Да</span>";
                spanIndicator.InnerHtml += "<span data-dir=\"false\">Нет</span>";
                li.InnerHtml += spanIndicator;

                span = new TagBuilder("span");
                span.AddCssClass("sx-indicator-txt");
                span.InnerHtml += settings.FuncGetLabel(item);
                li.InnerHtml += span;

                ul.InnerHtml += li;
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        private static void checkIndicatorGroupSettings<TModel>(SxIndicatorGroupSettings<TModel> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("Не определены настройки группы");
            if (settings.FuncGetLabel == null)
                throw new ArgumentNullException("Не определена функция определения названия итема группы");
        }
    }
}
