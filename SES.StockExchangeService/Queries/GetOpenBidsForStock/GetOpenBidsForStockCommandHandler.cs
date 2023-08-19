using MediatR;
using Microsoft.EntityFrameworkCore;
using StockExchangeService.Persistence;
using StockExchangeService.Queries.Entities;

namespace StockExchangeService.Queries.GetOpenBidsForStock;

public class GetOpenBidsForStockCommandHandler : IRequestHandler<GetOpenBidsForStockCommand, List<StockBid>>
{
    public ILogger<GetOpenBidsForStockCommandHandler> _logger;
    public QueryDbContext _context;

    public GetOpenBidsForStockCommandHandler(
        ILogger<GetOpenBidsForStockCommandHandler> logger, 
        QueryDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<StockBid>> Handle(GetOpenBidsForStockCommand request, CancellationToken cancellationToken)
    {
        var availableStockBids = _context
            .StockBids
            .AsNoTracking()
            .Where(x => x.CompanyStockId == request.CompanyStockId)
            .Where(x => x.BidType == request.BidType)
            .Where(x => 
                x.PricePerShare >= request.PricePerShare && request.BidType == BidType.Sell ||
                x.PricePerShare <= request.PricePerShare && request.BidType == BidType.Buy
            )
            .OrderBy(x=>x.CreationDateTime)
            .Take(50);

        return await availableStockBids.ToListAsync();
    }
}