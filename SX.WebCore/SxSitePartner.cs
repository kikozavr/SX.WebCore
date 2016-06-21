using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_PARTNER")]
    public class SxSitePartner : SxDbUpdatedModel<int>
    {
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(255), Index]
        public string TitleUrl { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        public Guid? PictureId { get; set; }
        public virtual SxPicture Picture { get; set; }
    }
}
