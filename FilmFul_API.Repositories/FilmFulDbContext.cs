using FilmFul_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilmFul_API.Repositories
{
    public partial class FilmFulDbContext : DbContext
    {
        public FilmFulDbContext()
        {
        }

        public FilmFulDbContext(DbContextOptions<FilmFulDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Entities.Action> Action { get; set; }
        public virtual DbSet<Actor> Actor { get; set; }
        public virtual DbSet<Direction> Direction { get; set; }
        public virtual DbSet<Director> Director { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Entities.Action>(entity =>
            {
                entity.HasKey(e => new { e.ActorId, e.MovieId })
                    .HasName("action_pkey");

                entity.ToTable("action");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Action)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("action_actor_id_fkey");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Action)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("action_movie_id_fkey");
            });

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Direction>(entity =>
            {
                entity.HasKey(e => new { e.DirectorId, e.MovieId })
                    .HasName("direction_pkey");

                entity.ToTable("direction");

                entity.Property(e => e.DirectorId).HasColumnName("director_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.DirectorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("direction_director_id_fkey");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("direction_movie_id_fkey");
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("director");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.Genre1 })
                    .HasName("genre_pkey");

                entity.ToTable("genre");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.Genre1)
                    .HasColumnName("genre")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Genre)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("genre_movie_id_fkey");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Certificate)
                    .IsRequired()
                    .HasColumnName("certificate")
                    .HasColumnType("character varying");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("character varying");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Gross).HasColumnName("gross");

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasColumnName("poster");

                entity.Property(e => e.RatingImdb).HasColumnName("rating_imdb");

                entity.Property(e => e.RatingMetascore).HasColumnName("rating_metascore");

                entity.Property(e => e.ReleaseYear).HasColumnName("release_year");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("character varying");

                entity.Property(e => e.VoteCount).HasColumnName("vote_count");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
