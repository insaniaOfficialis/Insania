namespace Insania.Models.OutCategories.ContentTypes;

/// <summary>
/// Типы контента
/// </summary>
public static class ContentTypes
{
    /// <summary>
    /// Список типов контента
    /// </summary>
    public static readonly Dictionary<string, string> DictionaryContentTypes = new()
    {
        { "gif", "image/gif" },
        { "jpeg", "image/jpeg" },
        { "jpg", "image/jpeg" },
        { "png", "image/png" },
        { "tiff", "image/tiff" },
        { "webp", "image/webp" }
    };
}