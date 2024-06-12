using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using Insania.BusinessLogic.System.Parameters;
using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Players;
using Insania.Database.Entities.Politics;
using Insania.Database.Entities.Sociology;
using Insania.Database.Entities.Users;
using Insania.Entities.Context;
using Insania.Models.Heroes.Heroes;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.BusinessLogic.Heroes.Heroes;

/// <summary>
/// Сервис работы с персонажами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
/// <param name="parameters">Интерфейс работы с параметрами</param>
/// <param name="userManager">Менеджер пользователей</param>
public class HeroesService(ApplicationContext applicationContext, ILogger<HeroesService> logger, IMapper mapper, 
    IParameters parameters, UserManager<User> userManager) : IHeroes
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<HeroesService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Интерфейс работы с параметрами
    /// </summary>
    private readonly IParameters _parameters = parameters;

    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Метод регистрации персонажа
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    public async Task<BaseResponse> Registration(AddHeroRequest? request)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredRegistationHeroMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем данные
            if (request == null) throw new InnerException(Errors.EmptyRequest);
            int globalDate = Convert.ToInt32((await _parameters.GetValueByAlias("Global'naya_data")).Value?.Split('|').Last());
            if (request.BirthCycle > globalDate) throw new InnerException(Errors.IncorrectCycle);

            //Создаём пользователя
            User user = new(request.User?.Login!,
                request.User?.Password,
                request.User?.PhoneNumber,
                request.User?.LinkVK,
                false,
                (request.User?.Gender ?? true),
                request.User?.LastName!,
                request.User?.FirstName!,
                request.User?.Patronymic!,
                request.User?.BirthDate);
            var userResult = await _userManager.CreateAsync(user, request.User?.Password!) ?? throw new InnerException(Errors.EmptyUser);
            if (!userResult.Succeeded) throw new InnerException(userResult?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);

            //Добавляем пользователю роль
            var userRoleResult = await _userManager.AddToRoleAsync(user, "guest");
            if (!userRoleResult.Succeeded) throw new InnerException(userRoleResult?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);

            //Создаём игрока
            Player player = new(user.UserName!, false, user, 0);
            await _applicationContext.Players.AddAsync(player);
            await _applicationContext.SaveChangesAsync();

            //Создаём персонажа
            PrefixName? prefixName = await _applicationContext.PrefixesNames.FirstAsync(x => x.Id == request.PrefixNameId) ?? throw new InnerException(Errors.EmptyPrefixName);
            Month? birthMonth = await _applicationContext.Months.FirstAsync(x => x.Id == request.BirthMonthId) ?? throw new InnerException(Errors.EmptyMonth);
            Nation? nation = await _applicationContext.Nations.FirstAsync(x => x.Id == request.NationId) ?? throw new InnerException(Errors.EmptyNation);
            HairsColor? hairsColor = await _applicationContext.HairsColors.FirstAsync(x => x.Id == request.HairsColorId) ?? throw new InnerException(Errors.EmptyHairsColor);
            EyesColor? eyesColor = await _applicationContext.EyesColors.FirstAsync(x => x.Id == request.EyesColorId) ?? throw new InnerException(Errors.EmptyEyesColor);
            TypeBody? typeBody = await _applicationContext.TypesBodies.FirstAsync(x => x.Id == request.TypeBodyId) ?? throw new InnerException(Errors.EmptyTypeBody);
            TypeFace? typeFace = await _applicationContext.TypesFaces.FirstAsync(x => x.Id == request.TypeFaceId) ?? throw new InnerException(Errors.EmptyTypeFace);
            Area? area = await _applicationContext.Areas.FirstAsync(x => x.Id == request.CurrentLocationId) ?? throw new InnerException(Errors.EmptyArea);
            Hero hero = new(user.UserName!,
                false,
                player,
                request.PersonalName!,
                prefixName,
                request.FamilyName,
                request.BirthDay ?? 0,
                birthMonth,
                request.BirthCycle ?? 0,
                nation,
                request.Gender ?? true,
                request.Height ?? 0,
                request.Weight ?? 0,
                hairsColor,
                eyesColor,
                typeBody,
                typeFace,
                true,
                true,
                null,
                area);
            await _applicationContext.Heroes.AddAsync(hero);
            await _applicationContext.SaveChangesAsync();

            //Создаём заявку на персонажа
            StatusRequestHeroRegistration? status = await _applicationContext.StatusesRequestsHeroesRegistration.FirstAsync(x => x.Alias == "Novaya") ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            RequestHeroRegistration requestHeroRegistration = new(user.UserName!, false, hero, status);
            await _applicationContext.RequestsHeroesRegistration.AddAsync(requestHeroRegistration);
            await _applicationContext.SaveChangesAsync();

            //Проходим по всем элементам биографии
            foreach (var item in request.Biographies!)
            {
                //Создаём биографию
                Month? beginMonth = await _applicationContext.Months.FirstAsync(x => x.Id == item.MonthBeginId) ?? throw new InnerException(Errors.EmptyMonth);
                Month? endMonth = item.MonthEndId != null ? await _applicationContext.Months.FirstAsync(x => x.Id == item.MonthEndId) : null;
                BiographyHero biographyHero = new(user.UserName!,
                    false,
                    hero,
                    item.DayBegin ?? 0,
                    beginMonth,
                    item.CycleBegin ?? 0,
                    item.DayEnd, 
                    endMonth,
                    item.CycleEnd,
                    item.Text!);
                await _applicationContext.BiographiesHeroes.AddAsync(biographyHero);
                await _applicationContext.SaveChangesAsync();

                //Создаём биографию заявки
                BiographyRequestHeroRegistration biographyRequest = new(user.UserName!, false, requestHeroRegistration, biographyHero);
                await _applicationContext.BiographiesRequestsHeroesRegistration.AddAsync(biographyRequest);
                await _applicationContext.SaveChangesAsync();
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new BaseResponse(true, hero.Id);
        }
        catch (InnerException ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
    }

    /// <summary>
    /// Метод получения персонажа по первичному ключу
    /// </summary>
    /// <param name="id">Первичный ключ</param>
    /// <returns cref="GetHeroResponse">Модель ответа получения персонажа</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetHeroResponse> GetById(long? id)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetHeroByIdMethod);

        try
        {
            //Проверяем данные
            if (id == null) throw new InnerException(Errors.EmptyRequest);

            //Получаем данные с базы
            Hero row = await _applicationContext
                .Heroes
                .Include(x => x.Nation)
                .Include(x => x.CurrentLocation)
                .ThenInclude(x => x.Region)
                .ThenInclude(x => x.Country)
                .Include(x => x.FilesHero)
                .FirstAsync(x => x.Id == id)
                ?? throw new InnerException(Errors.EmptyHero);

            //Конвертируем ответ
            GetHeroResponse response = _mapper.Map<GetHeroResponse>(row);
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
    /// Метод получения списка персонажей по текущему пользователю
    /// </summary>
    /// <param name="login">Текущий пользователь</param>
    /// <returns cref="GetHeroesResponseList">Модель ответа получения списка персонажей</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<GetHeroesResponseList> GetListByCurrent(string? login)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetHeroesListByCurrentMethod);

        try
        {
            //Проверяем данные
            if (login == null) throw new InnerException(Errors.EmptyCurrentUser);

            //Получаем данные с базы
            List<Hero>? rows = await _applicationContext
                .Heroes
                .Include(x => x.Player)
                .ThenInclude(y => y.User)
                .Include(x => x.PrefixName)
                .Where(x => x.DateDeleted == null && x.IsActive == true && x.Player.User.UserName == login)
                .ToListAsync()
                ?? throw new InnerException(Errors.EmptyHeroes);

            //Конвертируем ответ
            List<GetHeroesResponseListItem> items = rows.Select(_mapper.Map<GetHeroesResponseListItem>).ToList();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new(true, items);
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