using System.IO;
using System.Threading.Tasks;
using Castle.Core.Logging;
using kandoe_service_image.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

using Microsoft.AspNetCore.Http.Internal;
using NSubstitute;
using FluentAssertions;
using kandoe_service_image.Controllers;
using kandoe_service_image.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel;

namespace kandoe_service_image_test
{
    public class TestPostImage
    {
        [Fact]
        public async System.Threading.Tasks.Task TestPostImageAzureAsync()
        {
            #region Arrange
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=kandoeblob;AccountKey=wh1GcLth48ham33w9dAFIx27BaBnnfOCT8EbZCHg0NDDqzMW/FrhQRSLw4q16tqLrVavRv7F4JDwxe1Mgy9/wA==;";
            const string containerReference = "kandoe-images";

            var imageSender = new ImageSender(connectionString, containerReference);
            var mockFile = Substitute.For<IFormFile>();

            mockFile.OpenReadStream().Returns(new FileStream("booster.jpg", FileMode.Open));
            mockFile.FileName.Returns("Booster");
            #endregion

            #region Act
            var result = await imageSender.PostImage(mockFile);
            #endregion


            #region Assert
            result.name.Should().NotBe(null);
            result.size.Should().NotBe(null);
            result.uri.Should().StartWith("https://kandoeblob");
            #endregion
        }

        [Fact]
        public async Task TestPostImageControllerAsync()
        {
            #region Arrange
            const string connectionString = "DefaultEndpointsProtocol=https;AccountName=kandoeblob;AccountKey=wh1GcLth48ham33w9dAFIx27BaBnnfOCT8EbZCHg0NDDqzMW/FrhQRSLw4q16tqLrVavRv7F4JDwxe1Mgy9/wA==;";
            const string containerReference = "kandoe-images";

            var mockFile = Substitute.For<IFormFile>();

            mockFile.OpenReadStream().Returns(new FileStream("booster.jpg", FileMode.Open));
            mockFile.FileName.Returns("Booster");
            #endregion

            #region act
            var imageSender = new ImageSender(connectionString, containerReference);

            var controller = new ImageController(imageSender);
            var viewResult = (OkObjectResult)await controller.PostImage(mockFile);
            #endregion

            #region Assert
            viewResult.StatusCode.HasValue.Should().BeTrue();
            #endregion
        }
    }
}
