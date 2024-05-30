﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Appearance;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Appearance.TypesBodies;

/// <summary>
/// Сервис работы с типами телосложений
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
/// <param name="mapper">Интерфейс сервиса преобразования моделей</param>
public class TypesBodiesService(ApplicationContext applicationContext, ILogger<TypesBodiesService> logger, IMapper mapper) :
    ITypesBodies
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<TypesBodiesService> _logger = logger;

    /// <summary>
    /// Интерфейс сервиса преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения списка типов телосложений
    /// </summary>
    /// <returns></returns>
    public async Task<BaseResponseList> GetTypesBodiesList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetTypesBodiesListMethod);

        try
        {
            //Получаем данные с базы
            List<TypeBody> typesBodies = await _applicationContext.TypesBodies.Where(x => x.DateDeleted == null).AsNoTracking().ToListAsync();

            //Конвертируем ответ
            List<BaseResponseListItem> result = typesBodies.Select(_mapper.Map<BaseResponseListItem>).ToList();

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