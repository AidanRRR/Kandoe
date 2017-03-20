using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme.ADD
{
    public class AddOrganizerToTheme
    {
        public class Request : IAsyncRequest<Result>{
            public string ThemeId { get; set; }
            public string UserId { get; set; }
        }

        public class Result: ApiResult<Models.Models.Themes.Theme>{}

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IThemeRepository _themeRepository;

            public Handler(IThemeRepository themeRepository)
            {
                _themeRepository = themeRepository;
            }
            public async Task<Result> Handle(Request message)
            {
                var result = new Result();
                try
                {
                    var theme = _themeRepository.GetTheme(message.ThemeId).Result;
                    theme.Organizers.Add(message.UserId);
                    await _themeRepository.UpdateTheme(theme);
                    result.Data = theme;
                }
                catch
                {
                    result.HasErrors = true;
                }

                return result;
            }
        }
    }
}