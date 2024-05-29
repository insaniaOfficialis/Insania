using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Politics;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Politics.Countries;

/// <summary>
/// Сервис работы со странами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="mapper">Интерфейс сервиса преобразования моделей</param>
public class CountriesService(ApplicationContext applicationContext, ILogger<CountriesService> logger, IMapper mapper) :
    ICountries
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<CountriesService> _logger = logger;

    /// <summary>
    /// Интерфейс сервиса преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка стран
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponseList> GetCountriesList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetCountriesListMethod);

        try
        {
            //Получаем данные с базы
            List<Country> countries = await _applicationContext.Countries.Include(x => x.Organization).Where(x => x.DateDeleted == null).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = countries.Select(_mapper.Map<BaseResponseListItem>).ToList();

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