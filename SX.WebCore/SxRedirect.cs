using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    [Table("D_REDIRECT")]
    public sealed class SxRedirect : SxDbUpdatedModel<Guid>
    {
        [MaxLength(255), Required]
        public string OldUrl { get; set; }

        [MaxLength(255), Required]
        public string NewUrl { get; set; }
    }
}
