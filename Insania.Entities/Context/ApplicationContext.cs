using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Insania.Entities.Models.Administrators;
using Insania.Entities.Models.Appearance;
using Insania.Entities.Models.Biology;
using Insania.Entities.Models.Chronology;
using File = Insania.Entities.Models.Files.File;
using Insania.Entities.Models.Files;
using Insania.Entities.Models.Heroes;
using Insania.Entities.Models.Players;
using Insania.Entities.Models.Politics;
using Insania.Entities.Models.System;
using Insania.Entities.Models.Users;
using Insania.Entities.Models.Geography;

namespace Insania.Entities.Context;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, 
    IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, 
    IdentityUserToken<long>>
{
    #region Вне категорий

    #endregion

    #region Пользователи


    #endregion

    #region Системное

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
    public DbSet<StatusRequestHeroRegistration> StatusesRequestsHeroesRegistrations { get; set; }

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
        modelBuilder.Entity<IdentityUserRole<long>>().ToTable("sys_users_roles").HasKey(x => new { x.RoleId, x.UserId });
        modelBuilder.Entity<IdentityUser<long>>().ToTable("sys_users");
        modelBuilder.Entity<IdentityRole<long>>().ToTable("sys_roles");
        modelBuilder.Entity<IdentityUserClaim<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityUserLogin<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityRoleClaim<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
        modelBuilder.Entity<IdentityUserToken<long>>().HasNoKey().Metadata.SetIsTableExcludedFromMigrations(true);
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