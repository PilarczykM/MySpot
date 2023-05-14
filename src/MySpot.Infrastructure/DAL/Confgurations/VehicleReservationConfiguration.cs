using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Confgurations
{
    internal sealed class VehicleReservationConfiguration : IEntityTypeConfiguration<VehicleReservation>
    {
        public void Configure(EntityTypeBuilder<VehicleReservation> builder)
        {
            builder.Property(x => x.EmployeeName).HasConversion(x => x.Value, x => new(x));
            builder.Property(x => x.LicensePLate).HasConversion(x => x.Value, x => new(x));
        }
    }
}

