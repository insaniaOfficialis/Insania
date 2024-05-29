using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Politics;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Politics.Regions;

/// <summary>
/// Сервис работы с регионами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="mapper">Интерфейс сервиса преобразования моделей</param>
public class RegionsService(ApplicationContext applicationContext, ILogger<RegionsService> logger, IMapper mapper) :
    IRegions
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<RegionsService> _logger = logger;

    /// <summary>
    /// Интерфейс сервиса преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка регионов
    /// </summary>
    /// <param name="countryId">Страна</param>
    /// <returns></returns>
    public async Task<BaseResponseList> GetRegionsList(long? countryId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetRegionsListMethod);

        try
        {
            //Получаем данные с базы
            List<Region> regions = await _applicationContext.Regions
                .Where(x => x.DateDeleted == null && (countryId == null || x.CountryId == countryId)).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = regions.Select(_mapper.Map<BaseResponseListItem>).ToList();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new BaseResponseList(true, result);
        }
        catch (InnerException ex)
        {
            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
        catch (Exception ex)
        {
            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
    }
}