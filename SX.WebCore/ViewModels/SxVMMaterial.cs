﻿using System;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace SX.WebCore.ViewModels
{
    public class SxVMMaterial
    {
        public SxVMMaterial()
        {
            Videos = new SxVideo[0];
        }

        public int Id { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        public DateTime DateOfPublication { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateUpdate { get; set; }

        public string Title { get; set; }

        public string TitleUrl { get; set; }

        public string Html { get; set; }

        public string Foreword { get; set; }

        public bool Show { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxVMPicture FrontPicture { get; set; }

        public bool ShowFrontPictureOnDetailPage { get; set; }

        public int ViewsCount { get; set; }

        public virtual SxVMAppUser User { get; set; }
        public string UserId { get; set; }

        public virtual SxVMSeoTags SeoTags { get; set; }
        public int? SeoTagsId { get; set; }

        public virtual SxVMMaterialCategory Category { get; set; }
        public string CategoryId { get; set; }

        public SxVideo[] Videos { get; set; }

        public int LikeUpCount { get; set; }

        public int LikeDownCount { get; set; }

        public virtual string Url(UrlHelper url)
        {
            throw new NotImplementedException("Url не поддерживается в данном контексте");
        }

        public int CommentsCount { get; set; }

        public int Rating { get; set; }
    }
}
