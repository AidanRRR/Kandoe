using System.Threading.Tasks;
using API.Services;
using AutoMapper;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.User
{
    public class AddUser
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
            private readonly IAuthService _authService;


            public Handler(IUserRepository userRepository, IMapper mapper, IAuthService authService)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _authService = authService;
            }

            public async Task<Result> Handle(Request message)
            {
                var newUser = _mapper.Map<Models.Models.Users.User>(message);
                try
                {
                    await _userRepository.AddUser(newUser);
                    var result = new Result {Data = newUser};
                    return result;

                }
                catch
                {
                    var result = new Result {HasErrors = true};
                    return result;
                }
            }
        }
    }
}