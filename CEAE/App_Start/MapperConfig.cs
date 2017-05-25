using System.Web;
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

                MapModels(cfg);

            });
        }

        private static void MapModels(IProfileExpression cfg)
        {
            cfg.CreateMap<Models.DTO.Question, Models.Question>()
                .ForMember(a => a.Text, a => a.MapFrom(o => o.Image.FileName));
            cfg.CreateMap<Models.Question, Models.DTO.Question>()
                .ForMember(a => a.Image, a => a.Ignore());
        }
    }
}