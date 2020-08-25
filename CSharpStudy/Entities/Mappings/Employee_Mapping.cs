using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CSharpStudy.Entities
{
    public class Employee_Mapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Primary Key
            builder.HasKey( k => k.EmployeeId);
            
            // Properties
            builder.Property( t => t.EmployeeId)
                .IsRequired();
            builder.Property(t => t.EmployeeName)
                .HasMaxLength(255);
            builder.Property(t => t.EmployeeAge);
            builder.Property(t => t.DivisionCode);

            // Table and Column Mappings
            builder.ToTable("EMPlOYEE", "dbo");
            builder.Property( t => t.EmployeeId).HasColumnName("EmployeeId");
            builder.Property( t => t.EmployeeName).HasColumnName("EmployeeName");
            builder.Property( t => t.EmployeeAge).HasColumnName("EmployeeAge");
            builder.Property( t => t.DivisionCode).HasColumnName("DivisionCode");
        }
    }
}

