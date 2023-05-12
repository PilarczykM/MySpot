using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Confgurations;

internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.ParkingSpotId).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.EmployeeName).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.LicensePLate).HasConversion(x => x.Value, x => new(x));
        builder.Property(x => x.Date).HasConversion(x => x.Value, x => new(x));
    }
}
