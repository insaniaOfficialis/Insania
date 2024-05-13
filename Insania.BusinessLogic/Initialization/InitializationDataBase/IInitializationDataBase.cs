namespace Insania.BusinessLogic.Initialization.InitializationDataBase;

/// <summary>
/// Интерфейс инициализации базы данных
/// </summary>
public interface IInitializationDataBase
{
    #region Вне категорий

    /// <summary>
    /// Метод запуска инициализаций
    /// </summary>
    /// <returns></returns>
    Task Initialization();

    #endregion

    #region Пользователи

    /// <summary>
    /// Метод инициализации пользователей
    /// </summary>
    /// <returns></returns>
    Task InitializationUsers();

    #endregion

    #region Системное

    #endregion

    #region Права доступа

    /// <summary>
    /// Метод инициализации ролей
    /// </summary>
    /// <returns></returns>
    Task InitializationRoles();

    /// <summary>
    /// Метод инициализации ролей пользователей
    /// </summary>
    /// <returns></returns>
    Task InitializationUsersRoles();

    #endregion

    #region Игроки

    /// <summary>
    /// Метод инициализации игроков
    /// </summary>
    /// <returns></returns>
    Task InitializationPlayers();


    #endregion

    #region Персонажи

    /// <summary>
    /// Метод инициализации биографий персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationBiographiesHeroes();

    /// <summary>
    /// Метод инициализации биографий заявок на регистрацию персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationBiographiesRequestsHeroesRegistration();

    /// <summary>
    /// Метод инициализации персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationHeroes();

    /// <summary>
    /// Метод инициализации заявок на регистрацию персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationRequestsHeroesRegistration();

    /// <summary>
    /// Метод инициализации статусов регистраций персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationStatusesRequestsHeroesRegistration();

    #endregion

    #region Администраторы

    /// <summary>
    /// Метод инициализации администраторов
    /// </summary>
    /// <returns></returns>
    Task InitializationAdministrators();

    /// <summary>
    /// Метод инициализации капитулов
    /// </summary>
    /// <returns></returns>
    Task InitializationChapters();

    /// <summary>
    /// Метод инициализации должностей
    /// </summary>
    /// <returns></returns>
    Task InitializationPosts();

    /// <summary>
    /// Метод инициализации званий
    /// </summary>
    /// <returns></returns>
    Task InitializationRanks();

    #endregion

    #region Летоисчисление

    /// <summary>
    /// Метод инициализации месяцев
    /// </summary>
    /// <returns></returns>
    Task InitializationMonths();

    /// <summary>
    /// Метод инициализации сезонов
    /// </summary>
    /// <returns></returns>
    Task InitializationSeasons();

    #endregion

    #region Внешность

    /// <summary>
    /// Метод инициализации цветов глаз
    /// </summary>
    /// <returns></returns>
    Task InitializationEyesColors();

    /// <summary>
    /// Метод инициализации цветов волос
    /// </summary>
    /// <returns></returns>
    Task InitializationHairsColors();

    /// <summary>
    /// Метод инициализации типов телосложений
    /// </summary>
    /// <returns></returns>
    Task InitializationTypesBodies();

    /// <summary>
    /// Метод инициализации типов лиц
    /// </summary>
    /// <returns></returns>
    Task InitializationTypesFaces();

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    /// <summary>
    /// Метод иницализации областей
    /// </summary>
    /// <returns></returns>
    Task InitializationAreas();

    /// <summary>
    /// Метод инициализации стран
    /// </summary>
    /// <returns></returns>
    Task InitializationCountries();

    /// <summary>
    /// Метод инициализации фракций
    /// </summary>
    /// <returns></returns>
    Task InitializationFractions();

    /// <summary>
    /// Метод инициализации организаций
    /// </summary>
    /// <returns></returns>
    Task InitializationOrganizations();

    /// <summary>
    /// Метод инициализации владений
    /// </summary>
    /// <returns></returns>
    Task InitializationOwnerships();
    
    /// <summary>
    /// Метод инициализации регионов
    /// </summary>
    /// <returns></returns>
    Task InitializationRegions();

    /// <summary>
    /// Метод инициализации типов организаций
    /// </summary>
    /// <returns></returns>
    Task InitializationTypesOrganizations();

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
    Task InitializationNations();

    /// <summary>
    /// Метод инициализации рас
    /// </summary>
    /// <returns></returns>
    Task InitializationRaces();

    #endregion

    #region Социология

    #endregion

    #region Файлы

    /// <summary>
    /// Метод инициализации файлов
    /// </summary>
    /// <returns></returns>
    Task InitializationFiles();

    /// <summary>
    /// Метод инициализации файлов персонажей
    /// </summary>
    /// <returns></returns>
    Task InitializationFilesHeroes();

    /// <summary>
    /// Метод инициализации типов файлов
    /// </summary>
    /// <returns></returns>
    Task InitializationTypesFiles();

    #endregion

    #region География

    /// <summary>
    /// Метод инициализации географических объектов
    /// </summary>
    /// <returns></returns>
    Task InitializationGeographicalObjects();

    /// <summary>
    /// Метод инициализации типов географических объектов
    /// </summary>
    /// <returns></returns>
    Task InitializationTypesGeographicalObjects();

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}