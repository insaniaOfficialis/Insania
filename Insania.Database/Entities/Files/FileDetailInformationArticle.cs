using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Database.Entities.InformationArticles;

namespace Insania.Database.Entities.Files;

/// <summary>
/// Модель сущности файла детальной части информационной статьи
/// </summary>
[Table("un_files_details_information_articleses")]
[Comment("Файлы детальных частей информационных статей")]
public class FileDetailInformationArticle : FileEntity
{
    /// <summary>
    /// Ссылка на детальную часть информационной статьи
    /// </summary>
    [Column("detail_information_article_id")]
    [Comment("Ссылка на детальную часть информационной статьи")]
    public long DetailInformationArticleId { get; private set; }

    /// <summary>
    /// Навигационное свойство детальной части информационной статьи
    /// </summary>
    public DetailInformationArticle DetailInformationArticle { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности файла детальной части информационной статьи
    /// </summary>
    public FileDetailInformationArticle() : base()
    {
        DetailInformationArticle = new();
    }

    /// <summary>
    /// Конструктор модели сущности файла детальной части информационной статьи без id и порядкового номера
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="detailInformationArticle">Ссылка на детальную часть информационной статьи</param>
    public FileDetailInformationArticle(string user, File file, DetailInformationArticle detailInformationArticle) : 
        base(user, file)
    {
        DetailInformationArticleId = detailInformationArticle.Id;
        DetailInformationArticle = detailInformationArticle;
    }

    /// <summary>
    /// Конструктор модели сущности файла детальной части информационной статьи с id и без порядкового номера
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="detailInformationArticle">Ссылка на детальную часть информационной статьи</param>
    public FileDetailInformationArticle(long id, string user, File file, DetailInformationArticle detailInformationArticle) : 
        base(id, user, file)
    {
        DetailInformationArticleId = detailInformationArticle.Id;
        DetailInformationArticle = detailInformationArticle;
    }

    /// <summary>
    /// Конструктор модели сущности файла детальной части информационной статьи без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    /// <param name="detailInformationArticle">Ссылка на детальную часть информационной статьи</param>
    public FileDetailInformationArticle(string user, File file, int sequenceNumber,
        DetailInformationArticle detailInformationArticle) : base(user, file, sequenceNumber)
    {
        DetailInformationArticleId = detailInformationArticle.Id;
        DetailInformationArticle = detailInformationArticle;
    }

    /// <summary>
    /// Конструктор модели сущности файла детальной части информационной статьи с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="file">Ссылка на файл</param>
    /// <param name="sequenceNumber">орядковый номер</param>
    public FileDetailInformationArticle(long id, string user, File file, int sequenceNumber, 
        DetailInformationArticle detailInformationArticle) : base(id, user, file, sequenceNumber)
    {
        DetailInformationArticleId = detailInformationArticle.Id;
        DetailInformationArticle = detailInformationArticle;
    }

    /// <summary>
    /// Метод записи детальной части информационной статьи
    /// </summary>
    /// <param name="detailInformationArticle">Ссылка на детальную часть информационной статьи</param>
    public void SetDetailInformationArticle(DetailInformationArticle detailInformationArticle)
    {
        DetailInformationArticleId = detailInformationArticle.Id;
        DetailInformationArticle = detailInformationArticle;
    }
}