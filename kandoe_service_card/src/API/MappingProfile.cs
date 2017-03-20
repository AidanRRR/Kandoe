using System.Collections.Generic;
using API.Features.Card;
using API.Features.Theme;
using API.Features.Theme.ADD;
using AutoMapper;
using Models.Models.Cards;
using Models.Models.Themes;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCard.Request, Card>();
            CreateMap<UpdateCard.Request, Card>()
                .ForSourceMember(x => x.CardId, y => y.Ignore());
            CreateMap<List<AddCard.Request>, List<Card>>();

            CreateMap<AddTheme.Request, Theme>();
            CreateMap<UpdateTheme.Request, Theme>()
                .ForSourceMember(x => x.ThemeId, y => y.Ignore());
        }
    }
}
