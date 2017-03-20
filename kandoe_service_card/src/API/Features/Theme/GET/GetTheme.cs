using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme.GET
{
    public class GetTheme
    {
        public class Request : IAsyncRequest<Result>
        {
            public string ThemeId { get; set; }
        }
        public class Result : ApiResult<Models.Models.Themes.Theme> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IThemeRepository _themeRepository;

            public Handler(IThemeRepository themeRepository)
            {
                _themeRepository = themeRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var theme = await _themeRepository.GetTheme(message.ThemeId);
                    var result = new Result { Data = theme };

                    return result;
                }
                catch
                {
                    return new Result
                    {
                        HasErrors = true,
                        ErrorMessages = new List<string> { $"Error while retrieving Theme : {message.ThemeId}" }
                    };
                }
            }
        }
    }
}
