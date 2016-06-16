using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_QUESTION")]
    public class SxSiteTestQuestion : SxDbUpdatedModel<int>
    {
        public virtual SxSiteTestBlock Block { get; set; }
        public int BlockId { get; set; }

        [Required, MaxLength(400)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
