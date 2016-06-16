using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_FORUM_PART")]
    public class SxForumPart : SxDbUpdatedModel<int>, ISxHasHtml
    {
        [MaxLength(255), Required]
        public string Title { get; set; }

        [Required]
        public string Html { get; set; }

        public virtual ICollection<SxForumTheme> Themes { get; set; }
    }
}
