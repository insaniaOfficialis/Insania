using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Heroes;
using Insania.Entities.Context;
using Insania.Models.Heroes.BiographiesHeroes;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Heroes.BiographiesHeroes;

/// <summary>
/// Сервис работы с биографиями персонажей
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class BiographiesHeroesService(ApplicationContext applicationContext, ILogger<BiographiesHeroesService> logger,
    IMapper mapper) : IBiographiesHeroes
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<BiographiesHeroesService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Метод получения биографий персонажа
    /// </summary>
    /// <param name="heroId">Персонаж</param>
    /// <returns cref="GetBiographiesHeroResponseList">Модель ответа получения биографий персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetBiographiesHeroResponseList> GetList(long? heroId)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetBiographiesHeroMethod);

        try
        {
            //Проверяем данные
            if (heroId == null) throw new InnerException(Errors.EmptyRequest);

            //Получаем данные с базы
            List<BiographyHero> rows = await _applicationContext
                .BiographiesHeroes
                .Where(x => x.DateDeleted == null && x.HeroId == heroId)
                .AsNoTracking()
                .ToListAsync()
                ?? throw new InnerException(Errors.EmptyBiographies);

            //Конвертируем ответ
            List<GetBiographiesHeroResponseListItem> response = rows.Select(_mapper.Map<GetBiographiesHeroResponseListItem>).ToList();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new GetBiographiesHeroResponseList(true, response);
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