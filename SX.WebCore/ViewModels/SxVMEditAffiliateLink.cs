using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditAffiliateLink
    {
        public Guid Id { get; set; }

        [MaxLength(255), Required, Index, Display(Name ="Внутренняя ссылка")]
        public string RawUrl { get; set; }

        [AllowHtml, Display(Name ="Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name ="Стоимость клика")]
        public int ViewsCount { get; set; }
    }
}
