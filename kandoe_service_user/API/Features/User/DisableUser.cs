using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User
{
    public class DisableUser
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
                var result = new Result();

                var user = await _userRepository.GetUser(message.UserName);

                if (user == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"User with userName {message.UserName} was not found");
                    return result;
                }

                try
                {
                    user.IsEnabled = false;
                    result.Data = user;

                    await _userRepository.DisableUser(user.UserName);
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to disable User: {message.UserName}");
                }

                return result;
            }
        }
    }
}