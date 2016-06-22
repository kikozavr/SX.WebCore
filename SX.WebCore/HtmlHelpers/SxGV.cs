using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public sealed class SxGVSettings<TModel>
        {
            public SxGVSettings()
            {
                Columns = new SxGvColumn<TModel>[0];
                PagerInfo = new SxPagerInfo(1,10);
            }

            public string GridId { get; set; }
            public SxGvColumn<TModel>[] Columns { get; set; }
            public bool HasRow { get; set; }
            public SxPagerInfo PagerInfo { get; set; }
            public TModel FilterModel { get; set; }
            public Func<TModel, string> RowCssClass { get; set; }
            public bool ShowFilterRow { get; set; } = true;
            public bool EnableEditing { get; set; } = false;
            public Func<TModel, string> EditRowUrl { get; set; }
        }

        public sealed class SxGvColumn<TModel>
        {
            private string _caption;

            public string FieldName { get; set; }
            public string Caption
            {
                get
                {
                    return _caption ?? FieldName;
                }
                set
                {
                    _caption = value;
                }
            }

            public Func<TModel, string> Template { get; set; }
        }

        public static MvcHtmlString SxGV<TModel>(this HtmlHelper htmlHelper, TModel[] collection, SxGVSettings<TModel> settings = null, object htmlAttributes = null)
        {
            settings = settings ?? new SxGVSettings<TModel>();
            settings.HasRow = collection.Any();
            if (settings.HasRow)
            {
                var pagerInfo = htmlHelper.ViewBag.PagerInfo;
                if (pagerInfo != null)
                    settings.PagerInfo = (SxPagerInfo)pagerInfo;
                else
                    throw new ArgumentNullException("ViewBag.PagerInfo");
            }
            settings.FilterModel = htmlHelper.ViewBag.FilterModel;

            var guid = Guid.NewGuid().ToString().ToLower();
            settings.GridId = guid;
            var div = new TagBuilder("div");
            div.AddCssClass("sx-gv");
            div.MergeAttribute("id", guid);

            var table = new TagBuilder("table");
            table.AddCssClass("table table-condensed table-bordered table-responsive table-striped");

            table.InnerHtml += getHeader(settings);

            table.InnerHtml += getBody(htmlHelper, collection, settings);

            if(settings.HasRow && settings.PagerInfo.TotalPages>1)
                table.InnerHtml += getFooter(htmlHelper, settings);

            div.InnerHtml += table;

            return MvcHtmlString.Create(div.ToString());
        }

        private static TagBuilder getHeader<TModel>(SxGVSettings<TModel> settings)
        {
            var columns = new List<SxGvColumn<TModel>>();
            var propertyInfoes = typeof(TModel).GetProperties();
            SxGvColumn<TModel> column;
            if (!settings.Columns.Any())
            {
                
                foreach (var propertyInfo in propertyInfoes)
                {
                    columns.Add(new SxGvColumn<TModel>
                    {
                        FieldName = propertyInfo.Name
                    });
                }
            }
            else
            {
                bool existProperty = false;
                for (int i = 0; i < settings.Columns.Length; i++)
                {
                    column = settings.Columns[i];
                    existProperty = propertyInfoes.SingleOrDefault(x => Equals(x.Name, column.FieldName)) != null;
                    if (existProperty)
                        columns.Add(column);
                    else
                        throw new ArgumentNullException(column.FieldName);
                }
            }

            settings.Columns = columns.ToArray();


            var header = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            TagBuilder th;
            
            for (int i = 0; i < settings.Columns.Length+1; i++)
            {
                th = new TagBuilder("th");
                if (i == 0)
                {
                    th.InnerHtml += "#";
                    th.AddCssClass("sx-gv_first-column");
                }
                else
                {
                    column = settings.Columns[i-1];
                    th.InnerHtml += column.Caption;
                }
                tr.InnerHtml += th;
            }
            header.InnerHtml += tr;

            return header;
        }

        private static TagBuilder getBody<TModel>(HtmlHelper htmlHelper, TModel[] collection, SxGVSettings<TModel> settings)
        {
            var tbody = new TagBuilder("tbody");
            if (settings.ShowFilterRow)
                tbody.InnerHtml += getFilterRow(htmlHelper, settings);

            TagBuilder tr;
            SxGvColumn<TModel> column;
            TagBuilder td;
            object value;
            Type type;
            string rowCssClass;
            if (settings.HasRow)
            {
                TModel model;
                for (int i = 0; i < collection.Length; i++)
                {
                    model = collection[i];

                    tr = new TagBuilder("tr");
                    if (settings.RowCssClass != null)
                    {
                        rowCssClass = settings.RowCssClass(model);
                        if (rowCssClass!=null)
                            tr.AddCssClass(rowCssClass);
                    }

                    type = model.GetType();
                    for (int y = 0; y < settings.Columns.Length + 1; y++)
                    {
                        td = new TagBuilder("td");
                        if (y == 0)
                        {
                            if (settings.EnableEditing && settings.EditRowUrl != null)
                            {
                                var a = new TagBuilder("a");
                                a.MergeAttribute("href", settings.EditRowUrl(model));
                                a.MergeAttribute("data-toggle", "tooltip");
                                a.MergeAttribute("title", "Редактировать");

                                var link = new TagBuilder("i");
                                link.AddCssClass("fa fa-pencil");
                                a.InnerHtml += link;
                                td.InnerHtml += a;
                            }
                        }
                        else {
                            column = settings.Columns[y - 1];

                            if (column.Template != null)
                                value = column.Template(model);
                            else
                                value = type.GetProperty(column.FieldName).GetValue(model);
                            td.InnerHtml += value;

                        }
                        tr.InnerHtml += td;
                    }

                    tbody.InnerHtml += tr;
                }
            }
            else
            {
                tr = new TagBuilder("tr");
                td = new TagBuilder("td");
                td.MergeAttribute("colspan", settings.Columns.Length.ToString());
                var span = new TagBuilder("span");
                span.AddCssClass("text-warning");
                span.InnerHtml += "Отсутствуют данные для отображения";
                td.InnerHtml += span;
                tr.InnerHtml += td;
                tbody.InnerHtml += tr;
            }

            return tbody;
        }

        private static TagBuilder getFilterRow<TModel>(HtmlHelper htmlHelper, SxGVSettings<TModel> settings)
        {
            var tr = new TagBuilder("tr");
            tr.AddCssClass("sx-gv__filter-row");
            TagBuilder td;
            TagBuilder input;
            SxGvColumn<TModel> column;
            var filterProperties=new Dictionary<string, string>();
            if (settings.FilterModel != null)
            {
                object propValue;
                foreach (var prop in settings.FilterModel.GetType().GetProperties())
                {
                    propValue = prop.GetValue(settings.FilterModel);
                    if(propValue!=null)
                        filterProperties.Add(prop.Name, propValue.ToString());
                }
            }
            for (int i = 0; i < settings.Columns.Length+1; i++)
            {
                td = new TagBuilder("td");
                if (i == 0)
                {
                    
                }
                else {
                    column = settings.Columns[i-1];
                    input = new TagBuilder("input");
                    input.MergeAttribute("type", "text");
                    input.MergeAttribute("name", column.FieldName);
                    input.MergeAttribute("autocomplete", "off");
                    if (filterProperties.ContainsKey(column.FieldName) && filterProperties[column.FieldName].ToString() != "0")
                        input.MergeAttribute("value", filterProperties[column.FieldName].ToString());
                    td.InnerHtml += input;
                }
                tr.InnerHtml += td;
            }

            return tr;
        }

        private static TagBuilder getFooter<TModel>(HtmlHelper htmlHelper, SxGVSettings<TModel> settings)
        {
            var footer = new TagBuilder("tfoot");
            var tr = new TagBuilder("tr");
            TagBuilder td;

            td = new TagBuilder("td");
            tr.InnerHtml += td;

            td = new TagBuilder("td");
            td.InnerHtml += htmlHelper.SxPager(settings.PagerInfo, htmlAttributes: new { @class= "list-unstyled list-inline sx-gv__pager" }, pageUrl:(x)=>"/valutes?page="+x, isAjax: true);
            td.MergeAttribute("colspan", settings.Columns.Length.ToString());

            tr.InnerHtml += td;
            footer.InnerHtml += tr;

            return footer;
        }
    }
}
