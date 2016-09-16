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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/MaterialCategories/_TreeViewMenu.cshtml")]
    public partial class _Areas_Admin_Views_MaterialCategories__TreeViewMenu_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMMaterialCategory[]>
    {
        public _Areas_Admin_Views_MaterialCategories__TreeViewMenu_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\MaterialCategories\_TreeViewMenu.cshtml"
  
    int maxLevel = ViewBag.MaxTreeViewLevel;
    SX.WebCore.Enums.ModelCoreType mct = ViewBag.ModelCoreType;
    string cur = ViewBag.CurrentCategory ?? ViewContext.ParentActionViewContext.ViewBag.CategoryId;
    Func<SxVMMaterialCategory, string> funcContent = ViewBag.TreeViewMenuFuncContent;

            
            #line default
            #line hidden
WriteLiteral("\n\n");

            
            #line 9 "..\..\Areas\Admin\Views\MaterialCategories\_TreeViewMenu.cshtml"
Write(Html.SxTreeView(
    data: Model,
    settings: new SxExtantions.SxTreeViewTreeViewSettings<SxVMMaterialCategory>
    {
        FuncChildren = x => x.ChildCategories,
        FuncContent = funcContent,
        FuncCreateUrl = () => Url.Action("edit", new { controller= "MaterialCategories", mct=mct }),
        FuncCurLevel = x => x.Level,
        FuncEditUrl = x => Url.Action("edit", new { controller = "MaterialCategories", mct = x.ModelCoreType, id=x.Id }),
        FuncEditSubNodeUrl = x => Url.Action("edit", new { controller = "MaterialCategories", mct = mct, pcid=x.Id }),
        MaxLevel = maxLevel,
        FuncSearchUrl = () => Url.Action("index", new { controller = "MaterialCategories" }),
        UpdateTargetId = "matcat-tv",
        EnableEditing = false,
        EnableFiltering = false,
        FunActiveRow = x => String.Compare(x.Id, cur, true) == 0
    },
    htmlAttributes: new { @class = "sx-tv table table-condensed table-bordered table-responsive" }
    ));

            
            #line default
            #line hidden
WriteLiteral("\n");

        }
    }
}
#pragma warning restore 1591