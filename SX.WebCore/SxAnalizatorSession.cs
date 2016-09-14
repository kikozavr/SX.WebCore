using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_ANALIZATOR_SESSION")]
    public class SxAnalizatorSession : SxDbModel<int>
    {
        [Required, MaxLength(128)]
        public string UserId { get; set; }
        public virtual SxAppUser User { get; set; }

        public virtual ICollection<SxAnalizatorUrl> Urls { get; set; }
    }
}
