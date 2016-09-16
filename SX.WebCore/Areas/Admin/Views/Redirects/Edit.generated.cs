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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Redirects/Edit.cshtml")]
    public partial class _Areas_Admin_Views_Redirects_Edit_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMRedirect>
    {
        public _Areas_Admin_Views_Redirects_Edit_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
  
    var isNew = Model.Id == Guid.Empty;
    ViewBag.Title = isNew ? "Добавить 301 редирект" : "Редактировать 301 редирект для \"" + Model.OldUrl + "\"";

            
            #line default
            #line hidden
WriteLiteral("\n\n<h2>");

            
            #line 7 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\n");

            
            #line 8 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
 if (!isNew)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"text-right\"");

WriteLiteral(">\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n            <form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 311), Tuple.Create("\"", 355)
            
            #line 12 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 320), Tuple.Create<System.Object, System.Int32>(Url.Action("Delete","Redirects")
            
            #line default
            #line hidden
, 320), false)
);

WriteLiteral(">\n");

WriteLiteral("                ");

            
            #line 13 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
           Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("                ");

            
            #line 14 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
           Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\n                <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-danger\"");

WriteLiteral(" onclick=\"if (!confirm(\'Удалить запись?\')) { return false;}\"");

WriteLiteral(">Удалить редирект</button>\n            </form>\n        </div>\n    </div>\n");

            
            #line 19 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\n<form");

WriteLiteral(" method=\"post\"");

WriteAttribute("action", Tuple.Create(" action=\"", 657), Tuple.Create("\"", 699)
            
            #line 21 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 666), Tuple.Create<System.Object, System.Int32>(Url.Action("Edit","Redirects")
            
            #line default
            #line hidden
, 666), false)
);

WriteLiteral(">\n");

WriteLiteral("    ");

            
            #line 22 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("    ");

            
            #line 23 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
Write(Html.HiddenFor(x => x.Id));

            
            #line default
            #line hidden
WriteLiteral("\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n");

WriteLiteral("        ");

            
            #line 26 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.LabelFor(x => x.OldUrl, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 27 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.EditorFor(x => x.OldUrl, new { htmlAttributes = new { @class = "form-control", @placeholder = "Введите старый адрес" } }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 28 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.ValidationMessageFor(x => x.OldUrl));

            
            #line default
            #line hidden
WriteLiteral("\n    </div>\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n");

WriteLiteral("        ");

            
            #line 32 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.LabelFor(x => x.NewUrl, new { @class = "control-label" }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 33 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.EditorFor(x => x.NewUrl, new { htmlAttributes = new { @class = "form-control", @placeholder = "Введите новый адрес" } }));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 34 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
   Write(Html.ValidationMessageFor(x => x.NewUrl));

            
            #line default
            #line hidden
WriteLiteral("\n    </div>\n\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(">");

            
            #line 38 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
                                                  Write(isNew ? "Добавить" : "Сохранить");

            
            #line default
            #line hidden
WriteLiteral("</button>\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1498), Tuple.Create("\"", 1538)
            
            #line 39 "..\..\Areas\Admin\Views\Redirects\Edit.cshtml"
, Tuple.Create(Tuple.Create("", 1505), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Redirects")
            
            #line default
            #line hidden
, 1505), false)
);

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Назад</a>\n    </div>\n</form>\n\n");

DefineSection("scripts", () => {

WriteLiteral("\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1623), Tuple.Create("\"", 1669)
, Tuple.Create(Tuple.Create("", 1629), Tuple.Create<System.Object, System.Int32>(Href("~/content/dist/js/jquery.validate.min.js")
, 1629), false)
);

WriteLiteral("></script>\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1692), Tuple.Create("\"", 1750)
, Tuple.Create(Tuple.Create("", 1698), Tuple.Create<System.Object, System.Int32>(Href("~/content/dist/js/jquery.validate.unobtrusive.min.js")
, 1698), false)
);

WriteLiteral(@"></script>
    <script>
        jQuery.validator.unobtrusive.adapters.add(
        'notequalto', ['other'], function (options) {
            options.rules['notEqualTo'] = '#' + options.params.other;
            if (options.message) {
                options.messages['notEqualTo'] = options.message;
            }
        });

        jQuery.validator.addMethod('notEqualTo', function (value, element, param) {
            return this.optional(element) || value != $(param).val();
        }, '');
    </script>
");

});

        }
    }
}
#pragma warning restore 1591
