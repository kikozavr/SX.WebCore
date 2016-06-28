using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SEO_KEYWORD")]
    public class SxSeoKeyword : SxDbUpdatedModel<int>
    {
        [MaxLength(50), Required]
        public string Value { get; set; }

        public virtual SxSeoTags SeoTags { get; set; }
        public int SeoTagsId { get; set; }

        internal bool Any()
        {
            throw new NotImplementedException();
        }
    }
}
