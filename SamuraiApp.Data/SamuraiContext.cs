using System;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        private readonly Guid _userId;         
        
        public SamuraiContext(DbContextOptions<SamuraiContext> options ,IUserIdService userIdService = null)
            :base(options)
        {
            _userId = userIdService?.GetUserId()                           //#D
                      ?? new ReplacementUserIdService().GetUserId();      //#D

        }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<PriceOffer> PriceOffers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Order> Orders { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>().HasKey(x => new { x.BookId, x.AuthorId });

            modelBuilder.Entity<Book>().HasQueryFilter(p => !p.SoftDeleted);                      
            modelBuilder.Entity<LineItem>()
                .HasOne(p => p.ChosenBook) 
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Order>()                                   //#G
                .HasQueryFilter(x => x.CustomerId == _userId);      
            
            modelBuilder.Entity<Samurai>()
             .HasMany(s => s.Battles)
             .WithMany(b => b.Samurais)
             .UsingEntity<BattleSamurai>
              (bs => bs.HasOne<Battle>().WithMany(),
               bs => bs.HasOne<Samurai>().WithMany())
             .Property(bs => bs.DateJoined)
             .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Horse>().ToTable("Horses");
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }
    }
}
