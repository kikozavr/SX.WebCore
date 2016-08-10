using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditAffiliateLink
    {
        public Guid Id { get; set; }

        [AllowHtml, Display(Name ="Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name ="Стоимость клика")]
        public int ClickCost { get; set; }
    }
}
