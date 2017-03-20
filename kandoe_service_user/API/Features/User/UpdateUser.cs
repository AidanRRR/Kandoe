using System;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User
{
    public class UpdateUser
    {
        public class Request : IAsyncRequest<Result>
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
			public bool Notifications { get; set; }
        }
        public class Result : ApiResult<Models.Models.Users.User> { }
        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public Handler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<UpdateUser.Result> Handle(UpdateUser.Request message)
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

                    var updatedUser = _mapper.Map<Models.Models.Users.User>(message);
                    result.Data = updatedUser;

                    await _userRepository.UpdateUser(user.UserName, updatedUser);
                }
                catch (Exception)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add($"An error has occured while attempting to update User: {message.UserName}");
                }

                return result;
            }
        }
    }
}
