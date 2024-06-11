using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Heroes;
using Insania.Entities.Context;
using Insania.Models.Heroes.RequestsHeroesRegistration;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Heroes.RequestsHeroesRegistration;

/// <summary>
/// Сервис работы с заявками на регистрацию персонажей
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
/// <param name="parameters">Интерфейс работы с параметрами</param>
/// <param name="userManager">Менеджер пользователей</param>
public class ReuestsHeroRegistrationService(ApplicationContext applicationContext, ILogger<ReuestsHeroRegistrationService> logger, IMapper mapper) : 
    IRequestsHeroesRegistration
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<ReuestsHeroRegistrationService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения заявки на регистрацию персонажа по id
    /// </summary>
    /// <param name="id">Первичный ключ заявки</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetRequestRegistrationHeroResponse> GetById(long? id)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetRequestHeroRegistrationByIdMethod);

        try
        {
            //Проверяем данные
            if (id == null) throw new InnerException(Errors.EmptyRequest);

            //Получаем данные с базы
            RequestHeroRegistration row = await _applicationContext.RequestsHeroesRegistration.FirstAsync(x => x.Id == id) ?? throw new InnerException(Errors.EmptyRequestHeroRegistration);

            //Конвертируем ответ
            GetRequestRegistrationHeroResponse response = _mapper.Map<GetRequestRegistrationHeroResponse>(row);
            response.Success = true;

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return response;
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

    /// <summary>
    /// Метод получения заявки на регистрацию персонажа по персонажу
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetRequestRegistrationHeroResponse">Ответ</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetRequestRegistrationHeroResponse> GetByHero(long? heroId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetRequestHeroRegistrationByHeroMethod);

        try
        {
            //Проверяем данные
            if (heroId == null) throw new InnerException(Errors.EmptyRequest);

            //Получаем данные с базы
            RequestHeroRegistration row = await _applicationContext
                .RequestsHeroesRegistration
                .OrderByDescending(x => x.DateCreate)
                .FirstAsync(x => x.DateDeleted == null && x.HeroId == heroId)
                ?? throw new InnerException(Errors.EmptyRequestHeroRegistration);

            //Конвертируем ответ
            GetRequestRegistrationHeroResponse response = _mapper.Map<GetRequestRegistrationHeroResponse>(row);
            response.Success = true;

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return response;
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