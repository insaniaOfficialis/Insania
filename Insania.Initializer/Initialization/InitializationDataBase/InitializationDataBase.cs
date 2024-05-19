using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using File = System.IO.File;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Insania.Entities.Context;
using Insania.Database.Entities.AccessRights;
using Insania.Database.Entities.Appearance;
using Insania.Database.Entities.Chronology;
using Insania.Database.Entities.Biology;
using Insania.Database.Entities.Files;
using Insania.Database.Entities.Heroes;
using Insania.Database.Entities.Players;
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

            //ПЕРСОНАЖИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationHeroes"])) await InitializationHeroes();

            //ТИПЫ ФАЙЛОВ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesFiles"])) await InitializationTypesFiles();
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
    /// <exception cref="InnerException"></exception>
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
                Player player = new("initializer", true, user, 999999);
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
        Console.WriteLine(Informations.EnteredInitializationHeroesMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем персонажа Амлус пользователя божества
            Player? player = await _applicationContext.Players.Include(x => x.User).FirstAsync(x => x.User.UserName == "divinitas") ?? throw new InnerException(Errors.EmptyPlayer);
            Month? month = await _applicationContext.Months.FirstAsync(x => x.Name == "Гроз") ?? throw new InnerException(Errors.EmptyMonth);
            Nation? nation = await _applicationContext.Nations.FirstAsync(x => x.Name == "Древний") ?? throw new InnerException(Errors.EmptyNation);
            HairsColor? hairsColor = null;
            EyesColor? eyesColor = await _applicationContext.EyesColors.FirstAsync(x => x.Name == "Зелёные") ?? throw new InnerException(Errors.EmptyEyesColor);
            TypeBody? typeBody = await _applicationContext.TypesBodies.FirstAsync(x => x.Name == "Эктоэндоморф с фигурой-грушей") ?? throw new InnerException(Errors.EmptyTypeBody);
            TypeFace? typeFace = await _applicationContext.TypesFaces.FirstAsync(x => x.Name == "Грушевидное") ?? throw new InnerException(Errors.EmptyTypeFace);
            if (!await _applicationContext.Heroes.AnyAsync(x => x.Player == player))
            {
                //Добавляем персонажа Амлус пользователю божество
                Hero hero = new("initializer", true, player, "Алмус", null, null, 1, month, -9999, nation, true, 354, 201, hairsColor, eyesColor, typeBody, typeFace, true, true, null);
                await _applicationContext.Heroes.AddAsync(hero);

                //Логгируем
                Console.WriteLine("Алмус/divinitas{0}", Informations.HeroAdded);
            }
            else Console.WriteLine("Алмус/divinitas{0}", Informations.HeroAlreadyAdded);
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
                Season season = new("initializer", "Снега", 2);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Снега{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Снега{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон расцвета
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Расцвета"))
            {
                //Добавляем сезон расцвета
                Season season = new("initializer", "Расцвета", 3);
                await _applicationContext.Seasons.AddAsync(season);

                //Логгируем
                Console.WriteLine("Расцвета{0}", Informations.SeasonAdded);
            }
            else Console.WriteLine("Расцвета{0}", Informations.SeasonAlreadyAdded);

            //Проверяем сезон тепла
            if (!await _applicationContext.Seasons.AnyAsync(x => x.Name == "Тепла"))
            {
                //Добавляем сезон тепла
                Season season = new("initializer", "Тепла", 4);
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
                EyesColor eyesColor = new("initializer", "Синие", "#0000FF");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Синие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Синие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз голубые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Голубые"))
            {
                //Добавляем цвет глаз голубые
                EyesColor eyesColor = new("initializer", "Голубые", "#42AAFF");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Голубые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Голубые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз серые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Серые"))
            {
                //Добавляем цвет глаз серые
                EyesColor eyesColor = new("initializer", "Серые", "#808080");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Серые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Серые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз зелёные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Зелёные"))
            {
                //Добавляем цвет глаз зелёные
                EyesColor eyesColor = new("initializer", "Зелёные", "#008000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Зелёные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Зелёные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз буро-жёлто-зелёные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Буро-жёлто-зелёные"))
            {
                //Добавляем цвет глаз буро-жёлто-зелёные
                EyesColor eyesColor = new("initializer", "Буро-жёлто-зелёные", "#7F8F18");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Буро-жёлто-зелёные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Буро-жёлто-зелёные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз жёлтые
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Жёлтые"))
            {
                //Добавляем цвет глаз жёлтые
                EyesColor eyesColor = new("initializer", "Жёлтые", "#FFFF00");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Жёлтые{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Жёлтые{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз светло-карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Светло-карие"))
            {
                //Добавляем цвет глаз светло-карие
                EyesColor eyesColor = new("initializer", "Светло-карие", "#987654");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Светло-карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Светло-карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Карие"))
            {
                //Добавляем цвет глаз карие
                EyesColor eyesColor = new("initializer", "Карие", "#70493D");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз тёмно-карие
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Тёмно-карие"))
            {
                //Добавляем цвет глаз тёмно-карие
                EyesColor eyesColor = new("initializer", "Тёмно-карие", "#654321");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Тёмно-карие{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Тёмно-карие{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз чёрные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Чёрные"))
            {
                //Добавляем цвет глаз чёрные
                EyesColor eyesColor = new("initializer", "Чёрные", "#000000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Чёрные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Чёрные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз красные
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Красные"))
            {
                //Добавляем цвет глаз красные
                EyesColor eyesColor = new("initializer", "Красные", "#FF0000");
                await _applicationContext.EyesColors.AddAsync(eyesColor);

                //Логгируем
                Console.WriteLine("Красные{0}", Informations.EyesColorAdded);
            }
            else Console.WriteLine("Красные{0}", Informations.EyesColorAlreadyAdded);

            //Проверяем цвет глаз гетерохромия
            if (!await _applicationContext.EyesColors.AnyAsync(x => x.Name == "Гетерохромия"))
            {
                //Добавляем цвет глаз гетерохромия
                EyesColor eyesColor = new("initializer", "Гетерохромия", null);
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
    /// <exception cref="InnerException"></exception>
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
                HairsColor hairsColor = new("initializer", "Брюнет", "#2D170E");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Брюнет{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Брюнет{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос рыжий
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Рыжий"))
            {
                //Добавляем цвет волос рыжий
                HairsColor hairsColor = new("initializer", "Рыжий", "#91302B");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Рыжий{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Рыжий{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос блондин
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Блондин"))
            {
                //Добавляем цвет волос блондин
                HairsColor hairsColor = new("initializer", "Блондин", "#FAF0BE");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Блондин{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Блондин{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос шатен
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Шатен"))
            {
                //Добавляем цвет волос шатен
                HairsColor hairsColor = new("initializer", "Шатен", "#742802");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Шатен{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Шатен{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос русый
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Русый"))
            {
                //Добавляем цвет волос русый
                HairsColor hairsColor = new("initializer", "Русый", "#8E7962");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Русый{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Русый{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос седой
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Седой"))
            {
                //Добавляем цвет волос седой
                HairsColor hairsColor = new("initializer", "Седой", "#C6C3B5");
                await _applicationContext.HairsColors.AddAsync(hairsColor);

                //Логгируем
                Console.WriteLine("Седой{0}", Informations.HairsColorAdded);
            }
            else Console.WriteLine("Седой{0}", Informations.HairsColorAlreadyAdded);

            //Проверяем цвет волос платиновый
            if (!await _applicationContext.HairsColors.AnyAsync(x => x.Name == "Платиновый"))
            {
                //Добавляем цвет волос платиновый
                HairsColor hairsColor = new("initializer", "Платиновый", "#E5E4E2");
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
    /// <exception cref="InnerException"></exception>
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
                TypeBody typeBody = new("initializer", "Эктоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эндоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эндоморф"))
            {
                //Добавляем тип телосложения эндоморф
                TypeBody typeBody = new("initializer", "Эндоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эндоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эндоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения мезоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Мезоморф"))
            {
                //Добавляем тип телосложения мезоморф
                TypeBody typeBody = new("initializer", "Мезоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Мезоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Мезоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эктомезоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эктомезоморф"))
            {
                //Добавляем тип телосложения эктомезоморф
                TypeBody typeBody = new("initializer", "Эктомезоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктомезоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктомезоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эктоэндоморф с фигурой-грушей
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эктоэндоморф с фигурой-грушей"))
            {
                //Добавляем тип телосложения эктоэндоморф с фигурой-грушей
                TypeBody typeBody = new("initializer", "Эктоэндоморф с фигурой-грушей");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Эктоэндоморф с фигурой-грушей{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Эктоэндоморф с фигурой-грушей{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения мезоэндоморф
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Мезоэндоморф"))
            {
                //Добавляем тип телосложения мезоэндоморф
                TypeBody typeBody = new("initializer", "Мезоэндоморф");
                await _applicationContext.TypesBodies.AddAsync(typeBody);

                //Логгируем
                Console.WriteLine("Мезоэндоморф{0}", Informations.TypeBodyAdded);
            }
            else Console.WriteLine("Мезоэндоморф{0}", Informations.TypeBodyAlreadyAdded);

            //Проверяем тип телосложения эндоморф с фигурой-яблоком
            if (!await _applicationContext.TypesBodies.AnyAsync(x => x.Name == "Эндоморф с фигурой-яблоком"))
            {
                //Добавляем тип телосложения эндоморф с фигурой-яблоком
                TypeBody typeBody = new("initializer", "Эндоморф с фигурой-яблоком");
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
    /// <exception cref="InnerException"></exception>
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
                TypeFace typeFace = new("initializer", "Овальное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Овальное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Овальное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц квадратное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Квадратное"))
            {
                //Добавляем тип лиц квадратное
                TypeFace typeFace = new("initializer", "Квадратное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Квадратное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Квадратное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц круглое
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Круглое"))
            {
                //Добавляем тип лиц круглое
                TypeFace typeFace = new("initializer", "Круглое");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Круглое{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Круглое{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц прямоугольное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Прямоугольное"))
            {
                //Добавляем тип лиц прямоугольное
                TypeFace typeFace = new("initializer", "Прямоугольное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Прямоугольное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Прямоугольное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц ромбовидное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Ромбовидное"))
            {
                //Добавляем тип лиц ромбовидное
                TypeFace typeFace = new("initializer", "Ромбовидное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Ромбовидное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Ромбовидное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц треугольное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Треугольное"))
            {
                //Добавляем тип лиц треугольное
                TypeFace typeFace = new("initializer", "Треугольное");
                await _applicationContext.TypesFaces.AddAsync(typeFace);

                //Логгируем
                Console.WriteLine("Треугольное{0}", Informations.TypeFaceAdded);
            }
            else Console.WriteLine("Треугольное{0}", Informations.TypeFaceAlreadyAdded);

            //Проверяем тип лиц грушевидное
            if (!await _applicationContext.TypesFaces.AnyAsync(x => x.Name == "Грушевидное"))
            {
                //Добавляем тип лиц грушевидное
                TypeFace typeFace = new("initializer", "Грушевидное");
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
                Nation nation = new("initializer", "Древний", race, "Латынь");
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
                Nation nation = new("initializer", "Альв", race, "Исландский");
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
                Nation nation = new("initializer", "Западный вампир", race, "Шотландский");
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
                Nation nation = new("initializer", "Восточный вампир", race, "Шотландский");
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
                Nation nation = new("initializer", "Серый орк", race, "Норвежский");
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
                Nation nation = new("initializer", "Чёрный орк", race, "Норвежский");
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
                Nation nation = new("initializer", "Зелёный орк", race, "Норвежский");
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
                Nation nation = new("initializer", "Белый орк", race, "Норвежский");
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
                Nation nation = new("initializer", "Южный орк", race, "Норвежский");
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
                Nation nation = new("initializer", "Лисциец", race, "Итальянский");
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
                Nation nation = new("initializer", "Рифут", race, "Итальянский");
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
                Nation nation = new("initializer", "Ластат", race, "Итальянский");
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
                Nation nation = new("initializer", "Дестинец", race, "Итальянский");
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
                Nation nation = new("initializer", "Илмариец", race, "Итальянский");
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
                Nation nation = new("initializer", "Асуд", race, "Итальянский");
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
                Nation nation = new("initializer", "Вальтирец", race, "Итальянский");
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
                Nation nation = new("initializer", "Саорсин", race, "Ирландский");
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
                Nation nation = new("initializer", "Теоранец", race, "Ирландский");
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
                Nation nation = new("initializer", "Анкостец", race, "Ирландский");
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
                Nation nation = new("initializer", "Тавалинец", race, "Эстонский");
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
                Nation nation = new("initializer", "Иглессиец", race, "Литовский");
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
                Nation nation = new("initializer", "Плекиец", race, "Литовский");
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
                Nation nation = new("initializer", "Сиервин", race, "Литовский");
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
                Nation nation = new("initializer", "Виегиец", race, "Литовский");
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
                Nation nation = new("initializer", "Горный тролль", race, "Шведский");
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
                Nation nation = new("initializer", "Снежный тролль", race, "Шведский");
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
                Nation nation = new("initializer", "Болотный тролль", race, "Шведский");
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
                Nation nation = new("initializer", "Лесной тролль", race, "Шведский");
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
                Nation nation = new("initializer", "Баккер", race, "Немецкий");
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
                Nation nation = new("initializer", "Нордерец", race, "Немецкий");
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
                Nation nation = new("initializer", "Вервирунгец", race, "Немецкий");
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
                Nation nation = new("initializer", "Шмид", race, "Немецкий");
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
                Nation nation = new("initializer", "Кригер", race, "Немецкий");
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
                Nation nation = new("initializer", "Куфман", race, "Немецкий");
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
                Nation nation = new("initializer", "Ихтид", race, "Хинди");
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
                Nation nation = new("initializer", "Удстирец", race, "Датский");
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
                Nation nation = new("initializer", "Фискирец", race, "Датский");
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
                Nation nation = new("initializer", "Монт", race, "Датский");
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
                Nation nation = new("initializer", "Волчий метаморф", race, "Эсперанто");
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
                Nation nation = new("initializer", "Медвежий метаморф", race, "Эсперанто");
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
                Nation nation = new("initializer", "Кошачий метаморф", race, "Эсперанто");
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
                Nation nation = new("initializer", "Высший эльф", race, "Французский");
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
                Nation nation = new("initializer", "Ночной эльф", race, "Французский");
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
                Nation nation = new("initializer", "Кровавый эльф", race, "Французский");
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
                Nation nation = new("initializer", "Лесной эльф", race, "Французский");
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
                Nation nation = new("initializer", "Горный эльф", race, "Французский");
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
                Nation nation = new("initializer", "Речной эльф", race, "Французский");
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
                Nation nation = new("initializer", "Солнечный эльф", race, "Французский");
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
                Nation nation = new("initializer", "Морской эльф", race, "Французский");
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
                Nation nation = new("initializer", "Дану", race, "Французский");
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
                Nation nation = new("initializer", "Элвин", race, "Эсперанто");
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
                Nation nation = new("initializer", "Антропозавр", race, "Латынь");
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
                Nation nation = new("initializer", "Наг", race, "Латынь");
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
                Nation nation = new("initializer", "Цивилизованный мраат", race, "Исландский");
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
                Nation nation = new("initializer", "Дикий  мраат", race, "Исландский");
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
                TypeFile typeFile = new("initializer", "Персонажи", "E:\\Program\\Files\\Personazhi");
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