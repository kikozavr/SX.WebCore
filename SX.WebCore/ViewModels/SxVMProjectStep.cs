using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMProjectStep
    {
        public SxVMProjectStep()
        {
            Steps = new SxVMProjectStep[0];
        }
        public int Id { get; set; }

        public int? ParentStepId { get; set; }

        public SxVMProjectStep[] Steps { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(400, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [AllowHtml, Display(Name = "Содержание"), DataType(DataType.MultilineText)]
        public string Html { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        [Display(Name = "Порядок")]
        public int Order { get; set; }

        public bool IsDone { get; set; }
    }
}
