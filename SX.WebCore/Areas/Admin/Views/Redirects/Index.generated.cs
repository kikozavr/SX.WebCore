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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Redirects/Index.cshtml")]
    public partial class _Areas_Admin_Views_Redirects_Index_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMRedirect[]>
    {
        public _Areas_Admin_Views_Redirects_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Redirects\Index.cshtml"
  
    ViewBag.Title = "301 редирект";

            
            #line default
            #line hidden
WriteLiteral("\n\n<h2>");

            
            #line 6 "..\..\Areas\Admin\Views\Redirects\Index.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\n<div");

WriteLiteral(" class=\"alert alert-warning\"");

WriteLiteral(@">
    <ol>
        <li>
            При добавлении редиректа происходит обновление конечных адресов существующих редиректов на конечный адрес нового редиректа при условии, что конечный адрес обновляемых редиректов равен начальному адресу добавляемого. <b>Внимание!</b> Наличие нескольких страниц, ведущих на один и тот же новый адрес приведет к наличию <b");

WriteLiteral(" class=\"text-danger\"");

WriteLiteral(">дублей</b> страниц\n        </li>\n        <li>\n            При обновлени существу" +
"ющего редиректа происходит обновлении только редактируемой модели редиректа\n    " +
"    </li>\n    </ol>\n</div>\n\n<div");

WriteLiteral(" id=\"grid-redirects\"");

WriteLiteral(">\n");

WriteLiteral("    ");

            
            #line 19 "..\..\Areas\Admin\Views\Redirects\Index.cshtml"
Write(Html.Partial("_GridView", Model));

            
            #line default
            #line hidden
WriteLiteral("\n</div>\n\n");

DefineSection("scripts", () => {

WriteLiteral("\n    <script>\n        $(function () {\n            $(\'#grid-redirects\').sx_gv();\n " +
"       });\n    </script>\n");

});

        }
    }
}
#pragma warning restore 1591