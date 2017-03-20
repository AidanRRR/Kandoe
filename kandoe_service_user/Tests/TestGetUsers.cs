using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.User.GET;
using DAL.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class TestGetUsers
    {
        [Fact]
        public async Task GetUsersTest()
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
                },
                new Models.Models.Users.User()
                {
                    CreatedOn = DateTime.UtcNow,
                    Email = "fred@outlook.com",
                    FirstName = "Fred",
                    LastName = "Huybrechts,",
                    UserName = "Fred H"
                }
            };

            var mockRepo = Substitute.For<IUserRepository>();
            mockRepo.GetAllUsers().Returns(usersMockList);

            var request = new GetUsers.Request();
            #endregion

            #region act
            var handler = new GetUsers.Handler(mockRepo);
            var users = await handler.Handle(request);
            #endregion

            #region assert
            users.Should().NotBeNull();
            users.HasErrors.Should().BeFalse();
            users.Data.Count().Should().BeGreaterOrEqualTo(1);
            #endregion
        }
    }
}
