using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionSerice.Data;
using Contracts;
using MassTransit;

namespace AuctionService
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _dbContext;

        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("--> Consuming bid placed.");

            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if(auction.CurentHighBid == null 
            || context.Message.BidStatus.Contains("Accepted")
            && context.Message.Amount > auction.CurentHighBid)
            {
                auction.CurentHighBid = context.Message.Amount;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}