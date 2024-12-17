using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TripSplit.Domain;

namespace TripSplit.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() : base()
        { 
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplits { get; set; }
        public DbSet<TripUser> TripUsers { get; set; }
        public DbSet<Invitation> Invitations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripUser>()
                .HasKey(tu => new { tu.TripId, tu.UserId });

            modelBuilder.Entity<TripUser>()
                .HasOne(tu => tu.Trip)
                .WithMany(t => t.Users)
                .HasForeignKey(tu => tu.TripId);

            modelBuilder.Entity<TripUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(tu => tu.UserId);

            modelBuilder.Entity<ExpenseSplit>()
                .HasKey(es => new { es.ExpenseId, es.UserId });

            modelBuilder.Entity<ExpenseSplit>()
                .HasOne(es => es.Expense)
                .WithMany(e => e.Splits)
                .HasForeignKey(es => es.ExpenseId);

            modelBuilder.Entity<ExpenseSplit>()
                .HasOne(es => es.User)
                .WithMany(u => u.ExpenseSplits)
                .HasForeignKey(es => es.UserId);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Trip)
                .WithMany(t => t.Expenses)
                .HasForeignKey(e => e.TripId);

            modelBuilder.Entity<Invitation>()
                .HasKey(tu => new { tu.TripId, tu.UserId });

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Trip)
                .WithMany(t => t.Invitations)
                .HasForeignKey(i => i.TripId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invitations)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ExpenseSplit>()
                .Property(es => es.Amount)
                .HasPrecision(18, 4);

            base.OnModelCreating(modelBuilder);
        }
    }
}