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
    [Migration("20201009041720_photoresized")]
    partial class photoresized
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

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
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Addresses","Inspections");
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
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

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

                    b.ToTable("CheckLists","Inspections");
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
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.ToTable("CheckListItems","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListParam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CheckListId")
                        .HasColumnType("integer");

                    b.Property<int?>("CheckListItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.HasIndex("CheckListItemId");

                    b.ToTable("CheckListParams","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<string>("RemarksLabelText")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ReportsConfiguration","Inspections");
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
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<bool>("NeedsCheck")
                        .HasColumnType("boolean");

                    b.Property<int>("ReportId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Notes","Inspections");
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
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int>("ReportId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Photos","Inspections");
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

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RemarksLabelText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reports","Inspections");
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

                    b.Property<string>("DrawedSign")
                        .HasColumnType("text");

                    b.Property<bool>("IsConfiguration")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

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

                    b.ToTable("Signatures","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int?>("LastEditedReport")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckList", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", "ReportConfiguration")
                        .WithMany("ChecksDefinition")
                        .HasForeignKey("ReportConfigurationId");

                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", "Report")
                        .WithMany("CheckList")
                        .HasForeignKey("ReportId");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListItem", b =>
                {
                    b.HasOne("Inspections.Core.Domain.CheckListAggregate.CheckList", null)
                        .WithMany("Checks")
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListParam", b =>
                {
                    b.HasOne("Inspections.Core.Domain.CheckListAggregate.CheckList", "CheckList")
                        .WithMany("TextParams")
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Inspections.Core.Domain.CheckListAggregate.CheckListItem", "CheckListItem")
                        .WithMany("TextParams")
                        .HasForeignKey("CheckListItemId")
                        .OnDelete(DeleteBehavior.NoAction);
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
                    b.OwnsOne("Inspections.Core.Domain.ReportsAggregate.EMALicense", "License", b1 =>
                        {
                            b1.Property<int>("ReportId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<int>("LicenseType")
                                .HasColumnType("integer");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ReportId");

                            b1.ToTable("Reports");

                            b1.WithOwner()
                                .HasForeignKey("ReportId");

                            b1.OwnsOne("Inspections.Shared.DateTimeRange", "Validity", b2 =>
                                {
                                    b2.Property<int>("EMALicenseReportId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer")
                                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                                    b2.Property<DateTime>("End")
                                        .HasColumnType("timestamp without time zone");

                                    b2.Property<DateTime>("Start")
                                        .HasColumnType("timestamp without time zone");

                                    b2.HasKey("EMALicenseReportId");

                                    b2.ToTable("Reports");

                                    b2.WithOwner()
                                        .HasForeignKey("EMALicenseReportId");
                                });
                        });
                });

            modelBuilder.Entity("Inspections.Core.Domain.SignaturesAggregate.Signature", b =>
                {
                    b.HasOne("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", "ReportConfiguration")
                        .WithMany("SignatureDefinitions")
                        .HasForeignKey("ReportConfigurationId");

                    b.HasOne("Inspections.Core.Domain.ReportsAggregate.Report", "Report")
                        .WithMany("Signatures")
                        .HasForeignKey("ReportId");

                    b.OwnsOne("Inspections.Core.Domain.SignaturesAggregate.Responsable", "Responsable", b1 =>
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
                });
#pragma warning restore 612, 618
        }
    }
}
