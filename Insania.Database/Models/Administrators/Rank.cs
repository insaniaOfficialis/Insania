using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Models.Administrators;

/// <summary>
/// Модель сущности звания
/// </summary>
[Table("dir_ranks")]
[Comment("Звания")]
public class Rank : Guide
{
    /// <summary>
    /// Коэффициент начисления баллов почёта
    /// </summary>
    [Column("coefficient_accrual_honor_points")]
    [Comment("Коэффициент начисления баллов почёта")]
    public double CoefficientAccrualHonorPoints { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности звания
    /// </summary>
    public Rank() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности звания без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="name">Наименование</param>
    /// <param name="сoefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    public Rank(string user, string name, double сoefficientAccrualHonorPoints) : base(user,
        name)
    {
        CoefficientAccrualHonorPoints = сoefficientAccrualHonorPoints;
    }

    /// <summary>
    /// Конструктор модели сущности звания c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="name">Наименование</param>
    /// <param name="сoefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    public Rank(long id, string user, string name, double сoefficientAccrualHonorPoints) : base(id, user, name)
    {
        CoefficientAccrualHonorPoints = сoefficientAccrualHonorPoints;
    }

    /// <summary>
    /// Метод записи коэффициента начисления баллов почёта
    /// </summary>
    /// <param name="сoefficientAccrualHonorPoints">Коэффициент начисления баллов почёта</param>
    public void SetCoefficientAccrualHonorPoints(double сoefficientAccrualHonorPoints)
    {
        CoefficientAccrualHonorPoints = сoefficientAccrualHonorPoints;
    }
}