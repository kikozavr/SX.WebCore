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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Valutes/Index.cshtml")]
    public partial class _Areas_Admin_Views_Valutes_Index_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMValute[]>
    {
        public _Areas_Admin_Views_Valutes_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Valutes\Index.cshtml"
  
    ViewBag.Title = "Текущие курсы валют ЦБ РФ";

            
            #line default
            #line hidden
WriteLiteral("\n\n<h2>");

            
            #line 6 "..\..\Areas\Admin\Views\Valutes\Index.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\n<div");

WriteLiteral(" class=\"alert alert-info\"");

WriteLiteral(">\n    <strong>Для справки:</strong> данные предоставлены с <b><a");

WriteLiteral(" href=\"http://www.cbr.ru/scripts/Root.asp?PrtId=SXML\"");

WriteLiteral(">http://www.cbr.ru/scripts/Root.asp?PrtId=SXML</a></b>.Частота обновления данных " +
"- <b>2 часа</b>\n</div>\n\n<div");

WriteLiteral(" id=\"grid-valutes\"");

WriteLiteral(">\n");

WriteLiteral("    ");

            
            #line 12 "..\..\Areas\Admin\Views\Valutes\Index.cshtml"
Write(Html.Partial("_GridView", Model));

            
            #line default
            #line hidden
WriteLiteral("\n</div>\n\n\n");

DefineSection("scripts", () => {

WriteLiteral("\n    <script>\n        $(function () {\n            $(\'#grid-valutes\').sx_gv();\n   " +
"     });\n    </script>\n");

});

        }
    }
}
#pragma warning restore 1591
