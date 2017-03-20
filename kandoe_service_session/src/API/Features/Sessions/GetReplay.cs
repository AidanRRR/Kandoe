using System;
using System.Threading.Tasks;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Session;
using BL;

namespace API.Features.Sessions
{
    public class GetReplay
    {
        public class Request : IAsyncRequest<Result>
        {
            public string Token { get; set; }
            public Guid SessionId { get; set; }
            public Guid? ReplayKey { get; set; } = null;
        }

        public class Result : ApiResult<IReplay>
        { }

        public class Handler : IAsyncRequestHandler<Request, Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            private readonly IServiceManager serviceManager;

            public Handler(ISessionEventsRepository sessionEventsRepository, IServiceManager serviceManager)
            {
                _sessionEventsRepository = sessionEventsRepository;
                this.serviceManager = serviceManager;
            }

            public async Task<Result> Handle(Request message)
            {
                var result = new Result();
                var session = await _sessionEventsRepository.GetSession(message.SessionId);

                if (session == null)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add("Sessie bestaat niet");
                } 
                else
                {
                    if (session.Phase != SessionPhase.Finished)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Deze sessie is nog niet afgelopen.");
                    }
                    else
                    {
                        var isMgr = await serviceManager.IsTokenManager(message.SessionId, message.Token); 
                        Console.WriteLine("Replay key: " + session.ReplayKey);
                        Console.WriteLine("IsMgr = " + isMgr);
                        if (isMgr || (message.ReplayKey.Equals(session.ReplayKey)))
                        {
                            var replay = new Replay
                            {
                                Session = new Session(session), // TODO: model veranderen naar DTO?
                                Events = session.SessionEvents,
                                Snapshots = session.SessionSnapshots
                            };
                            result.Data = replay;
                        }
                        else
                        {
                            result.HasErrors = true;
                            result.ErrorMessages.Add("Ongeldige replay key.");
                        }
                    }
                }

                return result;
            }
        }
    }
}