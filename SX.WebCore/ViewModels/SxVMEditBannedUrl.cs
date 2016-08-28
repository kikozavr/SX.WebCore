using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditBannedUrl
    {
        [Display(Name = "Адрес")]
        public int Id { get; set; }

        [Required, MaxLength(255), Display(Name = "Адрес"), DataType(DataType.Url, ErrorMessage ="Введите валидный url")]
        public string Url { get; set; }

        [Required, MaxLength(255), Display(Name = "Причина бана"), DataType(DataType.MultilineText)]
        public string Couse { get; set; }
    }
}
