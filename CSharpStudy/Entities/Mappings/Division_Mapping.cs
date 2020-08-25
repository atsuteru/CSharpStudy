using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpStudy.Entities
{
    public class Division_Mapping : IEntityTypeConfiguration<Division>
    {
        public void Configure(EntityTypeBuilder<Division> builder)
        {
            // Primary Key
            builder.HasKey( k => k.DivisionCode);
            
            // Properties
            builder.Property( t => t.DivisionCode)
                .IsRequired();
            builder.Property(t => t.DivisionName)
                .HasMaxLength(255);

            // Table and Column Mappings
            builder.ToTable("DIVISION", "dbo");
            builder.Property( t => t.DivisionCode).HasColumnName("DivisionCode");
            builder.Property( t => t.DivisionName).HasColumnName("DivisionName");
        }
    }
}

