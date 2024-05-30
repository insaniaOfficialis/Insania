using System.Text.RegularExpressions;

using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

namespace Insania.Models.Users.Users;

/// <summary>
/// Модель запроса добавления пользователя
/// </summary>
public partial class AddUserRequest
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
    /// Маска номера телефона
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex RegexPhoneNumber();

    /// <summary>
    /// Маска почты
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^\S+@\w+.\w+$")]
    private static partial Regex RegexEmail();

    /// <summary>
    /// Маска ссылки в вк
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^https://vk.com/\S+$")]
    private static partial Regex RegexLinkVK();

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
        //Обрабатываем входные данные
        if (!string.IsNullOrWhiteSpace(phoneNumber)) phoneNumber = FormattingPhoneNumber(phoneNumber);

        //Проверяем входные данные
        if (string.IsNullOrWhiteSpace(login)) throw new InnerException(Errors.EmptyLogin);
        if (string.IsNullOrWhiteSpace(password)) throw new InnerException(Errors.EmptyPassword);
        if (string.IsNullOrWhiteSpace(lastName)) throw new InnerException(Errors.EmptyLastName);
        if (string.IsNullOrWhiteSpace(firstName)) throw new InnerException(Errors.EmptyFirstName);
        if (string.IsNullOrWhiteSpace(patronymic)) throw new InnerException(Errors.EmptyPatronymic);
        if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(linkVK)) throw new InnerException(Errors.EmptyPhoneNumberEmailLinkVK);
        if (!string.IsNullOrWhiteSpace(phoneNumber) && !CheckingPhoneNumber(phoneNumber!)) throw new InnerException(Errors.IncorrectPhoneNumber);
        if (!string.IsNullOrWhiteSpace(email) && !CheckingEmail(email!)) throw new InnerException(Errors.IncorrectEmail);
        if (!string.IsNullOrWhiteSpace(linkVK) && !CheckingLinkVK(linkVK!)) throw new InnerException(Errors.IncorrectLinkVK);
        if (birthDate > DateTime.Now.AddYears(-16).AddDays(1)) throw new InnerException(string.Format("{0} {1}", Errors.IncorrectBirthDate, DateTime.Now.AddYears(-16).AddDays(1).ToString("dd.MM.yyyy")));

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

    /// <summary>
    /// Метод форматирования номера телефона
    /// </summary>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <returns></returns>
    public static string FormattingPhoneNumber(string phoneNumber)
    {
        return phoneNumber.Trim().Replace("+7", "8").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
    }

    /// <summary>
    /// Метод проверки номера телефона
    /// </summary>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <returns></returns>
    public static bool CheckingPhoneNumber(string phoneNumber)
    {
        Regex regex = RegexPhoneNumber();
        return regex.IsMatch(phoneNumber);
    }

    /// <summary>
    /// Метод проверки почты
    /// </summary>
    /// <param name="email">Почта</param>
    /// <returns></returns>
    public static bool CheckingEmail(string email)
    {
        Regex regex = RegexEmail();
        return regex.IsMatch(email);
    }

    /// <summary>
    /// Метод проверки ссылки в вк
    /// </summary>
    /// <param name="linkVK">Ссылка в вк</param>
    /// <returns></returns>
    public static bool CheckingLinkVK(string linkVK)
    {
        Regex regex = RegexLinkVK();
        return regex.IsMatch(linkVK);
    }
}