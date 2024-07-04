using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STRaceLifePG.Models;


namespace StreetRaceLifeVK.Data
{
    public class AppContextDb : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppContextDb(DbContextOptions<AppContextDb> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<ClubLike> ClubLikes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Clubs)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Races)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Каскадное удаление для Club -> Races
            modelBuilder.Entity<Club>()
                .HasMany(c => c.Races)
                .WithOne(r => r.Club)
                .HasForeignKey(r => r.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            // Удаляем каскадное удаление для всех внешних ключей
            modelBuilder.Entity<Club>()
                .HasOne(c => c.User)
                .WithMany(u => u.Clubs)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Race>()
                .HasOne(r => r.User)
                .WithMany(u => u.Races)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Race>()
                .HasOne(r => r.Club)
                .WithMany(c => c.Races)
                .HasForeignKey(r => r.ClubId)
                .OnDelete(DeleteBehavior.NoAction);
            // Дополнительная конфигурация для ClubLike
            modelBuilder.Entity<ClubLike>()
                .HasKey(cl => cl.Id);

            modelBuilder.Entity<ClubLike>()
                .HasIndex(cl => new { cl.ClubId, cl.UserId }).IsUnique();
        }
    }
}
