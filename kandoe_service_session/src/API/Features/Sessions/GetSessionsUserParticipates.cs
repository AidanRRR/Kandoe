using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;

namespace API.Features.Sessions
{
    public class GetSessionsUserParticipates
    {
        public class Request : IAsyncRequest<Result>
        {
            public string UserName { get; set; }
        }

        public class Result : EnumerableApiResult<string>
        { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;

            public Handler(ISessionEventsRepository sessionEventsRepository)
            {
                _sessionEventsRepository = sessionEventsRepository;
            }

            public async Task<Result> Handle(Request message)
            {
                var sessionUser = await _sessionEventsRepository.GetActiveSessionsUserParticipates(message.UserName);
                var result = new Result { Data = sessionUser };

                return result;
            }
        }
    }
}