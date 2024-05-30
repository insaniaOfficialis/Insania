﻿namespace Insania.Models.OutCategories.Logging;

public static class Errors
{
    #region Вне категорий

    public const string EmptyRequest = "Пустой запрос";
    public const string EmptyResponse = "Пустой ответ";
    public const string EmptyServiceCheckConnection = "Пустой сервис проверки соединения";
    public const string EmptyServiceInitializtionDataBase = "Пустой сервис инициализации базы данных";
    public const string EmptyToken = "Пустой токен";
    public const string EmptyUrl = "Пустая ссылка";
    public const string EmptyUrlCheckConnection = "Пустая ссылка проверки соединения";
    public const string EmptyVersion = "Пустая версия api";
    public const string Error = "Ошибка:";
    public const string IncorrectToken = "Некорректный токен";
    public const string NoConnection = "Нет соединения";
    public const string ServerError = "Ошибка сервера";
    public const string Unknown = "Неопознанная ошибка";

    #endregion

    #region Пользователи

    public const string EmptyAudience = "Пустой получатель";
    public const string EmptyBirthDate = "Пустая дата рождения";
    public const string EmptyFirstName = "Пустое имя";
    public const string EmptyExpires = "Пустое время жизни токена";
    public const string EmptyGender = "Пустой пол";
    public const string EmptyIssuer = "Пустой отправитель";
    public const string EmptyKeyToken = "Пустой ключ токена";
    public const string EmptyLastName = "Пустая фамилия";
    public const string EmptyLogin = "Пустой логин";
    public const string EmptyPassword = "Пустой пароль";
    public const string EmptyPatronymic = "Пустое отчество";
    public const string EmptyPhoneNumberEmailLinkVK = "Пустой номер телефона и почта, и ссылка на вк";
    public const string EmptyServiceAuthentication = "Пустой сервис аутентификации";
    public const string EmptyServiceUsers = "Пустой сервис работы с пользователями";
    public const string EmptyUser = "Пустой пользователь";
    public const string EmptyUrlAuthentication = "Пустая ссылка аутентификации";
    public const string EmptyUrlUsers = "Пустая ссылка работы с пользователями";
    public const string FailedCreateUser = "Не удалось создать пользователя";
    public const string IncorrectBirthDate = "Некорректная дата рождение. Минимально допустимая:";
    public const string IncorrectEmail = "Некорректная почта";
    public const string IncorrectLinkVK = "Некорректная ссылка в вк";
    public const string IncorrectPassword = "Некорректный пароль";
    public const string IncorrectPhoneNumber = "Некорректный номер телефона";
    public const string LoginAlreadyExists = "Логин уже существует";
    public const string NotExistsUser = "Не найден пользователь";
    public const string UserIsBlocked = "Пользвоатель заблокирован";

    #endregion

    #region Системное

    public const string EmptyResultExecution = "Пустой результат выполнения";
    public const string EmptyScriptsPath = "Пустой путь к скриптам";

    #endregion

    #region Права доступа

    public const string FailedAddUserRole = "Не удалось добавить роль пользователю";
    public const string FailedCreateRole = "Не удалось создать роль";

    #endregion

    #region Игроки

    public const string EmptyPlayer = "Пустой игрок";

    #endregion

    #region Персонажи

    public const string EmptyBiography= "Пустая биография";
    public const string EmptyHero= "Пустой персонаж";
    public const string EmptyRequestsHeroesRegistration = "Пустая заявок на регистрацию персонажей";
    public const string EmptyStatusRequestsHeroesRegistration = "Пустой статус заявок на регистрацию персонажей";

    #endregion

    #region Администраторы

    public const string EmptyAdministrator = "Пустой администратор";
    public const string EmptyPost = "Пустая должность";
    public const string EmptyRank = "Пустое звание";

    #endregion

    #region Летоисчисление

    public const string EmptyMonth = "Пустой месяц";
    public const string EmptySeason = "Пустой сезон";
    public const string EmptyServiceMonths = "Пустой сервис работы с месяцами";
    public const string EmptyUrlMonths = "Пустая ссылка работы с месяцами";

    #endregion

    #region Внешность

    public const string EmptyEyesColor = "Пустой цвет глаз";
    public const string EmptyTypeBody = "Пустой тип телосложения";
    public const string EmptyTypeFace = "Пустой тип лица";
    public const string EmptyServiceEyesColors = "Пустой сервис работы с цветами глаз";
    public const string EmptyServiceHairsColors = "Пустой сервис работы с цветами волос";
    public const string EmptyServiceTypesBodies = "Пустой сервис работы с типами телосложений";
    public const string EmptyServiceTypesFaces = "Пустой сервис работы с типами лиц";
    public const string EmptyUrlEyesColors = "Пустая ссылка работы с цветами глаз";
    public const string EmptyUrlHairsColors = "Пустая ссылка работы с цветами волос";
    public const string EmptyUrlTypesBodies = "Пустая ссылка работы с типами телосложений";
    public const string EmptyUrlTypesFaces = "Пустая ссылка работы с типами лиц";

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    public const string EmptyArea = "Пустая область";
    public const string EmptyChapter = "Пустой капитул";
    public const string EmptyCountry = "Пустая страна";
    public const string EmptyFraction = "Пустая фракция";
    public const string EmptyOrganization = "Пустая организация";
    public const string EmptyOwnership = "Пустое владение";
    public const string EmptyRegion = "Пустой регион";
    public const string EmptyServiceAreas = "Пустой сервис работы с областями";
    public const string EmptyServiceСountries = "Пустой сервис работы со странами";
    public const string EmptyServiceRegions = "Пустой сервис работы с регионами";
    public const string EmptyTypeOrganization = "Пустой тип организации";
    public const string EmptyUrlAreas = "Пустая ссылка работы с областями";
    public const string EmptyUrlCountries = "Пустая ссылка работы со странами";
    public const string EmptyUrlRegions = "Пустая ссылка работы с регионами";

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

    public const string EmptyNation = "Пустая нация";
    public const string EmptyRace = "Пустая раса";
    public const string EmptyServiceNations = "Пустой сервис работы с нациями";
    public const string EmptyServiceRaces = "Пустой сервис работы с расами";
    public const string EmptyUrlNations = "Пустая ссылка работы с нациями";
    public const string EmptyUrlRaces = "Пустая ссылка работы с расами";

    #endregion

    #region Социология

    public const string EmptyPrefixName = "Пустой префикс имени";

    #endregion

    #region Файлы

    public const string EmptyFile = "Пустой файл";
    public const string EmptyTypeFile = "Пустой тип файла";

    #endregion

    #region География

    public const string EmptyGeographicalObject = "Пустой географический объект";
    public const string EmptyTypeGeographicalObjects = "Пустой тип географических объектов";

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}