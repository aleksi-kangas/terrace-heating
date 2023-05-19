﻿// <auto-generated />
using System;
using HeatingService.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeatingService.Application.Migrations
{
    [DbContext(typeof(HeatingDbContext))]
    [Migration("20230519140338_HeatPumpRecordTimeAsKeyAndIndex")]
    partial class HeatPumpRecordTimeAsKeyAndIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HeatingService.Application.Domain.HeatPumpRecord", b =>
                {
                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Time");

                    b.HasIndex("Time")
                        .IsUnique();

                    b.ToTable("HeatPumpRecords", (string)null);
                });

            modelBuilder.Entity("HeatingService.Application.Domain.HeatPumpRecord", b =>
                {
                    b.OwnsOne("HeatingService.Application.Domain.TankLimits", "TankLimits", b1 =>
                        {
                            b1.Property<DateTime>("HeatPumpRecordTime")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<int>("LowerTankMaximum")
                                .HasColumnType("integer");

                            b1.Property<int>("LowerTankMaximumAdjusted")
                                .HasColumnType("integer");

                            b1.Property<int>("LowerTankMinimum")
                                .HasColumnType("integer");

                            b1.Property<int>("LowerTankMinimumAdjusted")
                                .HasColumnType("integer");

                            b1.Property<int>("UpperTankMaximum")
                                .HasColumnType("integer");

                            b1.Property<int>("UpperTankMaximumAdjusted")
                                .HasColumnType("integer");

                            b1.Property<int>("UpperTankMinimum")
                                .HasColumnType("integer");

                            b1.Property<int>("UpperTankMinimumAdjusted")
                                .HasColumnType("integer");

                            b1.HasKey("HeatPumpRecordTime");

                            b1.ToTable("HeatPumpRecords");

                            b1.WithOwner()
                                .HasForeignKey("HeatPumpRecordTime");
                        });

                    b.OwnsOne("HeatingService.Application.Domain.Temperatures", "Temperatures", b1 =>
                        {
                            b1.Property<DateTime>("HeatPumpRecordTime")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<float>("Circuit1")
                                .HasColumnType("real");

                            b1.Property<float>("Circuit2")
                                .HasColumnType("real");

                            b1.Property<float>("Circuit3")
                                .HasColumnType("real");

                            b1.Property<float>("GroundInput")
                                .HasColumnType("real");

                            b1.Property<float>("GroundOutput")
                                .HasColumnType("real");

                            b1.Property<float>("HotGas")
                                .HasColumnType("real");

                            b1.Property<float>("Inside")
                                .HasColumnType("real");

                            b1.Property<float>("LowerTank")
                                .HasColumnType("real");

                            b1.Property<float>("Outside")
                                .HasColumnType("real");

                            b1.Property<float>("UpperTank")
                                .HasColumnType("real");

                            b1.HasKey("HeatPumpRecordTime");

                            b1.ToTable("HeatPumpRecords");

                            b1.WithOwner()
                                .HasForeignKey("HeatPumpRecordTime");
                        });

                    b.Navigation("TankLimits")
                        .IsRequired();

                    b.Navigation("Temperatures")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
