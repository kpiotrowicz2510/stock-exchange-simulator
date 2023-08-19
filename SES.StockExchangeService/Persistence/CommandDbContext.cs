using Microsoft.EntityFrameworkCore;
using SES.Shared.Extensions;
using StockExchangeService.Commands.Entities;

namespace StockExchangeService.Persistence;

public class CommandDbContext : DbContext
{
    public DbSet<StockBid> StockBids { get; set; }

    public CommandDbContext(DbContextOptions<CommandDbContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<StockBid>()
            .Property(e => e.Id)
            .HasConversion(new UlidToStringConverter());
        modelBuilder
            .Entity<StockBid>()
            .Property(e => e.ProcessingId)
            .HasConversion(new UlidToStringConverter());
        modelBuilder
            .Entity<StockBid>()
            .Property(e => e.CompanyStockId)
            .HasConversion(new UlidToStringConverter());
        modelBuilder
            .Entity<StockBid>()
            .Property(e => e.UserId)
            .HasConversion(new UlidToStringConverter());
    }
}