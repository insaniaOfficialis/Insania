using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Insania.Entities.Context;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Players;
using Insania.Database.Entities.System;
using Insania.Database.Entities.Users;
using Insania.Models.Exceptions;
using Insania.Models.Logging;
using Insania.Database.Entities.Biology;

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

            //РАСЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationNations"])) await InitializationNations();
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
    /// <exception cref="InnerException"></exception>
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
            Script script = new("initializer", true, filePath, isSuccess, resultExecution);
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
    /// <exception cref="InnerException"></exception>
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
                User user = new("demiurge", "insania_officialis@vk.com", "+79996370439", "https://vk.com/khachko09", false, true, "Хачко", "Иван", "Валерьевич", DateTime.ParseExact("31.03.1999", "dd.MM.yyyy", CultureInfo.InvariantCulture));
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
                User user = new("divinitas", "poetrevolution_09@outlook.com", "+79996370439", "https://vk.com/allenobrien", false, true, "Брайен", "Аллен", "O'", DateTime.ParseExact("09.08.1996", "dd.MM.yyyy", CultureInfo.InvariantCulture));
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

    #endregion

    #region Права доступа

    /// <summary>
    /// Метод инициализации ролей
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InnerException"></exception>
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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationUsersRoles()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsedrsRolesMethod);

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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationPlayers()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsedrsRolesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем игрока пользователя божества
            User? user = await _userManager.FindByNameAsync("divinitas") ?? throw new InnerException(Errors.EmptyUser);
            if (!await _applicationContext.Players.AnyAsync(x => x.User == user))
            {
                //Добавляем игрока пользователю божество
                Player player = new("initializer", true, user, 999999);
                await _applicationContext.Players.AddAsync(player);

                //Логгируем
                Console.WriteLine("divinitas{0}", Informations.PlayerAdded);
            }
            else Console.WriteLine("divinitas{0}", Informations.PlayerAlreadyAdded);

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
    /// <returns></returns>
    public async Task InitializationBiographiesHeroes() { }

    /// <summary>
    /// Метод инициализации биографий заявок на регистрацию персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationBiographiesRequestsHeroesRegistration() { }

    /// <summary>
    /// Метод инициализации персонажей
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationHeroes()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsedrsRolesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем персонажа Амлус пользователя божества
            Player? player = await _applicationContext.Players.Include(x => x.User).FirstAsync(x => x.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyPlayer);
            Month? month = await _applicationContext.Months.FirstAsync(x => x.Name == "Гроз") ?? throw new InnerException(Errors.EmptyMonth);
            if (!await _applicationContext.Heroes.AnyAsync(x => x.Player == player))
            {
                //Добавляем персонажа Амлус пользователю божество
                //Hero hero = new("initializer", true, player, "Алмус", null, 1, month, -9999, );
                //await _applicationContext.Heroes.AddAsync(player);

                //Логгируем
                Console.WriteLine("divinitas{0}", Informations.PlayerAdded);
            }
            else Console.WriteLine("divinitas{0}", Informations.PlayerAlreadyAdded);
            player = null;

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
    /// <returns></returns>
    public async Task InitializationRequestsHeroesRegistration() { }

    /// <summary>
    /// Метод инициализации статусов регистраций персонажей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationStatusesRequestsHeroesRegistration() { }

    #endregion

    #region Администраторы

    /// <summary>
    /// Метод инициализации администраторов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationAdministrators() { }

    /// <summary>
    /// Метод инициализации капитулов
    /// </summary>
    /// <returns></returns>
    public async Task InitializationChapters() { }

    /// <summary>
    /// Метод инициализации должностей
    /// </summary>
    /// <returns></returns>
    public async Task InitializationPosts() { }

    /// <summary>
    /// Метод инициализации званий
    /// </summary>
    /// <returns></returns>
    public async Task InitializationRanks() { }

    #endregion

    #region Летоисчисление

    /// <summary>
    /// Метод инициализации месяцев
    /// </summary>
    /// <exception cref="InnerException"></exception>
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
                Month month = new("initializer", "Золота", season, 1);
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
                Month month = new("initializer", "Ливней", season, 2);
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
                Month month = new("initializer", "Заморозков", season, 3);
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
                Month month = new("initializer", "Снегопадов", season, 4);
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
                Month month = new("initializer", "Морозов", season, 5);
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
                Month month = new("initializer", "Оттепели", season, 6);
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
                Month month = new("initializer", "Цветения", season, 7);
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
                Month month = new("initializer", "Посевов", season, 8);
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
                Month month = new("initializer", "Гроз", season, 9);
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
                Month month = new("initializer", "Поспевания", season, 10);
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
                Month month = new("initializer", "Жары", season, 11);
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
                Month month = new("initializer", "Сборов", season, 12);
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
    /// <exception cref="InnerException"></exception>
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
                Season season = new("initializer", "Дождей", 1);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Дождей{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Дождей{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон снега
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Снега"))
            {
                //Добавляем сезон снега
                Season season = new("initializer", "Снега", 1);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Снега{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Снега{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон расцвета
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Расцвета"))
            {
                //Добавляем сезон расцвета
                Season season = new("initializer", "Расцвета", 1);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Расцвета{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Расцвета{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон тепла
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Тепла"))
            {
                //Добавляем сезон тепла
                Season season = new("initializer", "Тепла", 1);
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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationEyesColors() { }

    /// <summary>
    /// Метод инициализации цветов волос
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationHairsColors() { }

    /// <summary>
    /// Метод инициализации типов телосложений
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationTypesBodies() { }

    /// <summary>
    /// Метод инициализации типов лиц
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationTypesFaces() { }

    #endregion

    #region Информационные статьи

    #endregion

    #region Политика

    /// <summary>
    /// Метод иницализации областей
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationAreas() { }

    /// <summary>
    /// Метод инициализации стран
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationCountries() { }

    /// <summary>
    /// Метод инициализации фракций
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationFractions() { }

    /// <summary>
    /// Метод инициализации организаций
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationOrganizations() { }

    /// <summary>
    /// Метод инициализации владений
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationOwnerships() { }

    /// <summary>
    /// Метод инициализации регионов
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationRegions() { }

    /// <summary>
    /// Метод инициализации типов организаций
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationTypesOrganizations() { }

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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationNations()
    {
        //Логгируем
        Console.WriteLine(Informations.EnteredInitializationUsedrsRolesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем нацию древних
            Race? race = await _applicationContext.Races.FirstAsync(x => x.Name == "Древний") ?? throw new InnerException(Errors.EmptyRace);
            if (!await _applicationContext.Nations.AnyAsync(x => x.Race == race && x.Name == "Древний"))
            {
                //Добавляем нацию древних
                Nation nation = new("initializer", "Древний", race, "Латынь");
                await _applicationContext.Nations.AddAsync(nation);

                //Логгируем
                Console.WriteLine("Древний{0}", Informations.PlayerAdded);
            }
            else Console.WriteLine("Древний{0}", Informations.PlayerAlreadyAdded);
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
    /// <exception cref="InnerException"></exception>
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
                Race race = new("initializer", "Древний");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Древний{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Древний{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу альвов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Альв"))
            {
                //Добавляем расу альвов
                Race race = new("initializer", "Альв");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Альв{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Альв{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу вампиров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Вампир"))
            {
                //Добавляем расу вампиров
                Race race = new("initializer", "Вампир");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Вампир{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Вампир{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу орков
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Орк"))
            {
                //Добавляем расу орков
                Race race = new("initializer", "Орк");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Орк{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Орк{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу людей
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Человек"))
            {
                //Добавляем расу людей
                Race race = new("initializer", "Человек");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Человек{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Человек{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу троллей
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Тролль"))
            {
                //Добавляем расу троллей
                Race race = new("initializer", "Тролль");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Тролль{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Тролль{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу дворфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Дворф"))
            {
                //Добавляем расу дворфов
                Race race = new("initializer", "Дворф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Дворф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Дворф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу ихтидов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Ихтид"))
            {
                //Добавляем расу ихтидов
                Race race = new("initializer", "Ихтид");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Ихтид{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Ихтид{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу гоблинов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Гоблин"))
            {
                //Добавляем расу гоблинов
                Race race = new("initializer", "Гоблин");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Гоблин{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Гоблин{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу огров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Огр"))
            {
                //Добавляем расу огров
                Race race = new("initializer", "Огр");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Огр{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Огр{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу метоморфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Метаморф"))
            {
                //Добавляем расу метоморфов
                Race race = new("initializer", "Метаморф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Метаморф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Метаморф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу эльфов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Эльф"))
            {
                //Добавляем расу эльфов
                Race race = new("initializer", "Эльф");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Эльф{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Эльф{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу дану
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Дану"))
            {
                //Добавляем расу дану
                Race race = new("initializer", "Дану");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Дану{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Дану{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу элвинов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Элвин"))
            {
                //Добавляем расу элвинов
                Race race = new("initializer", "Элвин");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Элвин{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Элвин{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу антропозавров
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Антропозавр"))
            {
                //Добавляем расу антропозавров
                Race race = new("initializer", "Антропозавр");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Антропозавр{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Антропозавр{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу нагов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Наг"))
            {
                //Добавляем расу нагов
                Race race = new("initializer", "Наг");
                await _applicationContext.Races.AddAsync(race);

                //Логгируем
                Console.WriteLine("Наг{0}", Informations.RaceAdded);
            }
            else Console.WriteLine("Наг{0}", Informations.RaceAlreadyAdded);

            //Проверяем расу мраатов
            if (!await _applicationContext.Races.AnyAsync(x => x.Name == "Мраат"))
            {
                //Добавляем расу мраатов
                Race race = new("initializer", "Мраат");
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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationFiles() { }

    /// <summary>
    /// Метод инициализации файлов персонажей
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationFilesHeroes() { }

    /// <summary>
    /// Метод инициализации типов файлов
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationTypesFiles() { }

    #endregion

    #region География

    /// <summary>
    /// Метод инициализации географических объектов
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationGeographicalObjects() { }

    /// <summary>
    /// Метод инициализации типов географических объектов
    /// </summary>
    /// <exception cref="InnerException"></exception>
    public async Task InitializationTypesGeographicalObjects() { }

    #endregion

    #region Карта

    #endregion

    #region Культура

    #endregion

    #region Технологии

    #endregion
}