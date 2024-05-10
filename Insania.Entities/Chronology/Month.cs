using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Chronology;

/// <summary>
/// Модель сущности месяца
/// </summary>
[Table("dir_months")]
[Comment("Месяцы")]
public class Month : Guide
{
    /// <summary>
    /// Ссылка на сезон
    /// </summary>
    [Column("season_id")]
    [Comment("Ссылка на сезон")]
    public long SeasonId { get; private set; }

    /// <summary>
    /// Навигационное свойство сезонов
    /// </summary>
    public Season Season { get; private set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности месяца
    /// </summary>
    public Month() : base()
    {
        Season = new();
    }

    /// <summary>
    /// Конструктор модели сущности месяца без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="season">Ссылка на сезон</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public Month(string user, string name, Season season, int sequenceNumber) : base(user, name)
    {
        SeasonId = season.Id;
        Season = season;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Конструктор модели сущности месяца c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="season">Ссылка на сезон</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public Month(long id, string user, string name, Season season, int sequenceNumber) : base(id, user, name)
    {
        SeasonId = season.Id;
        Season = season;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Метод записи ссылки на сезон
    /// </summary>
    /// <param name="season">Ссылка на сезон</param>
    public void SetSeason(Season season)
    {
        SeasonId = season.Id;
        Season = season;
    }

    /// <summary>
    /// Метод записи порядкового номера
    /// </summary>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public void SetSequenceNumber(int sequenceNumber)
    {
        SequenceNumber = sequenceNumber;
    }
}