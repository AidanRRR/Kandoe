using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User.GET
{
    public class GetUsersByIds
    {
        public class Request : IAsyncRequest<Result>
        {
            public IEnumerable<string> UserNames { get; set; }
        }

        public class Result : ApiResult<IEnumerable<Models.Models.Users.User>>
        {
        }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                var users = await _userRepository.GetAllUsersByIds(message.UserNames);

                var result = new Result {Data = users};

                return result;
            }
        }
    }
}