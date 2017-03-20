using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;
using Models.Models.Cards;
using Models.Models.Themes;

namespace API.Features.Theme
{
    public class UpdateTheme
    {
        public class Request : IAsyncRequest<Result>
        {
            public string ThemeId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ICollection<string> Organizers { get; set; }
            public bool IsPublic { get; set; }
            public ICollection<string> Tags { get; set; }
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
                    var updatedTheme = _mapper.Map<Models.Models.Themes.Theme>(message);
                    await _themeRepository.UpdateTheme(updatedTheme);
                    var newTheme = await _themeRepository.GetTheme(updatedTheme.ThemeId);
                    result.Data = newTheme;
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to update Theme: {message.ThemeId}");
                }

                return result;
            }
        }
    }
}
