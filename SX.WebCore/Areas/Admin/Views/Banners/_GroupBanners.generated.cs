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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Admin/Views/Banners/_GroupBanners.cshtml")]
    public partial class _Areas_Admin_Views_Banners__GroupBanners_cshtml : SX.WebCore.MvcWebViewPage.SxWebViewPage<SxVMBanner[]>
    {
        public _Areas_Admin_Views_Banners__GroupBanners_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Admin\Views\Banners\_GroupBanners.cshtml"
  
    var bannerGroupId = (Guid)ViewBag.BannerGroupId;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

            
            #line 7 "..\..\Areas\Admin\Views\Banners\_GroupBanners.cshtml"
Write(Html.SxGridView(Model, new SxExtantions.SxGridViewSettings<SxVMBanner>
{
    Columns = new SxExtantions.SxGridViewColumn<SxVMBanner>[] {
        new SxExtantions.SxGridViewColumn<SxVMBanner>{FieldName="Id", Caption="Идентификатор"},
        new SxExtantions.SxGridViewColumn<SxVMBanner>{FieldName="Url", Caption="Адрес перехода", Template=(b)=> {
            return string.Format("<i class=\"fa fa-link\"></i>&nbsp;<a target=\"_blank\" href=\"{0}\">{0}</a>", b.Url);
        } },
        new SxExtantions.SxGridViewColumn<SxVMBanner>{FieldName="Title", Caption="Заголовок"},
        new SxExtantions.SxGridViewColumn<SxVMBanner>{FieldName="PictureId", Caption="Картинка", Template=(p)=> {
            return string.Format("<a data-lightbox=\"roadtrip\" data-title=\"{1}\" data-toggle=\"tooltip\" title=\"Помотреть картинку\" href=\"/pictures/picture/{0}\"><i class=\"fa fa-picture-o\"></i></a>", p.PictureId, p.Title);
        }}
},
    DataAjaxUrl= Url.Action("GroupBanners", "Banners",new { bgid= bannerGroupId, forgroup= ViewBag.ForGroup }),
    ShowSelectedCheckbox= ViewBag.ForGroup==null?false:true
}));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<br />\r\n<a");

WriteLiteral(" href=\"#info\"");

WriteLiteral(" aria-controls=\"info\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" onclick=\"$(\'a[href=&quot;#info&quot;]\').tab(\'show\'); return false;\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-long-arrow-left\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral("></i>&nbsp;Параметры</a>");

        }
    }
}
#pragma warning restore 1591
