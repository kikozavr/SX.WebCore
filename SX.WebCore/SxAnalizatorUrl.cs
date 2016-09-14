using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_ANALIZATOR_URL")]
    public class SxAnalizatorUrl : SxDbModel<int>
    {
        [Required, MaxLength(255), Index]
        public string Url { get; set; }

        public int AnalizatorSessionId { get; set; }
        public virtual SxAnalizatorSession AnalizatorSession { get; set; }

        public int? StatusCode { get; set; } = 200;
    }
}
