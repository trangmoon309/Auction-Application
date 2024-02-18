using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionSerice.Data;
using Contracts;
using MassTransit;

namespace AuctionService
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _dbContext;

        public AuctionFinishedConsumer(AuctionDbContext context)
        {
            _dbContext = context;
        }

        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("--> Consuming auction finished.");
            
            var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);

            if(context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            auction.Status = auction.SoldAmount > auction.ReservePrice 
                ? Entities.Status.Finished : Entities.Status.ReserveNotMet;

            await _dbContext.SaveChangesAsync();
        }
    }
}