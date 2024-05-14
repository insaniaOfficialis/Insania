using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Chronology;

/// <summary>
/// Модель сущности сезона
/// </summary>
[Table("dir_seasons")]
[Comment("Сезоны")]
public class Season : Guide
{
    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности сезона
    /// </summary>
    public Season() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности сезона без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public Season(string user, string name, int sequenceNumber) : base(user, name)
    {
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Конструктор модели сущности сезона c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="sequenceNumber">Порядковый номер</param>
    public Season(long id, string user, string name, int sequenceNumber) : base(id, user, name)
    {
        SequenceNumber = sequenceNumber;
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