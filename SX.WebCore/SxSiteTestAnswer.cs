using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_ANSWER")]
    public class SxSiteTestAnswer : SxDbUpdatedModel<int>
    {
        public virtual SxSiteTestQuestion Question{ get; set; }
        public int QuestionId { get; set; }

        public virtual SxSiteTestSubject Subject { get; set; }
        public int SubjectId { get; set; }

        public int IsCorrect { get; set; }
    }
}
