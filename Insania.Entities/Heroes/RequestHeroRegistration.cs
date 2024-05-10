using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Heroes;

/// <summary>
/// ������ �������� ������ �� ����������� ���������
/// </summary>
[Table("re_requests_heroes_registration")]
[Comment("������ �� ����������� ���������")]
public class RequestHeroRegistration : Reestr
{
    /// <summary>
    /// ������ �� ���������
    /// </summary>
    [Column("hero_id")]
    [Comment("������ �� ���������")]
    public long HeroId { get; private set; }

    /// <summary>
    /// ������������� �������� ���������
    /// </summary>
    public Hero Hero { get; private set; }

    /// <summary>
    /// ������ �� �������
    /// </summary>
    [Column("status_id")]
    [Comment("������ �� ������")]
    public long StatusId { get; private set; }

    /// <summary>
    /// ������������� �������� �������
    /// </summary>
    public StatusRequestsHeroRegistration Status { get; private set; }


}