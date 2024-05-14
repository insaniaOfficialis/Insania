using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;
using Insania.Entities.Models.Users;

namespace Insania.Entities.Models.Players;

/// <summary>
/// Модель сущности игрока
/// </summary>
[Table("re_players")]
[Comment("Игроки")]
public class Player : Reestr
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
    /// Баллы верности
    /// </summary>
    [Column("loyalty_points")]
    [Comment("Баллы верности")]
    public int LoyaltyPoints { get; private set; }

    /// <summary>
    /// Простой конструктор модели сущности игрока
    /// </summary>
    public Player() : base()
    {
        LoyaltyPoints = 0;
        User = new();
    }

    /// <summary>
    /// Конструктор модели сущности игрока без id
    /// </summary>
    /// <param name="user">Пользователь, изменивший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="userPlayer">Ссылка на пользователя</param>
    /// <param name="loyaltyPoints">Баллы верности</param>
    public Player(string user, bool isSystem, User userPlayer, int loyaltyPoints) : base(user, isSystem)
    {
        UserId = userPlayer.Id;
        User = userPlayer;
        LoyaltyPoints = loyaltyPoints;
    }

    /// <summary>
    /// Конструктор модели сущности игрока c id
    /// </summary>
    /// <param name="id">Первичный ключ таблицы</param>
    /// <param name="user">Пользователь, создавший</param>
    /// <param name="isSystem">Признак системной записи</param>
    /// <param name="userPlayer">Ссылка на пользователя</param>
    /// <param name="loyaltyPoints">Баллы верности</param>
    public Player(long id, string user, bool isSystem, User userPlayer, int loyaltyPoints) : base(id, user,
        isSystem)
    {
        UserId = userPlayer.Id;
        User = userPlayer;
        LoyaltyPoints = loyaltyPoints;
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
    /// Метод записи баллов верности
    /// </summary>
    /// <param name="loyaltyPoints">Баллы верности</param>
    public void SetLoyalityPounts(int loyaltyPoints)
    {
        LoyaltyPoints = loyaltyPoints;
    }
}