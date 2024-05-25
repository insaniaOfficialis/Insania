using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.System;

/// <summary>
/// Модель сущности записи лога
/// </summary>
[Table("re_logs")]
[Comment("Логи")]
public class Log : Reestr
{
    /// <summary>
    /// Наименование вызываемого метода
    /// </summary>
    [Column("method")]
    [Comment("Наименование вызываемого метода")]
    public string Method { get; private set; }

    /// <summary>
    /// Тип вызываемого метода
    /// </summary>
    [Column("type")]
    [Comment("Тип вызываемого метода")]
    public string Type { get; private set; }

    /// <summary>
    /// Признак успешного выполнения
    /// </summary>
    [Column("success")]
    [Comment("Признак успешного выполнения")]
    public bool Success { get; private set; }

    /// <summary>
    /// Дата начала
    /// </summary>
    [Column("date_start")]
    [Comment("Дата начала")]
    public DateTime DateStart { get; private set; }

    /// <summary>
    /// Дата окончания
    /// </summary>
    [Column("date_end")]
    [Comment("Дата окончания")]
    public DateTime? DateEnd { get; private set; }

    /// <summary>
    /// Данные на вход
    /// </summary>
    [Column("data_in")]
    [Comment("Данные на вход")]
    public string? DataIn { get; private set; }

    /// <summary>
    /// Данные на выход
    /// </summary>
    [Column("data_out")]
    [Comment("Данные на выход")]
    public string? DataOut { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности записи лога
    /// </summary>
    public Log() : base()
    {
        Method = string.Empty;
        Type = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности записи лога без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="method">Наименование вызываемого метода</param>
    /// <param name="type">Тип вызываемого метода</param>
    /// <param name="dataIn">Данные на вход</param>
    public Log(string user, bool isSystem, string method, string type, string? dataIn) : base(user, isSystem)
    {
        Method = method;
        DataIn = dataIn;
        Type = type;
        DateStart = DateTime.UtcNow;
    }

    /// <summary>
    /// Конструктор модели сущности сущности записи лога с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="method">Наименование вызываемого метода</param>
    /// <param name="type">Тип вызываемого метода</param>
    /// <param name="dataIn">Данные на вход</param>
    public Log(long id, string user, bool isSystem, string method, string type, string dataIn) : base(id, user, isSystem)
    {
        Method = method;
        DataIn = dataIn;
        Type = type;
        DateStart = DateTime.UtcNow;
    }

    /// <summary>
    /// Метод записи завершения выполнения
    /// </summary>
    /// <param name="success">Признак успешного выполнения</param>
    /// <param name="dataOut">Данные на выход</param>
    public void SetEnd(bool success, string? dataOut)
    {
        Success = success;
        DataOut = dataOut;
        DateEnd = DateTime.UtcNow;
    }
}