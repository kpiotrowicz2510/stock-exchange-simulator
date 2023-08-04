namespace SES.Shared.Contract;

public record StockTransaction(
    Ulid Id,
    Ulid ProcessingId,
    Ulid CompanyStockId, 
    double PricePerShare, 
    double Amount, 
    DateTime TransactionDateTime,
    Ulid SellingUserId, 
    Ulid BuyingUserId);