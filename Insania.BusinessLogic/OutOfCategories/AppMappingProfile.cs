using AutoMapper;

using Insania.Database.Entities.Administrators;
using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Politics;
using Insania.Database.Entities.Sociology;
using Insania.Database.Entities.System;
using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;
using Insania.Models.Heroes.Heroes;
using Insania.Models.Heroes.RequestsHeroesRegistration;
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
        CreateMap<StatusRequestHeroRegistration, BaseResponseListItem>();
        CreateMap<Administrator, BaseResponseListItem>().ForMember(x => x.Name, y => y.MapFrom(z => z.Rank.Name + " " +
            z.Post.Name.ToLower() + " капитула \"" + z.Chapter.Name + "\" " + z.User.FullName));
        CreateMap<Hero, GetHeroResponse>()
            .ForMember(x => x.RaceId, y => y.MapFrom(z => z.Nation.RaceId))
            .ForMember(x => x.CurrentRegionId, y => y.MapFrom(z => z.CurrentLocation.RegionId))
            .ForMember(x => x.CurrentCountryId, y => y.MapFrom(z => z.CurrentLocation.Region.CountryId))
            .ForMember(x => x.FileId, y => y.MapFrom(z => z.FilesHero!.OrderBy(a => a.SequenceNumber).First().FileId));
        CreateMap<BiographyHero, GetBiographiesHeroResponseListItem>();
        CreateMap<BiographyRequestHeroRegistration, GetBiographyRequestHeroRegistrationResponse>();
        CreateMap<Hero, GetHeroesResponseListItem>().ForMember(x => x.Name, y => y.MapFrom(z =>
            (!string.IsNullOrWhiteSpace(z.PersonalName) ? z.PersonalName : "") +
            (z.PrefixName != null && !string.IsNullOrWhiteSpace(z.PrefixName.Name) ? " " + z.PrefixName.Name : "") +
            (!string.IsNullOrWhiteSpace(z.FamilyName) ? " " + z.FamilyName : "")));
    }
}