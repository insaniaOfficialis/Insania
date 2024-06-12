namespace Insania.Models.OutCategories.Logging;

/// <summary>
/// Список информационных сообщений
/// </summary>
public static class Informations
{
    #region Вне категорий

    public const string EnteredInitializationMethod = "Вошли в метод инициализации";
    public const string Success = "Успешно";

    #endregion

    #region Пользователи

    public const string EnteredAddUserMethod = "Вошли в метод добавления пользователя";
    public const string EnteredCheckLoginMethod = "Вошли в метод проверки доступности логина";
    public const string EnteredInitializationUsersMethod = "Вошли в метод инициализации пользователей";
    public const string UserAdded = "-пользователь добавлен";
    public const string UserAlreadyAdded = "-пользователь уже добавлен";

    #endregion

    #region Системное

    public const string EnteredInitializationParametersMethod = "Вошли в метод инициализации параметров";
    public const string EnteredGetValueByAliasMethod = "Вошли в метод получения значения по псевдониму";
    public const string ExecuteScript = "Выполняем скрипт:";
    public const string ExecutedScript = "Выполнен скрипт:";
    public const string NotExecutedScript = "Не выполнен скрипт:";
    public const string ParameterAdded = "-параметр добавлен";
    public const string ParameterAlreadyAdded = "-параметр уже добавлен";
    public const string ScriptsPath = "Путь к скриптам:";
    public const string ScriptAlreadyExecuted = "-скрипт уже выполнен";

    #endregion

    #region Права доступа

    public const string EnteredInitializationRolesMethod = "Вошли в метод инициализации ролей";
    public const string EnteredInitializationUsersRolesMethod = "Вошли в метод инициализации ролей пользователей";
    public const string RoleAdded = "-роль добавлена";
    public const string RoleAlreadyAdded = "-роль уже добавлена";
    public const string UserRoleAdded = "-роль пользователя добавлена";
    public const string UserRoleAlreadyAdded = "-роль пользователя уже добавлена";

    #endregion

    #region Игроки

    public const string EnteredInitializationPlayersMethod = "Вошли в метод инициализации игроков";
    public const string PlayerAdded = "-игрок добавлен";
    public const string PlayerAlreadyAdded = "-игрок уже добавлен";

    #endregion

    #region Персонажи

    public const string BiographyHeroAdded = "-биография персонажа добавлена";
    public const string BiographyHeroAlreadyAdded = "-биография персонажа уже добавлена";
    public const string BiographiesRequestsHeroesRegistrationAdded = "-биография заявки на регистрацию персонажа добавлена";
    public const string BiographiesRequestsHeroesRegistrationAlreadyAdded = "-биография заявки на регистрацию персонажа уже добавлена";
    public const string EnteredGetBiographyRequestHeroRegistrationByUniqueMethod = "Вошли в метод получения биографии заявки на регистраци персонажа по уникальному ключу";
    public const string EnteredGetBiographiesHeroMethod = "Вошли в метод получения биографий персонажа";
    public const string EnteredGetHeroByIdMethod = "Вошли в метод получения персонажа по id";
    public const string EnteredGetHeroesListByCurrentMethod = "Вошли в метод получения списка персонажей по текущему пользователю";
    public const string EnteredGetListStatusesRequestsHeroesRegistrationByIdMethod = "Вошли в метод получения список заявок на регистрацию персонажей";
    public const string EnteredGetRequestHeroRegistrationByIdMethod = "Вошли в метод получения заявки на регистрацию персонажа по id";
    public const string EnteredGetRequestHeroRegistrationByHeroMethod = "Вошли в метод получения заявки на регистрацию персонажа по персонажу";
    public const string EnteredInitializationBiographiesHeroesMethod = "Вошли в метод инициализации биографией персонажей";
    public const string EnteredInitializationBiographiesRequestsHeroesRegistrationMethod = "Вошли в метод инициализации биографией заявок на регистрацию персонажей";
    public const string EnteredInitializationHeroesMethod = "Вошли в метод инициализации персонажей";
    public const string EnteredInitializationRequestsHeroesRegistration = "Вошли в метод инициализации заявок на регистрацию персонажей";
    public const string EnteredInitializationStatusesRequestsHeroesRegistrationMethod = "Вошли в метод инициализации статусов заявок регистрации персонажей";
    public const string EnteredRegistationHeroMethod = "Вошли в метод регистрации персонажа";
    public const string HeroAdded = "-персонаж добавлен";
    public const string HeroAlreadyAdded = "-персонаж уже добавлен";
    public const string RequestsHeroesRegistrationAdded = "-заявка на регистрацию персонажа добавлена";
    public const string RequestsHeroesRegistrationAlreadyAdded = "-заявка на регистрацию персонажа добавлена";
    public const string StatusesRequestsHeroesRegistrationAdded = "-статус заявок на регистрацию персонажей добавлен";
    public const string StatusesRequestsHeroesRegistrationAlreadyAdded = "-статус заявок на регистрацию персонажей уже добавлен";

    #endregion

    #region Администраторы

    public const string AdministratorAdded = "-администратор добавлен";
    public const string AdministratorAlreadyAdded = "-администратор уже добавлен";
    public const string ChapterAdded = "-капитул добавлен";
    public const string ChapterAlreadyAdded = "-капитул уже добавлен";
    public const string EnteredGetListAdmistratorsMethod = "Вошли в метод получения списка администраторов";
    public const string EnteredInitializationAdmistratorsMethod = "Вошли в метод инициализации администраторов";
    public const string EnteredInitializationChaptersMethod = "Вошли в метод инициализации капитулов";
    public const string EnteredInitializationPostsMethod = "Вошли в метод инициализации должностей";
    public const string EnteredInitializationRanksMethod = "Вошли в метод инициализации званий";
    public const string PostAdded = "-должность добавлена";
    public const string PostAlreadyAdded = "-должность уже добавлена";
    public const string RankAdded = "-звание добавлено";
    public const string RankAlreadyAdded = "-звание уже добавлено";

    #endregion

    #region Летоисчисление

    public const string EnteredGetMonthsListMethod = "Вошли в метод получения списка месяцев";
    public const string EnteredInitializationMonthsMethod = "Вошли в метод инициализации месяцев";
    public const string EnteredInitializationSeasonsMethod = "Вошли в метод инициализации сезонов";
    public const string MonthAdded = "-месяц добавлен";
    public const string MonthAlreadyAdded = "-месяц уже добавлен";
    public const string SeasonAdded = "-сезон добавлен";
    public const string SeasonAlreadyAdded = "-сезон уже добавлен";

    #endregion

    #region Внешность

    public const string EnteredGetEyesColorsListMethod = "Вошли в метод получения списка цветов глаз";
    public const string EnteredGetHairsColorsListMethod = "Вошли в метод получения списка цветов волос";
    public const string EnteredGetTypesBodiesListMethod = "Вошли в метод получения списка типов телосложений";
    public const string EnteredGetTypesFacesListMethod = "Вошли в метод получения списка типов лиц";
    public const string EnteredInitializationEyesColorsMethod = "Вошли в метод инициализации цветов глаз";
    public const string EnteredInitializationHairsColorsMethod = "Вошли в метод инициализации цветов волос";
    public const string EnteredInitializationTypesBodiesMethod = "Вошли в метод инициализации типов телосложений";
    public const string EnteredInitializationTypesFacesMethod = "Вошли в метод инициализации типов лиц";
    public const string EyesColorAdded = "-цвет глаз добавлен";
    public const string EyesColorAlreadyAdded = "-цвет волос уже добавлен";
    public const string HairsColorAdded = "-цвет волос добавлен";
    public const string HairsColorAlreadyAdded = "-цвет волос уже добавлен";
    public const string TypeBodyAdded = "-тип телосложения добавлен";
    public const string TypeBodyAlreadyAdded = "-тип телосложения уже добавлен";
    public const string TypeFaceAdded = "-тип лица добавлен";
    public const string TypeFaceAlreadyAdded = "-тип лица уже добавлен";

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    public const string AreaAdded = "-область добавлена";
    public const string AreaAlreadyAdded = "-область уже добавлена";
    public const string CountryAdded = "-страна добавлена";
    public const string CountryAlreadyAdded = "-страна уже добавлена";
    public const string EnteredGetAreasListMethod = "Вошли в метод получения списка областей";
    public const string EnteredGetCountriesListMethod = "Вошли в метод получения списка стран";
    public const string EnteredGetRegionsListMethod = "Вошли в метод получения списка регионов";
    public const string EnteredInitializationAreasMethod = "Вошли в метод инициализации областей";
    public const string EnteredInitializationCountriesMethod = "Вошли в метод инициализации стран";
    public const string EnteredInitializationFractionsMethod = "Вошли в метод инициализации фракций";
    public const string EnteredInitializationOrganizationsMethod = "Вошли в метод инициализации организаций";
    public const string EnteredInitializationOwnershipsMethod = "Вошли в метод инициализации владений";
    public const string EnteredInitializationRegionsMethod = "Вошли в метод инициализации регионов";
    public const string EnteredInitializationRegionsOwnershipsMethod = "Вошли в метод инициализации регионов владений";
    public const string EnteredInitializationTypiesOrganizationsMethod = "Вошли в метод инициализации типов организаций";
    public const string OrganizationAdded = "-организация добавлена";
    public const string OrganizationAlreadyAdded = "-организация уже добавлена";
    public const string FractionAdded = "-фракция добавлена";
    public const string FractionAlreadyAdded = "-фракция уже добавлена";
    public const string OwnershipAdded = "-владение добавлено";
    public const string OwnershipAlreadyAdded = "-владение уже добавлено";
    public const string RegionAdded = "-регион добавлен";
    public const string RegionAlreadyAdded = "-регион уже добавлен";
    public const string RegionOwnershipAdded = "-регион владения добавлен";
    public const string RegionOwnershipAlreadyAdded = "-регион владения уже добавлен";
    public const string TypeOrganizationAdded = "-тип организации добавлен";
    public const string TypeOrganizationAlreadyAdded = "-тип организации уже добавлен";

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

    public const string EnteredGetNationsListMethod = "Вошли в метод получения списка наций";
    public const string EnteredGetRacesListMethod = "Вошли в метод получения списка рас";
    public const string EnteredInitializationNationsMethod = "Вошли в метод инициализации наций";
    public const string EnteredInitializationRacesMethod = "Вошли в метод инициализации рас";
    public const string NationAdded = "-нация добавлена";
    public const string NationAlreadyAdded = "-нация уже добавлена";
    public const string RaceAdded = "-раса добавлена";
    public const string RaceAlreadyAdded = "-раса уже добавлена";

    #endregion

    #region Социология

    public const string EnteredGetPrefixesNamesListMethod = "Вошли в метод получения списка префиксов имён";
    public const string EnteredInitializationPrefixesNamesMethod = "Вошли в метод инициализации префиксов имён";
    public const string EnteredInitializationPrefixesNamesNationsMethod = "Вошли в метод инициализации префиксов имён наций";
    public const string PrefixNameAdded = "-префикс имени добавлен";
    public const string PrefixNameAlreadyAdded = "-префикс имени уже добавлен";
    public const string PrefixNameNationAdded = "-префикс имени нации добавлен";
    public const string PrefixNameNationAlreadyAdded = "-префикс имени нации уже добавлен";

    #endregion

    #region Файлы

    public const string EnteredAddFileMethod = "Вошли в метод добавлеия файла";
    public const string EnteredGetFileByIdMethod = "Вошли в метод получения файла по id";
    public const string EnteredInitializationFilesMethod = "Вошли в метод инициализации файлов";
    public const string EnteredInitializationTypesFilesMethod = "Вошли в метод инициализации типов файлов";
    public const string FileAdded = "-файл добавлен";
    public const string FileAlreadyAdded = "-файл уже добавлен";
    public const string FileHeroAdded = "-файл персонажа добавлен";
    public const string FileHeroAlreadyAdded = "-файл персонажа уже добавлен";
    public const string TypeFilesAdded = "-тип файлов добавлен";
    public const string TypeFilesAlreadyAdded = "-тип файлов уже добавлен";

    #endregion

    #region География
    public const string EnteredInitializationGeographicalObjectsMethod = "Вошли в метод инициализации географических объектов";
    public const string EnteredInitializationTypesGeographicalObjectsMethod = "Вошли в метод инициализации типов географических объектов";
    public const string GeographicalObjectAdded = "-географический объект добавлен";
    public const string GeographicalObjectAlreadyAdded = "-географический объект уже добавлен";
    public const string TypeGeographicalObjectsAdded = "-тип географических объектов добавлен";
    public const string TypeGeographicalObjectsAlreadyAdded = "-тип географических объектов уже добавлен";

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}
