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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/BannedUrls/_GridView.cshtml")]
    public partial class _Areas_Admin_Views_BannedUrls__GridView_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMBannedUrl[]>
    {
        public _Areas_Admin_Views_BannedUrls__GridView_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Admin\Views\BannedUrls\_GridView.cshtml"
Write(Html.SxGridView(Model,
        new SxExtantions.SxGridViewSettings<SxVMBannedUrl>
        {
            Columns = new SxExtantions.SxGridViewColumn<SxVMBannedUrl>[]{
                new SxExtantions.SxGridViewColumn<SxVMBannedUrl> { FieldName="DateCreate", Caption="Дата занесения"},
                new SxExtantions.SxGridViewColumn<SxVMBannedUrl>{FieldName="Url", Caption="Адрес", Template=(u)=> {
                    return string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", u.Url);
                } },
                new SxExtantions.SxGridViewColumn<SxVMBannedUrl>{FieldName="Couse", Caption="Причина бана"}
            },
            EnableCreating = true,
            CreateRowUrl = Url.Action("Edit", "BannedUrls"),
            EnableEditing = true,
            EditRowUrl = (x) => { return Url.Action("Edit", "BannedUrls", new { id = x.Id }); },
            DataAjaxUrl = Url.Action("Index", "BannedUrls"),
            ShowPagerInfo = true
        }
    ));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
