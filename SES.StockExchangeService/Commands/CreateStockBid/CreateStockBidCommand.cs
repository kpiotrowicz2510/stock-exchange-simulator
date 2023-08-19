using MediatR;
using StockExchangeService.Commands.Entities;

namespace StockExchangeService.Commands.CreateStockBid;

public record CreateStockBidCommand(Ulid Id,
    Ulid ProcessingId,
    Ulid CompanyStockId, 
    double PricePerShare, 
    double Amount, 
    DateTime CreationDateTime,
    Ulid UserId, 
    BidType BidType) : IRequest<StockBid>;