using MediatR;
using Microsoft.EntityFrameworkCore;
using StockExchangeService.Persistence;

namespace StockExchangeService.Commands.RemoveStockBid;

public class RemoveStockBidCommandHandler : IRequestHandler<RemoveStockBidCommand>
{
    private readonly CommandDbContext _context;
    private readonly ILogger<RemoveStockBidCommandHandler> _logger;

    public RemoveStockBidCommandHandler(CommandDbContext context, ILogger<RemoveStockBidCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(RemoveStockBidCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.ToString());

        var stockBidToRemove = await _context
            .StockBids
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (stockBidToRemove == null) return;
        
        _context.StockBids.Remove(stockBidToRemove);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}