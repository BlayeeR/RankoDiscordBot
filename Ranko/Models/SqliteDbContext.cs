using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ranko.Resources.Database
{
    class SqliteDbContext : DbContext
    {
        public DbSet<GuildConfigEntity> GuildConfig { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<AdminRoleEntity> AdminRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DbLocation = Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.1", @"Data\");
            Options.UseSqlite($"Data Source=Data\\Database.sqlite");
            Options.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(p => p.Guild)
                .WithMany(b => b.Tasks)
                .HasForeignKey(p => p.GuildId)
                .HasConstraintName("ForeignKey_TaskEntity_GuildConfigEntity")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdminRoleEntity>()
                .HasOne(p => p.Guild)
                .WithMany(b => b.AdminRoles)
                .HasForeignKey(p => p.GuildId)
                .HasConstraintName("ForeignKey_AdminRoleEntity_GuildConfigEntity")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
