using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Models.Cards;
using MongoDB.Driver;

namespace DAL.Repositories.Cards
{
    public interface ICardRepository
    {
        Task<IEnumerable<Card>> GetAllCards();
        Task<Card> GetCard(string id);
        Task AddCard(Card card);
        Task<UpdateResult> DisableCard(string id);
        Task<UpdateResult> UpdateCard(Card card);
        Task AddCards(IEnumerable<Card> cards);
        Task<Card> AddReaction(Reaction reaction);
    }
}