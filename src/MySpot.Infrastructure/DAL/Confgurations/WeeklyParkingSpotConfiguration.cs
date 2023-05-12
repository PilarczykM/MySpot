using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Confgurations;

internal sealed class WeeklyParkingSpotConfiguration : IEntityTypeConfiguration<WeeklyParkingSpot>
{
    public void Configure(EntityTypeBuilder<WeeklyParkingSpot> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.Name).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.Week).HasConversion(x => x.To.Value, x => new(x));
    }
}
