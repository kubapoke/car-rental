﻿// <auto-generated />
using System;
using CarRentalAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarRentalAPI.Migrations
{
    [DbContext(typeof(CarRentalDbContext))]
    partial class CarRentalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        },
                        new
                        {
                            BrandId = 4,
                            Name = "BMW"
                        },
                        new
                        {
                            BrandId = 5,
                            Name = "Audi"
                        },
                        new
                        {
                            BrandId = 6,
                            Name = "Toyota"
                        },
                        new
                        {
                            BrandId = 7,
                            Name = "Mercedes-Benz"
                        },
                        new
                        {
                            BrandId = 8,
                            Name = "Volkswagen"
                        },
                        new
                        {
                            BrandId = 9,
                            Name = "Hyundai"
                        },
                        new
                        {
                            BrandId = 10,
                            Name = "Nissan"
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

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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
                            Location = "Warsaw",
                            Mileage = 15000,
                            ModelId = 1
                        },
                        new
                        {
                            CarId = 2,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 12000,
                            ModelId = 1
                        },
                        new
                        {
                            CarId = 3,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 5000,
                            ModelId = 2
                        },
                        new
                        {
                            CarId = 4,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 3000,
                            ModelId = 3
                        },
                        new
                        {
                            CarId = 5,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 2500,
                            ModelId = 3
                        },
                        new
                        {
                            CarId = 6,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 8000,
                            ModelId = 4
                        },
                        new
                        {
                            CarId = 7,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 15000,
                            ModelId = 4
                        },
                        new
                        {
                            CarId = 8,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 20000,
                            ModelId = 5
                        },
                        new
                        {
                            CarId = 9,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 7000,
                            ModelId = 5
                        },
                        new
                        {
                            CarId = 10,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 500,
                            ModelId = 6
                        },
                        new
                        {
                            CarId = 11,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 11000,
                            ModelId = 7
                        },
                        new
                        {
                            CarId = 12,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 13000,
                            ModelId = 7
                        },
                        new
                        {
                            CarId = 13,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 18000,
                            ModelId = 8
                        },
                        new
                        {
                            CarId = 14,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 7500,
                            ModelId = 8
                        },
                        new
                        {
                            CarId = 15,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 6200,
                            ModelId = 9
                        },
                        new
                        {
                            CarId = 16,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 4000,
                            ModelId = 9
                        },
                        new
                        {
                            CarId = 17,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 14000,
                            ModelId = 10
                        },
                        new
                        {
                            CarId = 18,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 6000,
                            ModelId = 10
                        },
                        new
                        {
                            CarId = 19,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 9200,
                            ModelId = 11
                        },
                        new
                        {
                            CarId = 20,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 10500,
                            ModelId = 11
                        },
                        new
                        {
                            CarId = 21,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 8900,
                            ModelId = 12
                        },
                        new
                        {
                            CarId = 22,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 10000,
                            ModelId = 12
                        },
                        new
                        {
                            CarId = 23,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 4300,
                            ModelId = 13
                        },
                        new
                        {
                            CarId = 24,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 5100,
                            ModelId = 13
                        },
                        new
                        {
                            CarId = 25,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 2200,
                            ModelId = 14
                        },
                        new
                        {
                            CarId = 26,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 14500,
                            ModelId = 15
                        },
                        new
                        {
                            CarId = 27,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 7500,
                            ModelId = 16
                        },
                        new
                        {
                            CarId = 28,
                            IsActive = true,
                            Location = "Warsaw",
                            Mileage = 14000,
                            ModelId = 17
                        },
                        new
                        {
                            CarId = 29,
                            IsActive = true,
                            Location = "Krakow",
                            Mileage = 9300,
                            ModelId = 18
                        },
                        new
                        {
                            CarId = 30,
                            IsActive = true,
                            Location = "Bydgoszcz",
                            Mileage = 6200,
                            ModelId = 18
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
                            Name = "Model S"
                        },
                        new
                        {
                            ModelId = 2,
                            BasePrice = 1500,
                            BrandId = 1,
                            Name = "Model X"
                        },
                        new
                        {
                            ModelId = 3,
                            BasePrice = 900,
                            BrandId = 1,
                            Name = "Model 3"
                        },
                        new
                        {
                            ModelId = 4,
                            BasePrice = 800,
                            BrandId = 2,
                            Name = "Mustang",
                            Year = "2023"
                        },
                        new
                        {
                            ModelId = 5,
                            BasePrice = 700,
                            BrandId = 2,
                            Name = "F-150"
                        },
                        new
                        {
                            ModelId = 6,
                            BasePrice = 900,
                            BrandId = 3,
                            Name = "Camaro"
                        },
                        new
                        {
                            ModelId = 7,
                            BasePrice = 950,
                            BrandId = 3,
                            Name = "Silverado",
                            Year = "2024"
                        },
                        new
                        {
                            ModelId = 8,
                            BasePrice = 950,
                            BrandId = 4,
                            Name = "X5"
                        },
                        new
                        {
                            ModelId = 9,
                            BasePrice = 1050,
                            BrandId = 4,
                            Name = "3 Series",
                            Year = "2023"
                        },
                        new
                        {
                            ModelId = 10,
                            BasePrice = 1000,
                            BrandId = 5,
                            Name = "A4"
                        },
                        new
                        {
                            ModelId = 11,
                            BasePrice = 1100,
                            BrandId = 5,
                            Name = "A6"
                        },
                        new
                        {
                            ModelId = 12,
                            BasePrice = 700,
                            BrandId = 6,
                            Name = "Corolla"
                        },
                        new
                        {
                            ModelId = 13,
                            BasePrice = 800,
                            BrandId = 6,
                            Name = "Camry",
                            Year = "2023"
                        },
                        new
                        {
                            ModelId = 14,
                            BasePrice = 1300,
                            BrandId = 7,
                            Name = "C-Class"
                        },
                        new
                        {
                            ModelId = 15,
                            BasePrice = 1500,
                            BrandId = 7,
                            Name = "E-Class",
                            Year = "2024"
                        },
                        new
                        {
                            ModelId = 16,
                            BasePrice = 650,
                            BrandId = 8,
                            Name = "Golf"
                        },
                        new
                        {
                            ModelId = 17,
                            BasePrice = 750,
                            BrandId = 8,
                            Name = "Passat",
                            Year = "2023"
                        },
                        new
                        {
                            ModelId = 18,
                            BasePrice = 600,
                            BrandId = 9,
                            Name = "Elantra"
                        },
                        new
                        {
                            ModelId = 19,
                            BasePrice = 750,
                            BrandId = 9,
                            Name = "Sonata",
                            Year = "2022"
                        },
                        new
                        {
                            ModelId = 20,
                            BasePrice = 750,
                            BrandId = 10,
                            Name = "Altima"
                        },
                        new
                        {
                            ModelId = 21,
                            BasePrice = 680,
                            BrandId = 10,
                            Name = "Sentra",
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
