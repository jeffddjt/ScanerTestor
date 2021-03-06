﻿// <auto-generated />
using DyTestor.Repositories.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DyTestor.Repositories.Migrations
{
    [DbContext(typeof(DYContext))]
    [Migration("20180308073335_2nd")]
    partial class _2nd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DyTestor.Domain.Model.QRCode", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Line");

                    b.Property<int>("Sort");

                    b.Property<bool>("Sync");

                    b.HasKey("ID");

                    b.ToTable("QRCode");
                });
#pragma warning restore 612, 618
        }
    }
}
