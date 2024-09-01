﻿// <auto-generated />
using System;
using HeatingGateway.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeatingGateway.Application.Migrations
{
    [DbContext(typeof(HeatingDbContext))]
    [Migration("20240218093314_SeparateHeatPumpRecordIntoPartitions")]
    partial class SeparateHeatPumpRecordIntoPartitions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HeatingGateway.Application.Domain.CompressorRecord", b =>
                {
                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<double?>("Usage")
                        .HasColumnType("double precision");

                    b.HasKey("Time");

                    b.HasIndex("Time")
                        .IsUnique();

                    b.ToTable("CompressorRecords", (string)null);
                });

            modelBuilder.Entity("HeatingGateway.Application.Domain.TankLimitRecord", b =>
                {
                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LowerTankMaximum")
                        .HasColumnType("integer");

                    b.Property<int>("LowerTankMaximumAdjusted")
                        .HasColumnType("integer");

                    b.Property<int>("LowerTankMinimum")
                        .HasColumnType("integer");

                    b.Property<int>("LowerTankMinimumAdjusted")
                        .HasColumnType("integer");

                    b.Property<int>("UpperTankMaximum")
                        .HasColumnType("integer");

                    b.Property<int>("UpperTankMaximumAdjusted")
                        .HasColumnType("integer");

                    b.Property<int>("UpperTankMinimum")
                        .HasColumnType("integer");

                    b.Property<int>("UpperTankMinimumAdjusted")
                        .HasColumnType("integer");

                    b.HasKey("Time");

                    b.HasIndex("Time")
                        .IsUnique();

                    b.ToTable("TankLimitRecords", (string)null);
                });

            modelBuilder.Entity("HeatingGateway.Application.Domain.TemperatureRecord", b =>
                {
                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Circuit1")
                        .HasColumnType("real");

                    b.Property<float>("Circuit2")
                        .HasColumnType("real");

                    b.Property<float>("Circuit3")
                        .HasColumnType("real");

                    b.Property<float>("GroundInput")
                        .HasColumnType("real");

                    b.Property<float>("GroundOutput")
                        .HasColumnType("real");

                    b.Property<float>("HotGas")
                        .HasColumnType("real");

                    b.Property<float>("Inside")
                        .HasColumnType("real");

                    b.Property<float>("LowerTank")
                        .HasColumnType("real");

                    b.Property<float>("Outside")
                        .HasColumnType("real");

                    b.Property<float>("UpperTank")
                        .HasColumnType("real");

                    b.HasKey("Time");

                    b.HasIndex("Time")
                        .IsUnique();

                    b.ToTable("TemperatureRecords", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}