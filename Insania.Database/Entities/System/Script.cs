using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;
using Insania.Models.Exceptions;
using Insania.Models.Logging;

namespace Insania.Database.Entities.System;

/// <summary>
/// Модель сущности скрипта
/// </summary>
[Table("re_scripts")]
[Comment("Скрипты")]
public class Script : Reestr
{
    /// <summary>
    /// Наименование
    /// </summary>
    [Column("name")]
    [Comment("Наименование")]
    public string Name { get; private set; }

    /// <summary>
    /// Успешность
    /// </summary>
    [Column("is_success")]
    [Comment("Успешность")]
    public bool? IsSuccess { get; private set; }

    /// <summary>
    /// Результат выполнения
    /// </summary>
    [Column("result_execution")]
    [Comment("Результат выполнения")]
    public string? ResultExecution { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности файла
    /// </summary>
    public Script() : base()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности скрипта без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="isSuccess">Успешность</param>
    /// <param name="resultExecution">Результат выполнения</param>
    public Script(string user, bool isSystem, string name, bool? isSuccess,
        string? resultExecution) : base(user, isSystem)
    {
        Name = name;
        IsSuccess = isSuccess;
        ResultExecution = resultExecution;
    }

    /// <summary>
    /// Конструктор модели сущности скрипта c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="name">Наименование</param>
    /// <param name="isSuccess">Успешность</param>
    /// <param name="resultExecution">Результат выполнения</param>
    public Script(long id, string user, bool isSystem, string name, bool? isSuccess,
        string? resultExecution) : base(id, user, isSystem)
    {
        Name = name;
        IsSuccess = isSuccess;
        ResultExecution = resultExecution;
    }

    /// <summary>
    /// Метод записи наименования
    /// </summary>
    /// <param name="name">Наименование</param>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Метод записи результата
    /// </summary>
    /// <param name="isSuccess">Успешность</param>
    /// <param name="resultExecution">Результат выполнения</param>
    public void SetResult(bool isSuccess, string? resultExecution)
    {
        if (!isSuccess && string.IsNullOrEmpty(resultExecution))
            throw new InnerException(Errors.EpmtyResultExecution);

        IsSuccess = isSuccess;
        ResultExecution = resultExecution;
    }
}