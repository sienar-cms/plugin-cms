using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sienar.Identity.Data;

internal class SienarRoleEntityConfigurer : IEntityTypeConfiguration<SienarRole>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<SienarRole> builder)
	{
		builder
			.HasIndex(r => r.Name)
			.IsUnique();
		builder
			.Property(r => r.Name)
			.HasMaxLength(100)
			.IsRequired();
	}
}