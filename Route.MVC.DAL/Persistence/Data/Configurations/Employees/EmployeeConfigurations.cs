using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Route.MVCApp.DAL.Common;
using Route.MVCApp.DAL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.MVCApp.DAL.Persistence.Data.Configurations.Employees
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar(50)").IsRequired();

            builder.Property(E => E.Address).HasColumnType("varchar(100)");

            builder.Property(E => E.Salary).HasColumnType("decimal(8, 2)");

            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            builder.Property(D => D.LastModifiedOn).HasDefaultValueSql("GETDATE()");

            builder.Property(D => D.Gender)
                   .HasConversion(
                    (gender) => gender.ToString(),
                    (gender) => (Gender)Enum.Parse(typeof(Gender), gender)
                    );

            builder.Property(D => D.EmployeeType)
                  .HasConversion(
                   (employeeType) => employeeType.ToString(),
                   (employeeType) => (EmployeeType)Enum.Parse(typeof(EmployeeType), employeeType)
                   );
        }
    }
}
