using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data.Contexts;

public partial class BookDbContext : DbContext
{
    public BookDbContext()
    {
    }

    public BookDbContext(DbContextOptions<BookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookLoan> BookLoans { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:kk-bookmanagement.database.windows.net,1433;Initial Catalog=BookDB;Persist Security Info=False;User ID=kalev;Password=Test123*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC07C0247F4D");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC0701BF5D53");

            entity.HasIndex(e => e.Isbn, "IX_Books_ISBN");

            entity.HasIndex(e => e.Title, "IX_Books_Title");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EADDCF1374").IsUnique();

            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.Publisher).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthors_Authors"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthors_Books"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PK__BookAuth__6AED6DC49FD811E9");
                        j.ToTable("BookAuthors");
                    });

            entity.HasMany(d => d.Genres).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookGenres_Genres"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookGenres_Books"),
                    j =>
                    {
                        j.HasKey("BookId", "GenreId").HasName("PK__BookGenr__CDD8925028B69719");
                        j.ToTable("BookGenres");
                    });
        });

        modelBuilder.Entity<BookLoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookLoan__3214EC070EFA98D5");

            entity.HasIndex(e => e.DueDate, "IX_BookLoans_DueDate");

            entity.HasIndex(e => e.Status, "IX_BookLoans_Status");

            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Book).WithMany(p => p.BookLoans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookLoans_Books");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Fines__3214EC071B59A59F");

            entity.HasIndex(e => e.Status, "IX_Fines_Status");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IssuedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaidDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.BookLoan).WithMany(p => p.Fines)
                .HasForeignKey(d => d.BookLoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fines_BookLoans");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC07F99B72BA");

            entity.HasIndex(e => e.Name, "UQ__Genres__737584F69BDF7FAF").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
