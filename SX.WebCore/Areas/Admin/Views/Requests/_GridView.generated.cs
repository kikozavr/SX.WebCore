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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Requests/_GridView.cshtml")]
    public partial class _Areas_Admin_Views_Requests__GridView_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMRequest[]>
    {
        public _Areas_Admin_Views_Requests__GridView_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Admin\Views\Requests\_GridView.cshtml"
Write(Html.SxGridView(Model, new SxExtantions.SxGridViewSettings<SxVMRequest>
{
    Columns = new SxExtantions.SxGridViewColumn<SxVMRequest>[] {
        new SxExtantions.SxGridViewColumn<SxVMRequest> { FieldName="DateCreate", Caption="Дата"},
        new SxExtantions.SxGridViewColumn<SxVMRequest> { FieldName="SessionId", Caption="Идентификатор сессии"},
        new SxExtantions.SxGridViewColumn<SxVMRequest> { FieldName="Browser", Caption="Браузер"},
        new SxExtantions.SxGridViewColumn<SxVMRequest> { FieldName="ClientIP", Caption="IP клиента"},
        new SxExtantions.SxGridViewColumn<SxVMRequest> { FieldName="UserAgent", Caption="Строка агента", Template=(m)=> {
            return m.UserAgent.Contains("bot") || m.UserAgent.Contains("Yahoo! Slurp") || m.UserAgent.Contains("spider")
                    ? string.Format("<i class=\"fa fa-android text-muted\">&nbsp;</i><span class=\"text-muted\">{0}</span>",m.UserAgent)
                : m.UserAgent;
        } },
        new SxExtantions.SxGridViewColumn<SxVMRequest>{FieldName="RequestType", Caption="Тип запросв"},
        new SxExtantions.SxGridViewColumn<SxVMRequest>{FieldName="UrlRef", Caption="UrlRef",Template=(m)=> {
                return !string.IsNullOrEmpty(m.UrlRef)? string.Format("<i class=\"fa fa-link\"></i>&nbsp;<a href=\"{0}\" target=\"_blank\" rel=\"nofollow\">{1}</a>", m.UrlRef, m.UrlRef.Length>=300?m.UrlRef.Substring(0,200)+"...":m.UrlRef):null;
            }},
        new SxExtantions.SxGridViewColumn<SxVMRequest>{FieldName="RawUrl", Caption="Целевой адрес"}
    },
    DataAjaxUrl = Url.Action("Index", "Requests"),
    ShowPagerInfo = true
}));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591