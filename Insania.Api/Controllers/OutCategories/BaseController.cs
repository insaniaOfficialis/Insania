using Microsoft.AspNetCore.Mvc;

using Insania.Models.OutCategories.Base;
using Insania.Models.OutCategories.Exceptions;

namespace Insania.Api.Controllers.OutCategories;

/// <summary>
/// Базовый контроллер
/// </summary>
/// <param name="logger">Интерфейс сервиса логгирования</param>
public class BaseController(ILogger<BaseController> logger) : Controller
{
    /// <summary>
    /// Интерфейс для записи логов
    /// </summary>
    private readonly ILogger<BaseController> _logger = logger;

    /// <summary>
    /// Построение станадртного ответа
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    /// <param name="action">Функция</param>
    /// <returns></returns>
    protected async Task<IActionResult> GetAnswerAsync<T>(Func<Task<T>> action)
    {
        try
        {
            T result = await action();

            if (result is BaseResponse baseResponse)
            {
                if (baseResponse.Success)
                {
                    _logger.LogInformation("{method}. Успешно", action.Method.Name);
                    return Ok(result);
                }
                else
                {
                    if (baseResponse != null && baseResponse.Error != null)
                    {
                        if (baseResponse.Error.Code != 500)
                        {
                            _logger.LogError("{method}. Обработанная ошибка: {error}", action.Method.Name, baseResponse.Error);
                            return StatusCode(baseResponse.Error.Code ?? 400, result);
                        }
                        else
                        {
                            _logger.LogError("{method}. Необработанная ошибка:  {error}", action.Method.Name, baseResponse.Error);
                            return StatusCode(baseResponse.Error.Code ?? 500, result);
                        }
                    }
                    else
                    {
                        _logger.LogError("{method}. Непредвиденная ошибка", action.Method.Name);
                        BaseResponse response = new(false, new BaseError(500, "Непредвиденная ошибка"));
                        return StatusCode(500, response);
                    }
                }
            }
            else
            {
                _logger.LogError("{method}. Нестандартная модель ответа", action.Method.Name);
                BaseResponse response = new(false, new BaseError(500, "Нестандартная модель ответа"));
                return StatusCode(500, response);
            }
        }
        catch (InnerException ex)
        {
            _logger.LogError("{method}. Обработанная ошибка: {error}", action.Method.Name, ex);
            return StatusCode(400, new BaseResponse(false, new BaseError(400, ex.Message)));
        }
        catch (Exception ex)
        {
            _logger.LogError("{method}. Необработанная ошибка: {error}", action.Method.Name, ex);
            return StatusCode(500, new BaseResponse(false, new BaseError(500, ex.Message)));
        }
    }
}