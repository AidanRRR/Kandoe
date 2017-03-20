using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API;
using API.Features.User;
using AutoMapper;
using DAL.Repositories;
using FluentAssertions;
using Models.Models.Users;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class TestAddUser
    {
        [Fact]
        public async Task AddUserTest()
        {
            #region arrange

            var user = new Models.Models.Users.User()
            {
                CreatedOn = DateTime.UtcNow,
                Email = "aidan.rypens@hotmail.com",
                FirstName = "aidan",
                LastName = "rypens",
                UserName = "AidanR"
            };

            var mockRepo = Substitute.For<IUserRepository>();
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            var mockAuthService = Substitute.For<API.Services.IAuthService>();
            var verifyResult = new Models.Models.VerifyResult {Success = true};

            mockAuthService.VerifyToken(null).Returns(verifyResult);

            mockRepo.AddUser(user).Returns(user);

            var request = new AddUser.Request()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };
            #endregion

            #region act
            var handler = new AddUser.Handler(mockRepo, mapper, mockAuthService);
            var returnUser = await handler.Handle(request);
            #endregion

            #region assert
            returnUser.Should().NotBeNull();
            returnUser.HasErrors.Should().BeFalse();
            returnUser.Data.UserName.Should().Be(user.UserName);
            #endregion
        }
    }
}
