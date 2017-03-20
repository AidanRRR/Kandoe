using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User.GET
{
    public class GetUser
    {
        public class Request : IAsyncRequest<Result>
        {
            public string UserName { get; set; }
        }
        public class Result : ApiResult<Models.Models.Users.User> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                try
                {
                    var user = await _userRepository.GetUser(message.UserName);
                    var result = new Result { Data = user };

                    return result;
                }
                catch
                {
                    return new Result
                    {
                        HasErrors = true,
                        ErrorMessages = new List<string> { $"Error while retrieving User : {message.UserName}" }
                    };
                }
            }
        }
    }
}
