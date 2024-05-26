using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.Administrators;
using Insania.Database.Entities.Players;

namespace Insania.Database.Entities.Users;

/// <summary>
/// Модель сущности пользователя
/// </summary>
[Table("sys_users")]
[Comment("Пользователи")]
public class User : IdentityUser<long>
{
    /// <summary>
    /// Ссылка в вк
    /// </summary>
    [Comment("Ссылка в вк")]
    public string? LinkVK { get; private set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Comment("Фамилия")]
    public string LastName { get; private set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Comment("Имя")]
    public string FirstName { get; private set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [Comment("Отчество")]
    public string Patronymic { get; private set; }

    /// <summary>
    /// Признак заблокированного пользователя
    /// </summary>
    [Comment("Признак заблокированного пользователя")]
    public bool IsBlocked { get; private set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    [Comment("Пол (истина - мужской/ложь - женский)")]
    public bool Gender { get; private set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    [Comment("Дата рождения")]
    public DateTime? BirthDate { get; private set; }

    /// <summary>
    /// Полное имя
    /// </summary>
    [NotMapped]
    public string? FullName
    {
        get => (!string.IsNullOrWhiteSpace(FirstName) ? (FirstName + " ") : string.Empty) +
            (!string.IsNullOrWhiteSpace(Patronymic) ? (Patronymic + " ") : string.Empty) +
            LastName;
    }

    /// <summary>
    /// Инициалы
    /// </summary>
    [NotMapped]
    public string? Initials 
    { 
        get => (!string.IsNullOrWhiteSpace(FirstName) ? (FirstName[0] + ". ") : string.Empty) + 
            (!string.IsNullOrWhiteSpace(Patronymic) ? (Patronymic[0] + ". ") : string.Empty) + 
            LastName; 
    }

    /// <summary>
    /// Навигационное свойство игрока
    /// </summary>
    public Player? Player { get; private set; }

    /// <summary>
    /// Навигационное свойство администратора
    /// </summary>
    public Administrator? Administrator { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности пользователя
    /// </summary>
    public User()
    {
        LastName = string.Empty;
        FirstName = string.Empty;
        Patronymic = string.Empty;
    }

    /// <summary>
    /// Конструктор модели сущности пользователя без id
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="email">Почта</param>
    /// <param name="phone">Номер телефона</param>
    /// <param name="linkVk">Ссылка в вк</param>
    /// <param name="isBlocked">Признак заблокированного пользователя</param>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="firstName">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="birthDate">Дата рождения</param>
    public User(string login, string? email, string? phone, string? linkVk, bool isBlocked, bool gender,
        string lastName, string firstName, string patronymic, DateTime? birthDate)
    {
        UserName = login;
        Email = email;
        PhoneNumber = phone;
        LinkVK = linkVk;
        IsBlocked = isBlocked;
        Gender = gender;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        BirthDate = birthDate;
    }

    /// <summary>
    /// Конструктор модели сущности пользователя с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="login">Логин</param>
    /// <param name="email">Почта</param>
    /// <param name="phone">Номер телефона</param>
    /// <param name="linkVk">Ссылка в вк</param>
    /// <param name="isBlocked">Признак заблокированного пользователя</param>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="firstName">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="birthDate">Дата рождения</param>
    public User(long id, string login, string? email, string? phone, string? linkVk, bool isBlocked, bool gender,
        string lastName, string firstName, string patronymic, DateTime? birthDate) : this(login, email, phone, 
            linkVk, isBlocked, gender, lastName, firstName, patronymic, birthDate)
    {
        Id = id;
    }

    /// <summary>
    /// Метод записи почты
    /// </summary>
    /// <param name="email">Почта</param>
    public void SetEmail(string email)
    {
        Email = email;
    }

    /// <summary>
    /// Метод записи номера телефона
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    public void SetPhone(string phone)
    {
        PhoneNumber = phone;
    }

    /// <summary>
    /// Метод записи ссылки в вк
    /// </summary>
    /// <param name="linkVk">Ссылка в вк</param>
    public void SetLinkVk(string linkVk)
    {
        LinkVK = linkVk;
    }

    /// <summary>
    /// Метод записи фамилии
    /// </summary>
    /// <param name="lastName">Фамилия</param>
    public void SetLastName(string lastName)
    {
        LastName = lastName;
    }

    /// <summary>
    /// Метод записи имени
    /// </summary>
    /// <param name="firstName">Имя</param>
    public void SetFirstName(string firstName)
    {
        FirstName = firstName;
    }

    /// <summary>
    /// Метод записи отчества
    /// </summary>
    /// <param name="patronymic">Отчество</param>
    public void SetPatronymic(string patronymic)
    {
        Patronymic = patronymic;
    }

    /// <summary>
    /// Метод записи признака блокировки
    /// </summary>
    /// <param name="isBlocked">Признак заблокированного пользователя</param>
    public void SetIsBlocked(bool isBlocked)
    {
        IsBlocked = isBlocked;
    }

    /// <summary>
    /// Метод записи пола
    /// </summary>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    public void SetGender(bool gender)
    {
        Gender = gender;
    }

    /// <summary>
    /// Метод записи пола
    /// </summary>
    /// <param name="birthDate">Дата рождения</param>
    public void SetBirthDate(DateTime birthDate)
    {
        BirthDate = birthDate;
    }
}