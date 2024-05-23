using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using File = System.IO.File;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Insania.Entities.Context;
using Insania.Database.Entities.Administrators;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Files;
using Insania.Database.Entities.Geography;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Players;
using Insania.Database.Entities.Politics;
using Insania.Database.Entities.System;
using Insania.Database.Entities.Users;
using Insania.Models.Exceptions;
using Insania.Models.Logging;

using FileEntity = Insania.Database.Entities.Files.File;

namespace Insania.Initializer.Initialization.InitializationDataBase;

/// <summary>
/// Сервис инициализации базы данных
/// </summary>
/// <param name="roleManager">Менеджер ролей</param>
/// <param name="userManager">Менедже пользователей</param>
/// <param name="applicationContext">Контекст основной базы данных</param>
/// <param name="configuration">Интерфейс конфигурации</param>
public class InitializationDataBase(RoleManager<Role> roleManager, UserManager<User> userManager, 
    ApplicationContext applicationContext, IConfiguration configuration) : IInitializationDataBase
{
    /// <summary>
    /// Менеджер ролей
    /// </summary>
    private readonly RoleManager<Role> _roleManager = roleManager;

    /// <summary>
    /// Менеджер пользователей
    /// </summary>
    private readonly UserManager<User> _userManager = userManager;

    /// <summary>
    /// Контекст основной базы данных приложения
    /// </summary>
    private readonly ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс конфигурации
    /// </summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Путь к скрпитам
    /// </summary>
    private string? ScriptsPath { get; set; }

    /// <summary>
    /// Создающий пользователь
    /// </summary>
    private readonly string _userCreated = "initializer";

    #region Вне категорий

    /// <summary>
    /// Метод запуска инициализаций
    /// </summary>
    /// <returns></returns>
    public async Task Initialization()
    {
        try
        {
            //Логгируем
            Console.WriteLine(Informations.EnteredInitializationMethod);

            //Получаем путь к сриптам
            ScriptsPath = _configuration["ScriptsPath"]?.ToString();

            //Проверяем наличие пути к скриптам
            if (string.IsNullOrEmpty(ScriptsPath)) throw new InnerException(Errors.EmptyScriptsPath);

            //Логгируем
            Console.WriteLine("{0} {1}", Informations.ScriptsPath, ScriptsPath);

            //ПАРАМЕТРЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationParameters"])) await InitializationParameters();

            //РОЛИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRoles"])) await InitializationRoles();

            //ПОЛЬЗОВАТЕЛИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationUsers"])) await InitializationUsers();

            //РОЛИ ПОЛЬЗОВАТЕЛЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationUsersRoles"])) await InitializationUsersRoles();

            //ИГРОКИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationPlayers"])) await InitializationPlayers();

            //СЕЗОНЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationSeasons"])) await InitializationSeasons();

            //МЕСЯЦЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationMonths"])) await InitializationMonths();

            //РАСЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRaces"])) await InitializationRaces();

            //НАЦИИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationNations"])) await InitializationNations();

            //ЦВЕТА ВОЛОС
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationHairsColors"])) await InitializationHairsColors();

            //ЦВЕТА ГЛАЗ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationEyesColors"])) await InitializationEyesColors();

            //ТИПЫ ТЕЛОСЛОЖЕНИЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesBodies"])) await InitializationTypesBodies();

            //ТИПЫ ЛИЦ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesFaces"])) await InitializationTypesFaces();

            //ТИПЫ ФАЙЛОВ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesFiles"])) await InitializationTypesFiles();

            //ФАЙЛЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationFiles"])) await InitializationFiles();

            //СТАТУСЫ НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationStatusesRequestsHeroesRegistration"])) await InitializationStatusesRequestsHeroesRegistration();

            //ДОЛЖНОСТИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationPosts"])) await InitializationPosts();

            //ЗВАНИЯ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRanks"])) await InitializationRanks();

            //ТИПЫ ГЕОГРАФИЧЕСКИХ ОБЪЕКТОВ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesGeographicalObjects"])) await InitializationTypesGeographicalObjects();

            //ГЕОГРАФИЧЕСКИЕ ОБЪЕКТЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationGeographicalObjects"])) await InitializationGeographicalObjects();

            //ТИПЫ ОРГАНИЗАЦИЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesOrganizations"])) await InitializationTypesOrganizations();

            //ОРГАНИЗАЦИИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationOrganizations"])) await InitializationOrganizations();

            //СТРАНЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationCountries"])) await InitializationCountries();

            //РЕГИОНЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRegions"])) await InitializationRegions();

            //ВЛАДЕНИЯ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationOwnerships"])) await InitializationOwnerships();

            //РЕГИОНЫ ВЛАДЕНИЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRegionsOwnerships"])) await InitializationRegionsOwnerships();

            //ФРАКЦИИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationFractions"])) await InitializationFractions();

            //ОБЛАСТИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationAreas"])) await InitializationAreas();

            //КАПИТУЛЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationChapters"])) await InitializationChapters();

            //АДМИНИСТРАТОРЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationAdministrators"])) await InitializationAdministrators();

            //ПЕРСОНАЖИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationHeroes"])) await InitializationHeroes();

            //БИОГРАФИИ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationBiographiesHeroes"])) await InitializationBiographiesHeroes();

            //ЗАЯВКИ НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRequestsHeroesRegistration"])) await InitializationRequestsHeroesRegistration();

            //БИОГРАФИИ ЗАЯВОК НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationBiographiesRequestsHeroesRegistration"])) await InitializationBiographiesRequestsHeroesRegistration();

            //ФАЙЛЫ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationFilesHeroes"])) await InitializationFilesHeroes();
        }
        catch (Exception ex)
        {
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод выполнения скриптов
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task ExecuteScript(string filePath)
    {
        //Логгируем
        Console.WriteLine("{0} {1}", Informations.ExecuteScript, filePath);

        //Проверяем наличие скрипта в базе
        if (!await _applicationContext.Scripts.AnyAsync(x => x.Name == filePath && x.IsSuccess == true))
        {
            //Объявляем текст ошибки
            Exception? error = null;

            try
            {
                //Считываем запрос
                FormattableString sql = FormattableStringFactory.Create(File.ReadAllText(filePath));

                //Выполняем запрос
                await _applicationContext.Database.ExecuteSqlInterpolatedAsync(sql);
            }
            catch (Exception ex)
            {
                //Фиксируем ошибку
                error = ex;
            }

            //Фиксируем выполнение
            bool isSuccess = error == null;
            string? resultExecution = isSuccess ? Informations.Success : string.Format("{0}, {1}", Errors.Error, error);
            if (isSuccess) Console.WriteLine("{0} {1}", Informations.ExecutedScript, filePath);
            else Console.WriteLine("{0} {1} из-за ошибки {2}", Informations.NotExecutedScript, filePath, resultExecution);

            //Добавляем новый скрипт в базу
            Script script = new(_userCreated, true, filePath, isSuccess, resultExecution);
            await _applicationContext.Scripts.AddAsync(script);
            await _applicationContext.SaveChangesAsync();
        }
        else Console.WriteLine("{0}{1}", filePath, Informations.ScriptAlreadyExecuted);
    }

    #endregion

    #region Пользователи

    /// <summary>
    /// Метод инициализации пользователей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationUsers()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsersMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем наличие пользователя демиург
            if (await _userManager.FindByNameAsync("demiurge") == null)
            {
                //Добавляем пользователя демиург
                User user = new("demiurge", "insania_officialis@vk.com", "+79996370439", "https://vk.com/khachko09", false, true, "Хачко", "Иван", "Валерьевич", DateTime.SpecifyKind(DateTime.ParseExact("31.03.1999", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc));
                var result = await _userManager.CreateAsync(user, "K02032018v.") ?? throw new InnerException(Errors.EmptySeason);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("demiurge{0}", Informations.UserAdded);
            }
            else Console.WriteLine("demiurge{0}", Informations.UserAlreadyAdded);

            //Проверяем наличие пользователя божество
            if (await _userManager.FindByNameAsync("divinitas") == null)
            {
                //Добавляем пользователя божество
                User user = new("divinitas", "poetrevolution_09@outlook.com", "+79996370439", "https://vk.com/allenobrien", false, true, "Брайен", "Аллен", "O'", DateTime.SpecifyKind(DateTime.ParseExact("09.08.1996", "dd.MM.yyyy", CultureInfo.InvariantCulture), DateTimeKind.Utc));
                var result = await _userManager.CreateAsync(user, "K02032018v.") ?? throw new InnerException(Errors.EmptySeason);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("divinitas{0}", Informations.UserAdded);
            }
            else Console.WriteLine("divinitas{0}", Informations.UserAlreadyAdded);

            //Создаём шаблон файла скриптов
            string pattern = @"^t_users_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Системное

    /// <summary>
    /// Метод инициализации параметров
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationParameters()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationParametersMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Объявляем переменную, по которой будет осуществляться поиск/добавление/логгирование
            string? value = null;

            //Проверяем наличие записи
            value = "Глобальная дата";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "1 день месяца золота 1800 цикла");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Ход";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "1");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Размер пикселей";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "1.69");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Шрифт чисел на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "Times New Roman");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Размер чисел стран на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "80");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Размер чисел регионов на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "20");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Размер чисел владений на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "15");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Размер чисел областей на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "10");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Цвет чисел на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "#7E0000");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Цвет границ стран на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "#464646");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Цвет границ регионов на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "#363636");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Цвет границ владений на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "#969696");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Проверяем наличие записи
            value = "Цвет границ областей на карте";
            if (!await _applicationContext.Parameters.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Parameter parameter = new(_userCreated, value, "#5C5C5C");
                await _applicationContext.Parameters.AddAsync(parameter);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.ParameterAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.ParameterAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_parameters_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Права доступа

    /// <summary>
    /// Метод инициализации ролей
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRoles()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRolesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем наличие роли гостя
            if (await _roleManager.FindByNameAsync("guest") == null)
            {
                //Добавляем роль гостя
                Role role = new("guest");
                var result = await _roleManager.CreateAsync(role) ?? throw new InnerException(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("guest{0}", Informations.RoleAdded);
            }
            else Console.WriteLine("guest{0}", Informations.RoleAlreadyAdded);

            //Проверяем наличие роли игрока
            if (await _roleManager.FindByNameAsync("player") == null)
            {
                //Добавляем роль игрока
                Role role = new("player");
                var result = await _roleManager.CreateAsync(role) ?? throw new InnerException(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new Exception(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("player{0}", Informations.RoleAdded);
            }
            else Console.WriteLine("player{0}", Informations.RoleAlreadyAdded);

            //Проверяем наличие роли админа
            if (await _roleManager.FindByNameAsync("admin") == null)
            {
                //Добавляем роль админа
                Role role = new("admin");
                var result = await _roleManager.CreateAsync(role) ?? throw new Exception(Errors.FailedCreateRole);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new Exception(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("admin{0}", Informations.RoleAdded);
            }
            else Console.WriteLine("admin{0}", Informations.RoleAlreadyAdded);

            //Создаём шаблон файла скриптов
            string pattern = @"^t_roles_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации ролей пользователей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationUsersRoles()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsersRolesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем наличие роли админа пользователя демиург
            User? user = await _applicationContext.Users.FirstOrDefaultAsync(y => y.UserName == "demiurge") ?? throw new InnerException(Errors.EmptyUser);
            if (!await _applicationContext.UserRoles.AnyAsync(x => x.RoleId == 3 && x.UserId == user.Id))
            {
                //Добавляем роль админ пользователю демиург                
                var result = await _userManager.AddToRoleAsync(user, "admin") ?? throw new InnerException(Errors.EmptySeason);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("admin/demiurge{0}", Informations.UserRoleAdded);
            }
            else Console.WriteLine("admin/demiurge{0}", Informations.UserRoleAlreadyAdded);
            user = null;

            //Проверяем наличие роли игрока пользователя божество
            user = await _applicationContext.Users.FirstOrDefaultAsync(y => y.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyUser);
            if (!await _applicationContext.UserRoles.AnyAsync(x => x.RoleId == 2 && x.UserId == user.Id))
            {
                //Добавляем роль админ пользователю демиург                
                var result = await _userManager.AddToRoleAsync(user, "player") ?? throw new InnerException(Errors.EmptyUser);

                //Если не успешно, выдаём ошибку
                if (!result.Succeeded) throw new InnerException(result?.Errors?.FirstOrDefault()?.Description ?? Errors.Unknown);
                else Console.WriteLine("player/divinitas{0}", Informations.UserRoleAdded);
            }
            else Console.WriteLine("player/divinitas{0}", Informations.UserRoleAlreadyAdded);
            user = null;

            //Создаём шаблон файла скриптов
            string pattern = @"^t_users_roles_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Игроки

    /// <summary>
    /// Метод инициализации игроков
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationPlayers()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationPlayersMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем игрока пользователя божества
            User? user = await _userManager.FindByNameAsync("divinitas") ?? throw new InnerException(Errors.EmptyUser);
            if (!await _applicationContext.Players.AnyAsync(x => x.User == user))
            {
                //Добавляем игрока пользователю божество
                Player player = new(_userCreated, true, user, 999999);
                await _applicationContext.Players.AddAsync(player);

                //Логгируем
                Console.WriteLine("divinitas{0}", Informations.PlayerAdded);
            }
            else Console.WriteLine("divinitas{0}", Informations.PlayerAlreadyAdded);
            user = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_players_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Персонажи

    /// <summary>
    /// Метод инициализации биографий персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationBiographiesHeroes()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationBiographiesHeroesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем биографию персонажа Амлус -9999 - -1
            Hero? hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.Player.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyHero);
            Month? month = await _applicationContext.Months.FirstAsync(x => x.Name == "Гроз") ?? throw new InnerException(Errors.EmptyMonth);
            if (!await _applicationContext.BiographiesHeroes.AnyAsync(x => x.Hero == hero && x.DayBegin == 1 && x.MonthBegin == month && x.CycleBegin == -9999))
            {
                //Добавляем биографию персонажа Амлус -9999 - -1
                Month? monthEnd = await _applicationContext.Months.FirstAsync(x => x.Name == "Сборов") ?? throw new InnerException(Errors.EmptyMonth);
                BiographyHero biographyHero = new(_userCreated, true, hero, 1, month, -9999, 30, monthEnd, -1, "Алмус был одним из " +
                    "первых ратиозавров обосновавшихся в Асфалии. Будучи одним из самых древних Бессметрных, а также весьма " +
                    "могущественным магов, исследовавшим тайны небес и истоки магии, он быстро стал главой асфалийской колонии. " +
                    "Он принимал участие в создании рас четвёртой волны, а позже основоположником их религиозных доктрин. Он " +
                    "первым ввёл в Асфалии божественную магию, изучив и проработав этот феномен, он заявязал на себя и других " +
                    "Бессметрных колони магические потоки. Долгое время он изучал тайны магии и её корни. Однако конец его" +
                    "исследованиям положила высадка людей в Асфалии");
                await _applicationContext.BiographiesHeroes.AddAsync(biographyHero);

                //Логгируем
                Console.WriteLine("Алмус/-9999 - -1{0}", Informations.BiographyHeroAdded);
            }
            else Console.WriteLine("Алмус/-9999 - -1{0}", Informations.BiographyHeroAlreadyAdded);
            hero = null;
            month = null;

            //Проверяем биографию персонажа Амлус 0 - 223
            hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.Player.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyHero);
            month = await _applicationContext.Months.FirstAsync(x => x.Name == "Золота") ?? throw new InnerException(Errors.EmptyMonth);
            if (!await _applicationContext.BiographiesHeroes.AnyAsync(x => x.Hero == hero && x.DayBegin == 1 && x.MonthBegin == month && x.CycleBegin == 0))
            {
                //Добавляем биографию персонажа Амлус 0 - 223
                Month? monthEnd = await _applicationContext.Months.FirstAsync(x => x.Name == "Сборов") ?? throw new InnerException(Errors.EmptyMonth);
                BiographyHero biographyHero = new(_userCreated, true, hero, 1, month, 0, 30, monthEnd, 223, "Поначалу Алмус не " +
                    "отнёсся серьёзно к высадке людей. Оторванный от Эмбрии и следовавший философии мирной колонизации, он не " +
                    "знал о войнах Дитики, и рассматривал людей исключтельно с академической стороны. Однако быстрое размножение" +
                    "людей, их алчность и чуство собственности, всё чаще приводило к конфликтам с расами 4 волны. И вскоре Алмус" +
                    "с отсальными Бессмертными пристальнее обратил на них внимание. Однако несколько войн, когда воины " +
                    "хладнокровный в суровом северном климмате понесли суровые потери, заставили его пересмотреть политику. " +
                    "Однако и посланные войска 4 волны не принесли быстрой победы. Всё магическое могущество Бессмертных ломалось " +
                    "о дикую жажду жизни людей. Их одарённые сжигали свои магоканалы лишь бы победить, а войны бросались в " +
                    "самоубийсственные атака. Такого яростного отпора ратиозавры не ожидали. Тогда Алмус предложил переселить с " +
                    "Дитики орков, которые удачно показали себя в войнах с людьми. Однако орки, оказавшиеся под более слабым " +
                    "слабым контролем ратиозавров восстали и объединились с людьми. После череды неудач хитрый план одного из " +
                    "молодых Бессмертных о внесении разлада в ряды людей и орков легло на благодатную почву. Алмус лично " +
                    "участвовал в создании артефакта, что менял генотип этих рас. И его подпись стоит на ряду с 9 " +
                    "подписями главных Бессмертных Асфалии.");
                await _applicationContext.BiographiesHeroes.AddAsync(biographyHero);

                //Логгируем
                Console.WriteLine("Алмус/0 - 223{0}", Informations.BiographyHeroAdded);
            }
            else Console.WriteLine("Алмус/0 - 223{0}", Informations.BiographyHeroAlreadyAdded);
            hero = null;
            month = null;

            //Проверяем биографию персонажа Амлус 224 - 642
            hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.Player.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyHero);
            month = await _applicationContext.Months.FirstAsync(x => x.Name == "Золота") ?? throw new InnerException(Errors.EmptyMonth);
            if (!await _applicationContext.BiographiesHeroes.AnyAsync(x => x.Hero == hero && x.DayBegin == 1 && x.MonthBegin == month && x.CycleBegin == 224))
            {
                //Добавляем биографию персонажа Амлус 224 - 642
                Month? monthEnd = await _applicationContext.Months.FirstAsync(x => x.Name == "Сборов") ?? throw new InnerException(Errors.EmptyMonth);
                BiographyHero biographyHero = new(_userCreated, true, hero, 1, month, 224, 30, monthEnd, 642, "Во вторую эпоху " +
                    "Алмус смог вернуться спокойно к своим исследованиям, пока люди и орки меняя свой генотип плодили новые расы " +
                    "и после начинали войны между собой по расовому признаку. Крах единой сильно коалиции позволил ратиозаврам " +
                    "частично вернуть контроль над северными ресурсами. В метрополии усилия Алмуса и его сторонников высоко " +
                    "оценили и даже предложили место в Великом Совете Бессмертных. Однако именно в этот момент начался Великий " +
                    "Северный Мор, который разнёсся пожаром по всем землям Асфалии на плечах бегущих проихводных рас людей и " +
                    "орков. Ратиозавры во главе с Алмусом приложили множество усилий, чтобы остановить болезнь. Однако, когда " +
                    "начали умирать первые Бессмертные из числа молодых панику охватила всю асфалийскую колонию. Бегущие " +
                    "Бессмертные несмотря на все запреты старших товарищей занесли болезнь в Эмбрию и Дитику. Алмус и его " +
                    "ближайшее окружение до последнего сопротивлялись болезни, используя свои магические возможности. Однако " +
                    "болезнь заняла весь основной поток их магических каналов, превратив их в бессмертных, но слабых существ. " +
                    "В этом ратиозавром помогли исследования Алмуса божественной магии. Это позволило им оставаться сильнее " +
                    "большинства других выживших Бессмертных. Но всё же недосаточно, чтобы сопротивляться растущей угрозе " +
                    "смертных рас. Поэтому Алмус принял решение со свои окружением отправиться на Коралловые острова, где " +
                    "продолжить искать лекарство от болезни.");
                await _applicationContext.BiographiesHeroes.AddAsync(biographyHero);

                //Логгируем
                Console.WriteLine("Алмус/224 - 642{0}", Informations.BiographyHeroAdded);
            }
            else Console.WriteLine("Алмус/224 - 642{0}", Informations.BiographyHeroAlreadyAdded);
            hero = null;
            month = null;

            //Проверяем биографию персонажа Амлус 643 - 
            hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.Player.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyHero);
            month = await _applicationContext.Months.FirstAsync(x => x.Name == "Золота") ?? throw new InnerException(Errors.EmptyMonth);
            if (!await _applicationContext.BiographiesHeroes.AnyAsync(x => x.Hero == hero && x.DayBegin == 1 && x.MonthBegin == month && x.CycleBegin == 643))
            {
                //Добавляем биографию персонажа Амлус 643 - 
                BiographyHero biographyHero = new(_userCreated, true, hero, 1, month, 643, null, null, null, "До текущего даты Алмус " +
                    "находится на Кораловых островах, где используя идущий поток от верующих ищет лекарство от мора. Однако " +
                    "уменьшающийся поток энергии от верующих оставляет всё меньше надежд на благополучный исход. Алмус угасает, " +
                    "хотя благодаря своим знаниям, опыту и накопленным артефактам остаётся одним из сильнейших существ " +
                    "Терравитрии.");
                await _applicationContext.BiographiesHeroes.AddAsync(biographyHero);

                //Логгируем
                Console.WriteLine("Алмус/643 - {0}", Informations.BiographyHeroAdded);
            }
            else Console.WriteLine("Алмус/643 - {0}", Informations.BiographyHeroAlreadyAdded);
            hero = null;
            month = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_biographies_heroes_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации биографий заявок на регистрацию персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationBiographiesRequestsHeroesRegistration()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationBiographiesRequestsHeroesRegistrationMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем биографию от -9999 заявки на регистрацию персонажа Амлус
            BiographyHero? biography = await _applicationContext
                .BiographiesHeroes
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas" 
                    && x.Hero.PersonalName == "Алмус" 
                    && x.CycleBegin == -9999) 
                ?? throw new InnerException(Errors.EmptyBiography);
            RequestHeroRegistration? request = await _applicationContext
                .RequestsHeroesRegistration
                .Include(x => x.Status)
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas" 
                    && x.Hero.PersonalName == "Алмус"
                    && x.Status.Name == "Принята") 
                ?? throw new InnerException(Errors.EmptyRequestsHeroesRegistration);
            if (!await _applicationContext.BiographiesRequestsHeroesRegistration.AnyAsync(x => x.Biography == biography && x.Request == request))
            {
                //Добавляем биографию от -9999 заявки на регистрацию персонажа Амлус
                BiographyRequestHeroRegistration? biographyRequestHeroRegistration = new(_userCreated, true, request, biography);
                biographyRequestHeroRegistration.SetDateBeginDecision(true, null);
                biographyRequestHeroRegistration.SetDateEndDecision(true, null);
                biographyRequestHeroRegistration.SetTextDecision(true, null);
                await _applicationContext.BiographiesRequestsHeroesRegistration.AddAsync(biographyRequestHeroRegistration);

                //Логгируем
                Console.WriteLine("Алмус/-9999{0}", Informations.BiographiesRequestsHeroesRegistrationAdded);
            }
            else Console.WriteLine("Алмус/-9999{0}", Informations.BiographiesRequestsHeroesRegistrationAlreadyAdded);
            biography = null;
            request = null;

            //Проверяем биографию от 0 заявки на регистрацию персонажа Амлус
            biography = await _applicationContext
                .BiographiesHeroes
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.CycleBegin == 0)
                ?? throw new InnerException(Errors.EmptyBiography);
            request = await _applicationContext
                .RequestsHeroesRegistration
                .Include(x => x.Status)
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.Status.Name == "Принята")
                ?? throw new InnerException(Errors.EmptyRequestsHeroesRegistration);
            if (!await _applicationContext.BiographiesRequestsHeroesRegistration.AnyAsync(x => x.Biography == biography && x.Request == request))
            {
                //Добавляем биографию от 0 заявки на регистрацию персонажа Амлус
                BiographyRequestHeroRegistration? biographyRequestHeroRegistration = new(_userCreated, true, request, biography);
                biographyRequestHeroRegistration.SetDateBeginDecision(true, null);
                biographyRequestHeroRegistration.SetDateEndDecision(true, null);
                biographyRequestHeroRegistration.SetTextDecision(true, null);
                await _applicationContext.BiographiesRequestsHeroesRegistration.AddAsync(biographyRequestHeroRegistration);

                //Логгируем
                Console.WriteLine("Алмус/0{0}", Informations.BiographiesRequestsHeroesRegistrationAdded);
            }
            else Console.WriteLine("Алмус/0{0}", Informations.BiographiesRequestsHeroesRegistrationAlreadyAdded);
            biography = null;
            request = null;

            //Проверяем биографию от 224 заявки на регистрацию персонажа Амлус
            biography = await _applicationContext
                .BiographiesHeroes
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.CycleBegin == 224)
                ?? throw new InnerException(Errors.EmptyBiography);
            request = await _applicationContext
                .RequestsHeroesRegistration
                .Include(x => x.Status)
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.Status.Name == "Принята")
                ?? throw new InnerException(Errors.EmptyRequestsHeroesRegistration);
            if (!await _applicationContext.BiographiesRequestsHeroesRegistration.AnyAsync(x => x.Biography == biography && x.Request == request))
            {
                //Добавляем биографию от 224 заявки на регистрацию персонажа Амлус
                BiographyRequestHeroRegistration? biographyRequestHeroRegistration = new(_userCreated, true, request, biography);
                biographyRequestHeroRegistration.SetDateBeginDecision(true, null);
                biographyRequestHeroRegistration.SetDateEndDecision(true, null);
                biographyRequestHeroRegistration.SetTextDecision(true, null);
                await _applicationContext.BiographiesRequestsHeroesRegistration.AddAsync(biographyRequestHeroRegistration);

                //Логгируем
                Console.WriteLine("Алмус/224{0}", Informations.BiographiesRequestsHeroesRegistrationAdded);
            }
            else Console.WriteLine("Алмус/224{0}", Informations.BiographiesRequestsHeroesRegistrationAlreadyAdded);
            biography = null;
            request = null;

            //Проверяем биографию от 643 заявки на регистрацию персонажа Амлус
            biography = await _applicationContext
                .BiographiesHeroes
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.CycleBegin == 643)
                ?? throw new InnerException(Errors.EmptyBiography);
            request = await _applicationContext
                .RequestsHeroesRegistration
                .Include(x => x.Status)
                .Include(x => x.Hero)
                .ThenInclude(y => y.Player)
                .ThenInclude(z => z.User)
                .FirstAsync(x => x.Hero.Player.User.UserName == "divinitas"
                    && x.Hero.PersonalName == "Алмус"
                    && x.Status.Name == "Принята")
                ?? throw new InnerException(Errors.EmptyRequestsHeroesRegistration);
            if (!await _applicationContext.BiographiesRequestsHeroesRegistration.AnyAsync(x => x.Biography == biography && x.Request == request))
            {
                //Добавляем биографию от 643 заявки на регистрацию персонажа Амлус
                BiographyRequestHeroRegistration? biographyRequestHeroRegistration = new(_userCreated, true, request, biography);
                biographyRequestHeroRegistration.SetDateBeginDecision(true, null);
                biographyRequestHeroRegistration.SetDateEndDecision(true, null);
                biographyRequestHeroRegistration.SetTextDecision(true, null);
                await _applicationContext.BiographiesRequestsHeroesRegistration.AddAsync(biographyRequestHeroRegistration);

                //Логгируем
                Console.WriteLine("Алмус/643{0}", Informations.BiographiesRequestsHeroesRegistrationAdded);
            }
            else Console.WriteLine("Алмус/643{0}", Informations.BiographiesRequestsHeroesRegistrationAlreadyAdded);
            biography = null;
            request = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_biographies_requests_heroes_registration_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationHeroes()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationHeroesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Объявляем переменную, по которой будет осуществляться поиск/добавление/логгирование
            string? value = null;

            //Проверяем наличие записи
            value = "Алмус";
            Player? player = await _applicationContext.Players.Include(x => x.User).FirstAsync(x => x.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyPlayer);
            Month? month = await _applicationContext.Months.FirstAsync(x => x.Name == "Гроз") ?? throw new InnerException(Errors.EmptyMonth);
            Nation? nation = await _applicationContext.Nations.FirstAsync(x => x.Name == "Древний") ?? throw new InnerException(Errors.EmptyNation);
            HairsColor? hairsColor = null;
            EyesColor? eyesColor = await _applicationContext.EyesColors.FirstAsync(x => x.Name == "Зелёные") ?? throw new InnerException(Errors.EmptyEyesColor);
            TypeBody? typeBody = await _applicationContext.TypesBodies.FirstAsync(x => x.Name == "Эктоэндоморф с фигурой-грушей") ?? throw new InnerException(Errors.EmptyTypeBody);
            TypeFace? typeFace = await _applicationContext.TypesFaces.FirstAsync(x => x.Name == "Грушевидное") ?? throw new InnerException(Errors.EmptyTypeFace);
            Area? area = await _applicationContext.Areas.FirstAsync(x => x.Code == "EC_1_1") ?? throw new InnerException(Errors.EmptyArea);
            if (!await _applicationContext.Heroes.AnyAsync(x => x.Player == player && x.PersonalName == value))
            {
                //Добавляем запись
                Hero hero = new(_userCreated, true, player, value, null, null, 1, month, -9999, nation, true, 354, 201, hairsColor, eyesColor, typeBody, typeFace, true, true, null, area);
                await _applicationContext.Heroes.AddAsync(hero);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.HeroAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.HeroAlreadyAdded);
            player = null;
            month = null;
            nation = null;
            hairsColor = null;
            eyesColor = null;
            typeBody = null;
            typeFace = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_heroes_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации заявок на регистрацию персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRequestsHeroesRegistration()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRequestsHeroesRegistration);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем заявку персонажа Амлус
            Hero? hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.Player.User.UserName == "divinitas" && x.PersonalName == "Алмус") ?? throw new InnerException(Errors.EmptyHero);
            StatusRequestHeroRegistration? status = await _applicationContext.StatusesRequestsHeroesRegistrations.FirstAsync(x => x.Name == "Принята") ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            Administrator? administrator = await _applicationContext.Administrators.Include(x => x.User).FirstAsync(x => x.User.UserName == "demiurge") ?? throw new InnerException(Errors.EmptyAdministrator);
            if (!await _applicationContext.RequestsHeroesRegistration.AnyAsync(x => x.Hero == hero && x.Status == status))
            {
                //Добавляем заявку персонажа Амлус
                RequestHeroRegistration request = new(_userCreated, true, hero, status);
                request.SetAdministrator(administrator);
                request.SetEyesColor(true, null);
                request.SetFamilyNameDecision(true, null);
                request.SetHairColorDescision(true, null);
                request.SetHeightDecision(true, null);
                request.SetImageDecision(true, null);
                request.SetLocationDecision(true, null);
                request.SetNationDecision(true, null);
                request.SetPersonalNameDecision(true, null);
                request.SetRaceDecision(true, null);
                request.SetTypeBodyDescision(true, null);
                request.SetTypeFaceDescision(true, null);
                request.SetWeightDecision(true, null);
                await _applicationContext.RequestsHeroesRegistration.AddAsync(request);

                //Логгируем
                Console.WriteLine("Алмус{0}", Informations.RequestsHeroesRegistrationsAdded);
            }
            else Console.WriteLine("Алмус{0}", Informations.RequestsHeroesRegistrationsAlreadyAdded);
            hero = null;
            status = null;
            administrator = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_requests_heroes_registration_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации статусов заявок на регистрацию персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationStatusesRequestsHeroesRegistration()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationStatusesRequestsHeroesRegistrationsMethod);

        try
        {
            //Проверяем статус новая
            StatusRequestHeroRegistration? status = null;
            if (!await _applicationContext.StatusesRequestsHeroesRegistrations.AnyAsync(x => x.Name == "Новая"))
            {
                //Добавляем статус новая
                StatusRequestHeroRegistration statusRequestHeroRegistration = new(_userCreated, "Новая", status);
                await _applicationContext.StatusesRequestsHeroesRegistrations.AddAsync(statusRequestHeroRegistration);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Новая{0}", Informations.StatusesRequestsHeroesRegistrationsAdded);
            }
            else Console.WriteLine("Новая{0}", Informations.StatusesRequestsHeroesRegistrationsAlreadyAdded);
            status = null;

            //Проверяем статус на рассмотрении
            status = await _applicationContext.StatusesRequestsHeroesRegistrations.FirstAsync(x => x.Name == "Новая") ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            if (!await _applicationContext.StatusesRequestsHeroesRegistrations.AnyAsync(x => x.Name == "На рассмотрении"))
            {
                //Добавляем статус на рассмотрении
                StatusRequestHeroRegistration statusRequestHeroRegistration = new(_userCreated, "На рассмотрении", status);
                await _applicationContext.StatusesRequestsHeroesRegistrations.AddAsync(statusRequestHeroRegistration);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("На рассмотрении{0}", Informations.StatusesRequestsHeroesRegistrationsAdded);
            }
            else Console.WriteLine("На рассмотрении{0}", Informations.StatusesRequestsHeroesRegistrationsAlreadyAdded);
            status = null;

            //Проверяем статус принята
            status = await _applicationContext.StatusesRequestsHeroesRegistrations.FirstAsync(x => x.Name == "На рассмотрении") ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            if (!await _applicationContext.StatusesRequestsHeroesRegistrations.AnyAsync(x => x.Name == "Принята"))
            {
                //Добавляем статус принята
                StatusRequestHeroRegistration statusRequestHeroRegistration = new(_userCreated, "Принята", status);
                await _applicationContext.StatusesRequestsHeroesRegistrations.AddAsync(statusRequestHeroRegistration);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Принята{0}", Informations.StatusesRequestsHeroesRegistrationsAdded);
            }
            else Console.WriteLine("Принята{0}", Informations.StatusesRequestsHeroesRegistrationsAlreadyAdded);
            status = null;

            //Проверяем статус отклонена
            status = await _applicationContext.StatusesRequestsHeroesRegistrations.FirstAsync(x => x.Name == "На рассмотрении") ?? throw new InnerException(Errors.EmptyStatusRequestsHeroesRegistration);
            if (!await _applicationContext.StatusesRequestsHeroesRegistrations.AnyAsync(x => x.Name == "Отклонена"))
            {
                //Добавляем статус отклонена
                StatusRequestHeroRegistration statusRequestHeroRegistration = new(_userCreated, "Отклонена", status);
                await _applicationContext.StatusesRequestsHeroesRegistrations.AddAsync(statusRequestHeroRegistration);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Отклонена{0}", Informations.StatusesRequestsHeroesRegistrationsAdded);
            }
            else Console.WriteLine("Отклонена{0}", Informations.StatusesRequestsHeroesRegistrationsAlreadyAdded);
            status = null;

            //Создаём шаблон файла скриптов
            string pattern = @"^t_statuses_requests_heroes_registration_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }
        }
        catch (Exception ex)
        {
            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Администраторы

    /// <summary>
    /// Метод инициализации администраторов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationAdministrators()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationAdmistratorsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем администратора пользователя демиург
            User? user = await _userManager.FindByNameAsync("demiurge") ?? throw new InnerException(Errors.EmptyUser);
            Post? post = await _applicationContext.Posts.FirstAsync(x => x.Name == "Демиург") ?? throw new InnerException(Errors.EmptyPost);
            Rank? rank = await _applicationContext.Ranks.FirstAsync(x => x.Name == "Демиург") ?? throw new InnerException(Errors.EmptyRank);
            Chapter? chapter = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Administrators.AnyAsync(x => x.User == user))
            {
                //Добавляем администратора пользователю демиург
                Administrator administrator = new(_userCreated, true, user, post, rank, chapter, 999999, null);
                await _applicationContext.Administrators.AddAsync(administrator);

                //Логгируем
                Console.WriteLine("demiurge{0}", Informations.AdministratorAdded);
            }
            else Console.WriteLine("demiurge{0}", Informations.AdministratorAlreadyAdded);
            user = null;
            post = null;
            rank = null;
            chapter = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_admistrators_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации капитулов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationChapters()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationChaptersMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем капитул Генеральный капитул
            Country? country = null;
            Chapter? parent = null;
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Генеральный капитул"))
            {
                //Добавляем капитул Генеральный капитул
                Chapter chapter = new(_userCreated, true, "Генеральный капитул", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Генеральный капитул{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Генеральный капитул{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Альвраатской империи
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyCountry);            
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Альвраатской империи"))
            {
                //Добавляем капитул Альвраатской империи
                Chapter chapter = new(_userCreated, true, "Капитул Альвраатской империи", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Альвраатской империи{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Альвраатской империи{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул княжества Саорса
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Княжество Саорса") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул княжества Саорса"))
            {
                //Добавляем капитул княжества Саорса
                Chapter chapter = new(_userCreated, true, "Капитул княжества Саорса", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул княжества Саорса{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул княжества Саорса{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул королевства Берген
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Королевство Берген") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул королевства Берген"))
            {
                //Добавляем капитул королевства Берген
                Chapter chapter = new(_userCreated, true, "Капитул королевства Берген", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул королевства Берген{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул королевства Берген{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Фесгарского княжества
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Фесгарское княжество") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Фесгарского княжества"))
            {
                //Добавляем капитул Фесгарского княжества
                Chapter chapter = new(_userCreated, true, "Капитул Фесгарского княжества", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Фесгарского княжества{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Фесгарского княжества{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Сверденского каганата
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Сверденский каганат") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Сверденского каганата"))
            {
                //Добавляем капитул Сверденского каганата
                Chapter chapter = new(_userCreated, true, "Капитул Сверденского каганата", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Сверденского каганата{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Сверденского каганата{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул ханства Тавалин
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Ханство Тавалин") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул ханства Тавалин"))
            {
                //Добавляем капитул ханства Тавалин
                Chapter chapter = new(_userCreated, true, "Капитул ханства Тавалин", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул ханства Тавалин{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул ханства Тавалин{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул княжества Саргиб
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Княжество Саргиб") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул княжества Саргиб"))
            {
                //Добавляем капитул княжества Саргиб
                Chapter chapter = new(_userCreated, true, "Капитул княжества Саргиб", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул княжества Саргиб{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул княжества Саргиб{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул царства Банду
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Царство Банду") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул царства Банду"))
            {
                //Добавляем капитул царства Банду
                Chapter chapter = new(_userCreated, true, "Капитул царства Банду", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул царства Банду{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул царства Банду{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул королевства Нордер
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Королевство Нордер") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул королевства Нордер"))
            {
                //Добавляем капитул королевства Нордер
                Chapter chapter = new(_userCreated, true, "Капитул королевства Нордер", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул королевства Нордер{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул королевства Нордер{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Альтерского княжества
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Альтерское княжество") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Альтерского княжества"))
            {
                //Добавляем капитул Альтерского княжества
                Chapter chapter = new(_userCreated, true, "Капитул Альтерского княжества", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Альтерского княжества{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Альтерского княжества{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Орлиадарской конфедерации
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Орлиадарская конфедерация") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Орлиадарской конфедерации"))
            {
                //Добавляем капитул Орлиадарской конфедерации
                Chapter chapter = new(_userCreated, true, "Капитул Орлиадарской конфедерации", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Орлиадарской конфедерации{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Орлиадарской конфедерации{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул королевства Удстир
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Королевство Удстир") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул королевства Удстир"))
            {
                //Добавляем капитул королевства Удстир
                Chapter chapter = new(_userCreated, true, "Капитул королевства Удстир", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул королевства Удстир{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул королевства Удстир{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул королевства Вервирунг
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Королевство Вервирунг") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул королевства Вервирунг"))
            {
                //Добавляем капитул королевства Вервирунг
                Chapter chapter = new(_userCreated, true, "Капитул королевства Вервирунг", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул королевства Вервирунг{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул королевства Вервирунг{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Дестинского ордена
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Дестинский орден") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Дестинского ордена"))
            {
                //Добавляем капитул Дестинского ордена
                Chapter chapter = new(_userCreated, true, "Капитул Дестинского ордена", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Дестинского ордена{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Дестинского ордена{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул вольного города Лийсет
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Вольный город Лийсет") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул вольного города Лийсет"))
            {
                //Добавляем капитул вольного города Лийсет
                Chapter chapter = new(_userCreated, true, "Капитул вольного города Лийсет", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул вольного города Лийсет{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул вольного города Лийсет{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Лисцийской империи
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Лисцийская империя") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Лисцийской империи"))
            {
                //Добавляем капитул Лисцийской империи
                Chapter chapter = new(_userCreated, true, "Капитул Лисцийской империи", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Лисцийской империи{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Лисцийской империи{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул королевства Вальтир
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Королевство Вальтир") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул королевства Вальтир"))
            {
                //Добавляем капитул королевства Вальтир
                Chapter chapter = new(_userCreated, true, "Капитул королевства Вальтир", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул королевства Вальтир{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул королевства Вальтир{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул вассального княжества Гратис
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Вассальное княжество Гратис") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул вассального княжества Гратис"))
            {
                //Добавляем капитул вассального княжества Гратис
                Chapter chapter = new(_userCreated, true, "Капитул вассального княжества Гратис", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул вассального княжества Гратис{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул вассального княжества Гратис{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул княжества Ректа
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Княжество Ректа") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул княжества Ректа"))
            {
                //Добавляем капитул княжества Ректа
                Chapter chapter = new(_userCreated, true, "Капитул княжества Ректа", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул княжества Ректа{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул княжества Ректа{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Волара
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Волар") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Волара"))
            {
                //Добавляем капитул Волара
                Chapter chapter = new(_userCreated, true, "Капитул Волара", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Волара{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Волара{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул союза Иль-Ладро
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Союз Иль-Ладро") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул союза Иль-Ладро"))
            {
                //Добавляем капитул союза Иль-Ладро
                Chapter chapter = new(_userCreated, true, "Капитул союза Иль-Ладро", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул союза Иль-Ладро{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул союза Иль-Ладро{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Проверяем капитул Мергерской унии
            country = await _applicationContext.Countries.Include(x => x.Organization).FirstAsync(x => x.Organization.Name == "Мергерская Уния") ?? throw new InnerException(Errors.EmptyCountry);
            parent = await _applicationContext.Chapters.FirstAsync(x => x.Name == "Генеральный капитул") ?? throw new InnerException(Errors.EmptyChapter);
            if (!await _applicationContext.Chapters.AnyAsync(x => x.Name == "Капитул Мергерской унии"))
            {
                //Добавляем капитул Мергерской унии
                Chapter chapter = new(_userCreated, true, "Капитул Мергерской унии", country, parent);
                await _applicationContext.Chapters.AddAsync(chapter);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Капитул Мергерской унии{0}", Informations.ChapterAdded);
            }
            else Console.WriteLine("Капитул Мергерской унии{0}", Informations.ChapterAlreadyAdded);
            country = null;
            parent = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_chapters_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации должностей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationPosts()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationPostsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем должность демиург
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Демиург"))
            {
                //Добавляем должность демиург
                Post post = new(_userCreated, "Демиург", "Общий контроль мира и проекта");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Демиург{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Демиург{0}", Informations.PostAlreadyAdded);

            //Проверяем должность магистр
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Магистр"))
            {
                //Добавляем должность магистр
                Post post = new(_userCreated, "Магистр", "Управление капитулом");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Магистр{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Магистр{0}", Informations.PostAlreadyAdded);

            //Проверяем должность комтур
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Комтур"))
            {
                //Добавляем должность комтур
                Post post = new(_userCreated, "Комтур", "Управление администрацией капитула");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Комтур{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Комтур{0}", Informations.PostAlreadyAdded);

            //Проверяем должность распорядитель
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Распорядитель"))
            {
                //Добавляем должность распорядитель
                Post post = new(_userCreated, "Распорядитель", "Приём игроков");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Распорядитель{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Распорядитель{0}", Informations.PostAlreadyAdded);

            //Проверяем должность мейстер
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Распорядитель"))
            {
                //Добавляем должность мейстер
                Post post = new(_userCreated, "Мейстер", "Гражданское и политическое судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Мейстер{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Мейстер{0}", Informations.PostAlreadyAdded);

            //Проверяем должность маршал
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Маршал"))
            {
                //Добавляем должность маршал
                Post post = new(_userCreated, "Маршал", "Военное судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Маршал{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Маршал{0}", Informations.PostAlreadyAdded);

            //Проверяем должность инженер
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Инженер"))
            {
                //Добавляем должность инженер
                Post post = new(_userCreated, "Инженер", "Технологическое судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Инженер{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Инженер{0}", Informations.PostAlreadyAdded);

            //Проверяем должность интендант
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Интендант"))
            {
                //Добавляем должность интендант
                Post post = new(_userCreated, "Интендант", "Экономическое судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Интендант{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Интендант{0}", Informations.PostAlreadyAdded);

            //Проверяем должность архимаг
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Архимаг"))
            {
                //Добавляем должность архимаг
                Post post = new(_userCreated, "Архимаг", "Магическое судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Архимаг{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Архимаг{0}", Informations.PostAlreadyAdded);

            //Проверяем должность жрец
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Жрец"))
            {
                //Добавляем должность жрец
                Post post = new(_userCreated, "Жрец", "Религиозное судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Жрец{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Жрец{0}", Informations.PostAlreadyAdded);

            //Проверяем должность бард
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Бард"))
            {
                //Добавляем должность бард
                Post post = new(_userCreated, "Бард", "Культурное судейство");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Бард{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Бард{0}", Informations.PostAlreadyAdded);

            //Проверяем должность глашатай
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Глашатай"))
            {
                //Добавляем должность глашатай
                Post post = new(_userCreated, "Глашатай", "Публикация и контроль новстей");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Глашатай{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Глашатай{0}", Informations.PostAlreadyAdded);

            //Проверяем должность посол
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Посол"))
            {
                //Добавляем должность посол
                Post post = new(_userCreated, "Посол", "Ведение рекламной и представительной деятельности");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Посол{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Посол{0}", Informations.PostAlreadyAdded);

            //Проверяем должность архивариус
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Архивариус"))
            {
                //Добавляем должность архивариус
                Post post = new(_userCreated, "Архивариус", "Контроль статистики и летоисчисления");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Архивариус{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Архивариус{0}", Informations.PostAlreadyAdded);

            //Проверяем должность картограф
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Картограф"))
            {
                //Добавляем должность картограф
                Post post = new(_userCreated, "Картограф", "Ведение карты");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Картограф{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Картограф{0}", Informations.PostAlreadyAdded);

            //Проверяем должность гофмалер
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Гофмалер"))
            {
                //Добавляем должность гофмалер
                Post post = new(_userCreated, "Гофмалер", "Создание и модификация дизайна");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Гофмалер{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Гофмалер{0}", Informations.PostAlreadyAdded);

            //Проверяем должность разработчик
            if (!await _applicationContext.Posts.AnyAsync(x => x.Name == "Разработчик"))
            {
                //Добавляем должность разработчик
                Post post = new(_userCreated, "Разработчик", "Разработка и сопровождение программных продуктов");
                await _applicationContext.Posts.AddAsync(post);

                //Логгируем
                Console.WriteLine("Разработчик{0}", Informations.PostAdded);
            }
            else Console.WriteLine("Разработчик{0}", Informations.PostAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_posts_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации званий
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRanks()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRanksMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем звание демиург
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Демиург"))
            {
                //Добавляем звание демиург
                Rank rank = new(_userCreated, "Демиург", 1024);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Демиург{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Демиург{0}", Informations.RankAlreadyAdded);

            //Проверяем звание верховный
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Верховный"))
            {
                //Добавляем звание верховный
                Rank rank = new(_userCreated, "Верховный", 16);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Верховный{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Верховный{0}", Informations.RankAlreadyAdded);

            //Проверяем звание главный
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Главный"))
            {
                //Добавляем звание главный
                Rank rank = new(_userCreated, "Главный", 8);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Главный{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Главный{0}", Informations.RankAlreadyAdded);

            //Проверяем звание ведущий
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Ведущий"))
            {
                //Добавляем звание ведущий
                Rank rank = new(_userCreated, "Ведущий", 4);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Ведущий{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Ведущий{0}", Informations.RankAlreadyAdded);

            //Проверяем звание старший
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Старший"))
            {
                //Добавляем звание старший
                Rank rank = new(_userCreated, "Старший", 2);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Старший{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Старший{0}", Informations.RankAlreadyAdded);

            //Проверяем звание младший
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Младший"))
            {
                //Добавляем звание младший
                Rank rank = new(_userCreated, "Младший", 1);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Младший{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Младший{0}", Informations.RankAlreadyAdded);

            //Проверяем звание фамилиар
            if (!await _applicationContext.Ranks.AnyAsync(x => x.Name == "Фамилиар"))
            {
                //Добавляем звание фамилиар
                Rank rank = new(_userCreated, "Фамилиар", 0.5);
                await _applicationContext.Ranks.AddAsync(rank);

                //Логгируем
                Console.WriteLine("Фамилиар{0}", Informations.RankAdded);
            }
            else Console.WriteLine("Фамилиар{0}", Informations.RankAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_ranks_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Летоисчисление

    /// <summary>
    /// Метод инициализации месяцев
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationMonths()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationMonthsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем месяц золота
            Season? season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Дождей") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Золота"))
            {
                //Добавляем месяц золота
                Month month = new(_userCreated, "Золота", season, 1);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Золота{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Золота{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц ливней
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Дождей") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Ливней"))
            {
                //Добавляем месяц ливней
                Month month = new(_userCreated, "Ливней", season, 2);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Ливней{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Ливней{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц заморозков
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Дождей") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Заморозков"))
            {
                //Добавляем месяц заморозков
                Month month = new(_userCreated, "Заморозков", season, 3);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Заморозков{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Заморозков{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц снегопадов
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Снега") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Снегопадов"))
            {
                //Добавляем месяц снегопадов
                Month month = new(_userCreated, "Снегопадов", season, 4);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Снегопадов{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Снегопадов{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц морозов
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Снега") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Морозов"))
            {
                //Добавляем месяц морозов
                Month month = new(_userCreated, "Морозов", season, 5);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Морозов{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Морозов{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц оттепели
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Снега") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Оттепели"))
            {
                //Добавляем месяц оттепели
                Month month = new(_userCreated, "Оттепели", season, 6);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Оттепели{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Оттепели{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц цветения
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Расцвета") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Цветения"))
            {
                //Добавляем месяц цветения
                Month month = new(_userCreated, "Цветения", season, 7);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Цветения{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Цветения{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц посевов
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Расцвета") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Посевов"))
            {
                //Добавляем месяц посевов
                Month month = new(_userCreated, "Посевов", season, 8);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Посевов{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Посевов{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц гроз
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Расцвета") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Гроз"))
            {
                //Добавляем месяц гроз
                Month month = new(_userCreated, "Гроз", season, 9);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Гроз{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Гроз{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц поспевания
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Тепла") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Поспевания"))
            {
                //Добавляем месяц поспевания
                Month month = new(_userCreated, "Поспевания", season, 10);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Поспевания{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Поспевания{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц жары
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Тепла") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Жары"))
            {
                //Добавляем месяц жары
                Month month = new(_userCreated, "Жары", season, 11);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Жары{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Жары{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Проверяем месяц сборов
            season = _applicationContext.Seasons.FirstOrDefault(x => x.Name == "Тепла") ?? throw new InnerException(Errors.EmptySeason);
            if (!await _applicationContext.Months.AnyAsync(x => x.Season == season && x.Name == "Сборов"))
            {
                //Добавляем месяц сборов
                Month month = new(_userCreated, "Сборов", season, 12);
                await _applicationContext.Months.AddAsync(month);

                //Логгируем
                Console.WriteLine("Сборов{0}", Informations.MonthAdded);
            }
            else Console.WriteLine("Сборов{0}", Informations.MonthAlreadyAdded);
            season = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_months_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации сезонов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationSeasons()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationSeasonsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем сезон дождей
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Дождей"))
            {
                //Добавляем сезон дождей
                Season season = new(_userCreated, "Дождей", 1);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Дождей{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Дождей{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон снега
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Снега"))
            {
                //Добавляем сезон снега
                Season season = new(_userCreated, "Снега", 2);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Снега{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Снега{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон расцвета
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Расцвета"))
            {
                //Добавляем сезон расцвета
                Season season = new(_userCreated, "Расцвета", 3);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Расцвета{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Расцвета{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон тепла
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Тепла"))
            {
                //Добавляем сезон тепла
                Season season = new(_userCreated, "Тепла", 4);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Тепла{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Тепла{0}", Informations.SeasonAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_seasons_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Внешность

    /// <summary>
    /// Метод инициализации цветов глаз
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationEyesColors()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationEyesColorsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем цвет глаз синие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Синие"))
            {
                //Добавляем цвет глаз синие
                EyesColor eyesColor = new(_userCreated, "Синие", "#0000FF");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Синие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Синие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз голубые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Голубые"))
            {
                //Добавляем цвет глаз голубые
                EyesColor eyesColor = new(_userCreated, "Голубые", "#42AAFF");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Голубые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Голубые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз серые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Серые"))
            {
                //Добавляем цвет глаз серые
                EyesColor eyesColor = new(_userCreated, "Серые", "#808080");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Серые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Серые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз зелёные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Зелёные"))
            {
                //Добавляем цвет глаз зелёные
                EyesColor eyesColor = new(_userCreated, "Зелёные", "#008000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Зелёные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Зелёные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз буро-жёлто-зелёные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Буро-жёлто-зелёные"))
            {
                //Добавляем цвет глаз буро-жёлто-зелёные
                EyesColor eyesColor = new(_userCreated, "Буро-жёлто-зелёные", "#7F8F18");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Буро-жёлто-зелёные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Буро-жёлто-зелёные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз жёлтые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Жёлтые"))
            {
                //Добавляем цвет глаз жёлтые
                EyesColor eyesColor = new(_userCreated, "Жёлтые", "#FFFF00");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Жёлтые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Жёлтые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз светло-карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Светло-карие"))
            {
                //Добавляем цвет глаз светло-карие
                EyesColor eyesColor = new(_userCreated, "Светло-карие", "#987654");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Светло-карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Светло-карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Карие"))
            {
                //Добавляем цвет глаз карие
                EyesColor eyesColor = new(_userCreated, "Карие", "#70493D");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз тёмно-карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Тёмно-карие"))
            {
                //Добавляем цвет глаз тёмно-карие
                EyesColor eyesColor = new(_userCreated, "Тёмно-карие", "#654321");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Тёмно-карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Тёмно-карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз чёрные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Чёрные"))
            {
                //Добавляем цвет глаз чёрные
                EyesColor eyesColor = new(_userCreated, "Чёрные", "#000000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Чёрные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Чёрные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз красные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Красные"))
            {
                //Добавляем цвет глаз красные
                EyesColor eyesColor = new(_userCreated, "Красные", "#FF0000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Красные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Красные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз гетерохромия
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Гетерохромия"))
            {
                //Добавляем цвет глаз гетерохромия
                EyesColor eyesColor = new(_userCreated, "Гетерохромия", null);
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Гетерохромия{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Гетерохромия{0}", Informations.EyesColorAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_eyes_colors_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации цветов волос
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationHairsColors()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationHairsColorsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем цвет волос брюнет
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Брюнет"))
            {
                //Добавляем цвет волос брюнет
                HairsColor hairsColor = new(_userCreated, "Брюнет", "#2D170E");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Брюнет{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Брюнет{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос рыжий
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Рыжий"))
            {
                //Добавляем цвет волос рыжий
                HairsColor hairsColor = new(_userCreated, "Рыжий", "#91302B");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Рыжий{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Рыжий{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос блондин
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Блондин"))
            {
                //Добавляем цвет волос блондин
                HairsColor hairsColor = new(_userCreated, "Блондин", "#FAF0BE");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Блондин{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Блондин{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос шатен
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Шатен"))
            {
                //Добавляем цвет волос шатен
                HairsColor hairsColor = new(_userCreated, "Шатен", "#742802");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Шатен{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Шатен{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос русый
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Русый"))
            {
                //Добавляем цвет волос русый
                HairsColor hairsColor = new(_userCreated, "Русый", "#8E7962");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Русый{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Русый{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос седой
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Седой"))
            {
                //Добавляем цвет волос седой
                HairsColor hairsColor = new(_userCreated, "Седой", "#C6C3B5");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Седой{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Седой{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос платиновый
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Платиновый"))
            {
                //Добавляем цвет волос платиновый
                HairsColor hairsColor = new(_userCreated, "Платиновый", "#E5E4E2");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Платиновый{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Платиновый{0}", Informations.HairsColorAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_hairs_colors_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации типов телосложений
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationTypesBodies()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationTypesBodiesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем тип телосложения эктоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эктоморф"))
            {
                //Добавляем тип телосложения эктоморф
                TypeBody typeBody = new(_userCreated, "Эктоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эндоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эндоморф"))
            {
                //Добавляем тип телосложения эндоморф
                TypeBody typeBody = new(_userCreated, "Эндоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эндоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эндоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения мезоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Мезоморф"))
            {
                //Добавляем тип телосложения мезоморф
                TypeBody typeBody = new(_userCreated, "Мезоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Мезоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Мезоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эктомезоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эктомезоморф"))
            {
                //Добавляем тип телосложения эктомезоморф
                TypeBody typeBody = new(_userCreated, "Эктомезоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктомезоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктомезоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эктоэндоморф с фигурой-грушей
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эктоэндоморф с фигурой-грушей"))
            {
                //Добавляем тип телосложения эктоэндоморф с фигурой-грушей
                TypeBody typeBody = new(_userCreated, "Эктоэндоморф с фигурой-грушей");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктоэндоморф с фигурой-грушей{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктоэндоморф с фигурой-грушей{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения мезоэндоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Мезоэндоморф"))
            {
                //Добавляем тип телосложения мезоэндоморф
                TypeBody typeBody = new(_userCreated, "Мезоэндоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Мезоэндоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Мезоэндоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эндоморф с фигурой-яблоком
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эндоморф с фигурой-яблоком"))
            {
                //Добавляем тип телосложения эндоморф с фигурой-яблоком
                TypeBody typeBody = new(_userCreated, "Эндоморф с фигурой-яблоком");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эндоморф с фигурой-яблоком{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эндоморф с фигурой-яблоком{0}", Informations.TypeBodyAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_types_bodies_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации типов лиц
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationTypesFaces()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationTypesFacesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем тип лиц овальное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Овальное"))
            {
                //Добавляем тип лиц овальное
                TypeFace typeFace = new(_userCreated, "Овальное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Овальное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Овальное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц квадратное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Квадратное"))
            {
                //Добавляем тип лиц квадратное
                TypeFace typeFace = new(_userCreated, "Квадратное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Квадратное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Квадратное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц круглое
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Круглое"))
            {
                //Добавляем тип лиц круглое
                TypeFace typeFace = new(_userCreated, "Круглое");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Круглое{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Круглое{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц прямоугольное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Прямоугольное"))
            {
                //Добавляем тип лиц прямоугольное
                TypeFace typeFace = new(_userCreated, "Прямоугольное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Прямоугольное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Прямоугольное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц ромбовидное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Ромбовидное"))
            {
                //Добавляем тип лиц ромбовидное
                TypeFace typeFace = new(_userCreated, "Ромбовидное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Ромбовидное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Ромбовидное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц треугольное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Треугольное"))
            {
                //Добавляем тип лиц треугольное
                TypeFace typeFace = new(_userCreated, "Треугольное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Треугольное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Треугольное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц грушевидное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Грушевидное"))
            {
                //Добавляем тип лиц грушевидное
                TypeFace typeFace = new(_userCreated, "Грушевидное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Грушевидное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Грушевидное{0}", Informations.TypeFaceAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_types_faces_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    /// <summary>
    /// Метод иницализации областей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationAreas()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationAreasMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Объявляем переменную, по которой будет осуществляться поиск/добавление/логгирование
            string? value = null;

            //Проверяем наличие записи
            value = "AE_1_1";
            Region? region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            GeographicalObject? geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Вороний глаз") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            Fraction? fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            Ownership? ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Воронний глаз", 1, "#828CD8", 160, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_2";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Кедровый остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Кедровый берег", 2, "#627FC1", 236, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_3";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Кедровый остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Аустура", 3, "#398E15", 249, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_4";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Дамара") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Клетты", 4, "#599618", 183, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_5";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Дамара") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Годоморхолла", 5, "#796996", 264, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_6";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Цитадель острых шпилей", 6, "#56B0FF", 1007, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_7";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Сегуула", 7, "#E647FF", 1019, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_8";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мыс рабовладельцев", 8, "#5E73FF", 691, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_9";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Содурфага", 9, "#4CFF4C", 515, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_10";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Барамила", 10, "#FF4423", 417, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_11";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Челюсти") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Виика", 11, "#FFF838", 393, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_12";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Восточный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Северный Восточный щит", 12, "#0004FF", 683, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_13";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Восточный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Центральный Восточный щит", 13, "#FFCE7F", 190, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_14";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Восточный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_5/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Южный Восточный щит", 14, "#6B0800", 374, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_15";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Пастбища мамонтов") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Адсофы", 15, "#387734", 230, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_16";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Пастбища мамонтов") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мамонтовы поля", 16, "#445D77", 261, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_17";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Бивень мамонта") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестноси Тааска", 17, "#478E6D", 189, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_18";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Бивень мамонта") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Костяной холм", 18, "#8E618E", 201, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_19";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Мамонтова колыбель") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Мамонтова колыбель", 19, "#8E6822", 182, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_20";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Ягодный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Ягодные холмы", 20, "#00FF6E", 248, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_21";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Ягодный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Форсберра", 21, "#93DEFF", 173, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_22";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Чёрный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Чёрный берег", 22, "#8725C4", 184, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_23";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Чёрный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Берег надежды", 23, "#91C444", 271, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_24";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Тверхедра", 24, "#57CCAB", 722, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_25";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Двухолмье", 25, "#2C3787", 1050, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_26";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Скогарстронда", 26, "#871530", 444, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_27";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Златогорье", 27, "#234177", 1416, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_28";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Золотое ущелье", 28, "#FF0022", 1234, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_29";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Сосновый замок", 29, "#7A871D", 847, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_30";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фалина", 30, "#777338", 615, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_31";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Четырёххолмье", 31, "#77026C", 1291, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_32";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Рыбий берег", 32, "#445277", 1076, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_33";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Блара", 33, "#29AEB5", 759, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_34";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Зелёный берег", 34, "#4EB544", 779, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_35";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Преступность") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестност Глейрхола", 35, "#774210", 535, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_36";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Морозный берег", 36, "#04007F", 1188, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_37";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Морозное побережье", 37, "#7F0015", 843, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_38";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Сосновый берег", 38, "#7F7328", 1435, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_39";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Сволитила", 39, "#007F6A", 749, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_40";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Умкриндура", 40, "#609499", 893, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_41";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Хрингинна", 41, "#99498F", 811, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_42";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Совиные холмы", 42, "#999600", 1268, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_1_43";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Тёмное устье", 43, "#C63585", 736, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_1";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Младший странник") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Младший Странник", 1, "#612A8C", 215, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_2";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Старший странник") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Свартпайка", 2, "#7A8C3F", 167, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_3";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Старший странник") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Рыбные берег", 3, "#00878C", 168, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_4";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Высокий остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Высокий", 4, "#104C00", 229, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_5";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Румера", 5, "#007224", 723, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_6";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Ледура", 6, "#720007", 472, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_7";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Хеедина", 7, "#000F72", 355, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_8";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Левокрылье", 8, "#257260", 1130, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_9";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Скиина", 9, "#726134", 579, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_10";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фарафрама", 10, "#CE301E", 419, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_11";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Костяные холмы", 11, "#FFBB02", 772, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_12";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мыс дальнего рубежа", 12, "#543D82", 597, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_13";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Идна", 13, "#8EFFA1", 508, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_14";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фурутра", 14, "#00CEB3", 350, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_15";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Инры", 15, "#CE6B8C", 540, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_16";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Маркадъюрина", 16, "#36FF32", 354, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_17";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мыс крови", 17, "#C45EFF", 421, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_18";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Залив рабовладельцев", 18, "#3052FF", 498, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_19";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Стронда", 19, "#29827A", 637, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_20";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Тёмные крылья") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Правокрылье", 20, "#7C821A", 1066, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_21";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Южный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фанди", 21, "#A09229", 388, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_22";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Южный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Берег работорговцев", 22, "#24A03B", 478, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_23";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Южный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Охранный залив", 23, "#A02F29", 423, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_24";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Южный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Ферилла", 24, "#3D6A9E", 264, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_25";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Преступность") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Рефура", 25, "#1806D8", 1170, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_26";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Орнугла", 26, "#D8151C", 383, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_27";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Армадиила", 27, "#6BC60F", 849, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_28";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Оскры", 28, "#D3D834", 508, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_29";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мамонтовы холмы", 29, "#C86CD8", 1060, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_30";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Киира", 30, "#45D8CC", 696, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_31";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_9") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фааглина", 31, "#D87649", 708, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_32";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_10") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Дирида", 32, "#9CD82B", 426, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_33";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_10") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Скридира", 33, "#1A99D8", 363, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_34";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_9") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Квикиндида", 34, "#00D890", 619, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_35";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_11") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Гибельный берег", 35, "#D856A6", 401, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_2_36";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_12") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Тёмный берег", 36, "#3200D8", 626, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_1";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Западный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_1/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Запад Западного щита", 1, "#FF8E86", 909, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_2";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Западный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Центр Западного щита", 2, "#C1FFFF", 671, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_3";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Западный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Восток Западного щита", 3, "#560008", 1024, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_4";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Рыбий остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Рыбий", 4, "#2DD1DB", 361, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_5";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Буранов") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Золотые поля", 5, "#89AF00", 244, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_6";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Буранов") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фискиниита", 6, "#C43C2D", 188, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_7";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Железный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Адскилина", 7, "#086357", 275, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_8";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Железный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Железный холм", 8, "#633A3C", 252, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_9";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Мраа") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Мраа", 9, "#009655", 212, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_10";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Клинок") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Северный Клинок", 10, "#FA70FF", 895, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_11";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Клинок") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Центральный Клинок", 11, "#7A1400", 976, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_12";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Клинок") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Южный Клинок", 12, "#469799", 695, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_13";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Рыбный залив", 13, "#077BB5", 660, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_14";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Укмкринга", 14, "#B51E2B", 554, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_15";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Трееда", 15, "#8F2FB5", 917, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_16";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Томта", 16, "#2FA9B5", 680, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_17";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Двугорье", 17, "#B59519", 1207, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_18";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Флаата", 18, "#27B569", 566, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_19";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Дордаана", 19, "#B0B534", 512, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_20";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Медвежий угол", 20, "#58A8B5", 941, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_21";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Миллихада", 21, "#00B238", 845, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_22";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Аинна", 22, "#856BB2", 657, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_23";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Междуречье", 23, "#B56746", 761, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_24";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Скарлатбурга", 24, "#B20020", 786, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_25";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Хунграда", 25, "#437A00", 836, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_26";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Камышовый берег", 26, "#424401", 718, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_27";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Серебрянные источники", 27, "#7A3E68", 1144, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_28";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Беррума", 28, "#30A1A5", 735, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_29";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Литида", 29, "#440019", 413, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_30";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Штормовой мыс", 30, "#3569A5", 1177, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_31";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Флотта", 31, "#A55C2E", 812, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_32";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Сноора", 32, "#31A541", 844, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_33";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Серебрянные ручьи", 33, "#7013A5", 1031, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_34";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Аинмана", 34, "#A57C00", 982, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_35";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Медвежьи холмы", 35, "#00991C", 1383, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_36";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Серебряное устье", 36, "#7F4347", 1173, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_37";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Еловые склоны", 37, "#374C99", 847, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_38";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Холура", 38, "#998010", 802, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_39";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Сандура", 39, "#1B9941", 649, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_40";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Солёный берег", 40, "#35C638", 1332, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_41";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Дадира", 41, "#992E22", 849, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_42";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Карфы", 42, "#4785C6", 481, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_43";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Бриина", 43, "#6B39C6", 471, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_44";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Альга", 44, "#C69125", 631, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_3_45";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Духовенство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Улфура", 45, "#41C6B2", 1034, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_1";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Дальний остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Дальний", 1, "#FF86DC", 436, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_2";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Северо-запад Северного щита", 2, "#540031", 788, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_3";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Хофнина", 3, "#B71705", 784, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_4";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Хъяги", 4, "#00027F", 486, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_5";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности  Афскудура", 5, "#B79F00", 328, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_6";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фъёрута", 6, "#00B7AE", 648, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_7";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Литиина", 7, "#542448", 309, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_8";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Военные") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Южный Северный щит", 8, "#00540B", 743, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_9";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Северный щит") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Знать") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Восток Северного щита", 9, "#896447", 783, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_10";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Мятный остров") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Преступность") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Мятный", 10, "#438765", 172, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_11";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Сосновые холмы", 11, "#FF263B", 768, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_12";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Солёный утёс", 12, "#66FFA0", 1091, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_13";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Турминна", 13, "#FFFF2B", 606, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_14";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Стиркинга", 14, "#FF42F8", 429, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_15";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Дождливый берег", 15, "#4F7BFF", 511, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_16";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Дорпида", 16, "#38FF2D", 590, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_17";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Скалистый залив", 17, "#0083FF", 975, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_18";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мраморный холм", 18, "#D8061B", 1137, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_19";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Мраморные склоны", 19, "#AED815", 823, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_20";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Халла", 20, "#D85B9E", 538, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_21";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Скалистый замок", 21, "#447F47", 656, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_22";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Гратта", 22, "#D86208", 888, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_23";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Миинна", 23, "#3AD864", 1163, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_24";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Скогура", 24, "#873A11", 1047, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_25";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Фъёлотта", 25, "#2B4C87", 1698, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_26";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Серебряные горы", 26, "#870E7D", 3393, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_27";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Окрестности Сердусвидра", 27, "#00CC69", 1334, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "AE_4_28";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Морозный клык") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Маги") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Штормовой берег", 28, "#C1CC4D", 722, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Проверяем наличие записи
            value = "EC_1_1";
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "EC_1") ?? throw new InnerException(Errors.EmptyRegion);
            geographicalObject = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Остров Жёлтый") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            fraction = await _applicationContext.Fractions.FirstAsync(x => x.Name == "Правительство") ?? throw new InnerException(Errors.EmptyFraction);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "EC_1_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Areas.AnyAsync(x => x.Code == value))
            {
                //Добавляем запись
                Area area = new(_userCreated, true, "Остров Жёлтый", 1, "#CCC345", 0, region, geographicalObject, fraction, ownership, value);
                await _applicationContext.Areas.AddAsync(area);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.AreaAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.AreaAlreadyAdded);
            region = null;
            geographicalObject = null;
            fraction = null;
            ownership = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_areas_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации стран
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationCountries()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationCountriesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем страну Альвраатская империя
            Organization? organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Альвраатская империя
                Country country = new(_userCreated, true, 1, "#20D1DB", "Исландский", organization, "AE");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Альвраатская империя{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Альвраатская империя{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Княжество Саорса
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Княжество Саорса") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Княжество Саорса
                Country country = new(_userCreated, true, 2, "#607F47", "Ирландский", organization, "SaP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Княжество Саорса{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Княжество Саорса{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Королевство Берген
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Королевство Берген") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Королевство Берген
                Country country = new(_userCreated, true, 3, "#00687C", "Шведский", organization, "BnK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Королевство Берген{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Королевство Берген{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Фесгарское княжество
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Фесгарское княжество") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Фесгарское княжество
                Country country = new(_userCreated, true, 4, "#B200FF", "Шотландский", organization, "FP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Фесгарское княжество{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Фесгарское княжество{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Сверденский каганат
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Сверденский каганат") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Сверденский каганат
                Country country = new(_userCreated, true, 5, "#7F3B00", "Норвежский", organization, "SK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Сверденский каганат{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Сверденский каганат{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Ханство Тавалин
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Ханство Тавалин") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Ханство Тавалин
                Country country = new(_userCreated, true, 6, "#7F006D", "Эстонский", organization, "TH");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Ханство Тавалин{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Ханство Тавалин{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Княжество Саргиб
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Княжество Саргиб") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Княжество Саргиб
                Country country = new(_userCreated, true, 7, "#007F0E", "Литовский", organization, "SbP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Княжество Саргиб{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Княжество Саргиб{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Царство Банду
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Царство Банду") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Царство Банду
                Country country = new(_userCreated, true, 8, "#47617C", "Хинди", organization, "BuK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Царство Банду{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Царство Банду{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Королевство Нордер
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Королевство Нордер") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Королевство Нордер
                Country country = new(_userCreated, true, 9, "#D82929", "Немецкий", organization, "NK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Королевство Нордер{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Королевство Нордер{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Альтерское княжество
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альтерское княжество") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Альтерское княжество
                Country country = new(_userCreated, true, 10, "#4ACC39", "Французский", organization, "AP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Альтерское княжество{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Альтерское княжество{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Орлиадарская конфедерация
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Орлиадарская конфедерация") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Орлиадарская конфедерация
                Country country = new(_userCreated, true, 11, "#AF9200", "Французский", organization, "OK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Орлиадарская конфедерация{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Орлиадарская конфедерация{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Королевство Удстир
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Королевство Удстир") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Королевство Удстир
                Country country = new(_userCreated, true, 12, "#8CAF00", "Датский", organization, "UK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Королевство Удстир{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Королевство Удстир{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Королевство Вервирунг
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Королевство Вервирунг") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Королевство Вервирунг
                Country country = new(_userCreated, true, 13, "#7F1700", "Немецкий", organization, "VgK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Королевство Вервирунг{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Королевство Вервирунг{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Дестинский орден
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дестинский орден") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Дестинский орден
                Country country = new(_userCreated, true, 14, "#2B7C55", "Итальянский", organization, "DO");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Дестинский орден{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Дестинский орден{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Вольный город Лийсет
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Вольный город Лийсет") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Вольный город Лийсет
                Country country = new(_userCreated, true, 15, "#7B7F00", "Итальянский", organization, "LFC");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Вольный город Лийсет{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Вольный город Лийсет{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Лисцийская империя
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Лисцийская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Лисцийская империя
                Country country = new(_userCreated, true, 16, "#7F002E", "Итальянский", organization, "LE");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Лисцийская империя{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Лисцийская империя{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Королевство Вальтир
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Королевство Вальтир") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Королевство Вальтир
                Country country = new(_userCreated, true, 17, "#B05BFF", "Финский", organization, "VrK");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Королевство Вальтир{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Королевство Вальтир{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Вассальное княжество Гратис
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Вассальное княжество Гратис") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Вассальное княжество Гратис
                Country country = new(_userCreated, true, 18, "#005DFF", "Итальянский", organization, "GVP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Вассальное княжество Гратис{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Вассальное княжество Гратис{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Княжество Ректа
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Княжество Ректа") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Княжество Ректа
                Country country = new(_userCreated, true, 19, "#487F00", "Эсперанто", organization, "RP");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Княжество Ректа{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Княжество Ректа{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Волар
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Волар") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Волар
                Country country = new(_userCreated, true, 20, "#32217A", "Испанский", organization, "V");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Волар{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Волар{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Союз Иль-Ладро
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Союз Иль-Ладро") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Союз Иль-Ладро
                Country country = new(_userCreated, true, 21, "#35513B", "Итальянский", organization, "ILU");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Союз Иль-Ладро{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Союз Иль-Ладро{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Мергерская Уния
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Мергерская Уния") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Мергерская Уния
                Country country = new(_userCreated, true, 22, "#BC3CB4", "Латынь", organization, "MU");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Мергерская Уния{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Мергерская Уния{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Проверяем страну Цивилизация Эмбрии
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Цивилизация Эмбрии") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Countries.AnyAsync(x => x.Organization == organization))
            {
                //Добавляем страну Цивилизация Эмбрии
                Country country = new(_userCreated, true, 99, "#1F3F3D", "Латынь", organization, "EC");
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Цивилизация Эмбрии{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Цивилизация Эмбрии{0}", Informations.CountryAlreadyAdded);
            organization = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_countries_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации фракций
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationFractions()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationFractionsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Объявляем переменную, по которой будет осуществляться поиск/добавление/логгирование
            string? value = null;

            //Проверяем наличие записи
            value = "Правительство";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#0004FF", "определение внутренней и внешней политики; управление финансами государства; контроль за исполнением приказов; судебные дела высшего уровня");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Знать";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#00FFFF", "определение внутренней и внешней политики в регионах; управление финансами региона; контроль за исполнением законов; судебные дела среднего и низшего уровня");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Духовенство";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#26FF00", "социальные функции (регистрация рождения, брака, смерти и т.д.); влияние на моральное направление последователей; влияние на внешнюю и внутреннюю политику государства (если большое количество последователей); управление подконтрольными землями (если имеются); распространение веры");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Маги";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#FF0AB9", "бытовая помощь населению; магическая поддержка армии; государственные службы (расследование преступлений, поддержание инфраструктуры населённых пунктов и т.д.); научные изыскания; управление областями (если имеются)");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Военные";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#FF1A0A", "защита государственных границ; поддержание внутреннего порядка при необходимости; поддержка притязаний государства на иные земли");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Купечество";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#F7FF0F", "внутренняя и внешняя торговля; добыча сырья и производство продукции; предоставление услуг");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Преступность";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#9000FF", "ведение незаконной деятельности; расширение влияния; противостояние официальной власти");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Интеллигенция";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#FF7105", "развитие технологий; накопление знаний о мире; распространение знаний другим людям; создание произведений искусства; продажа изобретений и продуктов творческой деятельности");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Проверяем наличие записи
            value = "Бесфракционники";
            if (!await _applicationContext.Fractions.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                Fraction fraction = new(_userCreated, value, "#74CE00", "самые разнообразные и определяются игроком или отыгрывающим администратором");
                await _applicationContext.Fractions.AddAsync(fraction);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.FractionAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.FractionAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_fractions_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации организаций
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationOrganizations()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationOrganizationsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем организацию Альвраатская империя
            TypeOrganization? typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            Organization? parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Альвраатская империя"))
            {
                //Добавляем организацию Альвраатская империя
                Organization organization = new(_userCreated, true, "Альвраатская империя", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Альвраатская империя{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Альвраатская империя{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Княжество Саорса
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Саорса"))
            {
                //Добавляем организацию Княжество Саорса
                Organization organization = new(_userCreated, true, "Княжество Саорса", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Княжество Саорса{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Саорса{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Королевство Берген
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Берген"))
            {
                //Добавляем организацию Королевство Берген
                Organization organization = new(_userCreated, true, "Королевство Берген", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Королевство Берген{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Берген{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Фесгарское княжество
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Фесгарское княжество"))
            {
                //Добавляем организацию Фесгарское княжество
                Organization organization = new(_userCreated, true, "Фесгарское княжество", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Фесгарское княжество{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Фесгарское княжество{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Сверденский каганат
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Сверденский каганат"))
            {
                //Добавляем организацию Сверденский каганат
                Organization organization = new(_userCreated, true, "Сверденский каганат", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Сверденский каганат{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Сверденский каганат{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Ханство Тавалин
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Ханство Тавалин"))
            {
                //Добавляем организацию Ханство Тавалин
                Organization organization = new(_userCreated, true, "Ханство Тавалин", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Ханство Тавалин{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Ханство Тавалин{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Княжество Саргиб
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Саргиб"))
            {
                //Добавляем организацию Княжество Саргиб
                Organization organization = new(_userCreated, true, "Княжество Саргиб", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Княжество Саргиб{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Саргиб{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Царство Банду
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Царство Банду"))
            {
                //Добавляем организацию Царство Банду
                Organization organization = new(_userCreated, true, "Царство Банду", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Царство Банду{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Царство Банду{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Королевство Нордер
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Нордер"))
            {
                //Добавляем организацию Королевство Нордер
                Organization organization = new(_userCreated, true, "Королевство Нордер", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Королевство Нордер{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Нордер{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Альтерское княжество
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Альтерское княжество"))
            {
                //Добавляем организацию Альтерское княжество
                Organization organization = new(_userCreated, true, "Альтерское княжество", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Альтерское княжество{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Альтерское княжество{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Орлиадарская конфедерация
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Орлиадарская конфедерация"))
            {
                //Добавляем организацию Орлиадарская конфедерация
                Organization organization = new(_userCreated, true, "Орлиадарская конфедерация", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Орлиадарская конфедерация{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Орлиадарская конфедерация{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Королевство Удстир
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Удстир"))
            {
                //Добавляем организацию Королевство Удстир
                Organization organization = new(_userCreated, true, "Королевство Удстир", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Королевство Удстир{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Удстир{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Вервирунг
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Вервирунг"))
            {
                //Добавляем организацию Королевство Вервирунг
                Organization organization = new(_userCreated, true, "Королевство Вервирунг", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Королевство Вервирунг{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Вервирунг{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дестинский орден
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дестинский орден"))
            {
                //Добавляем организацию Дестинский орден
                Organization organization = new(_userCreated, true, "Дестинский орден", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дестинский орден{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дестинский орден{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Вольный город Лийсет
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Вольный город Лийсет"))
            {
                //Добавляем организацию Вольный город Лийсет
                Organization organization = new(_userCreated, true, "Вольный город Лийсет", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Вольный город Лийсет{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Вольный город Лийсет{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Лисцийская империя
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Лисцийская империя"))
            {
                //Добавляем организацию Лисцийская империя
                Organization organization = new(_userCreated, true, "Лисцийская империя", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Лисцийская империя{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Лисцийская империя{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Королевство Вальтир
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Вальтир"))
            {
                //Добавляем организацию Королевство Вальтир
                Organization organization = new(_userCreated, true, "Королевство Вальтир", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Королевство Вальтир{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Вальтир{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Вассальное княжество Гратис
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Вассальное княжество Гратис"))
            {
                //Добавляем организацию Вассальное княжество Гратис
                Organization organization = new(_userCreated, true, "Вассальное княжество Гратис", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Вассальное княжество Гратис{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Вассальное княжество Гратис{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Княжество Ректа
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Ректа"))
            {
                //Добавляем организацию Княжество Ректа
                Organization organization = new(_userCreated, true, "Княжество Ректа", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Княжество Ректа{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Ректа{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Волар
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Волар"))
            {
                //Добавляем организацию Волар
                Organization organization = new(_userCreated, true, "Волар", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Волар{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Волар{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Союз Иль-Ладро
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Союз Иль-Ладро"))
            {
                //Добавляем организацию Союз Иль-Ладро
                Organization organization = new(_userCreated, true, "Союз Иль-Ладро", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Союз Иль-Ладро{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Союз Иль-Ладро{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Мергерская Уния
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Мергерская Уния"))
            {
                //Добавляем организацию Мергерская Уния
                Organization organization = new(_userCreated, true, "Мергерская Уния", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Мергерская Уния{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Мергерская Уния{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Цивилизация Эмбрии
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = null;
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Цивилизация Эмбрии"))
            {
                //Добавляем организацию Цивилизация Эмбрии
                Organization organization = new(_userCreated, true, "Цивилизация Эмбрии", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Цивилизация Эмбрии{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Цивилизация Эмбрии{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Брейл
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Брейл"))
            {
                //Добавляем организацию Дом Брейл
                Organization organization = new(_userCreated, true, "Дом Брейл", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Брейл{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Брейл{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Футур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Футур"))
            {
                //Добавляем организацию Племя Футур
                Organization organization = new(_userCreated, true, "Племя Футур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Футур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Футур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Годомор
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Годомор"))
            {
                //Добавляем организацию Дом Годомор
                Organization organization = new(_userCreated, true, "Дом Годомор", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Годомор{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Годомор{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Хагтандер
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Хагтандер"))
            {
                //Добавляем организацию Дом Хагтандер
                Organization organization = new(_userCreated, true, "Дом Хагтандер", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Хагтандер{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Хагтандер{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Сколбарер
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Сколбарер"))
            {
                //Добавляем организацию Дом Сколбарер
                Organization organization = new(_userCreated, true, "Дом Сколбарер", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Сколбарер{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Сколбарер{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Армия Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Военное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Армия Альвраатской империи"))
            {
                //Добавляем организацию Армия Альвраатской империи
                Organization organization = new(_userCreated, true, "Армия Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Армия Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Армия Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Восточная армия Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Военное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Восточная армия Альвраатской империи"))
            {
                //Добавляем организацию Восточная армия Альвраатской империи
                Organization organization = new(_userCreated, true, "Восточная армия Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Восточная армия Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Восточная армия Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Мутур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Мутур"))
            {
                //Добавляем организацию Дом Мутур
                Organization organization = new(_userCreated, true, "Дом Мутур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Мутур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Мутур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Берр
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Берр"))
            {
                //Добавляем организацию Племя Берр
                Organization organization = new(_userCreated, true, "Племя Берр", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Берр{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Берр{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Императрицы Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Правительственное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Императрицы Альвраатской империи"))
            {
                //Добавляем организацию Императрицы Альвраатской империи
                Organization organization = new(_userCreated, true, "Императрицы Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Императрицы Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Императрицы Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Каперы Ланиэль
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Преступное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Каперы Ланиэль"))
            {
                //Добавляем организацию Каперы Ланиэль
                Organization organization = new(_userCreated, true, "Каперы Ланиэль", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Каперы Ланиэль{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Каперы Ланиэль{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Фердмадр
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Фердмадр"))
            {
                //Добавляем организацию Дом Фердмадр
                Organization organization = new(_userCreated, true, "Дом Фердмадр", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Фердмадр{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Фердмадр{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Хаар
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Хаар"))
            {
                //Добавляем организацию Племя Хаар
                Organization organization = new(_userCreated, true, "Племя Хаар", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Хаар{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Хаар{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Вингар
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Вингар"))
            {
                //Добавляем организацию Дом Вингар
                Organization organization = new(_userCreated, true, "Дом Вингар", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Вингар{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Вингар{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Скид
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Скид"))
            {
                //Добавляем организацию Дом Скид
                Organization organization = new(_userCreated, true, "Дом Скид", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Скид{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Скид{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Южная армия Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Военное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Южная армия Альвраатской империи"))
            {
                //Добавляем организацию Южная армия Альвраатской империи
                Organization organization = new(_userCreated, true, "Южная армия Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Южная армия Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Южная армия Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Рефур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Рефур"))
            {
                //Добавляем организацию Дом Рефур
                Organization organization = new(_userCreated, true, "Дом Рефур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Рефур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Рефур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Наёмный отряд "Серебряные стрелы"
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Преступное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Рефур") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Наёмный отряд \"Серебряные стрелы\""))
            {
                //Добавляем организацию Наёмный отряд "Серебряные стрелы"
                Organization organization = new(_userCreated, true, "Наёмный отряд \"Серебряные стрелы\"", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Наёмный отряд \"Серебряные стрелы\"{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Наёмный отряд \"Серебряные стрелы\"{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Гунраад
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Гунраад"))
            {
                //Добавляем организацию Дом Гунраад
                Organization organization = new(_userCreated, true, "Дом Гунраад", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Гунраад{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Гунраад{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Беерун
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Беерун"))
            {
                //Добавляем организацию Племя Беерун
                Organization organization = new(_userCreated, true, "Племя Беерун", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Беерун{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Беерун{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Ул
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Ул"))
            {
                //Добавляем организацию Племя Ул
                Organization organization = new(_userCreated, true, "Племя Ул", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Ул{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Ул{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Сурруун
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Сурруун"))
            {
                //Добавляем организацию Племя Сурруун
                Organization organization = new(_userCreated, true, "Племя Сурруун", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Сурруун{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Сурруун{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Племя Дир
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Племя Дир"))
            {
                //Добавляем организацию Племя Дир
                Organization organization = new(_userCreated, true, "Племя Дир", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Племя Дир{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Племя Дир{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Беерритон
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Беерритон"))
            {
                //Добавляем организацию Дом Беерритон
                Organization organization = new(_userCreated, true, "Дом Беерритон", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Беерритон{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Беерритон{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Миркур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Миркур"))
            {
                //Добавляем организацию Дом Миркур
                Organization organization = new(_userCreated, true, "Дом Миркур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Миркур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Миркур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Сколбур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Сколбур"))
            {
                //Добавляем организацию Дом Сколбур
                Organization organization = new(_userCreated, true, "Дом Сколбур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Сколбур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Сколбур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Западная армия Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Военное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Западная армия Альвраатской империи"))
            {
                //Добавляем организацию Западная армия Альвраатской империи
                Organization organization = new(_userCreated, true, "Западная армия Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Западная армия Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Западная армия Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Фискур
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Фискур"))
            {
                //Добавляем организацию Дом Фискур
                Organization organization = new(_userCreated, true, "Дом Фискур", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Фискур{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Фискур{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Снобгород
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Снобгород"))
            {
                //Добавляем организацию Дом Снобгород
                Organization organization = new(_userCreated, true, "Дом Снобгород", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Снобгород{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Снобгород{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Малмгрит
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Малмгрит"))
            {
                //Добавляем организацию Дом Малмгрит
                Organization organization = new(_userCreated, true, "Дом Малмгрит", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Малмгрит{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Малмгрит{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Культ Мраа
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Религиозное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Культ Мраа"))
            {
                //Добавляем организацию Культ Мраа
                Organization organization = new(_userCreated, true, "Культ Мраа", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Культ Мраа{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Культ Мраа{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Магистериум Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Магическое формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Магистериум Альвраатской империи"))
            {
                //Добавляем организацию Магистериум Альвраатской империи
                Organization organization = new(_userCreated, true, "Магистериум Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Магистериум Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Магистериум Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Форсварер
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Форсварер"))
            {
                //Добавляем организацию Дом Форсварер
                Organization organization = new(_userCreated, true, "Дом Форсварер", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Форсварер{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Форсварер{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Северная армия Альвраатской империи
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Военное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Северная армия Альвраатской империи"))
            {
                //Добавляем организацию Северная армия Альвраатской империи
                Organization organization = new(_userCreated, true, "Северная армия Альвраатской империи", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Северная армия Альвраатской империи{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Северная армия Альвраатской империи{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Дом Эйнтри
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Семья") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Альвраатская империя") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дом Эйнтри"))
            {
                //Добавляем организацию Дом Эйнтри
                Organization organization = new(_userCreated, true, "Дом Эйнтри", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Дом Эйнтри{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дом Эйнтри{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию Наркокартель "Мятный"
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Преступное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Рефур") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Наркокартель \"Мятный\""))
            {
                //Добавляем организацию Наркокартель "Мятный"
                Organization organization = new(_userCreated, true, "Наркокартель \"Мятный\"", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Наркокартель \"Мятный\"{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Наркокартель \"Мятный\"{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Проверяем организацию
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Правительственное формирование") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            parent = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Цивилизация Эмбрии") ?? throw new InnerException(Errors.EmptyOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Круг Древних"))
            {
                //Добавляем организацию
                Organization organization = new(_userCreated, true, "Круг Древних", typeOrganization, parent);
                await _applicationContext.Organizations.AddAsync(organization);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Круг Древних{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Круг Древних{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;
            parent = null;

            //Создаём шаблон файла скриптов
            string pattern = @"^t_organizations_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации владений
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationOwnerships()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationOwnershipsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем владение Земли дома Брейл
            Organization? organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Брейл") ?? throw new InnerException(Errors.EmptyOrganization);
            Ownership? parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Брейл"))
            {
                //Добавляем владение Земли дома Брейл
                Ownership ownership = new(_userCreated, true, "Земли дома Брейл", 1, "#544A4A", organization, parent, "AE_1_1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Брейл{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Брейл{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Футур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Футур") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Футур"))
            {
                //Добавляем владение Земли племени Футур
                Ownership ownership = new(_userCreated, true, "Земли племени Футур", 2, "#8E685F", organization, parent, "AE_1_2");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Футур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Футур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Годомор
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Годомор") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Годомор"))
            {
                //Добавляем владение Земли дома Годомор
                Ownership ownership = new(_userCreated, true, "Земли дома Годомор", 3, "#60967F", organization, parent, "AE_1_3");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Годомор{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Годомор{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Хагтандер
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Хагтандер") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Хагтандер"))
            {
                //Добавляем владение Земли дома Хагтандер
                Ownership ownership = new(_userCreated, true, "Земли дома Хагтандер", 4, "#9E9973", organization, parent, "AE_1_4");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Хагтандер{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Хагтандер{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Сколбур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Сколбур") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Сколбур"))
            {
                //Добавляем владение Земли дома Сколбур
                Ownership ownership = new(_userCreated, true, "Земли дома Сколбур", 5, "#6B8599", organization, parent, "AE_1_5");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Сколбур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Сколбур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Заёмные земли восточной армии Альвраатской империи
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Восточная армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = await _applicationContext.Ownerships.FirstAsync(x => x.Name == "Земли дома Сколбур") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Заёмные земли восточной армии Альвраатской империи"))
            {
                //Добавляем владение Заёмные земли восточной армии Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Заёмные земли восточной армии Альвраатской империи", 1, "#543131", organization, parent, "AE_1_5/1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Заёмные земли восточной армии Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Заёмные земли восточной армии Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Мутур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Мутур") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Мутур"))
            {
                //Добавляем владение Земли дома Мутур
                Ownership ownership = new(_userCreated, true, "Земли дома Мутур", 6, "#995B43", organization, parent, "AE_1_6");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Мутур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Мутур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Берр
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Берр") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Берр"))
            {
                //Добавляем владение Земли племени Берр
                Ownership ownership = new(_userCreated, true, "Земли племени Берр", 7, "#809975", organization, parent, "AE_1_7");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Берр{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Берр{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли императриц Альвраатской империи
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Императрицы Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли императриц Альвраатской империи"))
            {
                //Добавляем владение Земли императриц Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Земли императриц Альвраатской империи", 8, "#75A0C6", organization, parent, "AE_1_8");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли императриц Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли императриц Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли каперов Ланиэль
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Каперы Ланиэль") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = await _applicationContext.Ownerships.FirstAsync(x => x.Name == "Земли императриц Альвраатской империи") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли каперов Ланиэль"))
            {
                //Добавляем владение Земли каперов Ланиэль
                Ownership ownership = new(_userCreated, true, "Земли каперов Ланиэль", 1, "#765B87", organization, parent, "AE_1_8/1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли каперов Ланиэль{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли каперов Ланиэль{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Фердмадр
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Фердмадр") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Фердмадр"))
            {
                //Добавляем владение Земли дома Фердмадр
                Ownership ownership = new(_userCreated, true, "Земли дома Фердмадр", 1, "#6284C4", organization, parent, "AE_2_1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Фердмадр{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Фердмадр{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Хаар
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Хаар") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Хаар"))
            {
                //Добавляем владение Земли племени Хаар
                Ownership ownership = new(_userCreated, true, "Земли племени Хаар", 2, "#965877", organization, parent, "AE_2_2");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Хаар{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Хаар{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Вингар
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Вингар") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Вингар"))
            {
                //Добавляем владение Земли дома Вингар
                Ownership ownership = new(_userCreated, true, "Земли дома Вингар", 3, "#797693", organization, parent, "AE_2_3");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Вингар{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Вингар{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Скид
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Скид") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Скид"))
            {
                //Добавляем владение Земли дома Скид
                Ownership ownership = new(_userCreated, true, "Земли дома Скид", 4, "#915F68", organization, parent, "AE_2_4");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Скид{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Скид{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Заёмные земли южной армии Альвраатской империи
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Южная армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = await _applicationContext.Ownerships.FirstAsync(x => x.Name == "Земли дома Скид") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Заёмные земли южной армии Альвраатской империи"))
            {
                //Добавляем владение Заёмные земли южной армии Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Заёмные земли южной армии Альвраатской империи", 1, "#487A44", organization, parent, "AE_2_4/1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Заёмные земли южной армии Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Заёмные земли южной армии Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Рефур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Наёмный отряд \"Серебряные стрелы\"") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Рефур"))
            {
                //Добавляем владение Земли дома Рефур
                Ownership ownership = new(_userCreated, true, "Земли дома Рефур", 5, "#937564", organization, parent, "AE_2_5");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Рефур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Рефур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Гунраад
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Гунраад") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Гунраад"))
            {
                //Добавляем владение Земли дома Гунраад
                Ownership ownership = new(_userCreated, true, "Земли дома Гунраад", 6, "#917879", organization, parent, "AE_2_6");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Гунраад{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Гунраад{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Беерун
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Беерун") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Беерун"))
            {
                //Добавляем владение Земли племени Беерун
                Ownership ownership = new(_userCreated, true, "Земли племени Беерун", 7, "#9178CD", organization, parent, "AE_2_7");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Беерун{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Беерун{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Ул
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Ул") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Ул"))
            {
                //Добавляем владение Земли племени Ул
                Ownership ownership = new(_userCreated, true, "Земли племени Ул", 8, "#8CCCAA", organization, parent, "AE_2_8");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Ул{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Ул{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Сурруун
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Сурруун") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Сурруун"))
            {
                //Добавляем владение Земли племени Сурруун
                Ownership ownership = new(_userCreated, true, "Земли племени Сурруун", 9, "#C9CC88", organization, parent, "AE_2_9");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Сурруун{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Сурруун{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли племени Дир
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Племя Дир") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли племени Дир"))
            {
                //Добавляем владение Земли племени Дир
                Ownership ownership = new(_userCreated, true, "Земли племени Дир", 10, "#579DCC", organization, parent, "AE_2_10");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли племени Дир{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли племени Дир{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Беерритон
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Беерритон") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Беерритон"))
            {
                //Добавляем владение Земли дома Беерритон
                Ownership ownership = new(_userCreated, true, "Земли дома Беерритон", 11, "#CC4F59", organization, parent, "AE_2_11");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Беерритон{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Беерритон{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Миркур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Миркур") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Миркур"))
            {
                //Добавляем владение Земли дома Миркур
                Ownership ownership = new(_userCreated, true, "Земли дома Миркур", 12, "#6CCCBC", organization, parent, "AE_2_12");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Миркур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Миркур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Сколбарер
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Сколбарер") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Сколбарер"))
            {
                //Добавляем владение Земли дома Сколбарер
                Ownership ownership = new(_userCreated, true, "Земли дома Сколбарер", 1, "#B2CC8C", organization, parent, "AE_3_1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Сколбарер{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Сколбарер{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Заёмные земли западной армии Альвраатской империи
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Западная армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = await _applicationContext.Ownerships.FirstAsync(x => x.Name == "Земли дома Сколбур") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Заёмные земли западной армии Альвраатской империи"))
            {
                //Добавляем владение Заёмные земли западной армии Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Заёмные земли западной армии Альвраатской империи", 1, "#CC6187", organization, parent, "AE_3_1/1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Заёмные земли западной армии Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Заёмные земли западной армии Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Фискур
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Фискур") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Фискур"))
            {
                //Добавляем владение Земли дома Фискур
                Ownership ownership = new(_userCreated, true, "Земли дома Фискур", 2, "#7EA1B5", organization, parent, "AE_3_2");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Фискур{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Фискур{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Снобгород
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Снобгород") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Снобгород"))
            {
                //Добавляем владение Земли дома Снобгород
                Ownership ownership = new(_userCreated, true, "Земли дома Снобгород", 3, "#A5CEA9", organization, parent, "AE_3_3");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Снобгород{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Снобгород{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Малмгрит
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Малмгрит") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Малмгрит"))
            {
                //Добавляем владение Земли дома Малмгрит
                Ownership ownership = new(_userCreated, true, "Земли дома Малмгрит", 4, "#CC848A", organization, parent, "AE_3_4");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Малмгрит{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Малмгрит{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли культа Мраа
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Малмгрит") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли культа Мраа"))
            {
                //Добавляем владение Земли культа Мраа
                Ownership ownership = new(_userCreated, true, "Земли культа Мраа", 5, "#7D9E7C", organization, parent, "AE_3_5");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли культа Мраа{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли культа Мраа{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли Магистериума Альвраатской империи
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Магистериум Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли Магистериума Альвраатской империи"))
            {
                //Добавляем владение Земли Магистериума Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Земли Магистериума Альвраатской империи", 1, "#AD5F8A", organization, parent, "AE_4_1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли Магистериума Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли Магистериума Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение Земли дома Форсварер
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Дом Форсварер") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли дома Форсварер"))
            {
                //Добавляем владение Земли дома Форсварер
                Ownership ownership = new(_userCreated, true, "Земли дома Форсварер", 2, "#7C6849", organization, parent, "AE_4_2");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли дома Форсварер{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли дома Форсварер{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Северная армия Альвраатской империи") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = await _applicationContext.Ownerships.FirstAsync(x => x.Name == "Земли дома Форсварер") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Заёмные земли северной армии Альвраатской империи"))
            {
                //Добавляем владение Заёмные земли северной армии Альвраатской империи
                Ownership ownership = new(_userCreated, true, "Заёмные земли северной армии Альвраатской империи", 1, "#2C6C93", organization, parent, "AE_4_2/1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Заёмные земли северной армии Альвраатской империи{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Заёмные земли северной армии Альвраатской империи{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Наркокартель \"Мятный\"") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли наркокартеля \"Мятный\""))
            {
                //Добавляем владение
                Ownership ownership = new(_userCreated, true, "Земли наркокартеля \"Мятный\"", 3, "#866393", organization, parent, "AE_4_3");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли наркокартеля \"Мятный\"{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли наркокартеля \"Мятный\"{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Проверяем владение
            organization = await _applicationContext.Organizations.FirstAsync(x => x.Name == "Круг Древних") ?? throw new InnerException(Errors.EmptyOrganization);
            parent = null;
            if (!await _applicationContext.Ownerships.AnyAsync(x => x.Name == "Земли круга Древних"))
            {
                //Добавляем владение
                Ownership ownership = new(_userCreated, true, "Земли круга Древних", 1, "#75D69C", organization, parent, "EC_1_1");
                await _applicationContext.Ownerships.AddAsync(ownership);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("Земли круга Древних{0}", Informations.OwnershipAdded);
            }
            else Console.WriteLine("Земли круга Древних{0}", Informations.OwnershipAlreadyAdded);
            organization = null;
            parent = null;

            //Создаём шаблон файла скриптов
            string pattern = @"^t_ownerships_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации регионов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRegions()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRegionsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем регион Восточный Зимний архипелаг
            Country? country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AE") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Восточный Зимний архипелаг"))
            {
                //Добавляем регион Восточный Зимний архипелаг
                Region region = new(_userCreated, true, "Восточный Зимний архипелаг", 1, "#0004FF", country, "AE_1");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Восточный Зимний архипелаг{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Восточный Зимний архипелаг{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Южный Зимний архипелаг
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AE") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Южный Зимний архипелаг"))
            {
                //Добавляем регион Южный Зимний архипелаг
                Region region = new(_userCreated, true, "Южный Зимний архипелаг", 2, "#00FFFF", country, "AE_2");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Южный Зимний архипелаг{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Южный Зимний архипелаг{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Западный Зимний архипелаг
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AE") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Западный Зимний архипелаг"))
            {
                //Добавляем регион Западный Зимний архипелаг
                Region region = new(_userCreated, true, "Западный Зимний архипелаг", 3, "#26FF00", country, "AE_3");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Западный Зимний архипелаг{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Западный Зимний архипелаг{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Северный Зимний архипелаг
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AE") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Северный Зимний архипелаг"))
            {
                //Добавляем регион Северный Зимний архипелаг
                Region region = new(_userCreated, true, "Северный Зимний архипелаг", 4, "#FF0AB9", country, "AE_4");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Северный Зимний архипелаг{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Северный Зимний архипелаг{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Дамхан
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Дамхан"))
            {
                //Добавляем регион Земли клана Дамхан
                Region region = new(_userCreated, true, "Земли клана Дамхан", 1, "#8C0275", country, "FP_1");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Дамхан{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Дамхан{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Анлион
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Анлион"))
            {
                //Добавляем регион Земли клана Анлион
                Region region = new(_userCreated, true, "Земли клана Анлион", 2, "#100089", country, "FP_2");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Анлион{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Анлион{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Маиран
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Маиран"))
            {
                //Добавляем регион Земли клана Маиран
                Region region = new(_userCreated, true, "Земли клана Маиран", 3, "#068700", country, "FP_3");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Маиран{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Маиран{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Алаид
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Алаид"))
            {
                //Добавляем регион Земли клана Алаид
                Region region = new(_userCreated, true, "Земли клана Алаид", 4, "#005684", country, "FP_4");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Алаид{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Алаид{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Сеолт
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Сеолт"))
            {
                //Добавляем регион Земли клана Сеолт
                Region region = new(_userCreated, true, "Земли клана Сеолт", 5, "#658200", country, "FP_5");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Сеолт{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Сеолт{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Гхоул
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Гхоул"))
            {
                //Добавляем регион Земли клана Гхоул
                Region region = new(_userCreated, true, "Земли клана Гхоул", 6, "#007F37", country, "FP_6");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Гхоул{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Гхоул{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Фуил
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Фуил"))
            {
                //Добавляем регион Земли клана Фуил
                Region region = new(_userCreated, true, "Земли клана Фуил", 7, "#7C002F", country, "FP_7");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Фуил{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Фуил{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Ятаг
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Ятаг"))
            {
                //Добавляем регион Земли клана Ятаг
                Region region = new(_userCreated, true, "Земли клана Ятаг", 8, "#7A2400", country, "FP_8");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Ятаг{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Ятаг{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Земли клана Сеар
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "FP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Земли клана Сеар"))
            {
                //Добавляем регион Земли клана Сеар
                Region region = new(_userCreated, true, "Земли клана Сеар", 9, "#BC0000", country, "FP_9");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Земли клана Сеар{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Земли клана Сеар{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Западный зубец
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Западный зубец"))
            {
                //Добавляем регион Западный зубец
                Region region = new(_userCreated, true, "Западный зубец", 1, "#F2DE00", country, "AP_1");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Западный зубец{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Западный зубец{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Светлый берег
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Светлый берег"))
            {
                //Добавляем регион Светлый берег
                Region region = new(_userCreated, true, "Светлый берег", 2, "#28E5EF", country, "AP_2");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Светлый берег{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Светлый берег{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Центральный зубец
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "AP") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Центральный зубец"))
            {
                //Добавляем регион Центральный зубец
                Region region = new(_userCreated, true, "Центральный зубец", 3, "#6568ED", country, "AP_3");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Центральный зубец{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Центральный зубец{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Проверяем регион Коралловые острова
            country = await _applicationContext.Countries.FirstAsync(x => x.Code == "EC") ?? throw new InnerException(Errors.EmptyCountry);
            if (!await _applicationContext.Regions.AnyAsync(x => x.Name == "Коралловые острова"))
            {
                //Добавляем регион Коралловые острова
                Region region = new(_userCreated, true, "Коралловые острова", 1, "#B32821", country, "EC_1");
                await _applicationContext.Regions.AddAsync(region);

                //Логгируем
                Console.WriteLine("Коралловые острова{0}", Informations.RegionAdded);
            }
            else Console.WriteLine("Коралловые острова{0}", Informations.RegionAlreadyAdded);
            country = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_regions_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации регионов владений
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRegionsOwnerships()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRegionsOwnershipsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем регион владения
            Region? region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            Ownership? ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_2{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_2{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_3{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_3{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_4{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_4{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_5{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_5{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_5/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_5/1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_5/1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_6{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_6{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_7{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_7{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_8{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_8{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_1") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_1_8/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_1/AE_1_8/1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_1/AE_1_8/1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_2{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_2{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_3{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_3{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_4{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_4{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_4/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_4/1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_4/1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_5{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_5{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_6") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_6{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_6{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_7") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_7{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_7{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_8") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_8{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_8{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_9") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_9{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_9{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_10") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_10{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_10{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_11") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_11{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_11{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_2") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_2_12") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_2/AE_2_12{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_2/AE_2_12{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_1/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_1/1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_1/1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_2{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_2{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_3{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_3{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_4") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_4{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_4{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_3") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_3_5") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_3/AE_3_5{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_3/AE_3_5{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_4/AE_4_1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_4/AE_4_1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_4/AE_4_2{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_4/AE_4_2{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_2/1") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_4/AE_4_2/1{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_4/AE_4_2/1{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Проверяем регион владения
            region = await _applicationContext.Regions.FirstAsync(x => x.Code == "AE_4") ?? throw new InnerException(Errors.EmptyRegion);
            ownership = await _applicationContext.Ownerships.FirstAsync(x => x.Code == "AE_4_3") ?? throw new InnerException(Errors.EmptyOwnership);
            if (!await _applicationContext.RegionsOwnership.AnyAsync(x => x.Region == region && x.Ownership == ownership))
            {
                //Добавляем регион владения
                RegionOwnership regionOwnership = new(_userCreated, region, ownership);
                await _applicationContext.RegionsOwnership.AddAsync(regionOwnership);

                //Логгируем
                Console.WriteLine("AE_4/AE_4_3{0}", Informations.RegionOwnershipAdded);
            }
            else Console.WriteLine("AE_4/AE_4_3{0}", Informations.RegionOwnershipAlreadyAdded);
            region = null;
            ownership = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_regions_ownerships_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации типов организаций
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationTypesOrganizations()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationTypiesOrganizationsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем тип организации государство
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Государство"))
            {
                //Добавляем тип организации государство
                TypeOrganization typeOrganization = new(_userCreated, "Государство");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Государство{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Государство{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации семья
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Семья"))
            {
                //Добавляем тип организации семья
                TypeOrganization typeOrganization = new(_userCreated, "Семья");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Семья{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Семья{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации правительственное формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Правительственное формирование"))
            {
                //Добавляем тип организации правительственное формирование
                TypeOrganization typeOrganization = new(_userCreated, "Правительственное формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Правительственное формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Правительственное формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации военное формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Военное формирование"))
            {
                //Добавляем тип организации военное формирование
                TypeOrganization typeOrganization = new(_userCreated, "Военное формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Военное формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Военное формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации религиозное формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Религиозное формирование"))
            {
                //Добавляем тип организации религиозное формирование
                TypeOrganization typeOrganization = new(_userCreated, "Религиозное формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Религиозное формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Религиозное формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации магическое формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Магическое формирование"))
            {
                //Добавляем тип организации магическое формирование
                TypeOrganization typeOrganization = new(_userCreated, "Магическое формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Магическое формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Магическое формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации торговое формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Торговое формирование"))
            {
                //Добавляем тип организации торговое формирование
                TypeOrganization typeOrganization = new(_userCreated, "Торговое формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Торговое формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Торговое формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации культурное формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Культурное формирование"))
            {
                //Добавляем тип организации культурное формирование
                TypeOrganization typeOrganization = new(_userCreated, "Культурное формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Культурное формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Культурное формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Проверяем тип организации преступное формирование
            if (!await _applicationContext.TypiesOrganizations.AnyAsync(x => x.Name == "Преступное формирование"))
            {
                //Добавляем тип организации преступное формирование
                TypeOrganization typeOrganization = new(_userCreated, "Преступное формирование");
                await _applicationContext.TypiesOrganizations.AddAsync(typeOrganization);

                //Логгируем
                Console.WriteLine("Преступное формирование{0}", Informations.TypeOrganizationAdded);
            }
            else Console.WriteLine("Преступное формирование{0}", Informations.TypeOrganizationAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_typies_orgnanizations_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Экономика

    #endregion

    #region Общие

    #endregion

    #region Новости

    #endregion

    #region Чаты и сообщения

    #endregion

    #region Обращения

    #endregion

    #region Уведомления

    #endregion

    #region Тесты

    #endregion

    #region Биология

    /// <summary>
    /// Метод инициализации наций
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationNations()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationNationsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем нацию древних
            Race? race = await _applicationContext.Races.FirstAsync(x => x.Name == "Древний") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Древний"))
            {
                //Добавляем нацию древних
                Nation nation = new(_userCreated, "Древний", race, "Латынь");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Древний{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Древний{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию альвов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Альв") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Альв"))
            {
                //Добавляем нацию альвов
                Nation nation = new(_userCreated, "Альв", race, "Исландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Альв{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Альв{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию западных вампиров
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Вампир") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Западный вампир"))
            {
                //Добавляем нацию западных вампиров
                Nation nation = new(_userCreated, "Западный вампир", race, "Шотландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Западный вампир{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Западный вампир{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию восточных вампиров
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Вампир") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Восточный вампир"))
            {
                //Добавляем нацию восточных вампиров
                Nation nation = new(_userCreated, "Восточный вампир", race, "Шотландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Восточный вампир{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Восточный вампир{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию серых орков
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Орк") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Серый орк"))
            {
                //Добавляем нацию серых орков
                Nation nation = new(_userCreated, "Серый орк", race, "Норвежский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Серый орк{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Серый орк{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию чёрных орков
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Орк") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Чёрный орк"))
            {
                //Добавляем нацию чёрных орков
                Nation nation = new(_userCreated, "Чёрный орк", race, "Норвежский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Чёрный орк{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Чёрный орк{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию зелёных орков
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Орк") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Зелёный орк"))
            {
                //Добавляем нацию зелёных орков
                Nation nation = new(_userCreated, "Зелёный орк", race, "Норвежский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Зелёный орк{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Зелёный орк{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию белых орков
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Орк") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Белый орк"))
            {
                //Добавляем нацию белых орков
                Nation nation = new(_userCreated, "Белый орк", race, "Норвежский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Белый орк{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Белый орк{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию южных орков
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Орк") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Южный орк"))
            {
                //Добавляем нацию южных орков
                Nation nation = new(_userCreated, "Южный орк", race, "Норвежский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Южный орк{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Южный орк{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию лисцийцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Лисциец"))
            {
                //Добавляем нацию лисцийцев
                Nation nation = new(_userCreated, "Лисциец", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Лисциец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Лисциец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию рифутов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Рифут"))
            {
                //Добавляем нацию рифутов
                Nation nation = new(_userCreated, "Рифут", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Рифут{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Рифут{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию ластатов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Ластат"))
            {
                //Добавляем нацию ластатов
                Nation nation = new(_userCreated, "Ластат", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Ластат{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Ластат{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию дестинцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Дестинец"))
            {
                //Добавляем нацию дестинцев
                Nation nation = new(_userCreated, "Дестинец", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Дестинец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Дестинец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию илмарийцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Илмариец"))
            {
                //Добавляем нацию илмарийцев
                Nation nation = new(_userCreated, "Илмариец", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Илмариец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Илмариец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию асудов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Асуд"))
            {
                //Добавляем нацию асудов
                Nation nation = new(_userCreated, "Асуд", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Асуд{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Асуд{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию вальтирцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Вальтирец"))
            {
                //Добавляем нацию вальтирцев
                Nation nation = new(_userCreated, "Вальтирец", race, "Итальянский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Вальтирец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Вальтирец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию саорсинов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Саорсин"))
            {
                //Добавляем нацию саорсинов
                Nation nation = new(_userCreated, "Саорсин", race, "Ирландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Саорсин{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Саорсин{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию теоранцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Теоранец"))
            {
                //Добавляем нацию теоранцев
                Nation nation = new(_userCreated, "Теоранец", race, "Ирландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Теоранец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Теоранец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию анкостцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Анкостец"))
            {
                //Добавляем нацию анкостцев
                Nation nation = new(_userCreated, "Анкостец", race, "Ирландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Анкостец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Анкостец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию тавалинцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Тавалинец"))
            {
                //Добавляем нацию тавалинцев
                Nation nation = new(_userCreated, "Тавалинец", race, "Эстонский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Тавалинец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Тавалинец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию иглессийцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Иглессиец"))
            {
                //Добавляем нацию иглессийцев
                Nation nation = new(_userCreated, "Иглессиец", race, "Литовский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Иглессиец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Иглессиец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию плекийцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Плекиец"))
            {
                //Добавляем нацию плекийцев
                Nation nation = new(_userCreated, "Плекиец", race, "Литовский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Плекиец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Плекиец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию сиервинов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Сиервин"))
            {
                //Добавляем нацию сиервинов
                Nation nation = new(_userCreated, "Сиервин", race, "Литовский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Сиервин{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Сиервин{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию виегийцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Человек") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Виегиец"))
            {
                //Добавляем нацию виегийцев
                Nation nation = new(_userCreated, "Виегиец", race, "Литовский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Виегиец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Виегиец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию горных троллей
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Тролль") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Горный тролль"))
            {
                //Добавляем нацию горных троллей
                Nation nation = new(_userCreated, "Горный тролль", race, "Шведский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Горный тролль{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Горный тролль{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию снежных троллей
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Тролль") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Снежный тролль"))
            {
                //Добавляем нацию снежных троллей
                Nation nation = new(_userCreated, "Снежный тролль", race, "Шведский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Снежный тролль{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Снежный тролль{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию болотных троллей
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Тролль") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Болотный тролль"))
            {
                //Добавляем нацию болотных троллей
                Nation nation = new(_userCreated, "Болотный тролль", race, "Шведский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Болотный тролль{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Болотный тролль{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию лесных троллей
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Тролль") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Лесной тролль"))
            {
                //Добавляем нацию лесных троллей
                Nation nation = new(_userCreated, "Лесной тролль", race, "Шведский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Лесной тролль{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Лесной тролль{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию баккеров
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Баккер"))
            {
                //Добавляем нацию баккеров
                Nation nation = new(_userCreated, "Баккер", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Баккер{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Баккер{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию нордерцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Нордерец"))
            {
                //Добавляем нацию нордерцев
                Nation nation = new(_userCreated, "Нордерец", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Нордерец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Нордерец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию вервирунгцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Вервирунгец"))
            {
                //Добавляем нацию вервирунгцев
                Nation nation = new(_userCreated, "Вервирунгец", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Вервирунгец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Вервирунгец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию шмидов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Шмид"))
            {
                //Добавляем нацию шмидов
                Nation nation = new(_userCreated, "Шмид", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Шмид{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Шмид{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию кригеров
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Кригер"))
            {
                //Добавляем нацию кригеров
                Nation nation = new(_userCreated, "Кригер", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Кригер{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Кригер{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию куфманов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дворф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Куфман"))
            {
                //Добавляем нацию куфманов
                Nation nation = new(_userCreated, "Куфман", race, "Немецкий");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Куфман{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Куфман{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию ихтидов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Ихтид") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Ихтид"))
            {
                //Добавляем нацию ихтидов
                Nation nation = new(_userCreated, "Ихтид", race, "Хинди");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Ихтид{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Ихтид{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию удстирцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Гоблин") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Удстирец"))
            {
                //Добавляем нацию удстирцев
                Nation nation = new(_userCreated, "Удстирец", race, "Датский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Удстирец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Удстирец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию фискирцев
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Гоблин") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Фискирец"))
            {
                //Добавляем нацию фискирцев
                Nation nation = new(_userCreated, "Фискирец", race, "Датский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Фискирец{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Фискирец{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию монтов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Гоблин") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Монт"))
            {
                //Добавляем нацию монтов
                Nation nation = new(_userCreated, "Монт", race, "Датский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Монт{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Монт{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию волчьих метаморфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Метаморф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Волчий метаморф"))
            {
                //Добавляем нацию волчьих метаморфов
                Nation nation = new(_userCreated, "Волчий метаморф", race, "Эсперанто");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Волчий метаморф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Волчий метаморф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию медвежьих метаморфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Метаморф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Медвежий метаморф"))
            {
                //Добавляем нацию медвежьих метаморфов
                Nation nation = new(_userCreated, "Медвежий метаморф", race, "Эсперанто");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Медвежий метаморф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Медвежий метаморф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию кошачьих метаморфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Метаморф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Кошачий метаморф"))
            {
                //Добавляем нацию кошачьих метаморфов
                Nation nation = new(_userCreated, "Кошачий метаморф", race, "Эсперанто");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Кошачий метаморф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Кошачий метаморф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию высших эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Высший эльф"))
            {
                //Добавляем нацию высших эльфов
                Nation nation = new(_userCreated, "Высший эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Высший эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Высший эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию ночных эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Ночной эльф"))
            {
                //Добавляем нацию ночных эльфов
                Nation nation = new(_userCreated, "Ночной эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Ночной эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Ночной эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию кровавых эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Кровавый эльф"))
            {
                //Добавляем нацию кровавых эльфов
                Nation nation = new(_userCreated, "Кровавый эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Кровавый эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Кровавый эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию лесных эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Лесной эльф"))
            {
                //Добавляем нацию лесных эльфов
                Nation nation = new(_userCreated, "Лесной эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Лесной эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Лесной эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию горных эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Горный эльф"))
            {
                //Добавляем нацию горных эльфов
                Nation nation = new(_userCreated, "Горный эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Горный эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Горный эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию речных эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Речной эльф"))
            {
                //Добавляем нацию речных эльфов
                Nation nation = new(_userCreated, "Речной эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Речной эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Речной эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию солнечных эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Солнечный эльф"))
            {
                //Добавляем нацию солнечных эльфов
                Nation nation = new(_userCreated, "Солнечный эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Солнечный эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Солнечный эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию морских эльфов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Эльф") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Морской эльф"))
            {
                //Добавляем нацию морских эльфов
                Nation nation = new(_userCreated, "Морской эльф", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Морской эльф{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Морской эльф{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию дану
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Дану") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Дану"))
            {
                //Добавляем нацию дану
                Nation nation = new(_userCreated, "Дану", race, "Французский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Дану{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Дану{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию элвинов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Элвин") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Элвин"))
            {
                //Добавляем нацию элвинов
                Nation nation = new(_userCreated, "Элвин", race, "Эсперанто");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Элвин{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Элвин{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию антропозавров
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Антропозавр") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Антропозавр"))
            {
                //Добавляем нацию антропозавров
                Nation nation = new(_userCreated, "Антропозавр", race, "Латынь");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Антропозавр{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Антропозавр{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию нагов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Наг") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Наг"))
            {
                //Добавляем нацию нагов
                Nation nation = new(_userCreated, "Наг", race, "Латынь");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Наг{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Наг{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию цивилизованных мраатов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Мраат") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Цивилизованный мраат"))
            {
                //Добавляем нацию цивилизованных мраатов
                Nation nation = new(_userCreated, "Цивилизованный мраат", race, "Исландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Цивилизованный мраат{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Цивилизованный мраат{0}", Informations.NationAlreadyAdded);
            race = null;

            //Проверяем нацию диких мраатов
            race = await _applicationContext.Races.FirstAsync(x => x.Name == "Мраат") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Дикий  мраат"))
            {
                //Добавляем нацию диких мраатов
                Nation nation = new(_userCreated, "Дикий  мраат", race, "Исландский");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Дикий  мраат{0}", Informations.NationAdded);
            }
            else Console.WriteLine("Дикий  мраат{0}", Informations.NationAlreadyAdded);
            race = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_nations_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации рас
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationRaces()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationRacesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем расу древних
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Древний"))
            {
                //Добавляем расу древних
                Race race = new(_userCreated, "Древний");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Древний{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Древний{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу альвов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Альв"))
            {
                //Добавляем расу альвов
                Race race = new(_userCreated, "Альв");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Альв{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Альв{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу вампиров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Вампир"))
            {
                //Добавляем расу вампиров
                Race race = new(_userCreated, "Вампир");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Вампир{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Вампир{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу орков
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Орк"))
            {
                //Добавляем расу орков
                Race race = new(_userCreated, "Орк");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Орк{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Орк{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу людей
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Человек"))
            {
                //Добавляем расу людей
                Race race = new(_userCreated, "Человек");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Человек{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Человек{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу троллей
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Тролль"))
            {
                //Добавляем расу троллей
                Race race = new(_userCreated, "Тролль");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Тролль{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Тролль{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу дворфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Дворф"))
            {
                //Добавляем расу дворфов
                Race race = new(_userCreated, "Дворф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Дворф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Дворф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу ихтидов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Ихтид"))
            {
                //Добавляем расу ихтидов
                Race race = new(_userCreated, "Ихтид");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Ихтид{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Ихтид{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу гоблинов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Гоблин"))
            {
                //Добавляем расу гоблинов
                Race race = new(_userCreated, "Гоблин");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Гоблин{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Гоблин{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу огров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Огр"))
            {
                //Добавляем расу огров
                Race race = new(_userCreated, "Огр");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Огр{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Огр{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу метоморфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Метаморф"))
            {
                //Добавляем расу метоморфов
                Race race = new(_userCreated, "Метаморф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Метаморф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Метаморф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу эльфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Эльф"))
            {
                //Добавляем расу эльфов
                Race race = new(_userCreated, "Эльф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Эльф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Эльф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу дану
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Дану"))
            {
                //Добавляем расу дану
                Race race = new(_userCreated, "Дану");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Дану{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Дану{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу элвинов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Элвин"))
            {
                //Добавляем расу элвинов
                Race race = new(_userCreated, "Элвин");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Элвин{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Элвин{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу антропозавров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Антропозавр"))
            {
                //Добавляем расу антропозавров
                Race race = new(_userCreated, "Антропозавр");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Антропозавр{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Антропозавр{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу нагов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Наг"))
            {
                //Добавляем расу нагов
                Race race = new(_userCreated, "Наг");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Наг{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Наг{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу мраатов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Мраат"))
            {
                //Добавляем расу мраатов
                Race race = new(_userCreated, "Мраат");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Мраат{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Мраат{0}", Informations.RaceAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_races_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Социология

    #endregion

    #region Файлы

    /// <summary>
    /// Метод инициализации файлов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationFiles() 
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationFilesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем файл Almus.jpg
            TypeFile? typeFile = await _applicationContext.TypesFiles.FirstAsync(x => x.Name == "Персонажи") ?? throw new InnerException(Errors.EmptyTypeFile);
            if (!await _applicationContext.Files.AnyAsync(x => x.Name == "Almus.jpg"))
            {
                //Добавляем файл Almus.jpg
                FileEntity file = new(_userCreated, true, "Almus.jpg", typeFile);
                await _applicationContext.Files.AddAsync(file);

                //Логгируем
                Console.WriteLine("Almus.jpg{0}", Informations.FileAdded);
            }
            else Console.WriteLine("Almus.jpg{0}", Informations.FileAlreadyAdded);
            typeFile = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_files_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации файлов персонажей
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationFilesHeroes()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationFilesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем файл Almus.jpg персонажу Алмус
            FileEntity? fileEntity = await _applicationContext.Files.FirstAsync(x => x.Name == "Almus.jpg") ?? throw new InnerException(Errors.EmptyFile);
            Hero? hero = await _applicationContext.Heroes.Include(x => x.Player).ThenInclude(y => y.User).FirstAsync(x => x.PersonalName == "Алмус" && x.Player.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyHero);
            if (!await _applicationContext.FilesHeroes.AnyAsync(x => x.File == fileEntity && x.Hero == hero))
            {
                //Добавляем файл Almus.jpg пероснажу Алмус
                FileHero fileHero = new(_userCreated, fileEntity, hero);
                await _applicationContext.FilesHeroes.AddAsync(fileHero);

                //Логгируем
                Console.WriteLine("Almus.jpg/Алмус{0}", Informations.FileHeroAdded);
            }
            else Console.WriteLine("Almus.jpg/Алмус{0}", Informations.FileHeroAlreadyAdded);
            fileEntity = null;
            hero = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_files_heroes_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации типов файлов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationTypesFiles()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationTypesFilesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем тип файлов персонажей
            if (!await _applicationContext.TypesFiles.AnyAsync(x => x.Name == "Персонажи"))
            {
                //Добавляем тип файлов персонажей
                TypeFile typeFile = new(_userCreated, "Персонажи", "E:\\Program\\Files\\");
                await _applicationContext.TypesFiles.AddAsync(typeFile);

                //Логгируем
                Console.WriteLine("Персонажи{0}", Informations.TypeFilesAdded);
            }
            else Console.WriteLine("Персонажи{0}", Informations.TypeFilesAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_types_files_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region География

    /// <summary>
    /// Метод инициализации географических объектов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationGeographicalObjects()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationGeographicalObjectsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Объявляем переменную, по которой будет осуществляться поиск/добавление/логгирование
            string? value = null;

            //Проверяем наличие записи
            value = "Асфалия";
            TypeGeographicalObject? type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Материк") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            GeographicalObject? parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#839FD3", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Эмбрия";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Материк") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#A4CC78", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Дитика";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Материк") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#CE3E2B", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Акраия";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Материк") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#26C0CC", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Арборибус";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Материк") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#87D18B", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Безмятежный океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#29587A", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Сияющий океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#1F247F", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Срединный океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#3F876F", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Стылый океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#547B82", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Бесконечный океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#243699", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Южный океан";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Океан") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = null;
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#2D7799", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Зимний архипелаг";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Архипелаг") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Асфалия") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#537684", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Коралловые острова";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Архипелаг") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Эмбрия") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9E6E88", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Вороний глаз";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#7F3623", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Кедровый остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#562B12", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Дамара";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#522654", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Челюсти";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#703836", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Восточный щит";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#30666D", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Пастбища мамонтов";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#BC6D4B", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Бивень мамонта";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9F5233", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Мамонтова колыбель";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9E8458", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Ягодный остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#809B49", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Чёрный остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#60657A", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Морозный клык";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#7DA5C4", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Младший странник";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#C1BC7D", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Старший странник";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9E8B31", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Высокий остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#669899", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Тёмные крылья";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#704353", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Южный щит";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#DBCD4C", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Западный щит";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#4C9E3C", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Рыбий остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#6586D8", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Буранов";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#6673A5", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Железный остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#89A3A1", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Мраа";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#7FA056", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Клинок";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#A05959", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Дальний остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9E4399", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Северный щит";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#9B497B", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Мятный остров";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Зимний архипелаг") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#60996F", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Проверяем наличие записи
            value = "Остров Жёлтый";
            type = await _applicationContext.TypesGeographicalObjects.FirstAsync(x => x.Name == "Остров") ?? throw new InnerException(Errors.EmptyTypeGeographicalObjects);
            parent = await _applicationContext.GeographicalObjects.FirstAsync(x => x.Name == "Коралловые острова") ?? throw new InnerException(Errors.EmptyGeographicalObject);
            if (!await _applicationContext.GeographicalObjects.AnyAsync(x => x.Name == value))
            {
                //Добавляем запись
                GeographicalObject geographicalObject = new(_userCreated, true, value, "#97992B", type, parent);
                await _applicationContext.GeographicalObjects.AddAsync(geographicalObject);

                //Сохраняем добавленные данные
                await _applicationContext.SaveChangesAsync();

                //Логгируем
                Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAdded);
            }
            else Console.WriteLine("{0}{1}", value, Informations.GeographicalObjectAlreadyAdded);
            type = null;
            parent = null;

            //Создаём шаблон файла скриптов
            string pattern = @"^t_geographical_objects_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    /// <summary>
    /// Метод инициализации типов географических объектов
    /// </summary>
    /// <exception cref="InnerException">Обработанная ошибка</exception>
    public async Task InitializationTypesGeographicalObjects()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationTypesGeographicalObjectsMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Океан"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Океан");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Океан{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Океан{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Море"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Море");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Море{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Море{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Залив"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Залив");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Залив{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Залив{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Пролив"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Пролив");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Пролив{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Пролив{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Река"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Река");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Река{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Река{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Озеро"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Озеро");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Озеро{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Озеро{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Материк"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Материк");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Материк{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Материк{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Архипелаг"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Архипелаг");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Архипелаг{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Архипелаг{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Остров"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Остров");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Остров{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Остров{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Полуостров"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Полуостров");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Полуостров{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Полуостров{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Горный хребет"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Горный хребет");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Горный хребет{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Горный хребет{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Равнина"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Равнина");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Равнина{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Равнина{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Лес"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Лес");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Лес{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Лес{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Проверяем тип географических объектов
            if (!await _applicationContext.TypesGeographicalObjects.AnyAsync(x => x.Name == "Болото"))
            {
                //Добавляем тип географических объектов
                TypeGeographicalObject typeGeographicalObject = new(_userCreated, "Болото");
                await _applicationContext.TypesGeographicalObjects.AddAsync(typeGeographicalObject);

                //Логгируем
                Console.WriteLine("Болото{0}", Informations.TypeGeographicalObjectsAdded);
            }
            else Console.WriteLine("Болото{0}", Informations.TypeGeographicalObjectsAlreadyAdded);

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

            //Создаём шаблон файла скриптов
            string pattern = @"^t_types_geographical_objects_\d+.sql";

            //Проходим по всем скриптам
            foreach (var file in Directory.GetFiles(ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
            {
                //Выполняем скрипт
                await ExecuteScript(file);
            }

            //Фиксируем транзакцию
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            //Откатываем транзакцию
            await transaction.RollbackAsync();

            //Логгируем
            Console.WriteLine("{0} {1}", Errors.Error, ex);
        }
    }

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}