using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Administrators;

/// <summary>
/// Модель сущности администратора
/// </summary>
[Table("re_administrators")]
[Comment("Администраторы")]
public class Administrator : Reestr
{
    /// <summary>
    /// Ссылка на должность
    /// </summary>
    [Column("post_id")]
    [Comment("Ссылка на должность")]
    public long PostId { get; private set; }

    /// <summary>
    /// Навигационное свойство должности
    /// </summary>
    public Post Post { get; private set; }

    /// <summary>
    /// Ссылка на звание
    /// </summary>
    [Column("rank_id")]
    [Comment("Ссылка на звание")]
    public long RankId { get; private set; }

    /// <summary>
    /// Навигационное свойство звания
    /// </summary>
    public Rank Rank { get; private set; }
}