using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Administrators;
using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Files;
using Insania.Database.Entities.Geography;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.InformationArticles;
using Insania.Database.Entities.Players;
using Insania.Database.Entities.Politics;
using Insania.Database.Entities.Sociology;
using Insania.Database.Entities.System;
using Insania.Database.Entities.Users;

using File = Insania.Database.Entities.Files.File;

namespace Insania.Entities.Context;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, IdentityUserRole<long>,
    IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
{
    #region Вне категорий

    #endregion

    #region Пользователи

    #endregion

    #region Системное

    /// <summary>
    /// Логи
    /// </summary>
    public DbSet<Log> Logs { get; set; }

    /// <summary>
    /// Параметры
    /// </summary>
    public DbSet<Parameter> Parameters { get; set; }

    /// <summary>
    /// Скрипты
    /// </summary>
    public DbSet<Script> Scripts { get; set; }

    #endregion

    #region Права доступа

    #endregion

    #region Игроки

    /// <summary>
    /// Игроки
    /// </summary>
    public DbSet<Player> Players { get; set; }

    #endregion

    #region Персонажи

    /// <summary>
    /// Персонажи
    /// </summary>
    public DbSet<Hero> Heroes { get; set; }

    /// <summary>
    /// Биографии персонажей
    /// </summary>
    public DbSet<BiographyHero> BiographiesHeroes { get; set; }

    /// <summary>
    /// Статусы заявки на регистрацию персонажа
    /// </summary>
    public DbSet<StatusRequestHeroRegistration> StatusesRequestsHeroesRegistration { get; set; }

    /// <summary>
    /// Заявки на регистрацию персонажа
    /// </summary>
    public DbSet<RequestHeroRegistration> RequestsHeroesRegistration { get; set; }

    /// <summary>
    /// Биографии заявки на регистрацию персонажа
    /// </summary>
    public DbSet<BiographyRequestHeroRegistration> BiographiesRequestsHeroesRegistration { get; set; }

    #endregion

    #region Администраторы

    /// <summary>
    /// Должности
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Звания
    /// </summary>
    public DbSet<Rank> Ranks { get; set; }

    /// <summary>
    /// Капитулы
    /// </summary>
    public DbSet<Chapter> Chapters { get; set; }

    /// <summary>
    /// Администраторы
    /// </summary>
    public DbSet<Administrator> Administrators { get; set; }

    #endregion

    #region Летоисчисление

    /// <summary>
    /// Сезоны
    /// </summary>
    public DbSet<Season> Seasons { get; set; }

    /// <summary>
    /// Месяцы
    /// </summary>
    public DbSet<Month> Months { get; set; }

    #endregion

    #region Внешность

    /// <summary>
    /// Цвета волос
    /// </summary>
    public DbSet<HairsColor> HairsColors { get; set; }

    /// <summary>
    /// Цвета глаз
    /// </summary>
    public DbSet<EyesColor> EyesColors { get; set; }

    /// <summary>
    /// Типы телосложений
    /// </summary>
    public DbSet<TypeBody> TypesBodies { get; set; }

    /// <summary>
    /// Типы лиц
    /// </summary>
    public DbSet<TypeFace> TypesFaces { get; set; }

    #endregion

    #region Информационные статьи

    /// <summary>
    /// Разделы информационных статей
    /// </summary>
    public DbSet<SectionInformationArticle> SectionsInformationArticles { get; set; }

    /// <summary>
    /// Оглавления информационных статей
    /// </summary>
    public DbSet<HeaderInformationArticle> HeadersInformationArticles { get; set; }

    /// <summary>
    /// Детальные части информационных статей
    /// </summary>
    public DbSet<DetailInformationArticle> DetailsInformationArticles { get; set; }

    #endregion

    #region Политика

    /// <summary>
    /// Области
    /// </summary>
    public DbSet<Area> Areas { get; set; }

    /// <summary>
    /// Страны
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Фракции
    /// </summary>
    public DbSet<Fraction> Fractions { get; set; }

    /// <summary>
    /// Организации
    /// </summary>
    public DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Владения
    /// </summary>
    public DbSet<Ownership> Ownerships { get; set; }

    /// <summary>
    /// Регионы
    /// </summary>
    public DbSet<Region> Regions { get; set; }

    /// <summary>
    /// Регионы владений
    /// </summary>
    public DbSet<RegionOwnership> RegionsOwnership { get; set; }

    /// <summary>
    /// Типы организаций
    /// </summary>
    public DbSet<TypeOrganization> TypiesOrganizations { get; set; }

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
    /// Расы
    /// </summary>
    public DbSet<Race> Races { get; set; }

    /// <summary>
    /// Нации
    /// </summary>
    public DbSet<Nation> Nations { get; set; }

    #endregion

    #region Социология

    /// <summary>
    /// Префиксы имён
    /// </summary>
    public DbSet<PrefixName> PrefixesNames { get; set; }

    /// <summary>
    /// Префиксы имён наций
    /// </summary>
    public DbSet<PrefixNameNation> PrefixesNamesNations { get; set; }

    #endregion

    #region Файлы

    /// <summary>
    /// Типы файлов
    /// </summary>
    public DbSet<TypeFile> TypesFiles { get; set; }

    /// <summary>
    /// Файлы
    /// </summary>
    public DbSet<File> Files { get; set; }

    /// <summary>
    /// Файлы персонажей
    /// </summary>
    public DbSet<FileHero> FilesHeroes { get; set; }

    /// <summary>
    /// Файлы детальных частей информационных статей
    /// </summary>
    public DbSet<FileDetailInformationArticle> FilesDetailsInformationArticles { get; set; }

    #endregion

    #region География

    /// <summary>
    /// Географические объекты
    /// </summary>
    public DbSet<GeographicalObject> GeographicalObjects { get; set; }

    /// <summary>
    /// Типы географических объектов
    /// </summary>
    public DbSet<TypeGeographicalObject> TypesGeographicalObjects { get; set; }

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region Вне категорий

        modelBuilder.Entity<IdentityUserClaim<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityUserLogin<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityRoleClaim<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityUserToken<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);

        #endregion

        #region Пользователи

        //Пользователи
        modelBuilder.Entity<IdentityUser<long>>().ToTable("sys_users").HasIndex(x => x.UserName);

        #endregion

        #region Системное

        //Логи
        modelBuilder.Entity<Log>().HasIndex(x => x.Method);

        //Параметры
        modelBuilder.Entity<Parameter>().HasIndex(x => x.Name);

        //Скрипты
        modelBuilder.Entity<Script>().HasIndex(x => x.Name);

        #endregion

        #region Права доступа

        //Роли
        modelBuilder.Entity<IdentityRole<long>>().ToTable("sys_roles").HasIndex(x => x.Name);

        //Роли пользователей
        modelBuilder.Entity<IdentityUserRole<long>>().ToTable("sys_users_roles").HasKey(x => new { x.RoleId, x.UserId });

        #endregion

        #region Игроки

        //Игроки
        modelBuilder.Entity<Player>();

        #endregion

        #region Персонажи

        //Биографии персонажей
        modelBuilder.Entity<BiographyHero>();

        //Биографии заявок на регистрацию персонажей
        modelBuilder.Entity<BiographyRequestHeroRegistration>().HasIndex(x => new { x.BiographyId, x.RequestId }).IsUnique();

        //Персонажи
        modelBuilder.Entity<Hero>();

        //Заявки на регистрацию персонажей
        modelBuilder.Entity<RequestHeroRegistration>();

        // Статусы заявки на регистрацию персонажа
        modelBuilder.Entity<StatusRequestHeroRegistration>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Администраторы

        //Администраторы
        modelBuilder.Entity<Administrator>();

        //Капитулы
        modelBuilder.Entity<Chapter>();

        //Должности
        modelBuilder.Entity<Post>().HasIndex(x => x.Alias).IsUnique();

        //Звания
        modelBuilder.Entity<Rank>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Летоисчисление

        //Сезоны
        modelBuilder.Entity<Season>().HasIndex(x => x.Alias).IsUnique();

        //Месяцы
        modelBuilder.Entity<Month>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Внешность

        //Цвета волос
        modelBuilder.Entity<HairsColor>().HasIndex(x => x.Alias).IsUnique();

        //Цвета глаз
        modelBuilder.Entity<EyesColor>().HasIndex(x => x.Alias).IsUnique();

        //Типы телосложений
        modelBuilder.Entity<TypeBody>().HasIndex(x => x.Alias).IsUnique();

        //Типы лиц
        modelBuilder.Entity<TypeFace>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Информационные статьи

        //Разделы информационных статей
        modelBuilder.Entity< SectionInformationArticle>().HasIndex(x => x.Alias).IsUnique();

        /// Оглавления информационных статей
        modelBuilder.Entity<HeaderInformationArticle>();

        //Детальные части информационных статей
        modelBuilder.Entity<DetailInformationArticle>();

        #endregion

        #region Политика

        //Области
        modelBuilder.Entity<Area>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Area>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Страны
        modelBuilder.Entity<Country>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Country>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Фракции
        modelBuilder.Entity<Fraction>().HasIndex(x => x.Alias).IsUnique();
        modelBuilder.Entity<Fraction>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Организации
        modelBuilder.Entity<Organization>();

        //Владения
        modelBuilder.Entity<Ownership>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Ownership>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Регионы
        modelBuilder.Entity<Region>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Region>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Регионы владений
        modelBuilder.Entity<RegionOwnership>().HasIndex(x => new { x.RegionId, x.OwnershipId }).IsUnique();

        //Типы организаций
        modelBuilder.Entity<TypeOrganization>().HasIndex(x => x.Alias).IsUnique();

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

        //Расы
        modelBuilder.Entity<Race>().HasIndex(x => x.Alias).IsUnique();

        //Нации
        modelBuilder.Entity<Nation>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Социология

        //Префиксы имён
        modelBuilder.Entity<PrefixName>().HasIndex(x => x.Alias).IsUnique();

        //Префиксы имён наций
        modelBuilder.Entity<PrefixNameNation>().HasIndex(x => new { x.PrefixNameId, x.NationId }).IsUnique();

        #endregion

        #region Файлы

        //Файлы
        modelBuilder.Entity<File>();

        //Файлы персонажей
        modelBuilder.Entity<FileHero>().HasIndex(x => new { x.FileId, x.HeroId }).IsUnique();

        //Типы файлов
        modelBuilder.Entity<TypeFile>().HasIndex(x => x.Alias).IsUnique();

        //Файлы детальных частей информационных статей
        modelBuilder.Entity<FileDetailInformationArticle>().HasIndex(x => new { x.FileId, x.DetailInformationArticleId }).IsUnique();

        #endregion

        #region География

        //Географические объекты
        modelBuilder.Entity<GeographicalObject>().HasIndex(x => x.ColorOnMap).IsUnique();

        //Типы географических объектов
        modelBuilder.Entity<TypeGeographicalObject>().HasIndex(x => x.Alias).IsUnique();

        #endregion

        #region Карта

        #endregion

        #region Культура

        #endregion

        #region Технологии

        #endregion

    }

    /// <summary>
    /// Конструктор контекста
    /// </summary>
    /// <param name="options"></param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Создаём базу и накатываем первоначальные таблицы
        Database.Migrate();
    }
}