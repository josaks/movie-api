﻿// <auto-generated />
using System;
using DomainModels.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DomainModels.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20181004083140_a")]
    partial class a
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainModels.EF.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("DomainModels.EF.ActorMovie", b =>
                {
                    b.Property<int>("ActorId");

                    b.Property<int>("MovieId");

                    b.HasKey("ActorId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("ActorMovies");
                });

            modelBuilder.Entity("DomainModels.EF.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("MovieId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DomainModels.EF.Favorite", b =>
                {
                    b.Property<string>("Username");

                    b.Property<int>("MovieId");

                    b.HasKey("Username", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("DomainModels.EF.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GenreValue");

                    b.Property<int?>("MovieId");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("DomainModels.EF.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AverageRating");

                    b.Property<string>("ContentRating");

                    b.Property<string>("Duration");

                    b.Property<double>("ImdbRating");

                    b.Property<string>("OriginalTitle");

                    b.Property<string>("Poster");

                    b.Property<string>("PosterURL");

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<string>("Storyline");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("DomainModels.EF.Rating", b =>
                {
                    b.Property<int>("MovieId");

                    b.Property<string>("Username");

                    b.Property<DateTime>("Date");

                    b.Property<int>("RatingValue");

                    b.HasKey("MovieId", "Username");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("DomainModels.EF.ActorMovie", b =>
                {
                    b.HasOne("DomainModels.EF.Actor", "Actor")
                        .WithMany("ActorMovies")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DomainModels.EF.Movie", "Movie")
                        .WithMany("Actors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DomainModels.EF.Comment", b =>
                {
                    b.HasOne("DomainModels.EF.Movie", "Movie")
                        .WithMany("Comments")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("DomainModels.EF.Favorite", b =>
                {
                    b.HasOne("DomainModels.EF.Movie", "Movie")
                        .WithMany("Favorites")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DomainModels.EF.Genre", b =>
                {
                    b.HasOne("DomainModels.EF.Movie")
                        .WithMany("Genres")
                        .HasForeignKey("MovieId");
                });

            modelBuilder.Entity("DomainModels.EF.Rating", b =>
                {
                    b.HasOne("DomainModels.EF.Movie", "Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
