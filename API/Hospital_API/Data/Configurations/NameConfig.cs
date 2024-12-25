using Hospital_API.Data.Comparers;
using Hospital_API.Data.Converters;
using Hospital_API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_API.Data.Configurations
{
    public class NameConfig : IEntityTypeConfiguration<Name>
    {
        public void Configure(EntityTypeBuilder<Name> builder)
        {
            builder.ToTable("PatientNames").HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(prop => prop.Use)
                .HasColumnType("NVARCHAR(max)");

            builder.Property(prop => prop.Family)
                .IsRequired(true)
                .HasColumnType("NVARCHAR(max)");

            builder.Property(c => c.Given)
                .HasConversion<CollectionToJsonConverter<string>, CollectionValueComparer<string>>();

            builder.HasOne(prop => prop.Patient)
                .WithOne(prop => prop.Name)
                .HasForeignKey<Name>(prop => prop.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
