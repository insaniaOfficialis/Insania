using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Appearance;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Appearance.EyesColors;

/// <summary>
/// Сервис работы с цветами глаз
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class EyesColorsService(ApplicationContext applicationContext, ILogger<EyesColorsService> logger, IMapper mapper) :
    IEyesColors
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<EyesColorsService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка цветов глаз
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponseList> GetEyesColorsList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetEyesColorsListMethod);

        try
        {
            //Получаем данные с базы
            List<EyesColor> eyesColor = await _applicationContext.EyesColors.Where(x => x.DateDeleted == null).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = eyesColor.Select(_mapper.Map<BaseResponseListItem>).ToList();

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