using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configurations;
using DAL.Contexts;
using Microsoft.Extensions.Options;
using Models.Models.Cards;
using Models.Models.Themes;
using MongoDB.Driver;

namespace DAL.Repositories.Themes
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly ThemesContext _context;

        public ThemeRepository(IOptions<MongoSettings> settings)
        {
            _context = new ThemesContext(settings);
        }

        public async Task<IEnumerable<Theme>> GetThemes()
        {
            var builder = Builders<Theme>.Filter;
            var filter = builder.Eq(e => e.IsEnabled, true);

            return await _context.Themes.Find(filter).ToListAsync();
        }

        public async Task<Theme> GetTheme(string id)
        {
            var builder = Builders<Theme>.Filter;
            var filter = builder.Eq(e => e.ThemeId, id) & builder.Eq(e => e.IsEnabled, true);

            var theme = await _context.Themes
                .Find(filter)
                .FirstOrDefaultAsync();

            return theme;
        }

        public async Task<IEnumerable<Theme>> GetThemesByUser(string userName)
        {
            var themes = await GetThemes();

            return themes.Where(theme => theme.Organizers.Contains(userName) || theme.Username.Equals(userName)).ToList();
        }

        public async Task<IEnumerable<Theme>> GetThemesUserParticipates(string userName)
        {
            /*
            var builder = Builders<Theme>.Filter;
            var filter = builder.Eq(e => e.IsEnabled, true)
                         & builder.ElemMatch(e => e.Organizers, userName);
            */

            var themes = await GetThemes();
            var themesByUser = new List<Theme>();
            foreach (var theme in themes)
            {
                if (theme.Organizers.Contains(userName))
                {
                    themesByUser.Add(theme);
                }
            }

            return themesByUser;
        }

        public async Task<bool> GetThemeModerator(string userName, string themeId)
        {
            var builder = Builders<Theme>.Filter;
            var filter = builder.Eq(e => e.IsEnabled, true)
                & builder.Eq(e => e.ThemeId, themeId);

            var theme = await _context.Themes.Find(filter).FirstOrDefaultAsync();

            return theme.Username.Equals(userName);
        }

        public async Task AddTheme(Theme theme)
        {
            await _context.Themes.InsertOneAsync(theme);
        }

        public async Task<UpdateResult> CloneTheme(string fromThemeId, string toThemeId)
        {
            var fromTheme = await GetTheme(fromThemeId);
            var toTheme = await GetTheme(toThemeId);

            foreach (var card in fromTheme.Cards)
            {
                if (!toTheme.Cards.Contains(card))
                {
                    toTheme.Cards.Add(card);
                }
            }

            return await UpdateTheme(toTheme);
        }

        public async Task<UpdateResult> DisableTheme(string id)
        {
            var filter = Builders<Theme>.Filter.Eq(u => u.ThemeId, id);
            var update = Builders<Theme>.Update
                                .Set(u => u.IsEnabled, false)
                                .CurrentDate(u => u.UpdatedOn);

            return await _context.Themes.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateTheme(Theme theme)
        {
            var filter = Builders<Theme>.Filter.Eq(u => u.ThemeId, theme.ThemeId);
            var update = Builders<Theme>.Update
                                .Set(u => u.Description, theme.Description)
                                .Set(u => u.Name, theme.Name)
                                .Set(u => u.Organizers, theme.Organizers)
                                .Set(u => u.Tags, theme.Tags)
                                .Set(u => u.IsPublic, theme.IsPublic)
                                .CurrentDate(u => u.UpdatedOn);

            return await _context.Themes.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> UpdateCardsOfTheme(Theme theme){
            var filter = Builders<Theme>.Filter.Eq(u => u.ThemeId, theme.ThemeId);
            var update = Builders<Theme>.Update
                                .Set(u => u.Cards, theme.Cards)
                                .CurrentDate(u => u.UpdatedOn);

            return await _context.Themes.UpdateOneAsync(filter, update);
        }

        public async Task<IEnumerable<Theme>> GetPublicThemes(){
            var publicThemes = new List<Theme>();
            var themes = await GetThemes();

            foreach(var t in themes){
                if(t.IsPublic == true) publicThemes.Add(t);
            }

            return publicThemes;
        }

        public async Task<Theme> ReplaceCardOfTheme(string themeId, string cardId, Card card){
            var theme =  await GetTheme(themeId);
            theme.Cards.Remove(theme.Cards.Where(c => c.CardId == cardId).FirstOrDefault());
            theme.Cards.Add(card);
            await UpdateCardsOfTheme(theme);
            return theme;
        }
    }
}