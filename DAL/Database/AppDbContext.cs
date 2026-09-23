using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DAL.Database;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<RelatedPerson> RelatedPersons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasIndex(c => c.CityName)
            .IsUnique();
        modelBuilder.Entity<PhoneNumber>()
            .HasIndex(pn => pn.Number)
            .IsUnique();


        modelBuilder.Entity<RelatedPerson>()
            .HasKey(rp => rp.RelatedPersonId);

        modelBuilder.Entity<RelatedPerson>()
            .HasOne(rp => rp.RelatedPersonPerson)
            .WithMany(p => p.RelatedPerson)
            .HasForeignKey(rp => rp.RelatedPersonPersonId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<RelatedPerson>()
            .HasOne(rp => rp.PersonRelated)
            .WithMany()
            .HasForeignKey(rp => rp.PersonRelatedId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<PhoneNumber>()
            .HasKey(pn => pn.PhoneId);

        modelBuilder.Entity<PhoneNumber>()
            .HasOne(pn => pn.Person)
            .WithMany(p => p.PhoneNumber)
            .HasForeignKey(rp => rp.PhonePersonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
