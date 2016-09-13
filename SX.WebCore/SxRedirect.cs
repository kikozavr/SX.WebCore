using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_REDIRECT")]
    public sealed class SxRedirect : SxDbUpdatedModel<Guid>
    {
        [MaxLength(255), Required, Index]
        public string OldUrl { get; set; }

        [MaxLength(255), Required]
        public string NewUrl { get; set; }
    }
}
