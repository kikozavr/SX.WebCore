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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/UserRoles/_GridView.cshtml")]
    public partial class _Areas_Admin_Views_UserRoles__GridView_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMAppRole[]>
    {
        public _Areas_Admin_Views_UserRoles__GridView_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\UserRoles\_GridView.cshtml"
  
    var canRedact = User.IsInRole("admin");

            
            #line default
            #line hidden
WriteLiteral("\n\n");

            
            #line 6 "..\..\Areas\Admin\Views\UserRoles\_GridView.cshtml"
Write(Html.SxGridView(Model, new SxExtantions.SxGridViewSettings<SxVMAppRole>()
{
    Columns = new SxExtantions.SxGridViewColumn<SxVMAppRole>[] {
        new SxExtantions.SxGridViewColumn<SxVMAppRole> { FieldName="Name", Caption="Роль"},
        new SxExtantions.SxGridViewColumn<SxVMAppRole> { FieldName="Description", Caption="Описание"},
    },
    EnableCreating = canRedact,
    CreateRowUrl = Url.Action("Edit", "UserRoles"),
    EnableEditing = canRedact,
    EditRowUrl = (x) => { return Url.Action("Edit", "UserRoles", new { id = x.Id }); },
    DataAjaxUrl = Url.Action("Index", "UserRoles")
}));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
