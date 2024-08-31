using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.OutCategories;

namespace Insania.Database.Entities.Feedback;

/// <summary>
/// Модель сущности статуса обратной связи
/// </summary>
[Table("dir_statuses_feedback")]
[Comment("Статусы обратной связи")]
public class StatusFeedback : Guide
{
    /// <summary>
    /// Простой конструктор модели сущности статуса обратной связи
    /// </summary>
    public StatusFeedback() : base()
    {

    }
}