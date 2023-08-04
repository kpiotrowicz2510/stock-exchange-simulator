namespace SES.Shared.Contract;

public record User(
    Ulid Id, 
    string Name, 
    string Password, 
    string Email, 
    double AvailableBalance);