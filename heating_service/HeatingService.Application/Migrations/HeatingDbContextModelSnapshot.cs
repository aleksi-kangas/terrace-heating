﻿// <auto-generated />
using System;
using HeatingService.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeatingService.Application.Migrations
{
    [DbContext(typeof(HeatingDbContext))]
    partial class HeatingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HeatingService.Application.Domain.HeatPumpRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("HeatPumpRecords", (string)null);
                });

            modelBuilder.Entity("HeatingService.Application.Domain.HeatPumpRecord", b =>
                {
                    b.OwnsOne("HeatingService.Application.Domain.TankLimits", "TankLimits", b1 =>
                        {
                            b1.Property<Guid>("HeatPumpRecordId")
                                .HasColumnType("uuid");

                            b1.Property<long>("LowerTankMaximum")
                                .HasColumnType("bigint");

                            b1.Property<long>("LowerTankMinimum")
                                .HasColumnType("bigint");

                            b1.Property<long>("UpperTankMaximum")
                                .HasColumnType("bigint");

                            b1.Property<long>("UpperTankMinimum")
                                .HasColumnType("bigint");

                            b1.HasKey("HeatPumpRecordId");

                            b1.ToTable("HeatPumpRecords");

                            b1.WithOwner()
                                .HasForeignKey("HeatPumpRecordId");
                        });

                    b.OwnsOne("HeatingService.Application.Domain.Temperatures", "Temperatures", b1 =>
                        {
                            b1.Property<Guid>("HeatPumpRecordId")
                                .HasColumnType("uuid");

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

                            b1.HasKey("HeatPumpRecordId");

                            b1.ToTable("HeatPumpRecords");

                            b1.WithOwner()
                                .HasForeignKey("HeatPumpRecordId");
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
