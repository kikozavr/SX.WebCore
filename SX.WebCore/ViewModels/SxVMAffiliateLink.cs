using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMAffiliateLink
    {
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        [AllowHtml, Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int ViewsCount { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Стоимость клика"), DataType(DataType.Currency)]
        public decimal? ClickCost { get; set; }
    }
}
