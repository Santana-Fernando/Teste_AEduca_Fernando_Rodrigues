using Domain.Students.Entidades;
using Infra.Data.EntityConfigurations.Students;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infra.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Definir o diretório base para procurar o appsettings.json
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Adicionar o appsettings.json
                .AddEnvironmentVariables(); // Adicionar variáveis de ambiente, se necessário

            var configuration = builder.Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DockerConnection"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new StudentsConfiguration());
        }
    }
}
