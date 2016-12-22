﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PhotoBank.Models;

namespace PhotoBank.Migrations
{
    [DbContext(typeof(PhotoBankContext))]
    partial class PhotoBankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhotoBank.Models.Photo", b =>
                {
                    b.Property<int>("PhotoID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PhotoID");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("PhotoBank.Models.PhotoTags", b =>
                {
                    b.Property<int>("PhotoID");

                    b.Property<int>("TagID");

                    b.HasKey("PhotoID", "TagID");

                    b.HasIndex("PhotoID");

                    b.HasIndex("TagID");

                    b.ToTable("PhotoTags");
                });

            modelBuilder.Entity("PhotoBank.Models.Tag", b =>
                {
                    b.Property<int>("TagID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TagPhrase")
                        .IsRequired();

                    b.HasKey("TagID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PhotoBank.Models.PhotoTags", b =>
                {
                    b.HasOne("PhotoBank.Models.Photo", "Photo")
                        .WithMany("PhotoTags")
                        .HasForeignKey("PhotoID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PhotoBank.Models.Tag", "Tag")
                        .WithMany("PhotoTags")
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
