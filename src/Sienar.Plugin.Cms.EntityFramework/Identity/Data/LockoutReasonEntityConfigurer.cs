using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sienar.Identity.Data;

internal class LockoutReasonEntityConfigurer : IEntityTypeConfiguration<LockoutReason>
{
	/// <inheritdoc />
	public void Configure(EntityTypeBuilder<LockoutReason> builder)
	{
		builder
			.HasIndex(l => l.Reason)
			.IsUnique();
		builder
			.Property(l => l.Reason)
			.HasMaxLength(255)
			.IsRequired();
	}
}