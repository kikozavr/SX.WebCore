using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SX.WebCore
{
    [Table("D_REQUEST")]
    public sealed class SxRequest : SxDbModel<Guid>
    {
        [Required, MaxLength(128), Index]
        public string SessionId { get; set; }

        public string UrlRef { get; set; }

        [MaxLength(150)]
        public string Browser { get; set; }

        [Required, MaxLength(150)]
        public string ClientIP { get; set; }

        [Required, MaxLength(150)]
        public string UserAgent { get; set; }

        [Required, MaxLength(20), Index]
        public string RequestType { get; set; }

        [Index, MaxLength(255)]
        public string RawUrl { get; set; }
    }
}
