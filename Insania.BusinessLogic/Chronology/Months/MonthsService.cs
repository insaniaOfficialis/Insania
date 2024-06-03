using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Chronology;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Chronology.Months;

/// <summary>
/// Сервис работы с месяцами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class MonthsService(ApplicationContext applicationContext, ILogger<MonthsService> logger, IMapper mapper) :
    IMonths
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<MonthsService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка месяцев
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponseList> GetMonthsList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetMonthsListMethod);

        try
        {
            //Получаем данные с базы
            List<Month> months = await _applicationContext.Months.Where(x => x.DateDeleted == null).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = months.Select(_mapper.Map<BaseResponseListItem>).ToList();

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