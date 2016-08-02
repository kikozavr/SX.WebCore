using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST_SUBJECT")]
    public class SxSiteTestSubject : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(400), Index]
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual SxSiteTest Test { get; set; }
        public int TestId { get; set; }

        public virtual SxPicture Picture { get; set; }
        public Guid? PictureId { get; set; }
    }
}
