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

            //БИОГРАФИИ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationBiographiesHeroes"])) await InitializationBiographiesHeroes();

            //ТИПЫ ФАЙЛОВ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesFiles"])) await InitializationTypesFiles();

            //ФАЙЛЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationFiles"])) await InitializationFiles();

            //ФАЙЛЫ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationFilesHeroes"])) await InitializationFilesHeroes();

            //СТАТУСЫ НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationStatusesRequestsHeroesRegistration"])) await InitializationStatusesRequestsHeroesRegistration();

            //ДОЛЖНОСТИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationPosts"])) await InitializationPosts();

            //ЗВАНИЯ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRanks"])) await InitializationRanks();

            //ТИПЫ ОРГАНИЗАЦИЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationTypesOrganizations"])) await InitializationTypesOrganizations();

            //ОРГАНИЗАЦИИ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationOrganizations"])) await InitializationOrganizations();

            //СТРАНЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationCountries"])) await InitializationCountries();

            //КАПИТУЛЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationChapters"])) await InitializationChapters();

            //АДМИНИСТРАТОРЫ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationAdministrators"])) await InitializationAdministrators();

            //ЗАЯВКИ НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationRequestsHeroesRegistration"])) await InitializationRequestsHeroesRegistration();

            //БИОГРАФИИ ЗАЯВОК НА РЕГИСТРАЦИЮ ПЕРСОНАЖЕЙ
            if (Convert.ToBoolean(_configuration["InitializeOptions:InitializationBiographiesRequestsHeroesRegistration"])) await InitializationBiographiesRequestsHeroesRegistration();
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
    /// <returns></returns>
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
                    "смертных рас. Поэтому Алмус принял решение со свои окружением отправиться на Кораловые острова, где " +
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
    /// <returns></returns>
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
                Hero hero = new(_userCreated, true, player, "Алмус", null, null, 1, month, -9999, nation, true, 354, 201, hairsColor, eyesColor, typeBody, typeFace, true, true, null);
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
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationAreas() { }

    /// <summary>
    /// Метод инициализации стран
    /// </summary>
    /// <exception cref="InnerException"></exception>
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
                Country country = new(_userCreated, true, 1, "#20D1DB", "Исландский", organization);
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
                Country country = new(_userCreated, true, 2, "#607F47", "Ирландский", organization);
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
                Country country = new(_userCreated, true, 3, "#00687C", "Шведский", organization);
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
                Country country = new(_userCreated, true, 4, "#B200FF", "Шотландский", organization);
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
                Country country = new(_userCreated, true, 5, "#7F3B00", "Норвежский", organization);
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
                Country country = new(_userCreated, true, 6, "#7F006D", "Эстонский", organization);
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
                Country country = new(_userCreated, true, 7, "#007F0E", "Литовский", organization);
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
                Country country = new(_userCreated, true, 8, "#47617C", "Хинди", organization);
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
                Country country = new(_userCreated, true, 9, "#D82929", "Немецкий", organization);
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
                Country country = new(_userCreated, true, 10, "#4ACC39", "Французский", organization);
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
                Country country = new(_userCreated, true, 11, "#AF9200", "Французский", organization);
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
                Country country = new(_userCreated, true, 12, "#8CAF00", "Датский", organization);
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
                Country country = new(_userCreated, true, 13, "#7F1700", "Немецкий", organization);
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
                Country country = new(_userCreated, true, 14, "#2B7C55", "Итальянский", organization);
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
                Country country = new(_userCreated, true, 15, "#7B7F00", "Итальянский", organization);
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
                Country country = new(_userCreated, true, 16, "#7F002E", "Итальянский", organization);
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
                Country country = new(_userCreated, true, 17, "#B05BFF", "Финский", organization);
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
                Country country = new(_userCreated, true, 18, "#005DFF", "Итальянский", organization);
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
                Country country = new(_userCreated, true, 19, "#487F00", "Эсперанто", organization);
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
                Country country = new(_userCreated, true, 20, "#32217A", "Испанский", organization);
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
                Country country = new(_userCreated, true, 21, "#35513B", "Итальянский", organization);
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
                Country country = new(_userCreated, true, 22, "#BC3CB4", "Латынь", organization);
                await _applicationContext.Countries.AddAsync(country);

                //Логгируем
                Console.WriteLine("Мергерская Уния{0}", Informations.CountryAdded);
            }
            else Console.WriteLine("Мергерская Уния{0}", Informations.CountryAlreadyAdded);
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
    /// <exception cref="InnerException"></exception>
    public async Task InitializationFractions() { }

    /// <summary>
    /// Метод инициализации организаций
    /// </summary>
    /// <exception cref="InnerException"></exception>
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
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Альвраатская империя"))
            {
                //Добавляем организацию Альвраатская империя
                Organization organization = new(_userCreated, true, "Альвраатская империя", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Альвраатская империя{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Альвраатская империя{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Княжество Саорса
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Саорса"))
            {
                //Добавляем организацию Княжество Саорса
                Organization organization = new(_userCreated, true, "Княжество Саорса", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Княжество Саорса{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Саорса{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Берген
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Берген"))
            {
                //Добавляем организацию Королевство Берген
                Organization organization = new(_userCreated, true, "Королевство Берген", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Королевство Берген{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Берген{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Фесгарское княжество
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Фесгарское княжество"))
            {
                //Добавляем организацию Фесгарское княжество
                Organization organization = new(_userCreated, true, "Фесгарское княжество", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Фесгарское княжество{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Фесгарское княжество{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Сверденский каганат
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Сверденский каганат"))
            {
                //Добавляем организацию Сверденский каганат
                Organization organization = new(_userCreated, true, "Сверденский каганат", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Сверденский каганат{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Сверденский каганат{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Ханство Тавалин
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Ханство Тавалин"))
            {
                //Добавляем организацию Ханство Тавалин
                Organization organization = new(_userCreated, true, "Ханство Тавалин", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Ханство Тавалин{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Ханство Тавалин{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Княжество Саргиб
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Саргиб"))
            {
                //Добавляем организацию Княжество Саргиб
                Organization organization = new(_userCreated, true, "Княжество Саргиб", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Княжество Саргиб{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Саргиб{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Царство Банду
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Царство Банду"))
            {
                //Добавляем организацию Царство Банду
                Organization organization = new(_userCreated, true, "Царство Банду", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Царство Банду{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Царство Банду{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Нордер
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Нордер"))
            {
                //Добавляем организацию Королевство Нордер
                Organization organization = new(_userCreated, true, "Королевство Нордер", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Королевство Нордер{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Нордер{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Альтерское княжество
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Альтерское княжество"))
            {
                //Добавляем организацию Альтерское княжество
                Organization organization = new(_userCreated, true, "Альтерское княжество", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Альтерское княжество{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Альтерское княжество{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Орлиадарская конфедерация
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Орлиадарская конфедерация"))
            {
                //Добавляем организацию Орлиадарская конфедерация
                Organization organization = new(_userCreated, true, "Орлиадарская конфедерация", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Орлиадарская конфедерация{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Орлиадарская конфедерация{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Удстир
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Удстир"))
            {
                //Добавляем организацию Королевство Удстир
                Organization organization = new(_userCreated, true, "Королевство Удстир", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Королевство Удстир{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Удстир{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Вервирунг
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Вервирунг"))
            {
                //Добавляем организацию Королевство Вервирунг
                Organization organization = new(_userCreated, true, "Королевство Вервирунг", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Королевство Вервирунг{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Вервирунг{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Дестинский орден
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Дестинский орден"))
            {
                //Добавляем организацию Дестинский орден
                Organization organization = new(_userCreated, true, "Дестинский орден", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Дестинский орден{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Дестинский орден{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Вольный город Лийсет
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Вольный город Лийсет"))
            {
                //Добавляем организацию Вольный город Лийсет
                Organization organization = new(_userCreated, true, "Вольный город Лийсет", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Вольный город Лийсет{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Вольный город Лийсет{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Лисцийская империя
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Лисцийская империя"))
            {
                //Добавляем организацию Лисцийская империя
                Organization organization = new(_userCreated, true, "Лисцийская империя", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Лисцийская империя{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Лисцийская империя{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Королевство Вальтир
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Королевство Вальтир"))
            {
                //Добавляем организацию Королевство Вальтир
                Organization organization = new(_userCreated, true, "Королевство Вальтир", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Королевство Вальтир{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Королевство Вальтир{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Вассальное княжество Гратис
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Вассальное княжество Гратис"))
            {
                //Добавляем организацию Вассальное княжество Гратис
                Organization organization = new(_userCreated, true, "Вассальное княжество Гратис", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Вассальное княжество Гратис{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Вассальное княжество Гратис{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Княжество Ректа
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Княжество Ректа"))
            {
                //Добавляем организацию Княжество Ректа
                Organization organization = new(_userCreated, true, "Княжество Ректа", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Княжество Ректа{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Княжество Ректа{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Волар
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Волар"))
            {
                //Добавляем организацию Волар
                Organization organization = new(_userCreated, true, "Волар", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Волар{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Волар{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Союз Иль-Ладро
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Союз Иль-Ладро"))
            {
                //Добавляем организацию Союз Иль-Ладро
                Organization organization = new(_userCreated, true, "Союз Иль-Ладро", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Союз Иль-Ладро{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Союз Иль-Ладро{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Проверяем организацию Мергерская Уния
            typeOrganization = await _applicationContext.TypiesOrganizations.FirstAsync(x => x.Name == "Государство") ?? throw new InnerException(Errors.EmptyTypeOrganization);
            if (!await _applicationContext.Organizations.AnyAsync(x => x.Name == "Мергерская Уния"))
            {
                //Добавляем организацию Мергерская Уния
                Organization organization = new(_userCreated, true, "Мергерская Уния", typeOrganization, null);
                await _applicationContext.Organizations.AddAsync(organization);

                //Логгируем
                Console.WriteLine("Мергерская Уния{0}", Informations.OrganizationAdded);
            }
            else Console.WriteLine("Мергерская Уния{0}", Informations.OrganizationAlreadyAdded);
            typeOrganization = null;

            //Сохраняем добавленные данные
            await _applicationContext.SaveChangesAsync();

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
    /// <exception cref="InnerException"></exception>
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
    /// <exception cref="InnerException"></exception>
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