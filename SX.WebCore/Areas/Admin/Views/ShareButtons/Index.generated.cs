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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/ShareButtons/Index.cshtml")]
    public partial class _Areas_Admin_Views_ShareButtons_Index_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMShareButton[]>
    {
        public _Areas_Admin_Views_ShareButtons_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\ShareButtons\Index.cshtml"
  
    ViewBag.Title = "Кнопки шар";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

DefineSection("styles", () => {

WriteLiteral("\r\n    <style>\r\n        #grid-share-buttons tbody .col-first{\r\n            width:0" +
";\r\n            text-align:center;\r\n            vertical-align:middle;\r\n        }" +
"\r\n    </style>\r\n    ");

});

WriteLiteral("\r\n<h2>");

            
            #line 16 "..\..\Areas\Admin\Views\ShareButtons\Index.cshtml"
Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n<br />\r\n<div");

WriteLiteral(" id=\"grid-share-buttons\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 19 "..\..\Areas\Admin\Views\ShareButtons\Index.cshtml"
Write(Html.Partial("_gridview", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n            $(\'#grid-share-buttons\').sx_" +
"gv();\r\n        });\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
