using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Sessions;
using AutoMapper;
using Models.Models.Events.Dto;
using Models.Models.Session;

namespace API
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionDto>();
            CreateMap<SessionDto, Session>();
            CreateMap<SessionDto, AddSession.Request>();
            CreateMap<AddSession.Request, SessionDto>();
        }
    }
}
