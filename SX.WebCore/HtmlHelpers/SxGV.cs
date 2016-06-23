﻿using System;
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
                Filter = new SxFilter{
                    PagerInfo=new SxPagerInfo(1,10)
                };
            }

            public string GridId { get; set; }
            public SxGvColumn<TModel>[] Columns { get; set; }
            public bool HasRow { get; set; }
            public SxFilter Filter { get; set; }
            public Func<TModel, string> RowCssClass { get; set; }
            public bool ShowFilterRow { get; set; } = true;
            public bool ShowSelectedCheckbox { get; set; } = false;
            public bool EnableEditing { get; set; } = false;
            public Func<TModel, string> EditRowUrl { get; set; }
            public bool EnableCreating { get; set; } = false;
            public string CreateRowUrl { get; set; }
            public string DataAjaxUrl { get; set; }
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
            if (settings.DataAjaxUrl == null)
                throw new ArgumentNullException("DataAjaxUrl");

            settings = settings ?? new SxGVSettings<TModel>();
            settings.HasRow = collection.Any();
            var filter = (SxFilter)htmlHelper.ViewBag.Filter;
            if (filter != null)
                settings.Filter = filter;
            else
                throw new ArgumentNullException("ViewBag.Filter");

            var guid = Guid.NewGuid().ToString().ToLower();
            settings.GridId = guid;
            var div = new TagBuilder("div");
            div.AddCssClass("sx-gv");
            div.MergeAttribute("id", guid);
            div.MergeAttribute("data-ajax-url", settings.DataAjaxUrl);

            var table = new TagBuilder("table");
            table.AddCssClass("table table-condensed table-bordered table-responsive table-striped");

            table.InnerHtml += getHeader(htmlHelper, settings);

            table.InnerHtml += getBody(htmlHelper, collection, settings);

            if (settings.HasRow && settings.Filter.PagerInfo.TotalPages > 1)
                table.InnerHtml += getFooter(htmlHelper, settings);

            div.InnerHtml += table;

            return MvcHtmlString.Create(div.ToString());
        }

        private static TagBuilder getHeader<TModel>(HtmlHelper htmlHelper, SxGVSettings<TModel> settings)
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

            for (int i = 0; i < settings.Columns.Length + 1; i++)
            {
                th = new TagBuilder("th");
                if (settings.HasRow)
                    th.MergeAttribute("style", "cursor: pointer;");
                if (i == 0)
                {
                    if (settings.EnableCreating && settings.CreateRowUrl != null)
                    {
                        var a = new TagBuilder("a");
                        a.MergeAttribute("data-toggle", "tooltip");
                        a.MergeAttribute("title", "Добавить");
                        a.MergeAttribute("href", settings.CreateRowUrl);
                        var link = new TagBuilder("i");
                        link.AddCssClass("fa fa-plus-circle");
                        a.InnerHtml += link;
                        th.InnerHtml += a;
                    }
                    else if(settings.ShowSelectedCheckbox)
                    {
                        th.InnerHtml += "<input type=\"checkbox\" data-toggle=\"tooltip\" title=\"Выделить все\" class=\"sx-gv__select-all-chbx\" />";
                    }
                    else
                    {
                        th.InnerHtml += "#";
                    }
                    th.AddCssClass("sx-gv_first-column");
                }
                else
                {
                    column = settings.Columns[i - 1];
                    th.InnerHtml += column.Caption;
                    th.MergeAttribute("data-field-name", column.FieldName);
                    if(settings.Filter.Order!=null && settings.Filter.Order.FieldName==column.FieldName)
                        th.InnerHtml += getSortArrow(settings.Filter.Order.Direction);
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
                    tr.AddCssClass("sx-gv__row");
                    if (settings.RowCssClass != null)
                    {
                        rowCssClass = settings.RowCssClass(model);
                        if (rowCssClass != null)
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
                            if(settings.ShowSelectedCheckbox)
                            {
                                var input = new TagBuilder("input");
                                input.MergeAttribute("type", "checkbox");
                                td.InnerHtml += input;
                            }
                        }
                        else
                        {
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
                tr.InnerHtml += td;

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
            var filterProperties = new Dictionary<string, string>();
            if (settings.Filter.WhereExpressionObject != null)
            {
                object propValue;
                string propStringValue;
                foreach (var prop in settings.Filter.WhereExpressionObject.GetType().GetProperties())
                {
                    propValue = prop.GetValue(settings.Filter.WhereExpressionObject);
                    propStringValue = propValue != null ? propValue.ToString() : null;
                    if (
                        propStringValue != null
                        && propStringValue != "0"
                        && propStringValue!= "01.01.0001 0:00:00"
                        && propStringValue!= "00000000-0000-0000-0000-000000000000"
                        && propStringValue!="False"
                        && propStringValue!= "Unknown"
                        )
                        filterProperties.Add(prop.Name, propStringValue);
                }
            }
            for (int i = 0; i < settings.Columns.Length + 1; i++)
            {
                td = new TagBuilder("td");
                if (i == 0)
                {
                    if (filterProperties.Count > 0)
                    {
                        var a = new TagBuilder("a");
                        a.MergeAttribute("href", "javascript:void(0)");
                        a.AddCssClass("sx-gv__clear-btn");
                        var link = new TagBuilder("i");
                        link.AddCssClass("fa fa-repeat");
                        link.MergeAttribute("data-toggle", "tooltip");
                        link.MergeAttribute("title", "Сбросить все");
                        a.InnerHtml += link;
                        td.InnerHtml += a;
                    }
                }
                else
                {
                    column = settings.Columns[i - 1];
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
            if(settings.ShowSelectedCheckbox)
            {
                var a = new TagBuilder("a");
                a.AddCssClass("sx-gv__add-from-chbx-btn");
                a.MergeAttribute("data-toggle", "tooltip");
                a.MergeAttribute("href", "javascript:void(0)");
                a.MergeAttribute("title", "Добавить выбранные");
                a.InnerHtml += "<i class=\"fa fa-plus-circle\" aria-hidden=\"true\"></i>";
                td.InnerHtml += a;
            }
            tr.InnerHtml += td;

            td = new TagBuilder("td");
            td.InnerHtml += htmlHelper.SxPager(settings.Filter.PagerInfo, htmlAttributes: new { @class = "list-unstyled list-inline sx-gv__pager" }, pageUrl: (x) => "/valutes?page=" + x, isAjax: true);
            td.MergeAttribute("colspan", settings.Columns.Length.ToString());

            tr.InnerHtml += td;
            footer.InnerHtml += tr;

            return footer;
        }

        //get sort direction arrow
        private static TagBuilder getSortArrow(SortDirection sortDirection)
        {
            if (Equals(sortDirection, SortDirection.Unknown)) return null;

            var i = new TagBuilder("i");
            var directionCssClass = Equals(sortDirection, SortDirection.Asc) ? "asc" : "desc";
            i.AddCssClass("sx-gv__sort-arow");
            i.MergeAttribute("data-sort-direction", sortDirection.ToString());
            i.AddCssClass("fa fa-sort-" + directionCssClass);
            return i;
        }
    }
}
