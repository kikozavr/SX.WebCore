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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Users/_UsersOnSite.cshtml")]
    public partial class _Areas_Admin_Views_Users__UsersOnSite_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMAppUser[]>
    {
        public _Areas_Admin_Views_Users__UsersOnSite_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
  
    var colCount = 4;
    var rowsCount = (int)Math.Ceiling((decimal)Model.Length / colCount);
    SxVMAppUser item = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 8 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
 if (Model.Any())
{
    for (int r = 0; r < rowsCount; r++)
    {
        var items = Model.Skip(r * colCount).Take(colCount).ToArray();

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

            
            #line 14 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
            
            
            #line default
            #line hidden
            
            #line 14 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
             for (int c = 0; c < colCount; c++)
            {
                if (c < items.Length)
                {
                    item = items[c];

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 511), Tuple.Create("\"", 542)
, Tuple.Create(Tuple.Create("", 519), Tuple.Create("col-md-", 519), true)
            
            #line 19 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
, Tuple.Create(Tuple.Create("", 526), Tuple.Create<System.Object, System.Int32>(12 / colCount
            
            #line default
            #line hidden
, 526), false)
);

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"us-on-site\"");

WriteLiteral(">\r\n                            <ul");

WriteLiteral(" class=\"list-inline\"");

WriteLiteral(">\r\n                                <li>\r\n");

            
            #line 23 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                     if (item.AvatarId.HasValue)
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <img");

WriteLiteral(" class=\"img-circle avatar\"");

WriteAttribute("alt", Tuple.Create(" alt=\"", 863), Tuple.Create("\"", 882)
            
            #line 25 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
, Tuple.Create(Tuple.Create("", 869), Tuple.Create<System.Object, System.Int32>(item.NikName
            
            #line default
            #line hidden
, 869), false)
);

WriteAttribute("src", Tuple.Create(" src=\"", 883), Tuple.Create("\"", 959)
            
            #line 25 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                 , Tuple.Create(Tuple.Create("", 889), Tuple.Create<System.Object, System.Int32>(Url.Action("picture", new { controller="pictures", id=item.AvatarId})
            
            #line default
            #line hidden
, 889), false)
);

WriteLiteral(" />\r\n");

            
            #line 26 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                </li>\r\n                                <li>\r\n    " +
"                                <div><strong>");

            
            #line 29 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                            Write(item.NikName);

            
            #line default
            #line hidden
WriteLiteral("</strong></div>\r\n                                    <div><a");

WriteAttribute("href", Tuple.Create(" href=\"", 1203), Tuple.Create("\"", 1228)
, Tuple.Create(Tuple.Create("", 1210), Tuple.Create("mailto:", 1210), true)
            
            #line 30 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
, Tuple.Create(Tuple.Create("", 1217), Tuple.Create<System.Object, System.Int32>(item.Email
            
            #line default
            #line hidden
, 1217), false)
);

WriteLiteral(">");

            
            #line 30 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                                                 Write(item.Email);

            
            #line default
            #line hidden
WriteLiteral("</a></div>\r\n                                    <div>");

            
            #line 31 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                                    Write(item.RoleNames);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                                </li>\r\n                            </ul>\r" +
"\n\r\n                        </div>\r\n                    </div>\r\n");

            
            #line 37 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1537), Tuple.Create("\"", 1568)
, Tuple.Create(Tuple.Create("", 1545), Tuple.Create("col-md-", 1545), true)
            
            #line 40 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
, Tuple.Create(Tuple.Create("", 1552), Tuple.Create<System.Object, System.Int32>(12 / colCount
            
            #line default
            #line hidden
, 1552), false)
);

WriteLiteral("></div>\r\n");

            
            #line 41 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
                }
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 44 "..\..\Areas\Admin\Views\Users\_UsersOnSite.cshtml"
    }
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
