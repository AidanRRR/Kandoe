using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.User.GET;
using DAL.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class TestGetUser
    {
        [Fact]
        public async Task GetUserTest()
        {
            #region arrange
            var usersMockList = new List<Models.Models.Users.User>
            {
                new Models.Models.Users.User()
                {
                    CreatedOn = DateTime.UtcNow,
                    Email = "aidan.rypens@outlook.com",
                    FirstName = "Aidan",
                    LastName = "Rypens,",
                    UserName = "AidanR"
                }
            };

            var mockRepo = Substitute.For<IUserRepository>();
            mockRepo.GetUser(usersMockList[0].UserName).Returns(Task.FromResult(usersMockList[0]));

            var request = new GetUser.Request()
            {
                UserName = usersMockList[0].UserName
            };
            #endregion

            #region act
            var handler = new GetUser.Handler(mockRepo);
            var user = await handler.Handle(request);
            #endregion

            #region assert
            user.Should().NotBeNull();
            user.HasErrors.Should().BeFalse();
            user.Data.UserName.Should().Be(usersMockList[0].UserName);
            #endregion
        }
    }
}
