using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;
using Insania.Entities.Models.Users;

namespace Insania.Entities.Models.Administrators;

/// <summary>
/// Модель сущности администратора
/// </summary>
[Table("re_administrators")]
[Comment("Администраторы")]
public class Administrator : Reestr
{
    /// <summary>
    /// Ссылка на пользователя
    /// </summary>
    [Column("user_id")]
    [Comment("Ссылка на пользователя")]
    public long UserId { get; private set; }

    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public User User { get; private set; }

    /// <summary>
    /// Ссылка на должность
    /// </summary>
    [Column("post_id")]
    [Comment("Ссылка на должность")]
    public long PostId { get; private set; }

    /// <summary>
    /// Навигационное свойство должности
    /// </summary>
    public Post Post { get; private set; }

    /// <summary>
    /// Ссылка на звание
    /// </summary>
    [Column("rank_id")]
    [Comment("Ссылка на звание")]
    public long RankId { get; private set; }

    /// <summary>
    /// Навигационное свойство звания
    /// </summary>
    public Rank Rank { get; private set; }

    /// <summary>
    /// Ссылка на капитул
    /// </summary>
    [Column("chapter_id")]
    [Comment("Ссылка на капитул")]
    public long ChapterId { get; private set; }

    /// <summary>
    /// Навигационное свойство капитула
    /// </summary>
    public Chapter Chapter { get; private set; }

    /// <summary>
    /// Баллы почёта
    /// </summary>
    [Column("honor_points")]
    [Comment("Баллы почёта")]
    public int HonorPoints { get; private set; }

    /// <summary>
    /// Ссылка на наставника
    /// </summary>
    [Column("mentor_id")]
    [Comment("Ссылка на наставника")]
    public long? MentorId { get; private set; }

    /// <summary>
    /// Навигационное свойство наставника
    /// </summary>
    public Administrator? Mentor { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности капитула
    /// </summary>
    public Administrator() : base()
    {
        User = new();
        Post = new();
        Rank = new();
        Chapter = new();
    }

    /// <summary>
    /// Конструктор модели сущности капитула без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="userPlayer">Ссылка на пользователя</param>
    /// <param name="post">Ссылка на должность</param>
    /// <param name="rank">Ссылка на звание</param>
    /// <param name="chapter">Ссылка на капитул</param>
    /// <param name="honorPoints">Баллы почёта</param>
    /// <param name="mentor">Ссылка на наставника</param>
    public Administrator(string user, bool isSystem, User userPlayer, Post post, Rank rank,
        Chapter chapter, int honorPoints, Administrator mentor) : base(user, isSystem)
    {
        UserId = userPlayer.Id;
        User = userPlayer;
        PostId = post.Id;
        Post = post;
        RankId = rank.Id;
        Rank = rank;
        ChapterId = chapter.Id;
        Chapter = chapter;
        HonorPoints = honorPoints;
        MentorId = mentor.Id;
        Mentor = mentor;
    }

    /// <summary>
    /// Конструктор модели сущности капитула с id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="userPlayer">Ссылка на пользователя</param>
    /// <param name="post">Ссылка на должность</param>
    /// <param name="rank">Ссылка на звание</param>
    /// <param name="chapter">Ссылка на капитул</param>
    /// <param name="honorPoints">Баллы почёта</param>
    /// <param name="mentor">Ссылка на наставника</param>
    public Administrator(long id, string user, bool isSystem, User userPlayer, Post post,
        Rank rank, Chapter chapter, int honorPoints, Administrator mentor) : base(id, user,
        isSystem)
    {
        UserId = userPlayer.Id;
        User = userPlayer;
        PostId = post.Id;
        Post = post;
        RankId = rank.Id;
        Rank = rank;
        ChapterId = chapter.Id;
        Chapter = chapter;
        HonorPoints = honorPoints;
        MentorId = mentor.Id;
        Mentor = mentor;
    }

    /// <summary>
    /// Метод записи пользователя
    /// </summary>
    /// <param name="user">Ссылка на пользователя</param>
    public void SetUser(User user)
    {
        UserId = user.Id;
        User = user;
    }

    /// <summary>
    /// Метод записи должности
    /// </summary>
    /// <param name="post">Ссылка на должность</param>
    public void SetPost(Post post)
    {
        PostId = post.Id;
        Post = post;
    }

    /// <summary>
    /// Метод записи звания
    /// </summary>
    /// <param name="rank">Ссылка на звание</param>
    public void SetRank(Rank rank)
    {
        RankId = rank.Id;
        Rank = rank;
    }

    /// <summary>
    /// Метод записи капитула
    /// </summary>
    /// <param name="chapter">Ссылка на капитул</param>
    public void SetChapter(Chapter chapter)
    {
        ChapterId = chapter.Id;
        Chapter = chapter;
    }

    /// <summary>
    /// Метод записи баллов почёта
    /// </summary>
    /// <param name="honorPoints">Баллы почёта</param>
    public void SetHonorPoints(int honorPoints)
    {
        HonorPoints = honorPoints;
    }

    /// <summary>
    /// Метод записи наставника
    /// </summary>
    /// <param name="mentor">Ссылка на наставника</param>
    public void SetMentor(Administrator mentor)
    {
        MentorId = mentor.Id;
        Mentor = mentor;
    }
}