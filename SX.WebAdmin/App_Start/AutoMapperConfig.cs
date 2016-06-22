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
                    cfg.CreateMap<SxRequest, SxVMRequest>();
                });
            }
        }
    }
}