using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.User;
using AutoMapper;
using Models.Models.Users;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddUser.Request, User>();
            CreateMap<UpdateUser.Request, User>();
        }
    }

}
