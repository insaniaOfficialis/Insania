using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Heroes;

namespace Insania.Database.Entities.Files;

/// <summary>
/// Модель сущности файла персонажа
/// </summary>
[Table("un_files_heroes")]
[Comment("Файлы персонажей")]
public class FileHero : EntityFile
{
    /// <summary>
    /// Ссылка на персонажа
    /// </summary>
    [Column("hero_id")]
    [Comment("Ссылка на персонажа")]
    public long HeroId { get; private set; }

    /// <summary>
    /// Навигационное свойство персонажа
    /// </summary>
    public Hero Hero { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности файла персонажа
    /// </summary>
    public FileHero() : base()
    {
        Hero = new();
    }

    /// <summary>
    /// Конструктор модели сущности файла персонажа без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="hero">Ссылка на персонажа</param>
    public FileHero(string user, File file, Hero hero) : base(user, file)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// Конструктор модели сущности файла персонажа с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="hero">Ссылка на персонажа</param>
    public FileHero(long id, string user, File file, Hero hero) : base(id, user, file)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// Конструктор модели сущности файла персонажа без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    /// <param name="hero">Ссылка на персонажа</param>
    public FileHero(string user, File file, int sequenceNumber, Hero hero) : base(user, file,
        sequenceNumber)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// Конструктор модели сущности файла персонажа с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public FileHero(long id, string user, File file, int sequenceNumber, Hero hero) : base(id,
        user, file, sequenceNumber)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// Метод записи персонажа
    /// </summary>
    /// <param name="hero">Ссылка на персонажа</param>
    public void SetHero(Hero hero)
    {
        HeroId = hero.Id;
        Hero = hero;
    }
}