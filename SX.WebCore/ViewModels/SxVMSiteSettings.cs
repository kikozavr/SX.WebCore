﻿using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteSettings
    {
        [Display(Name = "Наименование сайта"), Required, MaxLength(50)]
        public string SiteName { get; set; }
        [MaxLength(50)]
        public string OldSiteName { get; set; }

        [Display(Name = "Логотип сайта"), Required, MaxLength(255), UIHint("PicturesLookupGrid")]
        public string LogoPath { get; set; }
        [MaxLength(255)]
        public string OldLogoPath { get; set; }

        [Display(Name = "Фон сайта"), MaxLength(255), UIHint("PicturesLookupGrid")]
        public string SiteBgPath { get; set; }
        [MaxLength(255)]
        public string OldSiteBgPath { get; set; }

        [Display(Name = "Иконка сайта"), MaxLength(255), UIHint("PicturesLookupGrid")]
        public string SiteFaveiconPath { get; set; }
        [MaxLength(255)]
        public string OldSiteFaveiconPath { get; set; }

        [Required, Display(Name = "Домен управляемого сайта"), MaxLength(100), DataType(DataType.Url)]
        public string SiteDomain { get; set; }
        [MaxLength(100)]
        public string OldSiteDomain { get; set; }
    }
}
