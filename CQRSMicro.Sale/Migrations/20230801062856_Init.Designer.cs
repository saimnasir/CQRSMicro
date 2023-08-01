﻿// <auto-generated />
using System;
using CQRSMicro.Sale.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CQRSMicro.Sale.Migrations
{
    [DbContext(typeof(SaleDbContext))]
    [Migration("20230801062856_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CQRSMicro.Sale.DBContext.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("SaleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CQRSMicro.Sale.DBContext.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("CQRSMicro.Sale.DBContext.Entities.Product", b =>
                {
                    b.HasOne("CQRSMicro.Sale.DBContext.Entities.Sale", null)
                        .WithMany("Products")
                        .HasForeignKey("SaleId");
                });

            modelBuilder.Entity("CQRSMicro.Sale.DBContext.Entities.Sale", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
