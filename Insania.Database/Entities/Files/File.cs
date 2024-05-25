using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Files;

/// <summary>
/// Модель сущности файла
/// </summary>
[Table("re_files")]
[Comment("Файлы")]
public class File : Reestr
{
    /// <summary>
    /// Наименование 
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Расширение
    /// </summary>
    [Column("extention")]
    [Comment("Расширение")]
    public string Extention { get; private set; }

    /// <summary>
    /// Ссылка на тип
    /// </summary>
    [Column("type_id")]
    [Comment("Ссылка на тип")]
    public long TypeId { get; private set; }

    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public TypeFile Type { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности файла
    /// </summary>
    public File() : base()
    {
        Name = string.Empty;
        Extention = string.Empty;
        Type = new();
    }

    /// <summary>
    /// Конструктор модели сущности файла без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="type">Ссылка на тип</param>
    public File(string user, bool isSystem, string name, TypeFile type) : base(user, isSystem)
    {
        Name = name;
        Type = type;
        TypeId = type.Id;
        Extention = name[(name.LastIndexOf('.') + 1)..];
    }

    /// <summary>
    /// Конструктор модели сущности файла c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="type">Ссылка на тип</param>
    public File(long id, string user, bool isSystem, string name, TypeFile type) : base(id,
        user, isSystem)
    {
        Name = name;
        Type = type;
        TypeId = type.Id;
        Extention = name[(name.LastIndexOf('.') + 1)..];
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
        Extention = name[(name.LastIndexOf('.') + 1)..];
    }

    /// <summary>
    /// Метод записи типа
    /// </summary>
    /// <param name="type">Ссылка на тип</param>
    public void SetType(TypeFile type)
    {
        TypeId = type.Id;
        Type = type;
    }
}