﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using templater.Model;

#nullable disable

namespace templater.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiRef")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<DateTime?>("LastUsedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TemplateAppId")
                        .HasMaxLength(16)
                        .HasColumnType("INTEGER");

                    b.Property<int>("TemplateFormatId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TemplateFormatId");

                    b.HasIndex("TemplateAppId", "ApiRef")
                        .IsUnique();

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("templater.Model.TemplateApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApiRef")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemplateApps");
                });

            modelBuilder.Entity("templater.Model.TemplateFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TemplateFormats");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        },
                        new
                        {
                            Id = 2,
                            ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                        });
                });

            modelBuilder.Entity("templater.Model.Template", b =>
                {
                    b.HasOne("templater.Model.TemplateApp", "TemplateApp")
                        .WithMany()
                        .HasForeignKey("TemplateAppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("templater.Model.TemplateFormat", "TemplateFormat")
                        .WithMany()
                        .HasForeignKey("TemplateFormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TemplateApp");

                    b.Navigation("TemplateFormat");
                });
#pragma warning restore 612, 618
        }
    }
}
