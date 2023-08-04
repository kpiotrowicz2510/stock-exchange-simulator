namespace SES.Shared.Contract;

public record TopUpTransactionStatus(
    Ulid Id,
    string Status,
    string ProviderName, 
    bool IsFinal);

public record TopUpTransaction(
    Ulid Id, 
    Ulid UserId,
    string ProviderName, 
    double Amount, 
    DateTime TransactionDateTime, 
    TopUpTransactionStatus Status);