namespace SES.Shared.Contract;

public record StockOwnership(
    Ulid Id,
    Ulid ProcessingId,
    Ulid UserId, 
    Ulid CompanyStockId, 
    double PricePerShare, 
    double Amount);