using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext (DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>()
                .HasKey(t => new { t.TeamId, t.UserId });

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Team)
                .WithMany(t => t.UserTeams)
                .HasForeignKey(ut => ut.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(p => p.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(p => p.SprintBacklog)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SprintBacklog>()
                .HasOne(p => p.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Team)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Project.Models.User> User { get; set; }
        public DbSet<Project.Models.Team> Team { get; set; }
        public DbSet<Project.Models.Product> Product { get; set; }
        public DbSet<Project.Models.SprintBacklog> Backlog { get; set; }
        public DbSet<Project.Models.Item> Item { get; set; }

    }
}
