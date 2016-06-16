using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_VIDEO")]
    public class SxVideo : SxDbUpdatedModel<Guid>
    {
        [Required, MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(20), Required]
        public string VideoId { get; set; }

        [MaxLength(255)]
        public string SourceUrl { get; set; }

        public int ViewsCount { get; set; }
    }
}
