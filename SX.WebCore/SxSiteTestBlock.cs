using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_BLOCK")]
    public class SxSiteTestBlock : SxDbUpdatedModel<int>
    {
        public virtual SxSiteTest Test { get; set; }
        public int TestId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<SxSiteTestQuestion> Questions { get; set; }
    }
}
