using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using BaseEntity = Insania.Entities.Base.Base;

namespace Insania.Entities.Files;

/// <summary>
/// Модель сущности файла сущности
/// </summary>
public abstract class EntityFile : BaseEntity
{
    /// <summary>
    /// Ссылка на файл
    /// </summary>
    [Column("file_id")]
    [Comment("Ссылка на файл")]
    public long FileId { get; private set; }

    /// <summary>
    /// Навигационное свойство файла
    /// </summary>
    public File File { get; private set; }

    /// <summary>
    /// Порядковый номер
    /// </summary>
    [Column("sequence_number")]
    [Comment("Порядковый номер")]
    public int? SequenceNumber { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности файла сущности
    /// </summary>
    public EntityFile() : base()
    {
        File = new();
    }

    /// <summary>
    /// Конструктор модели сущности файла сущности без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    public EntityFile(string user, File file) : base(user)
    {
        FileId = file.Id;
        File = file;
    }

    /// <summary>
    /// Конструктор модели сущности файла сущности с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    public EntityFile(long id, string user, File file) : base(id, user)
    {
        FileId = file.Id;
        File = file;
    }

    /// <summary>
    /// Конструктор модели сущности файла сущности без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public EntityFile(string user, File file, int sequenceNumber) : base(user)
    {
        FileId = file.Id;
        File = file;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Конструктор модели сущности файла сущности с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public EntityFile(long id, string user, File file, int sequenceNumber) : base(id, user)
    {
        FileId = file.Id;
        File = file;
        SequenceNumber = sequenceNumber;
    }

    /// <summary>
    /// Метод записи файла
    /// </summary>
    /// <param name="file">Ссылка на файл</param>
    public void SetFile(File file)
    {
        FileId = file.Id;
        File = file;
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