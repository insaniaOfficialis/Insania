using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Users;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Models.Users.Users;

namespace Insania.BusinessLogic.Users.Users;

/// <summary>
/// Сервис работы с пользователями
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="userManager">Менеджер пользователей</param>
/// <param name="logger">Интерфейс сервиса записи логов</param>
public class UsersService(ApplicationContext applicationContext, UserManager<User> userManager, ILogger<UsersService> logger) : 
    IUsers
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Интерфейс сервиса записи логов
    /// </summary>
    private readonly ILogger<UsersService> _logger = logger;

    /// <summary>
    /// Метод добавления пользователя
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    public async Task<BaseResponse> AddUser(AddUserRequest? request)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredAddUserMethod);

        //Проверяем наличие запроса
        if (request == null) throw new InnerException(Errors.EmptyRequest);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Создаём новый экземпляр пользователя
            User user = new(request.Login!, request.Password, request.PhoneNumber, request.LinkVK, false, (request.Gender ?? true),
                request.LastName!, request.FirstName!, request.Patronymic!, request.BirthDate);

            var result = await _userManager.CreateAsync(user, request.Password!) ?? throw new InnerException(Errors.EmptySeason);

            //Если не успешно, выдаём ошибку
            if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);

            //Сохраняем результат
            await _applicationContext.SaveChangesAsync();

            //Фиксируем транзакцию
            transaction.Commit();

            //Логгируем
            _logger.LogInformation(Informations.UserAdded);

            //Возвращаем ответ с id
            return new BaseResponse(true, user.Id);
        }
        catch (InnerException ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
    }

    /// <summary>
    /// Метод проверки доступности логина
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponse> CheckLogin(string? login)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredAddUserMethod);

        //Проверяем наличие запроса
        if (login == null) throw new InnerException(Errors.EmptyLogin);

        try
        {
            //Ищем логин
            bool success = (await _userManager.FindByNameAsync(login)) == null;

            //Если нашли возвращаем ошибку
            if (!success) throw new InnerException(Errors.LoginAlreadyExists);

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем ответ
            return new BaseResponse(success);
        }
        catch (InnerException ex)
        {

            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
        catch (Exception ex)
        {
            //Логгируем
            _logger.LogError("{errors} {text}", Errors.Error, ex);

            //Прокидываем ошибку
            throw;
        }
    }
}