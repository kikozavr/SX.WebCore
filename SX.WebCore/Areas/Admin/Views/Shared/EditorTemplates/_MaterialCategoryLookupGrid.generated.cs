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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Shared/EditorTemplates/_MaterialCategoryLookupGrid.cshtml")]
    public partial class _Areas_Admin_Views_Shared_EditorTemplates__MaterialCategoryLookupGrid_cshtml_ : SX.WebCore.MvcWebViewPage.SxWebViewPage<string>
    {
        public _Areas_Admin_Views_Shared_EditorTemplates__MaterialCategoryLookupGrid_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Shared\EditorTemplates\_MaterialCategoryLookupGrid.cshtml"
  
    var caption = (string)ViewBag.MaterialCategoryTitle;
    SX.WebCore.Enums.ModelCoreType mct = ViewBag.ModelCoreType;

            
            #line default
            #line hidden
WriteLiteral("\n\n<div");

WriteAttribute("id", Tuple.Create(" id=\"", 145), Tuple.Create("\"", 168)
            
            #line 7 "..\..\Areas\Admin\Views\Shared\EditorTemplates\_MaterialCategoryLookupGrid.cshtml"
, Tuple.Create(Tuple.Create("", 150), Tuple.Create<System.Object, System.Int32>(Html.IdForModel()
            
            #line default
            #line hidden
, 150), false)
);

WriteLiteral(">\n    <input");

WriteLiteral(" type=\"hidden\"");

WriteAttribute("value", Tuple.Create(" value=\"", 195), Tuple.Create("\"", 224)
            
            #line 8 "..\..\Areas\Admin\Views\Shared\EditorTemplates\_MaterialCategoryLookupGrid.cshtml"
, Tuple.Create(Tuple.Create("", 203), Tuple.Create<System.Object, System.Int32>(Html.ValueForModel()
            
            #line default
            #line hidden
, 203), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 225), Tuple.Create("\"", 250)
            
            #line 8 "..\..\Areas\Admin\Views\Shared\EditorTemplates\_MaterialCategoryLookupGrid.cshtml"
, Tuple.Create(Tuple.Create("", 232), Tuple.Create<System.Object, System.Int32>(Html.IdForModel()
            
            #line default
            #line hidden
, 232), false)
);

WriteLiteral(" />\n\n");

WriteLiteral("    ");

            
            #line 10 "..\..\Areas\Admin\Views\Shared\EditorTemplates\_MaterialCategoryLookupGrid.cshtml"
Write(Html.SxGridLookup(new SxExtantions.SxGridLookupSettings
{
    DataAjaxUrl = Url.Action("FindTreeView", "MaterialCategories", new { mct=mct }),
    DefaulText = caption == null ? Html.ValueForModel().ToString() : caption,
    Placeholder="Выберите категорию материала"
},
    new SxVMMaterialCategory [0]
));

            
            #line default
            #line hidden
WriteLiteral("\n</div>");

        }
    }
}
#pragma warning restore 1591
