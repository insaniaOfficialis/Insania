using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.System;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.System.Parameters;

/// <summary>
/// Сервис работы с параметрами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class ParametersService(ApplicationContext applicationContext, ILogger<ParametersService> logger, IMapper mapper) :
    IParameters
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<ParametersService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения значения по алиасу
    /// </summary>
    /// <param name="alias">Псевдоним</param>
    /// <returns></returns>
    public async Task<BaseResponse> GetValueByAlias(string? alias)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetValueByAliasMethod);

        try
        {
            //Получаем данные с базы
            Parameter row = await _applicationContext.Parameters.AsNoTracking().FirstAsync(x => x.Alias == alias);

            //Конвертируем ответ
            BaseResponse result = _mapper.Map<BaseResponse>(row);

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return result;
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