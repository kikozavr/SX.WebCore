using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditPicture
    {
        public Guid Id { get; set; }

        [Display(Name = "Название"), Required]
        public string Caption { get; set; }

        [Display(Name = "Описание"), Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string ImgFormat { get; set; }
    }
}
