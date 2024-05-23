using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;
using Insania.Entities.Base;
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
    /// ������� �� ������� �����
    /// </summary>
    [Column("personal_name_decision")]
    [Comment("������� �� ������� �����")]
    public bool? PersonalNameDecision { get; private set; }

    /// <summary>
    /// ����������� � ������� �����
    /// </summary>
    [Column("comment_on_personal_name")]
    [Comment("����������� � ������� �����")]
    public string? CommentOnPersonalName { get; private set; }

    /// <summary>
    /// ������� �� ����� �����
    /// </summary>
    [Column("family_name_decision")]
    [Comment("������� �� ����� �����")]
    public bool? FamilyNameDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� �����
    /// </summary>
    [Column("comment_on_family_name")]
    [Comment("����������� � ����� �����")]
    public string? CommentOnFamilyName { get; private set; }

    /// <summary>
    /// ������� �� ����
    /// </summary>
    [Column("race_decision")]
    [Comment("������� �� ����")]
    public bool? RaceDecision { get; private set; }

    /// <summary>
    /// ����������� � ����
    /// </summary>
    [Column("comment_on_race")]
    [Comment("����������� � ����")]
    public string? CommentOnRace { get; private set; }

    /// <summary>
    /// ������� �� �����
    /// </summary>
    [Column("nation_decision")]
    [Comment("������� �� �����")]
    public bool? NationDecision { get; private set; }

    /// <summary>
    /// ����������� � �����
    /// </summary>
    [Column("comment_on_nation")]
    [Comment("����������� � �����")]
    public string? CommentOnNation { get; private set; }

    /// <summary>
    /// ������� �� ���� ��������
    /// </summary>
    [Column("birth_date_decision")]
    [Comment("������� �� ���� ��������")]
    public bool? BirthDateDecision { get; private set; }

    /// <summary>
    /// ����������� � ���� ��������
    /// </summary>
    [Column("comment_on_birth_date")]
    [Comment("����������� � ���� ��������")]
    public string? CommentOnBirthDate { get; private set; }

    /// <summary>
    /// ������� �� ��������������
    /// </summary>
    [Column("location_decision")]
    [Comment("������� �� ��������������")]
    public bool? LocationDecision { get; private set; }

    /// <summary>
    /// ����������� � ��������������
    /// </summary>
    [Column("comment_on_location")]
    [Comment("����������� � ��������������")]
    public string? CommentOnLocation { get; private set; }

    /// <summary>
    /// ������� �� �����
    /// </summary>
    [Column("height_decision")]
    [Comment("������� �� �����")]
    public bool? HeightDecision { get; private set; }

    /// <summary>
    /// ����������� � �����
    /// </summary>
    [Column("comment_on_height")]
    [Comment("����������� � �����")]
    public string? CommentOnHeight { get; private set; }

    /// <summary>
    /// ������� �� ����
    /// </summary>
    [Column("weight_decision")]
    [Comment("������� �� ����")]
    public bool? WeightDecision { get; private set; }

    /// <summary>
    /// ����������� � ����
    /// </summary>
    [Column("comment_on_weight")]
    [Comment("����������� � ����")]
    public string? CommentOnWeight { get; private set; }

    /// <summary>
    /// ������� �� ���� ������������
    /// </summary>
    [Column("type_body_decision")]
    [Comment("������� �� ���� ������������")]
    public bool? TypeBodyDecision { get; private set; }

    /// <summary>
    /// ����������� � ���� ������������
    /// </summary>
    [Column("comment_on_type_body")]
    [Comment("����������� � ���� ������������")]
    public string? CommentOnTypeBody { get; private set; }

    /// <summary>
    /// ������� �� ���� ����
    /// </summary>
    [Column("type_face_decision")]
    [Comment("������� �� ���� ����")]
    public bool? TypeFaceDecision { get; private set; }

    /// <summary>
    /// ����������� � ���� ����
    /// </summary>
    [Column("comment_on_type_face")]
    [Comment("����������� � ���� ����")]
    public string? CommentOnTypeFace { get; private set; }

    /// <summary>
    /// ������� �� ����� �����
    /// </summary>
    [Column("hair_color_decision")]
    [Comment("������� �� ����� �����")]
    public bool? HairColorDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� �����
    /// </summary>
    [Column("comment_on_hair_color")]
    [Comment("����������� � ����� �����")]
    public string? CommentOnHairColor { get; private set; }

    /// <summary>
    /// ������� �� ����� ����
    /// </summary>
    [Column("eye_color_decision")]
    [Comment("������� �� ����� ����")]
    public bool? EyesColorDecision { get; private set; }

    /// <summary>
    /// ����������� � ����� ����
    /// </summary>
    [Column("comment_on_eye_color")]
    [Comment("����������� � ����� ����")]
    public string? CommentOnEyesColor { get; private set; }

    /// <summary>
    /// ������� �� �����������
    /// </summary>
    [Column("image_decision")]
    [Comment("������� �� �����������")]
    public bool? ImageDecision { get; private set; }

    /// <summary>
    /// ����������� � �����������
    /// </summary>
    [Column("comment_on_image")]
    [Comment("����������� � �����������")]
    public string? CommentOnImage { get; private set; }

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
    /// ����� ������ ������� �� ������������� �����
    /// </summary>
    /// <param name="personalNameDecision">������� �� ������� �����</param>
    /// <param name="commentOnPersonalName">����������� � ������� �����</param>
    public void SetPersonalNameDecision(bool personalNameDecision,
        string? commentOnPersonalName)
    {
        PersonalNameDecision = personalNameDecision;
        CommentOnPersonalName = commentOnPersonalName;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� �����
    /// </summary>
    /// <param name="familyNameDecision">������� �� ����� �����</param>
    /// <param name="commentOnFamilyName">����������� � ����� �����</param>
    public void SetFamilyNameDecision(bool familyNameDecision,
        string? commentOnFamilyName)
    {
        FamilyNameDecision = familyNameDecision;
        CommentOnFamilyName = commentOnFamilyName;
    }

    /// <summary>
    /// ����� ������ ������� �� ����
    /// </summary>
    /// <param name="raceDecision">������� �� ����</param>
    /// <param name="commentOnRace">����������� � ����</param>
    public void SetRaceDecision(bool raceDecision, string? commentOnRace)
    {
        RaceDecision = raceDecision;
        CommentOnRace = commentOnRace;
    }

    /// <summary>
    /// ����� ������ ������� �� �����
    /// </summary>
    /// <param name="nationDecision">������� �� �����</param>
    /// <param name="commentOnNation">����������� � �����</param>
    public void SetNationDecision(bool nationDecision, string? commentOnNation)
    {
        NationDecision = nationDecision;
        CommentOnNation = commentOnNation;
    }

    /// <summary>
    /// ����� ������ ������� �� ��������������
    /// </summary>
    /// <param name="locationDecision">������� �� ��������������</param>
    /// <param name="commentOnLocation">����������� � ��������������</param>
    public void SetLocationDecision(bool locationDecision, string? commentOnLocation)
    {
        LocationDecision = locationDecision;
        CommentOnLocation = commentOnLocation;
    }

    /// <summary>
    /// ����� ������ ������� �� �����
    /// </summary>
    /// <param name="heightDecision">������� �� �����</param>
    /// <param name="commentOnHeight">����������� � �����</param>
    public void SetHeightDecision(bool heightDecision, string? commentOnHeight)
    {
        HeightDecision = heightDecision;
        CommentOnHeight = commentOnHeight;
    }

    /// <summary>
    /// ����� ������ ������� �� ����
    /// </summary>
    /// <param name="weightDecision">������� �� ����</param>
    /// <param name="commentOnWeight">����������� � ����</param>
    public void SetWeightDecision(bool weightDecision, string? commentOnWeight)
    {
        WeightDecision = weightDecision;
        CommentOnWeight = commentOnWeight;
    }

    /// <summary>
    /// ����� ������ ������� �� ���� ������������
    /// </summary>
    /// <param name="typeBodyDecision">������� �� ���� ������������</param>
    /// <param name="commentOnTypeBody">����������� � ���� ������������</param>
    public void SetTypeBodyDescision(bool typeBodyDecision, string? commentOnTypeBody)
    {
        TypeBodyDecision = typeBodyDecision;
        CommentOnTypeBody = commentOnTypeBody;
    }

    /// <summary>
    /// ����� ������ ������� �� ���� ����
    /// </summary>
    /// <param name="typeFaceDecision">������� �� ���� ����</param>
    /// <param name="commentOnTypeFace">����������� � ���� ����</param>
    public void SetTypeFaceDescision(bool typeFaceDecision, string? commentOnTypeFace)
    {
        TypeFaceDecision = typeFaceDecision;
        CommentOnTypeFace = commentOnTypeFace;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� �����
    /// </summary>
    /// <param name="hairColorDecision">������� �� ����� �����</param>
    /// <param name="comentOnHairColor">����������� � ����� �����</param>
    public void SetHairColorDescision(bool hairColorDecision, string? comentOnHairColor)
    {
        HairColorDecision = hairColorDecision;
        CommentOnHairColor = comentOnHairColor;
    }

    /// <summary>
    /// ����� ������ ������� �� ����� ����
    /// </summary>
    /// <param name="eyeColorDecision">������� �� ����� ����</param>
    /// <param name="comentOnEyesColor">����������� � ����� ����</param>
    public void SetEyesColor(bool eyeColorDecision, string? comentOnEyesColor)
    {
        EyesColorDecision = eyeColorDecision;
        CommentOnEyesColor = comentOnEyesColor;
    }

    /// <summary>
    /// ����� ������ ������� �� �����������
    /// </summary>
    /// <param name="imageDecision">������� �� �����������</param>
    /// <param name="commentOnImage">����������� � �����������</param>
    public void SetImageDecision(bool imageDecision, string? commentOnImage)
    {
        ImageDecision = imageDecision;
        CommentOnImage = commentOnImage;
    }
}