using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using API;
using API.Features.User;
using AutoMapper;
using DAL.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class TestUpdateUser
    {
        [Fact]
        public async Task UpdateUserTest()
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
            var ogUser = user;
            ogUser.LastName = "rijpens";
            var mockRepo = Substitute.For<IUserRepository>();
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            mockRepo.UpdateUser(user.UserName, user).Returns(Task.FromResult(user));

            var request = new UpdateUser.Request()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName
            };

            #endregion

            #region act

            var updateResult = await mockRepo.UpdateUser(user.UserName, user);
            #endregion

            #region assert
            updateResult.Should().NotBeNull();
            updateResult.Should().Be(user);
            #endregion
        }
    }
}
