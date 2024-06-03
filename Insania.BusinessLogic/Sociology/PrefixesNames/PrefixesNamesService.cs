using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Sociology;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Sociology.PrefixesNames;

/// <summary>
/// Сервис работы с префиксами имён
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class PrefixesNamesService(ApplicationContext applicationContext, ILogger<PrefixesNamesService> logger, IMapper mapper) :
    IPrefixesNames
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<PrefixesNamesService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка префиксов имён
    /// </summary>
    /// <param name="nationId">Нация</param>
    /// <returns></returns>
    public async Task<BaseResponseList> GetList(long? nationId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetPrefixesNamesListMethod);

        try
        {
            //Получаем данные с базы
            List<PrefixName> rows = await _applicationContext
                .PrefixesNames
                .Include(x => x.PrefixNameNations)
                .Where(x => x.DateDeleted == null 
                    && (nationId == null || (x.PrefixNameNations != null && x.PrefixNameNations.Any(y => y.NationId == nationId))))
                .AsNoTracking()
                .ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = rows.Select(_mapper.Map<BaseResponseListItem>).ToList();

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