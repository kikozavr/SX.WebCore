using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    /// <summary>
    /// Этапы развития проекта
    /// </summary>
    [Table("D_PROJECT_STEP")]
    public class SxProjectStep : SxDbUpdatedModel<int>, ISxHasHtml
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(400)]
        public string Foreword { get; set; }

        [Required]
        public string Html { get; set; }

        public virtual SxProjectStep ParentStep { get; set; }
        public int? ParentStepId { get; set; }

        public int Order { get; set; }

        public bool IsDone { get; set; }
    }
}
