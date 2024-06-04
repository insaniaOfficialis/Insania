using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Insania.Entities.OutCategories;
using Insania.Database.Entities.Administrators;

namespace Insania.Database.Entities.Heroes;

/// <summary>
/// ������ �������� ������ �� ����������� ���������
/// </summary>
[Table("re_requests_heroes_registration")]
[Comment("������ �� ����������� ����������")]
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
    public StatusRequestHeroRegistration Status { get; private set; }

    /// <summary>
    /// ������ �� �������������� ��������������
    /// </summary>
    [Column("administrator_id")]
    [Comment("������ �� �������������� ��������������")]
    public long? AdministratorId { get; private set; }

    /// <summary>
    /// ������������� �������� �������������� ��������������
    /// </summary>
    public Administrator? Administrator { get; private set; }

    /// <summary>
    /// ������� �� ����� �����
    /// </summary>
    [Column("general_block_decision")]
    [Comment("������� �� ����� �����")]
    public bool? GeneralBlockDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� �����
    /// </summary>
    [Column("comment_on_general_block")]
    [Comment("����������� � ����� �����")]
    public string? CommentOnGeneralBlock { get; private set; }

    /// <summary>
    /// ������� �� ����� ���� ��������
    /// </summary>
    [Column("birth_date_block_decision")]
    [Comment("������� �� ����� ���� ��������")]
    public bool? BirthDateBlockDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� ���� ��������
    /// </summary>
    [Column("comment_on_birth_date_block")]
    [Comment("����������� � ����� ���� ��������")]
    public string? CommentOnBirthDateBlock { get; private set; }

    /// <summary>
    /// ������� �� ����� ��������������
    /// </summary>
    [Column("location_block_decision")]
    [Comment("������� �� ����� ��������������")]
    public bool? LocationBlockDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� ��������������
    /// </summary>
    [Column("comment_on_location_block")]
    [Comment("����������� � ����� ��������������")]
    public string? CommentOnLocationBlock { get; private set; }

    /// <summary>
    /// ������� �� ����� ���������
    /// </summary>
    [Column("appearance_block_decision")]
    [Comment("������� �� ����� ���������")]
    public bool? AppearanceBlockDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� ���������
    /// </summary>
    [Column("comment_on_appearance_block")]
    [Comment("����������� � ����� ���������")]
    public string? CommentOnAppearanceBlock { get; private set; }

    /// <summary>
    /// ������� �� ����� �����������
    /// </summary>
    [Column("image_block_decision")]
    [Comment("������� �� ����� �����������")]
    public bool? ImageBlockDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� �����������
    /// </summary>
    [Column("comment_on_image_block")]
    [Comment("����������� � ����� �����������")]
    public string? CommentOnImageBlock { get; private set; }

    /// <summary>
    /// ������� ����������� ������ �������� ������ �� ����������� ���������
    /// </summary>
    public RequestHeroRegistration() : base()
    {
        Hero = new();
        Status = new();
    }

    /// <summary>
    /// ����������� ������ �������� ������ �� ����������� ��������� ��� id
    /// </summary>
    /// <param name="user">������������, ����������</param>
    /// <param name="isSystem">������� ��������� ������</param>
    /// <param name="hero">������ �� ���������</param>
    /// <param name="status">������ �� ������</param>
    public RequestHeroRegistration(string user, bool isSystem, Hero hero, StatusRequestHeroRegistration status) : base(user, 
        isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// ����������� ������ �������� ������ �� ����������� ��������� c id
    /// </summary>
    /// <param name="id">��������� ���� �������</param>
    /// <param name="user">������������, ����������</param>
    /// <param name="isSystem">������� ��������� ������</param>
    /// <param name="hero">������ �� ���������</param>
    /// <param name="status">������ �� ������</param>
    public RequestHeroRegistration(long id, string user, bool isSystem, Hero hero, StatusRequestHeroRegistration status) : 
        base(id, user, isSystem)
    {
        HeroId = hero.Id;
        Hero = hero;
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// ����� ������ ���������
    /// </summary>
    /// <param name="hero">������ �� ���������</param>
    public void SetHero(Hero hero)
    {
        HeroId = hero.Id;
        Hero = hero;
    }

    /// <summary>
    /// ����� ������ �������
    /// </summary>
    /// <param name="status">������ �� ������</param>
    public void SetStatus(StatusRequestHeroRegistration status)
    {
        StatusId = status.Id;
        Status = status;
    }

    /// <summary>
    /// ����� ������ �������������� ��������������
    /// </summary>
    /// <param name="administrator">������ �� �������������� ��������������</param>
    public void SetAdministrator(Administrator administrator)
    {
        AdministratorId = administrator.Id;
        Administrator = administrator;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� �����
    /// </summary>
    /// <param name="decision">�������</param>
    /// <param name="decision">�����������</param>
    public void SetGeneralBlockDecision(bool decision, string? comment)
    {
        GeneralBlockDecision = decision;
        CommentOnGeneralBlock = comment;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� ���� ��������
    /// </summary>
    /// <param name="decision">�������</param>
    /// <param name="decision">�����������</param>
    public void SetBirthDateBlockDecision(bool decision, string? comment)
    {
        BirthDateBlockDecision = decision;
        CommentOnBirthDateBlock = comment;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� ��������������
    /// </summary>
    /// <param name="decision">�������</param>
    /// <param name="decision">�����������</param>
    public void SetLocationBlockDecision(bool decision, string? comment)
    {
        LocationBlockDecision = decision;
        CommentOnLocationBlock = comment;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� ��������������
    /// </summary>
    /// <param name="decision">�������</param>
    /// <param name="decision">�����������</param>
    public void SetAppearanceBlockDecision(bool decision, string? comment)
    {
        AppearanceBlockDecision = decision;
        CommentOnAppearanceBlock = comment;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� �����������
    /// </summary>
    /// <param name="decision">�������</param>
    /// <param name="decision">�����������</param>
    public void SetImageBlockDecision(bool decision, string? comment)
    {
        ImageBlockDecision = decision;
        CommentOnImageBlock = comment;
    }
}