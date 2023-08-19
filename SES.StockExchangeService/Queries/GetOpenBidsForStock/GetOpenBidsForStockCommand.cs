using MediatR;
using StockExchangeService.Queries.Entities;

namespace StockExchangeService.Queries.GetOpenBidsForStock;

public record GetOpenBidsForStockCommand(
    Ulid CompanyStockId, 
    BidType BidType,
    double PricePerShare,
    double AmountToSatisfy) : IRequest<StockBid>, IRequest<List<StockBid>>;