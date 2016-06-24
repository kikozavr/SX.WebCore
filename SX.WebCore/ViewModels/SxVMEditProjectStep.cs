using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditProjectStep
    {
        public int Id { get; set; }
        public int? ParentStepId { get; set; }

        [Required, MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required, MaxLength(400), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Required, AllowHtml, Display(Name = "Содержание"), DataType(DataType.MultilineText)]
        public string Html { get; set; }

        [Display(Name = "Порядок")]
        public int Order { get; set; }
    }
}
