using System;
using System.Threading.Tasks;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;

namespace API.Features.Theme
{
    public class DisableTheme
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
                var result = new Result();

                var theme = await _themeRepository.GetTheme(message.ThemeId);

                if (theme == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"Theme with ID {message.ThemeId} was not found");
                    return result;
                }

                try
                {
                    theme.IsEnabled = false;
                    result.Data = theme;

                    await _themeRepository.DisableTheme(message.ThemeId);
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to disable Theme: {message.ThemeId}");
                }

                return result;
            }
        }
    }
}
