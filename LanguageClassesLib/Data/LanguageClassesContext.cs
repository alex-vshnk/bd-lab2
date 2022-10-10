using System;
using System.Collections.Generic;
using LanguageClasses.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LanguageClasses
{
    public partial class LanguageClassesContext : DbContext
    {
        public LanguageClassesContext()
        {
        }

        public LanguageClassesContext(DbContextOptions<LanguageClassesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeesCourse> EmployeesCourses { get; set; } = null!;
        public virtual DbSet<Listener> Listeners { get; set; } = null!;
        public virtual DbSet<ListenersCourse> ListenersCourses { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Purpose> Purposes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigurationBuilder builder = new();
                // установка пути к текущему каталогу
                builder.SetBasePath(Directory.GetCurrentDirectory());
                // получаем конфигурацию из файла appsettings.json
                builder.AddJsonFile("appsettings.json");
                // создаем конфигурацию
                IConfigurationRoot config = builder.Build();
                // получаем строку подключения
                //string connectionString = config.GetConnectionString("SqliteConnection");
                string connectionString = config.GetConnectionString("SQLConnection");
                _ = optionsBuilder
                    .UseSqlServer(connectionString)
                    //.UseSqlite(connectionString)
                    .Options;
                optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
            }
        }
    }
}
