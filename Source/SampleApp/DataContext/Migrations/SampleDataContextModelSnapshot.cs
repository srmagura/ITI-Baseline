﻿// <auto-generated />
using System;
using DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataContext.Migrations
{
    [DbContext(typeof(SampleDataContext))]
    partial class SampleDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("Relational:Sequence:sequences.Default", "'Default', 'sequences', '1', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:sequences.OrderNumber", "'OrderNumber', 'sequences', '10000', '5', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataContext.DbBar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateCreatedUtc");

                    b.Property<Guid>("FooId");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<string>("NotInEntity")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("FooId");

                    b.ToTable("Bars");
                });

            modelBuilder.Entity("DataContext.DbFoo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("DateCreatedUtc");

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<string>("NotInEntity")
                        .HasMaxLength(64);

                    b.Property<string>("SomeGuids");

                    b.Property<string>("SomeInts");

                    b.Property<decimal>("SomeMoney")
                        .HasColumnType("Money");

                    b.Property<long>("SomeNumber");

                    b.HasKey("Id");

                    b.ToTable("Foos");
                });

            modelBuilder.Entity("Iti.Core.Audit.AuditRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Aggregate")
                        .HasMaxLength(64);

                    b.Property<string>("AggregateId")
                        .HasMaxLength(64);

                    b.Property<string>("Changes");

                    b.Property<string>("Entity")
                        .HasMaxLength(64);

                    b.Property<string>("EntityId")
                        .HasMaxLength(64);

                    b.Property<string>("Event")
                        .HasMaxLength(64);

                    b.Property<string>("UserId")
                        .HasMaxLength(64);

                    b.Property<string>("UserName")
                        .HasMaxLength(64);

                    b.Property<DateTimeOffset>("WhenUtc");

                    b.HasKey("Id");

                    b.ToTable("AuditEntries");
                });

            modelBuilder.Entity("Iti.Core.UserTracker.UserTrack", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("LastAccessUtc");

                    b.Property<string>("Service")
                        .HasMaxLength(128);

                    b.Property<string>("UserId")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("UserTracks");
                });

            modelBuilder.Entity("Iti.Email.EmailRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<DateTimeOffset>("DateCreatedUtc");

                    b.Property<DateTimeOffset?>("NextRetry");

                    b.Property<int>("RetryCount");

                    b.Property<DateTimeOffset?>("SentUtc");

                    b.Property<int>("Status");

                    b.Property<string>("Subject")
                        .HasMaxLength(512);

                    b.Property<string>("ToAddress");

                    b.HasKey("Id");

                    b.ToTable("EmailRecords");
                });

            modelBuilder.Entity("Iti.Logging.LogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Exception");

                    b.Property<string>("Hostname")
                        .HasMaxLength(128);

                    b.Property<string>("Level")
                        .HasMaxLength(16);

                    b.Property<string>("Message");

                    b.Property<string>("Process")
                        .HasMaxLength(128);

                    b.Property<string>("Thread")
                        .HasMaxLength(128);

                    b.Property<string>("UserId")
                        .HasMaxLength(128);

                    b.Property<string>("UserName")
                        .HasMaxLength(128);

                    b.Property<DateTimeOffset>("WhenUtc");

                    b.HasKey("Id");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("Iti.Sms.SmsRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body");

                    b.Property<DateTimeOffset>("DateCreatedUtc");

                    b.Property<DateTimeOffset?>("NextRetry");

                    b.Property<int>("RetryCount");

                    b.Property<DateTimeOffset?>("SentUtc");

                    b.Property<int>("Status");

                    b.Property<string>("ToAddress");

                    b.HasKey("Id");

                    b.ToTable("SmsRecords");
                });

            modelBuilder.Entity("DataContext.DbBar", b =>
                {
                    b.HasOne("DataContext.DbFoo", "Foo")
                        .WithMany("Bars")
                        .HasForeignKey("FooId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.DbFoo", b =>
                {
                    b.OwnsOne("Iti.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("DbFooId");

                            b1.Property<string>("City")
                                .HasMaxLength(64);

                            b1.Property<string>("Line1")
                                .HasMaxLength(64);

                            b1.Property<string>("Line2")
                                .HasMaxLength(64);

                            b1.Property<string>("State")
                                .HasMaxLength(16);

                            b1.Property<string>("Zip")
                                .HasMaxLength(16);

                            b1.ToTable("Foos");

                            b1.HasOne("DataContext.DbFoo")
                                .WithOne("Address")
                                .HasForeignKey("Iti.ValueObjects.Address", "DbFooId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Iti.ValueObjects.PersonName", "PersonName", b1 =>
                        {
                            b1.Property<Guid>("DbFooId");

                            b1.Property<string>("First")
                                .HasMaxLength(64);

                            b1.Property<string>("Last")
                                .HasMaxLength(64);

                            b1.Property<string>("Middle")
                                .HasMaxLength(64);

                            b1.Property<string>("Prefix")
                                .HasMaxLength(64);

                            b1.ToTable("Foos");

                            b1.HasOne("DataContext.DbFoo")
                                .WithOne("PersonName")
                                .HasForeignKey("Iti.ValueObjects.PersonName", "DbFooId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Iti.ValueObjects.PhoneNumber", "PhoneNumber", b1 =>
                        {
                            b1.Property<Guid>("DbFooId");

                            b1.Property<string>("Value")
                                .HasMaxLength(16);

                            b1.ToTable("Foos");

                            b1.HasOne("DataContext.DbFoo")
                                .WithOne("PhoneNumber")
                                .HasForeignKey("Iti.ValueObjects.PhoneNumber", "DbFooId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
