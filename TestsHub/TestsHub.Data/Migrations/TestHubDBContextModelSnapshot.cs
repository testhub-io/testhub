﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestsHub.Data.DataModel;

namespace TestsHub.Data.Migrations
{
    [DbContext(typeof(TestHubDBContext))]
    partial class TestHubDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("TestsHub.Data.DataModel.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("OrganisationId");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.TestCase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<string>("File");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.Property<string>("SystemOut");

                    b.Property<int>("TestRunId");

                    b.Property<string>("Time");

                    b.HasKey("Id");

                    b.HasIndex("TestRunId");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.TestRun", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ProjectId");

                    b.Property<string>("TestRunName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("TestRuns");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.Project", b =>
                {
                    b.HasOne("TestsHub.Data.DataModel.Organisation", "Organisation")
                        .WithMany("Projects")
                        .HasForeignKey("OrganisationId");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.TestCase", b =>
                {
                    b.HasOne("TestsHub.Data.DataModel.TestRun")
                        .WithMany("TestCases")
                        .HasForeignKey("TestRunId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.TestRun", b =>
                {
                    b.HasOne("TestsHub.Data.DataModel.Project", "Project")
                        .WithMany("TestRuns")
                        .HasForeignKey("ProjectId");
                });
#pragma warning restore 612, 618
        }
    }
}
