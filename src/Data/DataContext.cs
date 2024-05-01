using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<AuthorBook> AuthorsBooks { get; set; }
        public DbSet<CollectionBook> CollectionsBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>()
                .HasKey(ab => new { ab.AuthorId, ab.BookId });
            modelBuilder.Entity<AuthorBook>()
                .HasOne(a => a.Author)
                .WithMany(ab => ab.AuthorBooks)
                .HasForeignKey(a => a.AuthorId);
            modelBuilder.Entity<AuthorBook>()
                .HasOne(b => b.Book)
                .WithMany(ab => ab.AuthorBooks)
                .HasForeignKey(b => b.BookId);


            modelBuilder.Entity<CollectionBook>()
                .HasKey(cb => new { cb.CollectionId, cb.BookId });
            modelBuilder.Entity<CollectionBook>()
                .HasOne(c => c.Collection)
                .WithMany(cb => cb.CollectionBooks)
                .HasForeignKey(c => c.CollectionId);
            modelBuilder.Entity<CollectionBook>()
                .HasOne(b => b.Book)
                .WithMany(cb => cb.CollectionBooks)
                .HasForeignKey(b => b.BookId);
        }
    }
}
