using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestQuestion
    {
        public int Id { get; set; }

        public SxVMSiteTest Test { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Тест"), UIHint("SiteTestsLookupGrid")]
        public int TestId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(500, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Вопрос"), DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime DateCreate { get; set; }
    }
}
