using MediatR;
using StockExchangeService.Commands.Entities;

namespace StockExchangeService.Commands.RemoveStockBid;

public record RemoveStockBidCommand(Ulid Id) : IRequest;