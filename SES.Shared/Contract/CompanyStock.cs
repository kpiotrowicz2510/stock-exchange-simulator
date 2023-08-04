namespace SES.Shared.Contract;

public record CompanyStock(Ulid Id, string Ticker, Ulid CompanyId, double PricePerShare);