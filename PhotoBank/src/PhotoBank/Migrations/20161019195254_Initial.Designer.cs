using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PhotoBank.Models;

namespace PhotoBank.Migrations
{
    [DbContext(typeof(PhotoBankContext))]
    [Migration("20161019195254_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PhotoBank.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Photos");
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
        }
    }
}
