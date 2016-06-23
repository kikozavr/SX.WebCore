using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteSettings
    {
        [Display(Name = "Наименование сайта"), Required, MaxLength(50)]
        public string SiteName { get; set; }
        [MaxLength(50)]
        public string OldSiteName { get; set; }

        [Display(Name = "Путь к изображению"), Required, MaxLength(255)]
        public string LogoPath { get; set; }
        [MaxLength(255)]
        public string OldLogoPath { get; set; }

        [Display(Name = "Путь к файлу фона сайта"), MaxLength(255)]
        public string SiteBgPath { get; set; }
        [MaxLength(255)]
        public string OldSiteBgPath { get; set; }

        [Display(Name = "Путь к иконке сайта"), MaxLength(255)]
        public string SiteFaveiconPath { get; set; }
        [MaxLength(255)]
        public string OldSiteFaveiconPath { get; set; }

        [Required, Display(Name = "Домен управляемого сайта"), MaxLength(100), DataType(DataType.Url)]
        public string SiteDomain { get; set; }
        [MaxLength(100)]
        public string OldSiteDomain { get; set; }
    }
}
