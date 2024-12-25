using Hospital_API.Constants;
using Hospital_API.Data.Converters;
using Hospital_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_API.Data.Configurations;

public class PatientConfig : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients").HasKey(prop => prop.Id);
        builder.Property(prop => prop.Id).HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(prop => prop.Gender).HasConversion<EnumToStringConverter<Genders>>();
        builder.Property(prop => prop.BirthDate)
            .HasColumnName("BirthDate")
            .HasColumnType("datetime2")
            .HasPrecision(0)
            .IsRequired(true);

        builder.Property(prop => prop.Active)
            .HasColumnType("TINYINT")
            .IsRequired(true)
            .HasMaxLength(1)
            .HasDefaultValue(0);
    }
}
