using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditSiteTest
    {
        public int Id { get; set; }

        [Required, MaxLength(200), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(255), Display(Name = "Строковый ключ"), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string TitleUrl { get; set; }

        [Required, MaxLength(1000), Display(Name = "Описание"), DataType(DataType.MultilineText), AllowHtml]
        public string Description { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        [Display(Name ="Тип теста")]
        public SxSiteTest.SiteTestType Type { get; set; }

        [Display(Name ="Правила"), AllowHtml, DataType(DataType.MultilineText)]
        public string Rules { get; set; }
    }
}
