﻿// <auto-generated />
using System;
using AzureAIContentSafety.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AzureAIContentSafety.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240910002755_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AzureAIContentSafety.API.Entities.Post", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ImageHateSeverity")
                        .HasColumnType("int");

                    b.Property<bool>("ImageIsHarmful")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ImageRequiresModeration")
                        .HasColumnType("bit");

                    b.Property<int>("ImageSelfHarmSeverity")
                        .HasColumnType("int");

                    b.Property<int>("ImageSexualSeverity")
                        .HasColumnType("int");

                    b.Property<int>("ImageViolenceSeverity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("TextHateSeverity")
                        .HasColumnType("int");

                    b.Property<bool>("TextIsHarmful")
                        .HasColumnType("bit");

                    b.Property<bool>("TextRequiresModeration")
                        .HasColumnType("bit");

                    b.Property<int>("TextSelfHarmSeverity")
                        .HasColumnType("int");

                    b.Property<int>("TextSexualSeverity")
                        .HasColumnType("int");

                    b.Property<int>("TextViolenceSeverity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}