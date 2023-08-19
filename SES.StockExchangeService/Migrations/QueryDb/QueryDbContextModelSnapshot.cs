﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StockExchangeService.Persistence;

#nullable disable

namespace StockExchangeService.Migrations.QueryDb
{
    [DbContext(typeof(QueryDbContext))]
    partial class QueryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StockExchangeService.Queries.Entities.StockBid", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("character varying(26)");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int>("BidType")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyStockId")
                        .IsRequired()
                        .HasColumnType("character varying(26)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("PricePerShare")
                        .HasColumnType("double precision");

                    b.Property<string>("ProcessingId")
                        .IsRequired()
                        .HasColumnType("character varying(26)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("character varying(26)");

                    b.HasKey("Id");

                    b.ToTable("StockBids");
                });
#pragma warning restore 612, 618
        }
    }
}
