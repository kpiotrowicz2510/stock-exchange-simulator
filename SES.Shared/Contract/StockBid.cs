namespace SES.Shared.Contract;

public enum BidType
{
    Buy = 0,
    Sell = 0
}

public record StockBid(
    Ulid Id,
    Ulid ProcessingId,
    Ulid CompanyStockId, 
    double PricePerShare, 
    double Amount, 
    DateTime CreationDateTime,
    Ulid UserId, 
    BidType BidType);