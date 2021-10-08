﻿// <auto-generated />
using System;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inspections.Infrastructure.Data.Migrations
{
    [DbContext(typeof(InspectionsContext))]
    [Migration("20211008050027_add_new_opre_fields_2")]
    partial class add_new_opre_fields_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Inspections.Core.Domain.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AddressLine")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("LicenseId")
                        .HasColumnType("integer");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LicenseId");

                    b.ToTable("Addresses", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Annotation")
                        .HasColumnType("text");

                    b.Property<bool>("IsConfiguration")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int?>("ReportConfigurationId")
                        .HasColumnType("integer");

                    b.Property<int?>("ReportId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportConfigurationId");

                    b.HasIndex("ReportId");

                    b.ToTable("CheckLists", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CheckListId")
                        .HasColumnType("integer");

                    b.Property<int>("Checked")
                        .HasColumnType("integer");

                    b.Property<bool>("Editable")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Touched")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.ToTable("CheckListItems", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CurrentTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("circuit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("currentData")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("current_data");

                    b.Property<string>("endDate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("end_date");

                    b.Property<string>("startDate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("start_date");

                    b.HasKey("id");

                    b.ToTable("current");
                });

            modelBuilder.Entity("Inspections.Core.Domain.EMALicense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Amp")
                        .HasColumnType("numeric");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("KVA")
                        .HasColumnType("numeric");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PersonInCharge")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Volt")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Footer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Inactive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("MarginBottom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MarginLeft")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MarginRight")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MarginTop")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RemarksLabelText")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ReportsConfiguration", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Checked")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("NeedsCheck")
                        .HasColumnType("boolean");

                    b.Property<int>("ReportId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Notes", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.OperationalReadings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<short>("EarthFaultA")
                        .HasColumnType("smallint");

                    b.Property<bool>("EarthFaultEFEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("EarthFaultEIREnabled")
                        .HasColumnType("boolean");

                    b.Property<short>("EarthFaultELRA")
                        .HasColumnType("smallint");

                    b.Property<short>("EarthFaultELRSec")
                        .HasColumnType("smallint");

                    b.Property<short>("EarthFaultMA")
                        .HasColumnType("smallint");

                    b.Property<bool>("EarthFaultRoobEnabled")
                        .HasColumnType("boolean");

                    b.Property<short>("EarthFaultSec")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<short>("MainBreakerAmp")
                        .HasColumnType("smallint");

                    b.Property<short>("MainBreakerCapacity")
                        .HasColumnType("smallint");

                    b.Property<short>("MainBreakerPoles")
                        .HasColumnType("smallint");

                    b.Property<short>("MainBreakerRating")
                        .HasColumnType("smallint");

                    b.Property<bool>("OverCurrentByMainBreaker")
                        .HasColumnType("boolean");

                    b.Property<short>("OverCurrentDTLA")
                        .HasColumnType("smallint");

                    b.Property<bool>("OverCurrentDTLEnabled")
                        .HasColumnType("boolean");

                    b.Property<short>("OverCurrentDTLSec")
                        .HasColumnType("smallint");

                    b.Property<bool>("OverCurrentDirectActing")
                        .HasColumnType("boolean");

                    b.Property<bool>("OverCurrentDirectActingEnabled")
                        .HasColumnType("boolean");

                    b.Property<short>("OverCurrentIDMTLA")
                        .HasColumnType("smallint");

                    b.Property<short>("OverCurrentIDMTLTm")
                        .HasColumnType("smallint");

                    b.Property<bool>("OverCurrentIDTMLEnabled")
                        .HasColumnType("boolean");

                    b.Property<short>("RunningLoadL1")
                        .HasColumnType("smallint");

                    b.Property<short>("RunningLoadL2")
                        .HasColumnType("smallint");

                    b.Property<short>("RunningLoadL3")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL1L2")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL1L3")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL1N")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL2L3")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL2N")
                        .HasColumnType("smallint");

                    b.Property<short>("VoltageL3N")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("OperationalReadings");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.PhotoRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileNameResized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PhotoStorageId")
                        .HasColumnType("text");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<int>("ReportId")
                        .HasColumnType("integer");

                    b.Property<string>("ThumbnailStorageId")
                        .HasColumnType("text");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Photos", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("EMALicenseId")
                        .HasColumnType("integer");

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OperationalReadingsId")
                        .HasColumnType("integer");

                    b.Property<string>("RemarksLabelText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EMALicenseId");

                    b.HasIndex("OperationalReadingsId");

                    b.ToTable("Reports", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.SignaturesAggregate.Signature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Annotation")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Designation")
                        .HasColumnType("text");

                    b.Property<string>("DrawnSign")
                        .HasColumnType("text");

                    b.Property<bool>("IsConfiguration")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<short>("Order")
                        .HasColumnType("smallint");

                    b.Property<bool>("Principal")
                        .HasColumnType("boolean");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<int?>("ReportConfigurationId")
                        .HasColumnType("integer");

                    b.Property<int?>("ReportId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportConfigurationId");

                    b.HasIndex("ReportId");

                    b.ToTable("Signatures", "Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int?>("LastEditedReport")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inspections.Core.QueryModels.ResumenCheckList", b =>
                {
                    b.Property<string>("Annotation")
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalItems")
                        .HasColumnType("integer");

                    b.ToTable("ResumenCheckList", t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("Inspections.Core.QueryModels.ResumenReportConfiguration", b =>
                {
                    b.Property<int>("DefinedCheckLists")
                        .HasColumnType("integer");

                    b.Property<int>("DefinedSignatures")
                        .HasColumnType("integer");

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("Inactive")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RemarksLabelText")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("UsedByReports")
                        .HasColumnType("integer");

                    b.ToTable("ResumenReportConfiguration", t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("Inspections.Core.Domain.Address", b =>
                {
                    b.HasOne("Inspections.Core.Domain.EMALicense", "License")
                        .WithMany()
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("License");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckList", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", "ReportConfiguration")
                        .WithMany("ChecksDefinition")
                        .HasForeignKey("ReportConfigurationId");

                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", "Report")
                        .WithMany("CheckList")
                        .HasForeignKey("ReportId");

                    b.Navigation("Report");

                    b.Navigation("ReportConfiguration");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListItem", b =>
                {
                    b.HasOne("Inspections.Core.Domain.CheckListAggregate.CheckList", null)
                        .WithMany("Checks")
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.EMALicense", b =>
                {
                    b.OwnsOne("Inspections.Shared.DateTimeRange", "Validity", b1 =>
                        {
                            b1.Property<int>("EMALicenseId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<DateTime>("End")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("timestamp without time zone");

                            b1.HasKey("EMALicenseId");

                            b1.ToTable("Licenses");

                            b1.WithOwner()
                                .HasForeignKey("EMALicenseId");
                        });

                    b.Navigation("Validity")
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Note", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", null)
                        .WithMany("Notes")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.PhotoRecord", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", null)
                        .WithMany("PhotoRecords")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Report", b =>
                {
                    b.HasOne("Inspections.Core.Domain.EMALicense", "License")
                        .WithMany()
                        .HasForeignKey("EMALicenseId");

                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.OperationalReadings", "OperationalReadings")
                        .WithMany()
                        .HasForeignKey("OperationalReadingsId");

                    b.Navigation("License");

                    b.Navigation("OperationalReadings");
                });

            modelBuilder.Entity("Inspections.Core.Domain.SignaturesAggregate.Signature", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", null)
                        .WithMany("SignatureDefinitions")
                        .HasForeignKey("ReportConfigurationId");

                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", null)
                        .WithMany("Signatures")
                        .HasForeignKey("ReportId");

                    b.OwnsOne("Inspections.Core.Domain.SignaturesAggregate.Responsible", "Responsible", b1 =>
                        {
                            b1.Property<int>("SignatureId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Type")
                                .HasColumnType("integer");

                            b1.HasKey("SignatureId");

                            b1.ToTable("Signatures");

                            b1.WithOwner()
                                .HasForeignKey("SignatureId");
                        });

                    b.Navigation("Responsible")
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckList", b =>
                {
                    b.Navigation("Checks");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", b =>
                {
                    b.Navigation("ChecksDefinition");

                    b.Navigation("SignatureDefinitions");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Report", b =>
                {
                    b.Navigation("CheckList");

                    b.Navigation("Notes");

                    b.Navigation("PhotoRecords");

                    b.Navigation("Signatures");
                });
#pragma warning restore 612, 618
        }
    }
}
