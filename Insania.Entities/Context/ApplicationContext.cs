using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Insania.Entities.Administrators;
using Insania.Entities.Appearance;
using Insania.Entities.Chronology;
using Insania.Entities.Files;
using File = Insania.Entities.Files.File;
using Insania.Entities.Heroes;
using Insania.Entities.Players;
using Insania.Entities.Politics;
using Insania.Entities.Sociology;
using Insania.Entities.System;
using Insania.Entities.Users;

namespace Insania.Entities.Context;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, 
    IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, 
    IdentityUserToken<long>>
{

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
    public DbSet<BiographyHeroes> BiographiesHeroes { get; set; }

    /// <summary>
    /// Статусы заявок на регистрацию персонажа
    /// </summary>
    public DbSet<StatusRequestsHeroRegistration> StatusesRequestsHeroRegistrations { get; set; }

    /// <summary>
    /// Заявки на регистрацию персонажа
    /// </summary>
    public DbSet<RequestHeroRegistration> RequestsHeroRegistration { get; set; }

    /// <summary>
    /// Биографии заявок на регистрацию персонажа
    /// </summary>
    public DbSet<BiographyRequestHeroRegistration> BiographiesRequestsHeroRegistration { get; set; }

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
    public DbSet<HairColor> HairColors { get; set; }

    /// <summary>
    /// Цвета глаз
    /// </summary>
    public DbSet<EyeColor> EyeColors { get; set; }

    /// <summary>
    /// Типы телосложений
    /// </summary>
    public DbSet<TypeBody> TypeBodies { get; set; }

    /// <summary>
    /// Типы лиц
    /// </summary>
    public DbSet<TypeFace> TypeFaces { get; set; }

    #endregion

    #region Политика

    /// <summary>
    /// Типы организаций
    /// </summary>
    public DbSet<TypeOrganization> TypiesOrganizations { get; set; }

    /// <summary>
    /// Организации
    /// </summary>
    public DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Страны
    /// </summary>
    public DbSet<Country> Countries { get; set; }

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

    #region Файлы

    /// <summary>
    /// Типы файлов
    /// </summary>
    public DbSet<TypeFile> TypeFiles { get; set; }

    /// <summary>
    /// Файлы
    /// </summary>
    public DbSet<File> Files { get; set; }

    /// <summary>
    /// Файлы персонажей
    /// </summary>
    public DbSet<FileHero> FileHeroes { get; set; }

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