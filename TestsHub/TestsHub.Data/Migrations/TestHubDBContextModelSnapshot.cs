﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
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
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

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

                    b.Property<string>("TestRunName");

                    b.HasKey("Id");

                    b.ToTable("TestRuns");
                });

            modelBuilder.Entity("TestsHub.Data.DataModel.TestCase", b =>
                {
                    b.HasOne("TestsHub.Data.DataModel.TestRun")
                        .WithMany("TestCases")
                        .HasForeignKey("TestRunId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
