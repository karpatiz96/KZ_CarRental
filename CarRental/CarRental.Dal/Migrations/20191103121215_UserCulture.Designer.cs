﻿// <auto-generated />
using System;
using CarRental.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRental.Dal.Migrations
{
    [DbContext(typeof(CarRentalDbContext))]
    [Migration("20191103121215_UserCulture")]
    partial class UserCulture
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarRental.Dal.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<bool>("IsInUse");

                    b.Property<string>("StreetAddress")
                        .IsRequired();

                    b.Property<int>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Budapest",
                            IsInUse = true,
                            StreetAddress = "Kacsa utca 23",
                            ZipCode = 1120
                        },
                        new
                        {
                            Id = 2,
                            City = "Budapest",
                            IsInUse = true,
                            StreetAddress = "Fenyves utca 25",
                            ZipCode = 1125
                        },
                        new
                        {
                            Id = 3,
                            City = "Budapest",
                            IsInUse = true,
                            StreetAddress = "Lomb utca 23",
                            ZipCode = 1135
                        },
                        new
                        {
                            Id = 4,
                            City = "Budapest",
                            IsInUse = true,
                            StreetAddress = "Galamb utca 25",
                            ZipCode = 1122
                        },
                        new
                        {
                            Id = 5,
                            City = "Budapest",
                            IsInUse = true,
                            StreetAddress = "Szarvas út 15",
                            ZipCode = 1134
                        });
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int?>("AddressId");

                    b.Property<string>("PlateNumber")
                        .IsRequired();

                    b.Property<int>("VehicleModelId");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("Cars");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            PlateNumber = "ABC-001",
                            VehicleModelId = 1
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            PlateNumber = "ABC-002",
                            VehicleModelId = 1
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            PlateNumber = "ABC-003",
                            VehicleModelId = 1
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            PlateNumber = "ABC-004",
                            VehicleModelId = 2
                        },
                        new
                        {
                            Id = 5,
                            Active = true,
                            PlateNumber = "ABC-005",
                            VehicleModelId = 2
                        },
                        new
                        {
                            Id = 6,
                            Active = true,
                            PlateNumber = "ABC-006",
                            VehicleModelId = 2
                        },
                        new
                        {
                            Id = 7,
                            Active = true,
                            PlateNumber = "ABC-007",
                            VehicleModelId = 3
                        },
                        new
                        {
                            Id = 8,
                            Active = true,
                            PlateNumber = "ABC-008",
                            VehicleModelId = 3
                        },
                        new
                        {
                            Id = 9,
                            Active = true,
                            PlateNumber = "ABC-009",
                            VehicleModelId = 3
                        },
                        new
                        {
                            Id = 10,
                            Active = true,
                            PlateNumber = "ABC-010",
                            VehicleModelId = 3
                        },
                        new
                        {
                            Id = 11,
                            Active = true,
                            PlateNumber = "ABC-011",
                            VehicleModelId = 4
                        },
                        new
                        {
                            Id = 12,
                            Active = true,
                            PlateNumber = "ABC-012",
                            VehicleModelId = 5
                        },
                        new
                        {
                            Id = 13,
                            Active = true,
                            PlateNumber = "ABC-013",
                            VehicleModelId = 6
                        },
                        new
                        {
                            Id = 14,
                            Active = true,
                            PlateNumber = "ABC-014",
                            VehicleModelId = 7
                        },
                        new
                        {
                            Id = 15,
                            Active = true,
                            PlateNumber = "ABC-015",
                            VehicleModelId = 8
                        },
                        new
                        {
                            Id = 16,
                            Active = true,
                            PlateNumber = "ABC-016",
                            VehicleModelId = 9
                        },
                        new
                        {
                            Id = 17,
                            Active = true,
                            PlateNumber = "ABC-017",
                            VehicleModelId = 10
                        },
                        new
                        {
                            Id = 18,
                            Active = true,
                            PlateNumber = "ABC-018",
                            VehicleModelId = 11
                        },
                        new
                        {
                            Id = 19,
                            Active = true,
                            PlateNumber = "ABC-019",
                            VehicleModelId = 11
                        },
                        new
                        {
                            Id = 20,
                            Active = true,
                            PlateNumber = "ABC-020",
                            VehicleModelId = 12
                        },
                        new
                        {
                            Id = 21,
                            Active = true,
                            PlateNumber = "ABC-021",
                            VehicleModelId = 13
                        },
                        new
                        {
                            Id = 22,
                            Active = true,
                            PlateNumber = "ABC-022",
                            VehicleModelId = 13
                        },
                        new
                        {
                            Id = 23,
                            Active = true,
                            PlateNumber = "ABC-023",
                            VehicleModelId = 13
                        },
                        new
                        {
                            Id = 24,
                            Active = true,
                            PlateNumber = "ABC-024",
                            VehicleModelId = 13
                        },
                        new
                        {
                            Id = 25,
                            Active = true,
                            PlateNumber = "ABC-025",
                            VehicleModelId = 14
                        },
                        new
                        {
                            Id = 26,
                            Active = true,
                            PlateNumber = "ABC-026",
                            VehicleModelId = 15
                        },
                        new
                        {
                            Id = 27,
                            Active = true,
                            PlateNumber = "ABC-027",
                            VehicleModelId = 16
                        },
                        new
                        {
                            Id = 28,
                            Active = true,
                            PlateNumber = "ABC-028",
                            VehicleModelId = 17
                        },
                        new
                        {
                            Id = 29,
                            Active = true,
                            PlateNumber = "ABC-029",
                            VehicleModelId = 18
                        },
                        new
                        {
                            Id = 30,
                            Active = true,
                            PlateNumber = "ABC-030",
                            VehicleModelId = 18
                        },
                        new
                        {
                            Id = 31,
                            Active = true,
                            PlateNumber = "ABC-031",
                            VehicleModelId = 19
                        },
                        new
                        {
                            Id = 32,
                            Active = true,
                            PlateNumber = "ABC-032",
                            VehicleModelId = 19
                        },
                        new
                        {
                            Id = 33,
                            Active = true,
                            PlateNumber = "ABC-033",
                            VehicleModelId = 19
                        },
                        new
                        {
                            Id = 34,
                            Active = true,
                            PlateNumber = "ABC-034",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 35,
                            Active = true,
                            PlateNumber = "ABC-035",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 36,
                            Active = true,
                            PlateNumber = "ABC-036",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 37,
                            Active = true,
                            PlateNumber = "ABC-037",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 38,
                            Active = true,
                            PlateNumber = "ABC-038",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 39,
                            Active = true,
                            PlateNumber = "ABC-039",
                            VehicleModelId = 20
                        },
                        new
                        {
                            Id = 40,
                            Active = true,
                            PlateNumber = "ABC-040",
                            VehicleModelId = 20
                        });
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreationDate");

                    b.Property<string>("Text");

                    b.Property<int?>("UserId");

                    b.Property<int>("VehicleModelId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("UserId");

                    b.Property<int>("Value");

                    b.Property<int>("VehicleModelId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId");

                    b.Property<int?>("CarId");

                    b.Property<DateTime>("DropOffTime");

                    b.Property<DateTime>("PickUpTime");

                    b.Property<decimal>("Price");

                    b.Property<int>("State");

                    b.Property<int?>("UserId");

                    b.Property<int?>("VehicleModelId");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.HasIndex("VehicleModelId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("CarRental.Dal.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Culture");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarRental.Dal.Entities.VehicleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<bool>("AirConditioning");

                    b.Property<bool>("Automatic");

                    b.Property<int>("NumberOfDoors");

                    b.Property<int>("NumberOfSeats");

                    b.Property<decimal>("PricePerDay");

                    b.Property<string>("VehicleType")
                        .IsRequired();

                    b.Property<string>("VehicleUrl")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("VehicleModels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            AirConditioning = false,
                            Automatic = false,
                            NumberOfDoors = 3,
                            NumberOfSeats = 4,
                            PricePerDay = 10000m,
                            VehicleType = "Kia Picanto MCMN",
                            VehicleUrl = "kia-picanto-3.png"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 4,
                            PricePerDay = 12000m,
                            VehicleType = "Kia Picanto MCMR",
                            VehicleUrl = "kia-picanto-5.png"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 12000m,
                            VehicleType = "Opel Corsa",
                            VehicleUrl = "opel-corsa-5.png"
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 13000m,
                            VehicleType = "Citroen C3",
                            VehicleUrl = "citroen-c3-5.png"
                        },
                        new
                        {
                            Id = 5,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 14000m,
                            VehicleType = "Citroen C4",
                            VehicleUrl = "citroen-c4-5.png"
                        },
                        new
                        {
                            Id = 6,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 17000m,
                            VehicleType = "Toyota Auris Hybrid",
                            VehicleUrl = "toyota-auris-hybrid-5.png"
                        },
                        new
                        {
                            Id = 7,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 16000m,
                            VehicleType = "Ford Focus",
                            VehicleUrl = "ford-focus-5.png"
                        },
                        new
                        {
                            Id = 8,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 13000m,
                            VehicleType = "Mini Cooper Countrman",
                            VehicleUrl = "mini-countryman-5.png"
                        },
                        new
                        {
                            Id = 9,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 19000m,
                            VehicleType = "BMW 116i",
                            VehicleUrl = "bmw-5.png"
                        },
                        new
                        {
                            Id = 10,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 15000m,
                            VehicleType = "Ford Focus SW",
                            VehicleUrl = "ford-focus-kombi-5.png"
                        },
                        new
                        {
                            Id = 11,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 4,
                            NumberOfSeats = 5,
                            PricePerDay = 18000m,
                            VehicleType = "Toyota Corolla Sedan",
                            VehicleUrl = "toyota-corolla-4.png"
                        },
                        new
                        {
                            Id = 12,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 16000m,
                            VehicleType = "Ford Mondeo",
                            VehicleUrl = "ford-mondeo-4.png"
                        },
                        new
                        {
                            Id = 13,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 4,
                            NumberOfSeats = 4,
                            PricePerDay = 13000m,
                            VehicleType = "Skoda Octavia",
                            VehicleUrl = "skoda-octavia-4.png"
                        },
                        new
                        {
                            Id = 14,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 14000m,
                            VehicleType = "Mercedes Benz B Class",
                            VehicleUrl = "mb-b-klasse-5.png"
                        },
                        new
                        {
                            Id = 15,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 4,
                            NumberOfSeats = 5,
                            PricePerDay = 18000m,
                            VehicleType = "BMW 318d",
                            VehicleUrl = "bmw-3er-4.png"
                        },
                        new
                        {
                            Id = 16,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 5,
                            NumberOfSeats = 5,
                            PricePerDay = 18000m,
                            VehicleType = "BMW X1",
                            VehicleUrl = "bmw-x1-5.png"
                        },
                        new
                        {
                            Id = 17,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 4,
                            NumberOfSeats = 5,
                            PricePerDay = 18000m,
                            VehicleType = "BMW 520d",
                            VehicleUrl = "bmw-5er-4.png"
                        },
                        new
                        {
                            Id = 18,
                            Active = true,
                            AirConditioning = true,
                            Automatic = true,
                            NumberOfDoors = 4,
                            NumberOfSeats = 5,
                            PricePerDay = 20000m,
                            VehicleType = "Mercedes-Benz S-Class",
                            VehicleUrl = "mb-s-klasse-4.png"
                        },
                        new
                        {
                            Id = 19,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 7,
                            PricePerDay = 10000m,
                            VehicleType = "Renault Grand Scenic",
                            VehicleUrl = "renault-grand-scenic-5.png"
                        },
                        new
                        {
                            Id = 20,
                            Active = true,
                            AirConditioning = true,
                            Automatic = false,
                            NumberOfDoors = 5,
                            NumberOfSeats = 8,
                            PricePerDay = 10000m,
                            VehicleType = "Ford Tourneo Custom Titanium",
                            VehicleUrl = "ford-tourneo-custom-5.png"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Car", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.Address", "Address")
                        .WithMany("Cars")
                        .HasForeignKey("AddressId");

                    b.HasOne("CarRental.Dal.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Cars")
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Comment", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");

                    b.HasOne("CarRental.Dal.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Comments")
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Rating", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId");

                    b.HasOne("CarRental.Dal.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Ratings")
                        .HasForeignKey("VehicleModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CarRental.Dal.Entities.Reservation", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.Address", "Address")
                        .WithMany("Reservations")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CarRental.Dal.Entities.Car", "Car")
                        .WithMany("Reservations")
                        .HasForeignKey("CarId");

                    b.HasOne("CarRental.Dal.Entities.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId");

                    b.HasOne("CarRental.Dal.Entities.VehicleModel", "VehicleModel")
                        .WithMany("Reservations")
                        .HasForeignKey("VehicleModelId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CarRental.Dal.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("CarRental.Dal.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
