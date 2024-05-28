using AutoMapper;

using Insania.Database.Entities.Biology;
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
    }
}