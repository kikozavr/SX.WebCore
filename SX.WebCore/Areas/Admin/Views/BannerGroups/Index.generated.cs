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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/BannerGroups/Index.cshtml")]
    public partial class _Areas_Admin_Views_BannerGroups_Index_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMBannerGroup[]>
    {
        public _Areas_Admin_Views_BannerGroups_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\BannerGroups\Index.cshtml"
  
    ViewBag.Title = "Группы баннеров";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>");

            
            #line 6 "..\..\Areas\Admin\Views\BannerGroups\Index.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n<div");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(">\r\n    <b>Внимание!</b> Функционал пока не реализован. Решается вопрос о целесооб" +
"разности внедрения в движок\r\n</div>\r\n<br />\r\n<div");

WriteLiteral(" id=\"grid-banner-groups\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 12 "..\..\Areas\Admin\Views\BannerGroups\Index.cshtml"
Write(Html.Partial("_GridView", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n            $(\'#grid-banner-groups\').sx_" +
"gv();\r\n        });\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
