using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;
using Insania.Database.Entities.Heroes;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.OutCategories.Base;

namespace Insania.BusinessLogic.Heroes.StatusesRequestsHeroesRegistration;

/// <summary>
/// Сервис работы со статусами заявок на регистрацию персонажей
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class StatusesRequestsHeroesRegistrationService(ApplicationContext applicationContext, 
    ILogger<StatusesRequestsHeroesRegistrationService> logger, IMapper mapper) : IStatusesRequestsHeroesRegistration
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<StatusesRequestsHeroesRegistrationService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка статусов заявок на регистрацию персонажей
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponseList> GetList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetListStatusesRequestsHeroesRegistrationByIdMethod);

        try
        {
            //Получаем данные с базы
            List<StatusRequestHeroRegistration> rows = await _applicationContext.StatusesRequestsHeroesRegistration.Where(x => x.DateDeleted == null).ToListAsync() ?? throw new InnerException(Errors.EmptyStatusesRequestsHeroesRegistration);

            //Конвертируем ответ
            List<BaseResponseListItem> response = rows.Select(_mapper.Map<BaseResponseListItem>).ToList();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new BaseResponseList(true, response);
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