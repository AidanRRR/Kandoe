using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme.GET
{
    public class GetThemesUserParticipates
    {
        public class Request : IAsyncRequest<Result>
        {
            public string UserName { get; set; }
        }
        public class Result : ApiResult<IEnumerable<Models.Models.Themes.Theme>> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IThemeRepository _themeRepository;

            public Handler(IThemeRepository themeRepository)
            {
                _themeRepository = themeRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                var themes = await _themeRepository.GetThemesUserParticipates(message.UserName);
                var result = new Result { Data = themes };

                return result;
            }
        }
    }
}
