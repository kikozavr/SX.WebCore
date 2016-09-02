using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMPicture
    {
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Название")]
        public string Caption { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        public string ImgFormat { get; set; }

        public int Size { get; set; }
    }
}
