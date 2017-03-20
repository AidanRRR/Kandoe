using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User.GET
{
    public class GetUserByEmail
    {
        public class Request : IAsyncRequest<Result>
        {
            public string Email { get; set; }
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
                    var user = await _userRepository.GetUserByEmail(message.Email);
                    var result = new Result { Data = user };

                    return result;
                }
                catch
                {
                    return new Result
                    {
                        HasErrors = true,
                        ErrorMessages = new List<string> { $"Error while retrieving User : {message.Email}" }
                    };
                }
            }
        }
    }
}
