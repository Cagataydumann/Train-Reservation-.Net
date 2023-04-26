using Microsoft.EntityFrameworkCore;
//using TrainReservation.Models;
using Tren_Rezervasyon.Controllers;

namespace TrainReservation.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
        //public DbSet<TrainCar> TrainCars { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Train>()
                .HasKey(t => t.Id);
        }
    }
}
