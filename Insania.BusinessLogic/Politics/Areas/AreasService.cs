using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Politics;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Politics.Areas;

/// <summary>
/// Сервис работы с областями
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="mapper">Интерфейс сервиса преобразования моделей</param>
public class AreasService(ApplicationContext applicationContext, ILogger<AreasService> logger, IMapper mapper) :
    IAreas
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<AreasService> _logger = logger;

    /// <summary>
    /// Интерфейс сервиса преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка областей
    /// </summary>
    /// <param name="regionId">Регион</param>
    /// <param name="ownershipId">Владение</param>
    /// <returns></returns>
    public async Task<BaseResponseList> GetAreasList(long? regionId, long? ownershipId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetAreasListMethod);

        try
        {
            //Получаем данные с базы
            List<Area> areas = await _applicationContext
                .Areas
                .Where(x => x.DateDeleted == null 
                    && (regionId == null || x.RegionId == regionId) 
                    && (ownershipId == null || x.OwnershipId == ownershipId))
                .AsNoTracking()
                .ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = areas.Select(_mapper.Map<BaseResponseListItem>).ToList();

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