using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

/// <summary>
/// Kullanıcının ilanlara yaptığı başvuruları temsil eden model.
/// </summary>
public partial class Application
{
	/// <summary>
	/// Başvurunun kimliği.
	/// </summary>
	public int AppId { get; set; }

	/// <summary>
	/// Başvuru yapan kullanıcının kimliği.
	/// </summary>
	public int UserId { get; set; }

	/// <summary>
	/// Başvurulan ilanın kimliği.
	/// </summary>
	public int AdvertId { get; set; }

	/// <summary>
	/// Başvuru tarihi.
	/// </summary>
	public string AppDate { get; set; } = null!;

	/// <summary>
	/// Başvuru mektubu.
	/// </summary>
	public string AppLetter { get; set; } = null!;

	/// <summary>
	/// Başvurulan ilan bilgileri.
	/// </summary>
	public virtual Advertisement Advert { get; set; } = null!;

	/// <summary>
	/// Başvuru yapan kullanıcı bilgileri.
	/// </summary>
	public virtual User User { get; set; } = null!;
}
