using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Files;

/// <summary>
/// Модель сущности типа файла
/// </summary>
[Table("dir_types_files")]
[Comment("Типы файла")]
public class TypeFile : Guide
{
    /// <summary>
    /// Путь
    /// </summary>
    [Column("path")]
    [Comment("Путь")]
    public string Path { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности типа файла
    /// </summary>
    public TypeFile() : base()
    {
        Path = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности типа файла без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="path">Путь</param>
    public TypeFile(string user, string name, string path) : base(user, name)
    {
        Path = path;
    }

    /// <summary>
    /// Конструктор модели сущности типа файла c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="path">Путь</param>
    public TypeFile(long id, string user, string name, string path) : base(id, user, name)
    {
        Path = path;
    }

    /// <summary>
    /// Метод записи порядкового номера
    /// </summary>
    /// <param name="path">Путь</param>
    public void SetPath(string path)
    {
        Path = path;
    }
}