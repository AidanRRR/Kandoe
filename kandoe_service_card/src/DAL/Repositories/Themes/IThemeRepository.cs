using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Models.Cards;
using Models.Models.Themes;
using MongoDB.Driver;

namespace DAL.Repositories.Themes
{
    public interface IThemeRepository
    {
        Task<IEnumerable<Theme>> GetThemes();
        Task<IEnumerable<Theme>> GetThemesByUser(string userName);
        Task<IEnumerable<Theme>> GetThemesUserParticipates(string userName);
        Task<Theme> GetTheme(string id);
        Task AddTheme(Theme theme);
        Task<bool> GetThemeModerator(string userName, string themeId);
        Task<UpdateResult> CloneTheme(string fromThemeId, string toThemeId);
        Task<UpdateResult> DisableTheme(string id);
        Task<UpdateResult> UpdateTheme(Theme theme);
        Task<UpdateResult> UpdateCardsOfTheme(Theme theme);
        Task<IEnumerable<Theme>> GetPublicThemes();

        Task<Theme> ReplaceCardOfTheme(string themeId, string cardId, Card card);
    }
}