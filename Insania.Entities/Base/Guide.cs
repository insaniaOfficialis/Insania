using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Supporting.Transliteration;

namespace Insania.Entities.Base;

/// <summary>
/// Модель сущности справочника
/// </summary>
public abstract class Guide : Base
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Английское наименование
    /// </summary>
    [Column("alias")]
    [Comment("Английское наименование")]
    public string Alias { get; private set; }

    /// <summary>
    /// Внутренний экземпляр сервиса транслитерации
    /// </summary>
    private ITransliteration? _transliteration;

    /// <summary>
    /// Внешний экземпляр сервиса транслитерации
    /// </summary>
    protected ITransliteration Transliteration => _transliteration ??= GetTransliteration();

    /// <summary>
    /// Простой конструктор модели сущности справочника
    /// </summary>
    public Guide() : base()
    {
        Name = string.Empty;
        Alias = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности справочника без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    public Guide(string user, string name) :
        base(user)
    {
        Name = name;
        Alias = Transliteration.Translit(name);
    }

    /// <summary>
    /// Конструктор модели сущности справочника с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    public Guide(long id, string user, string name):
        base(id, user)
    {
        Name = name;
        Alias = Transliteration.Translit(name);
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
        Alias = Transliteration.Translit(name);
    }

    /// <summary>
    /// Метод получения сервиса транслитерации
    /// </summary>
    /// <returns></returns>
    protected static ITransliteration GetTransliteration()
    {
        ITransliteration transliteration = new Transliteration();
        return transliteration;
    }
}