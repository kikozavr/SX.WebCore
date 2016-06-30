using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_QUESTION")]
    public class SxSiteTestQuestion : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(400), Index]
        public string Text { get; set; }

        public virtual SxSiteTest Test { get; set; }
        public int TestId { get; set; }
    }
}
