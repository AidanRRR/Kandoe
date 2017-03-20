using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;
using Models.Models.Cards;

namespace API.Features.Theme.ADD
{
    public class AddTheme
    {
        public class Request : IAsyncRequest<Result>
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public ICollection<ICard> Cards { get; set; }
            public bool IsPublic { get; set; }
            public ICollection<string> Tags { get; set; }
            public string Username { get; set; }
            public ICollection<string> Organizers {get; set;}
        }

        public class Result : ApiResult<Models.Models.Themes.Theme> { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IThemeRepository _themeRepository;
            private readonly IMapper _mapper;

            public Handler(IThemeRepository themeRepository, IMapper mapper)
            {
                _themeRepository = themeRepository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var newTheme = _mapper.Map<Models.Models.Themes.Theme>(message);
                    
                    foreach (var card in newTheme.Cards)
                    {
                        card.ThemeId = newTheme.ThemeId; 
                    }

                    await _themeRepository.AddTheme(newTheme);

                    var result = new Result { Data = newTheme };
                    return result;

                }
                catch
                {
                    var result = new Result { HasErrors = true };
                    return result;
                }
            }
        }
    }
}
