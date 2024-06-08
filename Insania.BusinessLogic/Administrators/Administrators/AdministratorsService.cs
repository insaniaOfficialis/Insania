using System.Security.Claims;

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Database.Entities.Administrators;
using Insania.Entities.Context;
using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;
using Insania.Models.OutCategories.Logging;
using Insania.Database.Entities.Users;

namespace Insania.BusinessLogic.Administrators.Administrators;

/// <summary>
/// Сервис работы с администраторами
/// </summary>
/// <param name="applicationContext">Контекст базы данных</param>
/// <param name="logger">Интерфейс записи логов</param>
/// <param name="mapper">Интерфейс преобразования моделей</param>
public class AdministratorsService(ApplicationContext applicationContext, ILogger<AdministratorsService> logger, IMapper mapper,
    IHttpContextAccessor httpContext) : IAdministrators
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public ApplicationContext _applicationContext = applicationContext;

    /// <summary>
    /// Интерфейс записи логов
    /// </summary>
    private readonly ILogger<AdministratorsService> _logger = logger;

    /// <summary>
    /// Интерфейс преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Контекст запроса
    /// </summary>
    private readonly IHttpContextAccessor? _httpContext = httpContext;

    /// <summary>
    /// Метод получения списка администраторов
    /// </summary>
    /// <returns cref="BaseResponseList">Базовая модель ответа для списка</returns>
    /// <exception cref="InnerException">Обработанное исключение</exception>
    /// <exception cref="Exception">Необработанное исключение</exception>
    public async Task<BaseResponseList> GetList()
    {
        //Логгируем
        _logger.LogInformation(Informations.EnteredGetListAdmistratorsMethod);

        try
        {
            //Получаем текущего пользователя
            User user = await _applicationContext
                .Users
                .Include(x => x.UserRoles)
                .FirstAsync(x => x.UserName == _httpContext!.HttpContext.User.FindFirstValue(ClaimTypes.Name)) 
                ?? throw new InnerException(Errors.EmptyUser);

            //Получаем данные с базы
            List<Administrator> rows = await _applicationContext
                .Administrators
                .Include(x => x.User)
                .Include(x => x.Post)
                .Include(x => x.Rank)
                .Include(x => x.Chapter)
                .Where(x => x.DateDeleted == null)
                .ToListAsync() 
                ?? throw new InnerException(Errors.EmptyAdministrators);

            //Если пользователь не содержит роли администратора, заменяем ФИО на ***
            if (user?.UserRoles?.Any(x => x.RoleId == 3) != true) rows.ForEach(x => x.User.SetName("***"));

            //Конвертируем ответ
            List<BaseResponseListItem> response = rows.Select(_mapper.Map<BaseResponseListItem>).ToList();

            //Логгируем
            _logger.LogInformation(Informations.Success);

            //Возвращаем результат
            return new BaseResponseList(true, response);
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