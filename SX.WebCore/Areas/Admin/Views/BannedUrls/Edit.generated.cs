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
    using SX.WebCore;
    using SX.WebCore.HtmlHelpers;
    using SX.WebCore.ViewModels;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/BannedUrls/Edit.cshtml")]
    public partial class _Areas_Admin_Views_BannedUrls_Edit_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMBannedUrl>
    {
        public _Areas_Admin_Views_BannedUrls_Edit_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
  
    var isNew = Model.Id == 0;
    ViewBag.Title = isNew ? "Добавить адрес в бан" : "Редактировать забаненный адрес \"" + Model.Url + "\"";

            
            #line default
            #line hidden
WriteLiteral("\n\n<h2>");

            
            #line 7 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\n");

            
            #line 8 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
 if (!isNew)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n        <div");

WriteLiteral(" class=\"text-right\"");

WriteLiteral(">\n            <form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 299), Tuple.Create("\"", 342)
            
            #line 12 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 308), Tuple.Create<System.Object, System.Int32>(Url.Action("Delete","BannedUrls")
            
            #line default
            #line hidden
, 308), false)
);

WriteLiteral(">\n");

WriteLiteral("                ");

            
            #line 13 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("                ");

            
            #line 14 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
           Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\n                <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-danger\"");

WriteLiteral(" onclick=\"if (!confirm(\'Удалить запись?\')) { return false;}\"");

WriteLiteral(">Удалить</button>\n            </form>\n        </div>\n    </div>\n");

            
            #line 19 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\n<form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 635), Tuple.Create("\"", 676)
            
            #line 21 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 644), Tuple.Create<System.Object, System.Int32>(Url.Action("Edit","BannedUrls")
            
            #line default
            #line hidden
, 644), false)
);

WriteLiteral(">\n");

WriteLiteral("    ");

            
            #line 22 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("    ");

            
            #line 23 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n");

WriteLiteral("        ");

            
            #line 26 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.LabelFor(x => x.Url));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 27 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.EditorFor(x => x.Url, new { htmlAttributes = new { @class = "form-control", @placeholder = "Введите адрес" } }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 28 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.ValidationMessageFor(x => x.Url));

            
            #line default
            #line hidden
WriteLiteral("\n    </div>\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n");

WriteLiteral("        ");

            
            #line 32 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.LabelFor(x => x.Couse));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 33 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.EditorFor(x => x.Couse, new { htmlAttributes = new { @class = "form-control", @placeholder = "Укажите причину бана" } }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 34 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
   Write(Html.ValidationMessageFor(x => x.Couse));

            
            #line default
            #line hidden
WriteLiteral("\n    </div>\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(">");

            
            #line 38 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
                                                  Write(isNew ? "Добавить" : "Сохранить");

            
            #line default
            #line hidden
WriteLiteral("</button>\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1389), Tuple.Create("\"", 1429)
            
            #line 39 "..\..\Areas\Admin\Views\BannedUrls\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 1396), Tuple.Create<System.Object, System.Int32>(Url.Action("Index","BannedUrls")
            
            #line default
            #line hidden
, 1396), false)
);

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Назад</a>\n    </div>\n</form>\n\n\n");

DefineSection("scripts", () => {

WriteLiteral("\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1515), Tuple.Create("\"", 1573)
, Tuple.Create(Tuple.Create("", 1521), Tuple.Create<System.Object, System.Int32>(Href("~/Areas/Admin/content/dist/js/jquery.validate.min.js")
, 1521), false)
);

WriteLiteral("></script>\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1596), Tuple.Create("\"", 1666)
, Tuple.Create(Tuple.Create("", 1602), Tuple.Create<System.Object, System.Int32>(Href("~/Areas/Admin/content/dist/js/jquery.validate.unobtrusive.min.js")
, 1602), false)
);

WriteLiteral("></script>\n");

});

        }
    }
}
#pragma warning restore 1591
