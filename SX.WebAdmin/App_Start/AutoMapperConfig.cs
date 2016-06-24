using AutoMapper;
using SX.WebCore;
using SX.WebCore.ViewModels;

namespace SX.WebAdmin
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfigurationInstance
        {
            get
            {
                return new MapperConfiguration(cfg =>
                {
                    //app role
                    cfg.CreateMap<SxAppRole, SxVMAppRole>();
                    cfg.CreateMap<SxAppRole, SxVMEditAppRole>();
                    cfg.CreateMap<SxVMEditAppRole, SxAppRole>();

                    //banned url
                    cfg.CreateMap<SxBannedUrl, SxVMBannedUrl>();
                    cfg.CreateMap<SxBannedUrl, SxVMEditBannedUrl>();
                    cfg.CreateMap<SxVMEditBannedUrl, SxBannedUrl>();

                    //banner
                    cfg.CreateMap<SxBanner, SxVMBanner>();
                    cfg.CreateMap<SxVMBanner, SxBanner>();
                    cfg.CreateMap<SxBanner, SxVMEditBanner>();
                    cfg.CreateMap<SxVMEditBanner, SxBanner>();

                    //banner group
                    cfg.CreateMap<SxBannerGroup, SxVMBannerGroup>();
                    cfg.CreateMap<SxBannerGroup, SxVMEditBannerGroup>();
                    cfg.CreateMap<SxVMEditBannerGroup, SxBannerGroup>();

                    //employee
                    cfg.CreateMap<SxEmployee, SxVMEmployee>();
                    cfg.CreateMap<SxEmployee, SxVMEditEmployee>();
                    cfg.CreateMap<SxVMEditEmployee, SxEmployee>();

                    //faq
                    cfg.CreateMap<SxManual, SxVMFAQ>();

                    //picture
                    cfg.CreateMap<SxPicture, SxVMPicture>();
                    cfg.CreateMap<SxVMPicture, SxPicture>();
                    cfg.CreateMap<SxPicture, SxVMEditPicture>();
                    cfg.CreateMap<SxVMEditPicture, SxPicture>();

                    //project step
                    cfg.CreateMap<SxProjectStep, SxVMProjectStep>();
                    cfg.CreateMap<SxProjectStep, SxVMEditProjectStep>();
                    cfg.CreateMap<SxVMEditProjectStep, SxProjectStep>();

                    //redirect
                    cfg.CreateMap<Sx301Redirect, SxVM301Redirect>();
                    cfg.CreateMap<Sx301Redirect, SxVMEdit301Redirect>();
                    cfg.CreateMap<SxVMEdit301Redirect, Sx301Redirect>();

                    //request
                    cfg.CreateMap<SxRequest, SxVMRequest>();

                    //seo keywords
                    cfg.CreateMap<SxSeoKeyword, SxVMSeoKeyword>();
                    cfg.CreateMap<SxSeoKeyword, SxVMEditSeoKeyword>();
                    cfg.CreateMap<SxVMEditSeoKeyword, SxSeoKeyword>();

                    //seo tags
                    cfg.CreateMap<SxSeoTags, SxVMSeoTags>();
                    cfg.CreateMap<SxVMSeoTags, SxSeoTags>();
                    cfg.CreateMap<SxSeoTags, SxVMEditSeoTags> ();
                    cfg.CreateMap<SxVMEditSeoTags, SxSeoTags>();

                    //site test
                    cfg.CreateMap<SxSiteTest, SxVMSiteTest>();
                    cfg.CreateMap<SxSiteTest, SxVMEditSiteTest>();
                    cfg.CreateMap<SxVMEditSiteTest, SxSiteTest>();

                    //site test block
                    cfg.CreateMap<SxSiteTestBlock, SxVMSiteTestBlock>();
                    cfg.CreateMap<SxVMSiteTestBlock, SxSiteTestBlock>();
                    cfg.CreateMap<SxSiteTestBlock, SxVMEditSiteTestBlock>();
                    cfg.CreateMap<SxVMEditSiteTestBlock, SxSiteTestBlock>();

                    //site test question
                    cfg.CreateMap<SxSiteTestQuestion, SxVMSiteTestQuestion>();
                    cfg.CreateMap<SxSiteTestQuestion, SxVMEditSiteTestQuestion>();
                    cfg.CreateMap<SxVMEditSiteTestQuestion, SxSiteTestQuestion>();

                    //statistic user logins
                    cfg.CreateMap<SxStatisticUserLogin, SxVMStatisticUserLogin>()
                    .ForMember(d => d.DateCreate, d => d.MapFrom(s => s.Statistic.DateCreate))
                    .ForMember(d => d.NikName, d => d.MapFrom(s => s.User.NikName))
                    .ForMember(d => d.AvatarId, d => d.MapFrom(s => s.User.AvatarId));

                    //user
                    cfg.CreateMap<SxAppUser, SxVMAppUser>();

                    //video
                    cfg.CreateMap<SxVideo, SxVMVideo>();
                    cfg.CreateMap<SxVideo, SxVMEditVideo>();
                    cfg.CreateMap<SxVMEditVideo, SxVideo>();
                });
            }
        }
    }
}