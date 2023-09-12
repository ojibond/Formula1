using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaOne.DataService.Data
{
    public class AppDbContext : DbContext
    {
        //Define the db entities
        public virtual DbSet<Driver> Drivers {get; set;}
        public virtual DbSet<Achievment> Achievments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //specified the relationship between the entities, in this case one-to-many
            modelBuilder.Entity<Achievment>(entity =>
            {
                entity.HasOne(d => d.Driver)
                .WithMany(e => e.Achievments)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Achievments_Driver");
            });
        }
    }
}
