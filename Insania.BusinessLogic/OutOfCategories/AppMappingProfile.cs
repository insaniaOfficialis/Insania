using AutoMapper;

using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Politics;
using Insania.Database.Entities.Sociology;
using Insania.Database.Entities.System;
using Insania.Models.Heroes.Heroes;
using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.OutOfCategories;

/// <summary>
/// Класс сервис преобразования моделей
/// </summary>
public class AppMappingProfile : Profile
{
    /// <summary>
    /// Конструктор сервис преобразования моделей
    /// </summary>
    public AppMappingProfile()
    {
        CreateMap<Race, BaseResponseListItem>();
        CreateMap<Nation, BaseResponseListItem>();
        CreateMap<Month, BaseResponseListItem>();
        CreateMap<Country, BaseResponseListItem>().ForMember(x => x.Name, y => y.MapFrom(z => z.Organization.Name));
        CreateMap<Region, BaseResponseListItem>();
        CreateMap<Area, BaseResponseListItem>();
        CreateMap<TypeBody, BaseResponseListItem>();
        CreateMap<TypeFace, BaseResponseListItem>();
        CreateMap<HairsColor, BaseResponseListItem>();
        CreateMap<EyesColor, BaseResponseListItem>();
        CreateMap<PrefixName, BaseResponseListItem>();
        CreateMap<Parameter, BaseResponse>();
        CreateMap<RequestHeroRegistration, GetRequestRegistrationHeroResponse>();
    }
}