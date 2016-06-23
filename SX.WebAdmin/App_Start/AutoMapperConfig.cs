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

                    //employee
                    cfg.CreateMap<SxEmployee, SxVMEmployee>();
                    cfg.CreateMap<SxEmployee, SxVMEditEmployee>();
                    cfg.CreateMap<SxVMEditEmployee, SxEmployee>();

                    //picture
                    cfg.CreateMap<SxPicture, SxVMPicture>();
                    cfg.CreateMap<SxVMPicture, SxPicture>();
                    cfg.CreateMap<SxPicture, SxVMEditPicture>();
                    cfg.CreateMap<SxVMEditPicture, SxPicture>();

                    //request
                    cfg.CreateMap<SxRequest, SxVMRequest>();

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