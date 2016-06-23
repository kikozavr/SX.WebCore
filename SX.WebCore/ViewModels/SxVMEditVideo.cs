using System;
using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEditVideo
    {
        public Guid Id { get; set; }

        [Required, MaxLength(255), Display(Name = "Название видео")]
        public string Title { get; set; }

        [Required, MaxLength(20), Display(Name = "Идентификатор видео")]
        public string VideoId { get; set; }

        [MaxLength(255), DataType(DataType.Url), Display(Name = "Источник видео")]
        public string SourceUrl { get; set; }
    }
}
