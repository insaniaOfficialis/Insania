using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Entities.Base;

namespace Insania.Entities.Administrators;

/// <summary>
/// Модель сущности капитула
/// </summary>
[Table("re_chapters")]
[Comment("Капитулы")]
public class Chapter : Reestr
{

}
