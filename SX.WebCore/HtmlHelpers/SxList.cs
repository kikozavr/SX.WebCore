using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxList<TModel>(this HtmlHelper<SxPagedCollection<TModel>> htmlHelper, SxPagedCollection<TModel> list, SxListSettings settings, object htmlAttributes = null)
        {
            var container = new TagBuilder("div");
            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                container.MergeAttributes(attributes, true);
            }
            container.AddCssClass("sx-list");
            switch(settings.ListMode)
            {
                case SxListMode.Table:
                    container.AddCssClass("t-mode");
                    break;
                case SxListMode.List:
                    container.AddCssClass("l-mode");
                    break;
            }


            var modelsCount = list.Length;
            var colCount = settings.ColCount;
            var rowsCount = (int)Math.Ceiling((decimal)modelsCount / colCount);
            for (int i = 0; i < rowsCount; i++)
            {
                var rowInidex = i + 1;
                var row = new TagBuilder("div");
                row.AddCssClass("row");
                var rowModels = list.Collection.Skip(rowInidex*colCount - colCount).Take(colCount).ToArray();
                for (int y = 0; y < colCount; y++)
                {
                    var cell = new TagBuilder("div");
                    cell.AddCssClass("col-md-" + settings.CssColCount);
                    
                    if (y < rowModels.Length)
                    {
                        var content = new TagBuilder("div");
                        if (!string.IsNullOrEmpty(settings.CellCssStyle))
                            content.AddCssClass(settings.CellCssStyle);

                        content.InnerHtml += htmlHelper.DisplayFor(x => rowModels[y]);
                        cell.InnerHtml += content;
                    }
                    
                    row.InnerHtml += cell;
                }
                container.InnerHtml += row;
            }

            return MvcHtmlString.Create(container.ToString());
        }

        public enum SxListMode : byte
        {
            Unknown = 0,
            List = 1,
            Table = 2
        }

        public class SxListSettings
        {
            private int _colCount = 3;
            public int ColCount 
            { 
                get
                {
                    return _colCount;
                }
                set
                {
                    _colCount = value;
                }
            }

            public int CssColCount 
            { 
                get
                {
                    return 12 / _colCount;
                }
            }

            private SxListMode _listMode = SxListMode.Table;
            public SxListMode ListMode 
            { 
                get
                {
                    return _listMode;
                }
                set
                {
                    _listMode = value;
                }
            }

            public string CellCssStyle { get; set; }
        }
    }
}
