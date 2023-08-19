using MediatR;
using StockExchangeService.Commands.Entities;
using StockExchangeService.Persistence;

namespace StockExchangeService.Commands.CreateStockBid;

public class CreateStockBidCommandHandler : IRequestHandler<CreateStockBidCommand, StockBid>
{
    private readonly CommandDbContext _context;
    private readonly ILogger<CreateStockBidCommandHandler> _logger;

    public CreateStockBidCommandHandler(CommandDbContext context, ILogger<CreateStockBidCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<StockBid> Handle(CreateStockBidCommand request, CancellationToken cancellationToken)
    {
        var stockBid = new StockBid(){
            Id = request.Id,
            ProcessingId = request.ProcessingId,
            CompanyStockId = request.CompanyStockId,
            PricePerShare = request.PricePerShare,
            Amount = request.Amount,
            CreationDateTime = request.CreationDateTime,
            UserId = request.UserId,
            BidType = request.BidType
        };
        
        _logger.LogInformation(request.ToString());
        
        await _context.StockBids.AddAsync(stockBid, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return stockBid;
    }
}