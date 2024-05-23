namespace Insania.Models.Logging;

public static class Errors
{
    #region Вне категорий

    public const string EmptyServiceInitializtionDataBase = "Пустой сервис инициализации базы данных";
    public const string Error = "Ошибка:";
    public const string Unknown = "Неопознанная ошибка";

    #endregion

    #region Пользователи

    public const string EmptyUser = "Пустой пользователь";
    public const string FailedCreateUSer = "Не удалось создать пользователя";

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

    #endregion

    #region Внешность

    public const string EmptyEyesColor = "Пустой цвет глаз";
    public const string EmptyTypeBody = "Пустой тип телосложения";
    public const string EmptyTypeFace = "Пустой тип лица";

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
    public const string EmptyTypeOrganization = "Пустой тип организации";

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

    #endregion

    #region Социология

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