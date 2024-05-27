using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Users.Users;

/// <summary>
/// Модель запроса добавления пользователя
/// </summary>
public class AddUserRequest
{
    /// <summary>
    /// Логин
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Пол (истина - мужской/ложь - женский)
    /// </summary>
    public bool? Gender { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Ссылка в вк
    /// </summary>
    public string? LinkVK { get; set; }

    /// <summary>
    /// Простой конструктор модели запроса добавления пользователя
    /// </summary>
    public AddUserRequest()
    {
        
    }

    /// <summary>
    /// Конструктор модели запроса добавления пользователя
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="firstName">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="gender">Пол (истина - мужской/ложь - женский)</param>
    /// <param name="birthDate">Дата рождения</param>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="email">Почта</param>
    /// <param name="linkVK">Ссылка в вк</param>
    /// <exception cref="InnerException"></exception>
    public AddUserRequest(string? login, string? password, string? lastName, string? firstName, string? patronymic, bool? gender,
        DateTime? birthDate, string? phoneNumber, string? email, string? linkVK)
    {
        //Проверяем входящие данные
        if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);
        if (string.IsNullOrWhiteSpace(password)) throw new InnerException(Errors.EmptyPassword);
        if (string.IsNullOrWhiteSpace(lastName)) throw new InnerException(Errors.EmptyLastName);
        if (string.IsNullOrWhiteSpace(firstName)) throw new InnerException(Errors.EmptyFirstName);
        if (string.IsNullOrWhiteSpace(patronymic)) throw new InnerException(Errors.EmptyPatronymic);
        if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(linkVK)) throw new InnerException(Errors.EmptyPhoneNumberEmailLinkVK);

        Gender = gender ?? throw new InnerException(Errors.EmptyGender);
        BirthDate = birthDate ?? throw new InnerException(Errors.EmptyBirthDate);
        Login = login;
        Password = password;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        PhoneNumber = phoneNumber;
        Email = email;
        LinkVK = linkVK;
    }
}