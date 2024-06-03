using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.BusinessLogic.Heroes.Heroes;
using Insania.Database.Entities.Files;
using Insania.Database.Entities.Heroes;
using Insania.Entities.Context;
using Insania.Models.Files.Files;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;

using FileEntity = Insania.Database.Entities.Files.File;

namespace Insania.BusinessLogic.Files.Files;

/// <summary>
/// Сервис работы с файлами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class FilesService(ApplicationContext applicationContext, ILogger<HeroesService> logger, IMapper mapper) : IFiles
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<HeroesService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Доступные расширения
    /// </summary>
    private readonly List<string> _allowedExtensions = ["pdf", "png", "jpeg", "jpg", "bmp"];

    /// <summary>
    /// Метод добавления файла
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <returns></returns>
    public async Task<BaseResponse> Add(AddFileRequest? request)
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredAddFileMethod);

        //Открываем транзакцию
        using var transaction = _applicationContext.Database.BeginTransaction();

        try
        {
            //Проверяем данные
            if (request == null) throw new InnerException(Errors.EmptyRequest);

            //Создаём файл
            Hero? hero = await _applicationContext.Heroes.FirstAsync(x => x.Id == request.Id) ?? throw new InnerException(Errors.EmptyHero);
            TypeFile? typeFile = await _applicationContext.TypesFiles.FirstAsync(x => x.Alias == request.Type) ?? throw new InnerException(Errors.EmptyTypeFile);
            FileEntity file = new(hero.Player.User.UserName!, false, request.Name!, typeFile);
            if (file.Extention == null || !_allowedExtensions.Contains(file.Extention)) throw new InnerException(Errors.IncorrectExtention);
            await _applicationContext.Files.AddAsync(file);
            await _applicationContext.SaveChangesAsync();

            //Создаём сущность файла
            switch (typeFile.Alias)
            {
                case "Personazhi":
                    {
                        FileHero fileHero = new(hero.Player.User.UserName!, file, hero);
                        await _applicationContext.FilesHeroes.AddAsync(fileHero);
                    }
                    break;
            }
            await _applicationContext.SaveChangesAsync();

            //Записываем файл
            var path = Path.Combine(typeFile.Path, typeFile.Alias, request.Id.ToString()!);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var pathFile = Path.Combine(path, request.Name!);
            if (!Directory.Exists(pathFile))
            {
                await using var fs = new FileStream(pathFile, FileMode.CreateNew);
                request.Stream!.Position = 0;
                await request.Stream.CopyToAsync(fs);
                request.Stream.Position = 0;
            }
            //Фиксируем транзакцию
            await transaction.CommitAsync();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new BaseResponse(true, hero.Id);
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
}