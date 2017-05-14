using AutoMapper;
using CEAE.Utils;

namespace CEAE
{
    public static class MapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<Models.DTO.User, Models.User>()
                    .ForMember(a => a.Administrator, a => a.MapFrom(o => Constants.Permissions.User));
            });
        }
    }
}