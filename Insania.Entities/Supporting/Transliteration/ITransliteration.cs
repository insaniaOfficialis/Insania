namespace Insania.Entities.Supporting.Transliteration;

/// <summary>
/// Интерфейс транслитерации
/// </summary>
public interface ITransliteration
{
    /// <summary>
    /// Метод транслитерации из кириллицы в латиницу
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    string Translit(string str);
}
