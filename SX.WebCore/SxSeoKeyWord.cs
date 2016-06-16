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
    [Table("D_SEO_KEYWORD")]
    public class SxSeoKeyword : SxDbUpdatedModel<int>
    {
        [MaxLength(50), Required]
        public string Value { get; set; }

        public virtual SxSeoInfo SeoInfo { get; set; }
        public int SeoInfoId { get; set; }
    }
}
