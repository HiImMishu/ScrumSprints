﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Data;

namespace Project.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20201230101709_v1.5.7")]
    partial class v157
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Project.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int?>("ModifiedBy");

                    b.Property<int>("ProductId");

                    b.Property<int?>("SprintId");

                    b.Property<string>("status")
                        .HasMaxLength(1000);

                    b.HasKey("ItemId");

                    b.HasIndex("ProductId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Project.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DevTeam");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("OwnerId");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Project.Models.SprintBacklog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(1000);

                    b.Property<DateTime>("EndTime");

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.ToTable("Backlog");
                });

            modelBuilder.Entity("Project.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("LeaderId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("TeamCode");

                    b.Property<int?>("TeamLeaderid");

                    b.HasKey("Id");

                    b.HasIndex("TeamLeaderid");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Project.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("ArchivedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<DateTime>("SignedAt");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Project.Models.UserTeam", b =>
                {
                    b.Property<int>("TeamId");

                    b.Property<int>("UserId");

                    b.HasKey("TeamId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTeam");
                });

            modelBuilder.Entity("Project.Models.Item", b =>
                {
                    b.HasOne("Project.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Models.Team", b =>
                {
                    b.HasOne("Project.Models.User", "TeamLeader")
                        .WithMany("LeadedTeams")
                        .HasForeignKey("TeamLeaderid");
                });

            modelBuilder.Entity("Project.Models.UserTeam", b =>
                {
                    b.HasOne("Project.Models.Team", "Team")
                        .WithMany("UserTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Project.Models.User", "User")
                        .WithMany("UserTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
