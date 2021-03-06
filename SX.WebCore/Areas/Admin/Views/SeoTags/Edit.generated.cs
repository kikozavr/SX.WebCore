﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using SX.WebCore.HtmlHelpers;
    using SX.WebCore.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/SeoTags/Edit.cshtml")]
    public partial class _Areas_Admin_Views_SeoTags_Edit_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMSeoTags>
    {
        public _Areas_Admin_Views_SeoTags_Edit_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
  
    var isNew = Model.Id == 0;
    ViewBag.Title = isNew ? "Добавить SEO теги" : "Редактировать SEO теги для адреса\"" + Model.RawUrl + "\"";

            
            #line default
            #line hidden
WriteLiteral("\r\n<h2>");

            
            #line 6 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n<br />\r\n");

            
            #line 8 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
 if (!isNew)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"text-right\"");

WriteLiteral(">\r\n            <form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 316), Tuple.Create("\"", 356)
            
            #line 12 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 325), Tuple.Create<System.Object, System.Int32>(Url.Action("Delete","SeoTags")
            
            #line default
            #line hidden
, 325), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 13 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
           Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 14 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n                <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-danger\"");

WriteLiteral(" onclick=\"if(!confirm(\'Удалить запись?\')) return false;\"");

WriteLiteral(">Удалить</button>\r\n            </form>\r\n        </div>\r\n    </div>\r\n");

            
            #line 19 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 652), Tuple.Create("\"", 690)
            
            #line 20 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 661), Tuple.Create<System.Object, System.Int32>(Url.Action("Edit","SeoTags")
            
            #line default
            #line hidden
, 661), false)
);

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 21 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("    ");

            
            #line 23 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 26 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
   Write(Html.LabelFor(x => x.RawUrl, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <span");

WriteLiteral(" class=\"text-info\"");

WriteLiteral("> (необходимо использовать внутренний адрес)</span>\r\n");

WriteLiteral("        ");

            
            #line 28 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
   Write(Html.EditorFor(x => x.RawUrl, new { htmlAttributes = new { @class = "form-control", @placeholder = "Введите адрес" } }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 29 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
   Write(Html.ValidationMessageFor(x => x.RawUrl));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <br />\r\n");

WriteLiteral("    ");

            
            #line 32 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
Write(Html.Partial("_GeneralEdit"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <br />\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(">");

            
            #line 36 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
                                                  Write(Model.Id == 0 ? "Добавить" : "Сохранить");

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1349), Tuple.Create("\"", 1386)
            
            #line 37 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 1356), Tuple.Create<System.Object, System.Int32>(Url.Action("Index","SeoTags")
            
            #line default
            #line hidden
, 1356), false)
);

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Назад</a>\r\n    </div>\r\n</form>\r\n\r\n<div");

WriteLiteral(" id=\"seo-kw\"");

WriteLiteral(">\r\n");

            
            #line 42 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
    
            
            #line default
            #line hidden
            
            #line 42 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
     if (!isNew)
    {
        
            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
   Write(Html.Action("Index", "SeoKeywords", new { stid = Model.Id }));

            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\Admin\Views\SeoTags\Edit.cshtml"
                                                                     
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1608), Tuple.Create("\"", 1666)
, Tuple.Create(Tuple.Create("", 1614), Tuple.Create<System.Object, System.Int32>(Href("~/Areas/Admin/content/dist/js/jquery.validate.min.js")
, 1614), false)
);

WriteLiteral("></script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1690), Tuple.Create("\"", 1760)
, Tuple.Create(Tuple.Create("", 1696), Tuple.Create<System.Object, System.Int32>(Href("~/Areas/Admin/content/dist/js/jquery.validate.unobtrusive.min.js")
, 1696), false)
);

WriteLiteral("></script>\r\n    <script>\r\n        $(function () {\r\n            $(\'#seo-kw\').sx_gv" +
"();\r\n        });\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
