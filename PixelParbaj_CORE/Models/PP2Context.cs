using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PixelParbaj_CORE.Models
{
    public partial class PP2Context : DbContext
    {
        //CREATE TABLE [PP2].[dbo].[Joins]([ID] [bigint] IDENTITY(1,1) NOT NULL, [UserID] [int], [RoomHash] varchar(52))
        //CREATE TABLE [PP2].[dbo].[Scenes]([ID] [bigint] IDENTITY(1,1) NOT NULL, [RoomHash] varchar(52), [Started] [int], [Finished] [int], [Scene1] [int], [Scene1Started] datetime, [Scene2] [int], [Scene2Started] datetime, [Scene3] [int], [Scene3Started] datetime, [Scene4] [int], [Scene4Started] datetime, [Scene5] [int], [Scene5Started] datetime, [Scene6] [int], [Scene6Started] datetime, [Scene7] [int], [Scene7Started] datetime, [Scene8] [int], [Scene8Started] datetime)
        public PP2Context(DbContextOptions<PP2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Join> Joins { get; set; } = null!;
        public virtual DbSet<Scene> Scenes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Room)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                //entity.HasNoKey();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Imdb)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TitleE)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Title_E");

                entity.Property(e => e.TitleH)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Title_H");

                entity.Property(e => e.Votes)
                    .HasMaxLength(255)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.OwnerUserId).HasColumnName("OwnerUserID");

                entity.Property(e => e.RoomName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ShareId)
                    .HasMaxLength(52)
                    .IsUnicode(false)
                    .HasColumnName("ShareID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.Cheater).HasColumnName("Cheater");
            });

            modelBuilder.Entity<Join>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoomHash)
                    .HasMaxLength(52)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Scene>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoomHash)
                    .HasMaxLength(52)
                    .IsUnicode(false);

                entity.Property(e => e.Started).HasColumnName("Started");
                entity.Property(e => e.Finished).HasColumnName("Finished");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
