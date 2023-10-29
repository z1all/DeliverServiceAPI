﻿// <auto-generated />
using System;
using ASPDotNetWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231028074701_change_user_and_dish_links")]
    partial class change_user_and_dish_links
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ASPDotNetWebAPI.Models.AddressElement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Isactive")
                        .HasColumnType("integer");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("Objectguid")
                        .HasColumnType("uuid");

                    b.Property<int>("Objectid")
                        .HasColumnType("integer");

                    b.Property<string>("Typename")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AddressElements");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.DeletedTokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("TokenJTI")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DeletedTokens");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsVegetairian")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<decimal?>("Rating")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.DishInCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("DishInCarts");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Hierarchy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Isactive")
                        .HasColumnType("integer");

                    b.Property<int>("Objectid")
                        .HasColumnType("integer");

                    b.Property<int>("Parentobjid")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Hierarchys");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.House", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Addnum1")
                        .HasColumnType("text");

                    b.Property<string>("Addnum2")
                        .HasColumnType("text");

                    b.Property<int?>("Addtype1")
                        .HasColumnType("integer");

                    b.Property<int?>("Addtype2")
                        .HasColumnType("integer");

                    b.Property<string>("Housenum")
                        .HasColumnType("text");

                    b.Property<int?>("Housetype")
                        .HasColumnType("integer");

                    b.Property<int>("Isactive")
                        .HasColumnType("integer");

                    b.Property<Guid>("Objectguid")
                        .HasColumnType("uuid");

                    b.Property<int>("Objectid")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Rating", b =>
                {
                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("DishId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("HashPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.DishInCart", b =>
                {
                    b.HasOne("ASPDotNetWebAPI.Models.Dish", "Dish")
                        .WithMany("DishInCarts")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASPDotNetWebAPI.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.HasOne("ASPDotNetWebAPI.Models.User", "User")
                        .WithMany("DishInCarts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Order", b =>
                {
                    b.HasOne("ASPDotNetWebAPI.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Rating", b =>
                {
                    b.HasOne("ASPDotNetWebAPI.Models.Dish", "Dish")
                        .WithMany("Ratings")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASPDotNetWebAPI.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.Dish", b =>
                {
                    b.Navigation("DishInCarts");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("ASPDotNetWebAPI.Models.User", b =>
                {
                    b.Navigation("DishInCarts");

                    b.Navigation("Orders");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}