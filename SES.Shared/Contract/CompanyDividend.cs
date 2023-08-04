namespace SES.Shared.Contract;

public record CompanyDividend(
    Ulid Id, 
    string Ticker, 
    Ulid CompanyId,
    double AmountPerShare, 
    DateTime PayDateTime);