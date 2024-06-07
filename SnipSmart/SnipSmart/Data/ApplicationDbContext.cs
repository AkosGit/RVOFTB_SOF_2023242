using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnipSmart.Models;

namespace SnipSmart.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Snippet> Snippets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Collection>()
            .HasMany(e => e.Snippets)
            .WithOne(e => e.Collection)
            .HasForeignKey(e => e.CollectionID);
        //.IsRequired();
        modelBuilder.Entity<Snippet>()
            .HasMany(e => e.Tags)
            .WithOne(e => e.Snippet)
            .HasForeignKey(e => e.SnippetID);
        //.IsRequired();
        
        
        base.OnModelCreating(modelBuilder);
        //Adding default user
        modelBuilder.Entity<IdentityRole>().HasData(
            new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new { Id = "2", Name = "User", NormalizedName = "USER" }
        );

        PasswordHasher<User> ph = new PasswordHasher<User>();
        User test = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@gmail.com",
            EmailConfirmed = true,
            UserName = "test@gmail.com",
            NormalizedUserName = "TEST@GMAIL.COM"
        };
        test.PasswordHash = ph.HashPassword(test, "test");
        modelBuilder.Entity<User>().HasData(test);

    }
}