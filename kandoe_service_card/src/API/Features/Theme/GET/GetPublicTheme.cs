using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme.GET
{
    public class GetPublicTheme
    {
        public class Request : IAsyncRequest<Result>{}
        public class Result : ApiResult<IEnumerable<Models.Models.Themes.Theme>>{}
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IThemeRepository _themeRepository;

            public Handler(IThemeRepository ThemeRepository)
            {
                _themeRepository = ThemeRepository;
            }
            
            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var theme = await _themeRepository.GetPublicThemes();
                    var result = new Result { Data = theme};
                    return result;
                }
                catch (Exception e)
                {
                    return new Result{
                        HasErrors = true,
                        ErrorMessages = new List<string>{e.StackTrace} 
                    };
                }
            }
        }
    }
}