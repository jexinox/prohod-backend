﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Prohod.Infrastructure.Database;

#nullable disable

namespace Prohod.Infrastructure.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20230528152821_RefactoredConfigurations")]
    partial class RefactoredConfigurations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Prohod.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Prohod.Domain.VisitRequests.Forms.Form", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("EmailToSendReply")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserToVisitId")
                        .HasColumnType("uuid");

                    b.Property<string>("VisitReason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("VisitTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("UserToVisitId");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("Prohod.Domain.VisitRequests.VisitRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FormId")
                        .HasColumnType("uuid");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("WhoProcessedId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FormId")
                        .IsUnique();

                    b.HasIndex("WhoProcessedId");

                    b.ToTable("VisitRequests");
                });

            modelBuilder.Entity("Prohod.Domain.VisitRequests.Forms.Form", b =>
                {
                    b.HasOne("Prohod.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserToVisitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Prohod.Domain.VisitRequests.Forms.Passport", "Passport", b1 =>
                        {
                            b1.Property<Guid>("FormId")
                                .HasColumnType("uuid");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<DateTimeOffset>("IssueDate")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Series")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("WhoIssued")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("FormId");

                            b1.ToTable("Forms");

                            b1.WithOwner()
                                .HasForeignKey("FormId");
                        });

                    b.Navigation("Passport")
                        .IsRequired();
                });

            modelBuilder.Entity("Prohod.Domain.VisitRequests.VisitRequest", b =>
                {
                    b.HasOne("Prohod.Domain.VisitRequests.Forms.Form", null)
                        .WithOne()
                        .HasForeignKey("Prohod.Domain.VisitRequests.VisitRequest", "FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Prohod.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("WhoProcessedId");
                });
#pragma warning restore 612, 618
        }
    }
}
