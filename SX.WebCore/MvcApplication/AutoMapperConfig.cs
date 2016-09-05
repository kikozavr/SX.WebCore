using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using SX.WebCore.ViewModels;
using System;

namespace SX.WebCore.MvcApplication
{
    internal class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfigurationInstance(Action<IMapperConfigurationExpression> customConfigAction=null)
        {
                return new MapperConfiguration(cfg =>
                {
                    //banner
                    cfg.CreateMap<SxBanner, SxVMBanner>();
                    cfg.CreateMap<SxVMBanner, SxBanner>();

                    //banner group
                    cfg.CreateMap<SxBannerGroup, SxVMBannerGroup>();
                    cfg.CreateMap<SxVMBannerGroup, SxBannerGroup>();

                    //banned url
                    cfg.CreateMap<SxBannedUrl, SxVMBannedUrl>();
                    cfg.CreateMap<SxVMBannedUrl, SxBannedUrl>();

                    //comment
                    cfg.CreateMap<SxComment, SxVMComment>();
                    cfg.CreateMap<SxVMComment, SxComment>();

                    //material category
                    cfg.CreateMap<SxMaterialCategory, SxVMMaterialCategory>();
                    cfg.CreateMap<SxVMMaterialCategory, SxMaterialCategory>();

                    //material tag
                    cfg.CreateMap<SxMaterialTag, SxVMMaterialTag>();
                    cfg.CreateMap<SxVMMaterialTag, SxMaterialTag>();

                    //net
                    cfg.CreateMap<SxNet, SxVMNet>();
                    cfg.CreateMap<SxVMNet, SxNet>();

                    //picture
                    cfg.CreateMap<SxPicture, SxVMPicture>();
                    cfg.CreateMap<SxVMPicture, SxPicture>();

                    //redirect
                    cfg.CreateMap<Sx301Redirect, SxVM301Redirect>();
                    cfg.CreateMap<SxVM301Redirect, Sx301Redirect>();

                    //request
                    cfg.CreateMap<SxRequest, SxVMRequest>();

                    //role
                    cfg.CreateMap<SxAppRole, SxVMAppRole>();
                    cfg.CreateMap<SxVMAppRole, SxAppRole>();
                    cfg.CreateMap<IdentityUserRole, SxVMAppRole>();

                    //role
                    cfg.CreateMap<SxShareButton, SxVMShareButton>();
                    cfg.CreateMap<SxVMShareButton, SxShareButton>();

                    //seo keyword
                    cfg.CreateMap<SxSeoKeyword, SxVMSeoKeyword>();
                    cfg.CreateMap<SxVMSeoKeyword, SxSeoKeyword>();

                    //seo tags
                    cfg.CreateMap<SxSeoTags, SxVMSeoTags>();
                    cfg.CreateMap<SxVMSeoTags, SxSeoTags>();

                    //site net
                    cfg.CreateMap<SxSiteNet, SxVMSiteNet>();
                    cfg.CreateMap<SxVMSiteNet, SxSiteNet>();

                    //StatisticUserLogin 
                    cfg.CreateMap<SxStatisticUserLogin, SxVMStatisticUserLogin>()
                        .ForMember(d => d.DateCreate, d => d.MapFrom(s => s.Statistic.DateCreate))
                        .ForMember(d => d.NikName, d => d.MapFrom(s => s.User.NikName))
                        .ForMember(d => d.AvatarId, d => d.MapFrom(s => s.User.AvatarId));

                    //user
                    cfg.CreateMap<SxAppUser, SxVMAppUser>();
                    cfg.CreateMap<SxVMAppUser, SxAppUser>();

                    //video
                    cfg.CreateMap<SxVideo, SxVMVideo>();
                    cfg.CreateMap<SxVMVideo, SxVideo>();

                    if (customConfigAction != null)
                        customConfigAction(cfg);

                });
        }
    }
}
