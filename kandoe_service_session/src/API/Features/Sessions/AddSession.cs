using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Repositories;
using MediatR;
using Models.Models.API;
using Models.Models.Events.Dto;
using Models.Models.Imported;
using Models.Models.Session;
using BL;
using System;

namespace API.Features.Sessions
{
    public class AddSession
    {
        public class Request : IAsyncRequest<AddSession.Result>
        {
            public string Token { get; set; } = "";
            public string Name {get; set;}
            public string Description {get; set;}
            public Guid ThemeId { get; set; }
            public int CircleType { get; set; }
            public IEnumerable<Card> Cards { get; set; }
            public int TurnTime { get; set; }
            public int MinPicks {get; set;}
            public int MaxPicks { get; set; }
            public DateTime? ScheduledStartTime { get; set; }
            public DateTime? ScheduledEndTime { get; set; }
            public IEnumerable<string> PlayerIds {get; set;} = new List<string>();
        }

        public class Result : ApiResult<SessionDto> { }

        public class Handler : IAsyncRequestHandler<AddSession.Request, AddSession.Result>
        {
            private readonly ISessionEventsRepository _sessionEventsRepository;
            private readonly IMapper _mapper;
            private readonly IServiceManager _serviceManager;
            public Handler(ISessionEventsRepository sessionEventsRepository, IMapper mapper, IServiceManager serviceManager)
            {
                _sessionEventsRepository = sessionEventsRepository;
                _serviceManager = serviceManager;
                _mapper = mapper;
            }

            public async Task<AddSession.Result> Handle(AddSession.Request message)
            {
                var result = new Result();
                try
                {
                    if (message.Cards.Count() <= 1)
                    { 
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Een sessie moet minstens 2 kaarten hebben.");
                        return result;
                    }
                    
                    if (message.MaxPicks <= 0)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Ongeldige max picks.");
                        return result;
                    }

                    if (message.MinPicks > message.MaxPicks)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Max picks moet groter dan of gelijk aan min picks zijn.");
                        return result;
                    }

                    if (message.TurnTime <= 0) 
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Ongeldige turn time");
                        return result;
                    }
                    message.TurnTime *= 60;

                    var user = await _serviceManager.VerifyToken(message.Token);
                    if (user == null)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Geen geldig JWT token.");
                        return result;
                    }

                    var theme = await _serviceManager.GetTheme(message.ThemeId);
                    if (theme == null)
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("Thema bestaat niet.");
                        return result;
                    }

                    if (!(theme.Username.Equals(user) || theme.Organizers.Contains(user)))
                    {
                        result.HasErrors = true;
                        result.ErrorMessages.Add("U bent geen beheerder van dit thema.");
                        return result;
                    }

                    Console.WriteLine("CircleType = " + message.CircleType);
                    var dto = new SessionDto()
                    {
                        Name = message.Name,
                        Description = message.Description,
                        ThemeId = message.ThemeId,
                        CircleType = message.CircleType == 0 ? CircleType.Chance : CircleType.Problem,
                        TurnTime = message.TurnTime,
                        Cards = message.Cards,
                        MinPicks = message.MinPicks,
                        MaxPicks = message.MaxPicks,
                        ScheduledStartTime = message.ScheduledStartTime,
                        ScheduledEndTime = message.ScheduledEndTime,
                        ReplayKey = Guid.NewGuid(),
                        OwnerId = user,
                        PlayerIds = message.PlayerIds,
                    };
                    var response = await _sessionEventsRepository.AddSession(dto);

                    result.Data = response;
                    return result;
                }
                catch (Exception ex)
                {
                    result.HasErrors = true;
                    result.ErrorMessages.Add(ex.Message);
                    return result;
                }
            }
        }
    }
}