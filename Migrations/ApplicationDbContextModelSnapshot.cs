﻿// <auto-generated />
using System;
using DepartmentReminderAppDemo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DepartmentReminderAppDemo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DepartmentReminderAppDemo.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<byte[]>("Logo")
                        .IsRequired()
                        .HasColumnType("VARBINARY(MAX)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentDepartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasDatabaseName("IDX_Departments_Name");

                    b.HasIndex("ParentDepartmentId")
                        .HasDatabaseName("IDX_Departments_ParentDepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DepartmentReminderAppDemo.Models.Reminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsNotified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId")
                        .HasDatabaseName("IDX_Reminders_DepartmentId");

                    b.HasIndex("DueDate")
                        .HasDatabaseName("IDX_Reminders_DueDate");

                    b.HasIndex("Title")
                        .HasDatabaseName("IDX_Reminders_Title");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("DepartmentReminderAppDemo.Models.Department", b =>
                {
                    b.HasOne("DepartmentReminderAppDemo.Models.Department", "ParentDepartment")
                        .WithMany("SubDepartments")
                        .HasForeignKey("ParentDepartmentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentDepartment");
                });

            modelBuilder.Entity("DepartmentReminderAppDemo.Models.Reminder", b =>
                {
                    b.HasOne("DepartmentReminderAppDemo.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Department");
                });

            modelBuilder.Entity("DepartmentReminderAppDemo.Models.Department", b =>
                {
                    b.Navigation("SubDepartments");
                });
#pragma warning restore 612, 618
        }
    }
}
