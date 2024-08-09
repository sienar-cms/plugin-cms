using System.Collections.Generic;
using Sienar.Data;

namespace Sienar.Identity;

[EntityName(Singular = "role", Plural ="roles")]
public class SienarRole : EntityBase
{
	/// <summary>
	/// Represents the name of the role
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// A list of all users in this role
	/// </summary>
	public List<SienarUser> Users { get; set; } = [];

	/// <inheritdoc/>
	public override string ToString() => Name;
}