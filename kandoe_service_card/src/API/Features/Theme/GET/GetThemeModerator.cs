using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme.GET
{
    public class GetThemeModerator
    {
        public class Request : IAsyncRequest<Result>
        {
            public string UserName { get; set; }
            public string ThemeId { get; set; }
        }
        public class Result : ApiResult<bool> { }
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
                    var isModerator = await _themeRepository.GetThemeModerator(message.UserName, message.ThemeId);
                    var result = new Result { Data = isModerator };

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
