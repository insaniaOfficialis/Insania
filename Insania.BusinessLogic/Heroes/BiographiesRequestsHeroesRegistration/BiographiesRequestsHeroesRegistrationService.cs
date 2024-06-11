using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Heroes;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Heroes.BiographiesRequestsHeroesRegistration;

namespace Insania.BusinessLogic.Heroes.BiographiesRequestsHeroesRegistration;

/// <summary>
/// Сервис работы с биографиями заявок на регистрацию персонажей
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class BiographiesRequestsHeroesRegistrationService(ApplicationContext applicationContext,
    ILogger<BiographiesRequestsHeroesRegistrationService> logger, IMapper mapper) : IBiographiesRequestsHeroesRegistration
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<BiographiesRequestsHeroesRegistrationService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения биографии заявки на регистрацию персонажа по уникальному ключу
    /// </summary>
    /// <param name="biographyId">Биография</param>
    /// <param name="requestId">Заявка на регистрацию персонажа</param>
    /// <returns cref="GetBiographyRequestHeroRegistrationResponse">Модель ответа получения биографии завяки на регистрацию персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetBiographyRequestHeroRegistrationResponse> GetByUnique(long? biographyId, long? requestId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetBiographyRequestHeroRegistrationByUniqueMethod);

        try
        {
            //Проверяем данные
            if (biographyId == null) throw new InnerException(Errors.EmptyBiography);
            if (requestId == null) throw new InnerException(Errors.EmptyRequestHeroRegistration);

            //Получаем данные с базы
            BiographyRequestHeroRegistration row = await _applicationContext
                .BiographiesRequestsHeroesRegistration
                .AsNoTracking()
                .FirstAsync(x => x.DateDeleted == null && x.RequestId == requestId && x.BiographyId == biographyId)
                ?? throw new InnerException(Errors.EmptyBiographies);

            //Конвертируем ответ
            GetBiographyRequestHeroRegistrationResponse? response = _mapper.Map<GetBiographyRequestHeroRegistrationResponse>(row);
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