using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMManual
    {
        public int Id { get; set; }

        public ModelCoreType ModelCoreType { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string CategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Контент"), DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }
    }
}
