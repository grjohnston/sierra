﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sierra.Model;

namespace Sierra.Model.Migrations
{
    [DbContext(typeof(SierraDbContext))]
    [Migration("20181106162256_RefactorForkToSourceCode")]
    partial class RefactorForkToSourceCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview2-35157")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sierra.Model.ResourceGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EnvironmentName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ResourceId");

                    b.Property<int>("State");

                    b.Property<string>("TenantCode")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.HasIndex("TenantCode");

                    b.ToTable("ResourceGroups");
                });

            modelBuilder.Entity("Sierra.Model.SourceCodeRepository", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Fork");

                    b.Property<int>("ProjectType");

                    b.Property<Guid>("RepoVstsId");

                    b.Property<string>("SourceRepositoryName")
                        .IsRequired();

                    b.Property<int>("State");

                    b.Property<string>("TenantCode")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("Id");

                    b.HasIndex("TenantCode");

                    b.ToTable("SourceCodeRepositories");
                });

            modelBuilder.Entity("Sierra.Model.Tenant", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(6);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Code");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Sierra.Model.VstsBuildDefinition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("SourceCodeId");

                    b.Property<int>("State");

                    b.Property<string>("TenantCode")
                        .IsRequired();

                    b.Property<int>("VstsBuildDefinitionId");

                    b.HasKey("Id");

                    b.HasIndex("SourceCodeId");

                    b.HasIndex("TenantCode");

                    b.ToTable("BuildDefinitions");
                });

            modelBuilder.Entity("Sierra.Model.VstsReleaseDefinition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BuildDefinitionId");

                    b.Property<int>("State");

                    b.Property<string>("TenantCode")
                        .IsRequired();

                    b.Property<int>("VstsReleaseDefinitionId");

                    b.HasKey("Id");

                    b.HasIndex("BuildDefinitionId")
                        .IsUnique();

                    b.HasIndex("TenantCode");

                    b.ToTable("ReleaseDefinitions");
                });

            modelBuilder.Entity("Sierra.Model.ResourceGroup", b =>
                {
                    b.HasOne("Sierra.Model.Tenant")
                        .WithMany("ResourceGroups")
                        .HasForeignKey("TenantCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sierra.Model.SourceCodeRepository", b =>
                {
                    b.HasOne("Sierra.Model.Tenant")
                        .WithMany("SourceRepos")
                        .HasForeignKey("TenantCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sierra.Model.VstsBuildDefinition", b =>
                {
                    b.HasOne("Sierra.Model.SourceCodeRepository", "SourceCode")
                        .WithMany()
                        .HasForeignKey("SourceCodeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sierra.Model.Tenant")
                        .WithMany("BuildDefinitions")
                        .HasForeignKey("TenantCode")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sierra.Model.VstsReleaseDefinition", b =>
                {
                    b.HasOne("Sierra.Model.VstsBuildDefinition", "BuildDefinition")
                        .WithOne("ReleaseDefinition")
                        .HasForeignKey("Sierra.Model.VstsReleaseDefinition", "BuildDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sierra.Model.Tenant")
                        .WithMany("ReleaseDefinitions")
                        .HasForeignKey("TenantCode")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}