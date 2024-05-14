using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Insania.Entities.Context;
using Insania.Entities.Models.AccessRights;
using Insania.Entities.Models.Users;
using Insania.Models.Exceptions;
using Insania.Models.Ecxeption;
using Microsoft.EntityFrameworkCore;

namespace Insania.BusinessLogic.Initialization.InitializationDataBase;

/// <summary>
/// Сервис инициализации базы данных
/// </summary>
/// <param name="roleManager">Менеджер ролей</param>
/// <param name="userManager">Менедже пользователей</param>
/// <param name="applicationContext">Репозиторий основной базы данных</param>
/// <param name="logger">Сервис записи логов</param>
/// <param name="configuration">Интерфейс конфигурации</param>
public class InitializationDataBase(RoleManager<Role> roleManager, UserManager<User> userManager,
    ApplicationContext applicationContext, ILogger<InitializationDataBase> logger, IConfiguration configuration)
    : IInitializationDataBase
{
    /// <summary>
    /// Менеджер ролей
    /// </summary>
    private readonly RoleManager<Role> _roleManager = roleManager;

    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Контекст основной базы данных приложения
    /// </summary>
    private readonly ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Сервис записи логов
    /// </summary>
    private readonly ILogger<InitializationDataBase> _logger = logger;

    /// <summary>
    /// Интерфейс конфигурации
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    #region Вне категорий

    /// <summary>
    /// Метод запуска инициализаций
    /// </summary>
    /// <returns></returns>
    public async Task Initialization()
    {
        try
        {
            //ПОЛЬЗОВАТЕЛИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRoles"]))
            {
                await InitializationRoles();
            }

            //ПОЛЬЗОВАТЕЛИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationUsers"]))
            {
                await InitializationUsers();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("InitializeDatabase. Ошибка: {exception}", ex);
        }
    }

    #endregion

    #region Пользователи

    /// <summary>
    /// Метод инициализации пользователей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationUsers()
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            _logger.LogError("InitializeDatabase. InitializationUsers. Ошибка: {exception}", ex);
        }
    }

    #endregion

    #region Системное

    #endregion

    #region Права доступа

    /// <summary>
    /// Метод инициализации ролей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRoles()
    {
        try
        {
            //Проверяем наличие роли гостя
            if (await _roleManager.FindByNameAsync("guest") == null)
            {
                //Добавляем роль гостя
                Role role = new("guest");
                var result = await _roleManager.CreateAsync(role) ?? throw new Exception(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded)
                    throw new Exception(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
            }

            //Проверяем наличие роли пользователя
            if (await _roleManager.FindByNameAsync("user") == null)
            {
                //Добавляем роль пользователя
                Role role = new("user");
                var result = await _roleManager.CreateAsync(role) ?? throw new Exception(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded)
                    throw new Exception(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
            }

            //Проверяем наличие роли админа
            if (await _roleManager.FindByNameAsync("admin") == null)
            {
                //Добавляем роль админа
                Role role = new("admin");
                var result = await _roleManager.CreateAsync(role) ?? throw new Exception(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded)
                    throw new Exception(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("InitializeDatabase. InitializationRoles. Ошибка: {exception}", ex);
        }
    }

    /// <summary>
    /// Метод инициализации ролей пользователей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationUsersRoles()
    {

    }

    #endregion

    #region Игроки

    /// <summary>
    /// Метод инициализации игроков
    /// </summary>
    /// <returns></returns>
    public async Task InitializationPlayers() { }


    #endregion

    #region Персонажи

    /// <summary>
    /// Метод инициализации биографий персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationBiographiesHeroes() { }

    /// <summary>
    /// Метод инициализации биографий заявок на регистрацию персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationBiographiesRequestsHeroesRegistration() { }

    /// <summary>
    /// Метод инициализации персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationHeroes() { }

    /// <summary>
    /// Метод инициализации заявок на регистрацию персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRequestsHeroesRegistration() { }

    /// <summary>
    /// Метод инициализации статусов регистраций персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationStatusesRequestsHeroesRegistration() { }

    #endregion

    #region Администраторы

    /// <summary>
    /// Метод инициализации администраторов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationAdministrators() { }

    /// <summary>
    /// Метод инициализации капитулов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationChapters() { }

    /// <summary>
    /// Метод инициализации должностей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationPosts() { }

    /// <summary>
    /// Метод инициализации званий
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRanks() { }

    #endregion

    #region Летоисчисление

    /// <summary>
    /// Метод инициализации месяцев
    /// </summary>
    /// <returns></returns>
    public async Task InitializationMonths() { }

    /// <summary>
    /// Метод инициализации сезонов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationSeasons() { }

    #endregion

    #region Внешность

    /// <summary>
    /// Метод инициализации цветов глаз
    /// </summary>
    /// <returns></returns>
    public async Task InitializationEyesColors() { }

    /// <summary>
    /// Метод инициализации цветов волос
    /// </summary>
    /// <returns></returns>
    public async Task InitializationHairsColors() { }

    /// <summary>
    /// Метод инициализации типов телосложений
    /// </summary>
    /// <returns></returns>
    public async Task InitializationTypesBodies() { }

    /// <summary>
    /// Метод инициализации типов лиц
    /// </summary>
    /// <returns></returns>
    public async Task InitializationTypesFaces() { }

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    /// <summary>
    /// Метод иницализации областей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationAreas() { }

    /// <summary>
    /// Метод инициализации стран
    /// </summary>
    /// <returns></returns>
    public async Task InitializationCountries() { }

    /// <summary>
    /// Метод инициализации фракций
    /// </summary>
    /// <returns></returns>
    public async Task InitializationFractions() { }

    /// <summary>
    /// Метод инициализации организаций
    /// </summary>
    /// <returns></returns>
    public async Task InitializationOrganizations() { }

    /// <summary>
    /// Метод инициализации владений
    /// </summary>
    /// <returns></returns>
    public async Task InitializationOwnerships() { }

    /// <summary>
    /// Метод инициализации регионов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRegions() { }

    /// <summary>
    /// Метод инициализации типов организаций
    /// </summary>
    /// <returns></returns>
    public async Task InitializationTypesOrganizations() { }

    #endregion

    #region Экономика

    #endregion

    #region Общие

    #endregion

    #region Новости

    #endregion

    #region Чаты и сообщения

    #endregion

    #region Обращения

    #endregion

    #region Уведомления

    #endregion

    #region Тесты

    #endregion

    #region Биология

    /// <summary>
    /// Метод инициализации наций
    /// </summary>
    /// <returns></returns>
    public async Task InitializationNations() { }

    /// <summary>
    /// Метод инициализации рас
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRaces() { }

    #endregion

    #region Социология

    #endregion

    #region Файлы

    /// <summary>
    /// Метод инициализации файлов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationFiles() { }

    /// <summary>
    /// Метод инициализации файлов персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationFilesHeroes() { }

    /// <summary>
    /// Метод инициализации типов файлов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationTypesFiles() { }

    #endregion

    #region География

    /// <summary>
    /// Метод инициализации географических объектов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationGeographicalObjects() { }

    /// <summary>
    /// Метод инициализации типов географических объектов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationTypesGeographicalObjects() { }

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}