using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Configurations;
using DAL.Contexts;
using Microsoft.Extensions.Options;
using Models.Models.Cards;
using MongoDB.Driver;

namespace DAL.Repositories.Cards
{
    public class CardRepository : ICardRepository
    {
        private readonly CardsContext _context;

        public CardRepository(IOptions<MongoSettings> settings)
        {
            _context = new CardsContext(settings);
        }

        public async Task<IEnumerable<Card>> GetAllCards()
        {
            var builder = Builders<Card>.Filter;
            var filter = builder.Eq(e => e.IsEnabled, true);

            return await _context.Cards.Find(filter).ToListAsync();
        }

        public async Task<Card> GetCard(string id)
        {
            var builder = Builders<Card>.Filter;
            var filter = builder.Eq(e => e.CardId, id) & builder.Eq(e => e.IsEnabled, true);

            var card = await _context.Cards
                .Find(filter)
                .FirstOrDefaultAsync();

            return card;
        }

        public async Task AddCard(Card card)
        {
            await _context.Cards.InsertOneAsync(card);
        }

        public async Task<UpdateResult> DisableCard(string id)
        {
            var filter = Builders<Card>.Filter.Eq(u => u.CardId, id);
            var update = Builders<Card>.Update
                                .Set(u => u.IsEnabled, false)
                                .CurrentDate(u => u.UpdatedOn);

            return await _context.Cards.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateCard(Card card)
        {
            var filter = Builders<Card>.Filter.Eq(u => u.CardId, card.CardId);
            var update = Builders<Card>.Update
                                .Set(u => u.ThemeId, card.ThemeId)
                                .Set(u => u.ImageUrl, card.ImageUrl)
                                .Set(u => u.Text, card.Text)
                                .Set(u => u.IsEnabled, card.IsEnabled)
                                .Set(u => u.Reactions, card.Reactions)
                                .CurrentDate(u => u.UpdatedOn);

            return await _context.Cards.UpdateOneAsync(filter, update);

        }

        public async Task AddCards(IEnumerable<Card> cards)
        {
            await _context.Cards.InsertManyAsync(cards);
        }

        public async Task<Card> AddReaction(Reaction reaction)
        {
            var card = await GetCard(reaction.CardId);
            card.Reactions.Add(reaction);
            await UpdateCard(card);
            return card;
        }
    }
}
