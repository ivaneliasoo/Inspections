﻿// <auto-generated />
using System;
using Inspections.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inspections.Infrastructure.Data.Migrations
{
    [DbContext(typeof(InspectionsContext))]
    partial class InspectionsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Annotation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsConfiguration")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int?>("ReportConfigurationId")
                        .HasColumnType("int");

                    b.Property<int?>("ReportId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReportConfigurationId");

                    b.HasIndex("ReportId");

                    b.ToTable("CheckLists","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CheckListId")
                        .HasColumnType("int");

                    b.Property<int>("Checked")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Required")
                        .HasColumnType("bit");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.ToTable("CheckListItems","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.CheckListAggregate.CheckListParam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CheckListId")
                        .HasColumnType("int");

                    b.Property<int?>("CheckListItemId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.HasIndex("CheckListItemId");

                    b.ToTable("CheckListParams","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportConfigurationAggregate.ReportConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("RemarksLabelText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ReportsConfiguration","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Notes","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.PhotoRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Photos","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.ReportsAggregate.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reports","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.SignaturesAggregate.Signature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Annotation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Designation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsConfiguration")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<bool>("Principal")
                        .HasColumnType("bit");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReportConfigurationId")
                        .HasColumnType("int");

                    b.Property<int?>("ReportId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReportConfigurationId");

                    b.HasIndex("ReportId");

                    b.ToTable("Signatures","Inspections");
                });

            modelBuilder.Entity("Inspections.Core.Domain.User", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTimeOffset>("LastEdit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastEditUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

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
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("LicenseType")
                                .HasColumnType("int");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ReportId");

                            b1.ToTable("Reports");

                            b1.WithOwner()
                                .HasForeignKey("ReportId");

                            b1.OwnsOne("Inspections.Shared.DateTimeRange", "Validity", b2 =>
                                {
                                    b2.Property<int>("EMALicenseReportId")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("int")
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<DateTime>("End")
                                        .HasColumnType("datetime2");

                                    b2.Property<DateTime>("Start")
                                        .HasColumnType("datetime2");

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
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

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
