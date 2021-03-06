﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore.Abstract
{
    [Table("DV_MATERIAL")]
    public abstract class SxMaterial : SxDbUpdatedModel<int>, ISxMaterial, IHasLikes
    {
        [Index]
        public DateTime DateOfPublication { get; set; }

        [MaxLength(255), Required]
        public string Title { get; set; }

        [MaxLength(255), Required, Index]
        public string TitleUrl { get; set; }

        public string Html { get; set; }

        [MaxLength(400)]
        public string Foreword { get; set; }

        public Enums.ModelCoreType ModelCoreType { get; set; }

        public bool Show { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        public bool ShowFrontPictureOnDetailPage { get; set; }

        public int ViewsCount { get; set; }

        public virtual SxAppUser User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public virtual SxSeoTags SeoTags { get; set; }
        public int? SeoTagsId { get; set; }

        public virtual SxMaterialCategory Category { get; set; }
        public string CategoryId { get; set; }

        public virtual ICollection<SxVideoLink> VideoLinks { get; set; }
        public virtual ICollection<SxPictureLink> PictureLinks { get; set; }

        public bool IsTop { get; set; }

        [MaxLength(255)]
        public string SourceUrl { get; set; }

        public int LikeUpCount { get; set; }

        public int LikeDownCount { get; set; }
    }
}
