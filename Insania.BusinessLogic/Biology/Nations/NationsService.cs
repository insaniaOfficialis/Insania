using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Entities.Context;
using Insania.Database.Entities.Biology;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Biology.Nations;

/// <summary>
/// Сервис работы с нациями
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="mapper">Интерфейс сервиса преобразования моделей</param>
public class NationsService(ApplicationContext applicationContext, ILogger<NationsService> logger, IMapper mapper) :
    INations
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<NationsService> _logger = logger;

    /// <summary>
    /// Интерфейс сервиса преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка наций
    /// </summary>
    /// <param name="raceId">Раса</param>
    /// <returns></returns>
    public async Task<BaseResponseList> GetNationsList(long? raceId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetNationsListMethod);

        try
        {
            //Получаем данные с базы
            List<Nation> nations = await _applicationContext.Nations
                .Where(x => x.DateDeleted == null && (raceId == null || x.RaceId == raceId)).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = nations.Select(_mapper.Map<BaseResponseListItem>).ToList();

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