﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using templater.Model;

#nullable disable

namespace templater.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220622100529_TimeStamps")]
    partial class TimeStamps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("templater.Model.Document", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("templater.Model.Template", b =>
                {
                    b.Property<string>("TemplateAppId")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreateTimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUsedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("TemplateAppId", "Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("templater.Model.TemplateApp", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemplateApps");
                });

            modelBuilder.Entity("templater.Model.Template", b =>
                {
                    b.HasOne("templater.Model.TemplateApp", "TemplateApp")
                        .WithMany()
                        .HasForeignKey("TemplateAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TemplateApp");
                });
#pragma warning restore 612, 618
        }
    }
}
