using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories.Themes;
using MediatR;
using Models.Models.API;
using MongoDB.Driver;

namespace API.Features.Theme
{
    public class CloneTheme
    {
        public class Request : IAsyncRequest<Result>
        {
            public string FromThemeId { get; set; }
            public string ToThemeId { get; set; }
        }

        public class Result : ApiResult<UpdateResult> { }

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
                    var updateResult = await _themeRepository.CloneTheme(message.FromThemeId, message.ToThemeId);

                    var result = new Result { Data = updateResult };
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
