using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Biology;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Biology.Races;

/// <summary>
/// Сервис работы с расами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class RacesService(ApplicationContext applicationContext, ILogger<RacesService> logger, IMapper mapper) :
    IRaces
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<RacesService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка рас
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponseList> GetRacesList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetRacesListMethod);

        try
        {
            //Получаем данные с базы
            List<Race> races = await _applicationContext.Races.Where(x => x.DateDeleted == null).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = races.Select(_mapper.Map<BaseResponseListItem>).ToList();

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