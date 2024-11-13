﻿// <auto-generated />
using System;
using CarRentalAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarRentalAPI.Migrations
{
    [DbContext(typeof(CarRentalDbContext))]
    [Migration("20241113213945_RentModelChanged")]
    partial class RentModelChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CarRentalAPI.Models.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BrandId");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            BrandId = 1,
                            Name = "Tesla"
                        },
                        new
                        {
                            BrandId = 2,
                            Name = "Ford"
                        },
                        new
                        {
                            BrandId = 3,
                            Name = "Chevrolet"
                        });
                });

            modelBuilder.Entity("CarRentalAPI.Models.Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.HasKey("CarId");

                    b.HasIndex("ModelId");

                    b.ToTable("Cars");

                    b.HasData(
                        new
                        {
                            CarId = 1,
                            IsActive = true,
                            Mileage = 15000,
                            ModelId = 1
                        },
                        new
                        {
                            CarId = 2,
                            IsActive = false,
                            Mileage = 12000,
                            ModelId = 1
                        },
                        new
                        {
                            CarId = 3,
                            IsActive = true,
                            Mileage = 5000,
                            ModelId = 2
                        },
                        new
                        {
                            CarId = 4,
                            IsActive = true,
                            Mileage = 3000,
                            ModelId = 3
                        },
                        new
                        {
                            CarId = 5,
                            IsActive = false,
                            Mileage = 2500,
                            ModelId = 4
                        });
                });

            modelBuilder.Entity("CarRentalAPI.Models.Model", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModelId"));

                    b.Property<int>("BasePrice")
                        .HasColumnType("int");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("ModelId");

                    b.HasIndex("BrandId");

                    b.ToTable("Models");

                    b.HasData(
                        new
                        {
                            ModelId = 1,
                            BasePrice = 1000,
                            BrandId = 1,
                            Name = "Model S",
                            Year = "2024"
                        },
                        new
                        {
                            ModelId = 2,
                            BasePrice = 1500,
                            BrandId = 1,
                            Name = "Model X",
                            Year = "2024"
                        },
                        new
                        {
                            ModelId = 3,
                            BasePrice = 800,
                            BrandId = 2,
                            Name = "Mustang",
                            Year = "2023"
                        },
                        new
                        {
                            ModelId = 4,
                            BasePrice = 900,
                            BrandId = 3,
                            Name = "Camaro",
                            Year = "2023"
                        });
                });

            modelBuilder.Entity("CarRentalAPI.Models.Rent", b =>
                {
                    b.Property<int>("RentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RentId"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("RentEnd")
                        .HasColumnType("date");

                    b.Property<DateOnly>("RentStart")
                        .HasColumnType("date");

                    b.Property<short>("RentState")
                        .HasColumnType("smallint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("RentId");

                    b.HasIndex("CarId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("CarRentalAPI.Models.Car", b =>
                {
                    b.HasOne("CarRentalAPI.Models.Model", "Model")
                        .WithMany("Cars")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("CarRentalAPI.Models.Model", b =>
                {
                    b.HasOne("CarRentalAPI.Models.Brand", "Brand")
                        .WithMany("Models")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("CarRentalAPI.Models.Rent", b =>
                {
                    b.HasOne("CarRentalAPI.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarRentalAPI.Models.Brand", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("CarRentalAPI.Models.Model", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
