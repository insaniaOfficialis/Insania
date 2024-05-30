using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Biology;

using BaseEntity = Insania.Entities.OutCategories.Base;

namespace Insania.Database.Entities.Sociology;

/// <summary>
/// Модель сущности префикса имени нации
/// </summary>
[Table("un_prefixes_names_nations")]
[Comment("Префиксы имён")]
public class PrefixNameNation : BaseEntity
{
    /// <summary>
    /// Ссылка на префикс имени
    /// </summary>
    [Column("prefix_name_id")]
    [Comment("Ссылка на префикс имени")]
    public long PrefixNameId { get; private set; }

    /// <summary>
    /// Навигационное свойство префикса имени
    /// </summary>
    public PrefixName PrefixName { get; private set; }

    /// <summary>
    /// Ссылка на нацию
    /// </summary>
    [Column("nation_id")]
    [Comment("Ссылка на нацию")]
    public long NationId { get; private set; }

    /// <summary>
    /// Навигационное свойство нации
    /// </summary>
    public Nation Nation { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности префикса имени нации
    /// </summary>
    public PrefixNameNation() : base()
    {
        PrefixName = new();
        Nation = new();
    }

    /// <summary>
    /// Конструктор модели сущности префикса имени нации без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="prefixName">Префикс имени</param>
    /// <param name="nation">Нация</param>
    public PrefixNameNation(string user, PrefixName prefixName, Nation nation) : base(user)
    {
        PrefixNameId = prefixName.Id;
        PrefixName = prefixName;
        NationId = nation.Id;
        Nation = nation;
    }

    /// <summary>
    /// Конструктор модели сущности префикса имени нации с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="prefixName">Префикс имени</param>
    /// <param name="nation">Нация</param>
    public PrefixNameNation(long id, string user, PrefixName prefixName, Nation nation) : base(id, user)
    {
        PrefixNameId = prefixName.Id;
        PrefixName = prefixName;
        NationId = nation.Id;
        Nation = nation;
    }

    /// <summary>
    /// Метод записи префикса имени
    /// </summary>
    /// <param name="prefixName">Префикс имени</param>
    public void SetPrefixName(PrefixName prefixName)
    {
        PrefixNameId = prefixName.Id;
        PrefixName = prefixName;
    }

    /// <summary>
    /// Метод записи нации
    /// </summary>
    /// <param name="nation">Нация</param>
    public void SetNation(Nation nation)
    {
        NationId = nation.Id;
        Nation = nation;
    }
}