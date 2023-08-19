using System.ComponentModel.DataAnnotations;

namespace StockExchangeService.Commands.Entities;

public enum BidType
{
    Buy = 0,
    Sell = 0
}

public class StockBid
{
    [Key]
    public Ulid Id { get; set; }
    
    public Ulid ProcessingId { get; set; }
    public Ulid CompanyStockId { get; set; }
    
    public double PricePerShare { get; set; }
    public double Amount { get; set; }
    public DateTime CreationDateTime { get; set; }
    public Ulid UserId { get; set; }
    public BidType BidType { get; set; }
}