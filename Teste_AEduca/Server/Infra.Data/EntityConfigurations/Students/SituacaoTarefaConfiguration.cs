using Domain.Students.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.EntityConfigurations.Students
{
    public class StudentsConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(p => p.RA).ValueGeneratedOnAdd();
            builder.Property(p => p.Nome).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(100).IsRequired();
            builder.Property(p => p.CPF).HasMaxLength(11).IsRequired();

            builder.HasData(
                new Student
                {
                    RA = 1,
                    Nome = "system",
                    Email = "system@gmail.com",
                    CPF = "39788175007"
                }
           );
        }
    }
}
