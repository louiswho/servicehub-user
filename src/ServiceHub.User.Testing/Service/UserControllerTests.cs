using Microsoft.AspNetCore.Mvc;
using ServiceHub.User.Context.Repositories;
using ServiceHub.User.Service.Controllers;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using ServiceHub.User.Context.Utilities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ServiceHub.User.Testing.Service
{
    public class UserControllerTests
    {
        private readonly List<User.Context.Models.User> contextUsers;

        /// <summary>
        /// Set up test users
        /// </summary>
        public UserControllerTests()
        {
            contextUsers = new List<User.Context.Models.User>();

            User.Context.Models.User validAmericanUser = new User.Context.Models.User();
            validAmericanUser.Address = new User.Context.Models.Address();
            validAmericanUser.Address.AddressId = new Guid("11111111-1111-1111-1111-111111111111");
            validAmericanUser.Address.Address1 = "Apt 500, 100 Long Street";
            validAmericanUser.Address.Address2 = null;
            validAmericanUser.Address.City = "Tampa";
            validAmericanUser.Address.PostalCode = "12345";
            validAmericanUser.Address.State = "FL";
            validAmericanUser.Address.Country = "us";
            validAmericanUser.Email = "john@smith.com";
            validAmericanUser.Gender = "M";
            validAmericanUser.Location = "Tampa";
            validAmericanUser.Name = new User.Context.Models.Name();
            validAmericanUser.Name.First = "John";
            validAmericanUser.Name.Middle = null;
            validAmericanUser.Name.Last = "Smith";
            validAmericanUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            validAmericanUser.Type = "Associate";
            validAmericanUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");

            User.Context.Models.User validAmericanUser2 = new User.Context.Models.User();
            validAmericanUser2.Address = new User.Context.Models.Address();
            validAmericanUser2.Address.AddressId = new Guid("44444444-4444-4444-4444-444444444444");
            validAmericanUser2.Address.Address1 = "100 Short Street";
            validAmericanUser2.Address.Address2 = "Apt 100";
            validAmericanUser2.Address.City = "Reston";
            validAmericanUser2.Address.PostalCode = "12321";
            validAmericanUser2.Address.State = "va";
            validAmericanUser2.Address.Country = "US";
            validAmericanUser2.Email = "Sophie@email.com";
            validAmericanUser2.Gender = "F";
            validAmericanUser2.Location = "Reston";
            validAmericanUser2.Name = new User.Context.Models.Name();
            validAmericanUser2.Name.First = "Sophie";
            validAmericanUser2.Name.Middle = "Anna";
            validAmericanUser2.Name.Last = "Johnson";
            validAmericanUser2.Name.NameId = new Guid("55555555-5555-5555-5555-555555555555");
            validAmericanUser2.Type = "Associate";
            validAmericanUser2.UserId = new Guid("77777777-7777-7777-7777-777777777777");

            contextUsers.Add(validAmericanUser);
            contextUsers.Add(validAmericanUser2);
        }

        /// <summary>
        /// Test that valid Get request returns a 200 status code and the
        /// list of users.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task ValidGet_ShouldReturn200_AndList()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);

            UserController c = new UserController(mockRepo.Object, new LoggerFactory());

            ObjectResult result = await c.Get() as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            List<User.Library.Models.User> usersResult =
                (List<User.Library.Models.User>)result?.Value;
            Assert.Equal(2, usersResult.Count);
            var enumerator = usersResult.GetEnumerator();
            enumerator.MoveNext();
            Assert.Equal("John", enumerator.Current.Name.First);
            enumerator.MoveNext();
            Assert.Equal("Sophie", enumerator.Current.Name.First);
            enumerator.Dispose();
        }

        /// <summary>
        /// Test that an invalid Get request returns a 500 status code
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task InvalidGet_ShouldReturn500()
        {
            var mockRepo = new Mock<IUserRepository>();
            // Make user list invalid
            contextUsers[0].Name = null;
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);

            UserController c = new UserController(mockRepo.Object, new LoggerFactory());

            StatusCodeResult result = await c.Get() as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(500, result?.StatusCode);
        }

        /// <summary>
        /// Tests that the controller getById returns Ok status and model
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task ValidGetById_Return200_And_Model()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(((Guid y) => contextUsers.First(z => z.UserId == y)));

            UserController c = new UserController(mockRepo.Object, new LoggerFactory());

            ObjectResult result = await c.Get(new Guid("33333333-3333-3333-3333-333333333333")) as ObjectResult;

            Assert.NotNull(result);

            Assert.Equal(200, result?.StatusCode);

            ServiceHub.User.Library.Models.User user = (ServiceHub.User.Library.Models.User) result.Value;

            Assert.Equal("John", user.Name.First);
        }

        /// <summary>
        /// Tests that invalid id doesn't work.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task InValidGetById_Return404()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(((Guid y) => contextUsers.First(z => z.UserId == y)));

            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());

            NotFoundObjectResult result = await controller.Get(Guid.Empty) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        /// <summary>
        /// Tests that GetByGender returns list with a 200 status code.
        /// </summary>
        [Theory]
        [InlineData("M", "John")]
        [InlineData("m", "John")]
        [InlineData("F", "Sophie")]
        [InlineData("f", "Sophie")]
        [Trait("Type", "Controller")]
        public async Task ValidGetByGender_Returns200_List(string gender, string firstName)
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            // Act
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);

            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());

            ObjectResult result = await controller.GetByGender(gender) as ObjectResult;
            List<ServiceHub.User.Library.Models.User> users = (List<ServiceHub.User.Library.Models.User>)result?.Value;

            Assert.NotNull(result);
            Assert.Single(users);
            Assert.Equal(firstName, users[0].Name.First);
            Assert.Equal(200, result.StatusCode);
        }

        /// <summary>
        /// Tests to make sure all valid types return a list of corresponding users.
        /// </summary>
        /// <param name="type"> String of user type to be returned. </param>
        [Theory]
        [InlineData("associate")]
        [InlineData("ASSOCIATE")]
        [InlineData("aSsOcIaTe")]
        [Trait("Type", "Controller")]
        public async Task GetByType_ValidTypes_Return200AndList(string type)
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            // Act
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);

            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());

            ObjectResult result = await controller.GetByType(type) as ObjectResult;
            List<ServiceHub.User.Library.Models.User> users = (List<ServiceHub.User.Library.Models.User>) result?.Value;

            Assert.NotNull(result);
            Assert.Equal(2, users.Count);
            Assert.Equal("John", users[0].Name.First);
            Assert.Equal("Sophie", users[1].Name.First);
            Assert.Equal(200, result?.StatusCode);
        }

        /// <summary>
        /// Tests that an invalid gender request returns 500 status code.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task InValidGender_Returns400()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);

            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());

            ObjectResult result = await controller.GetByGender("Pie") as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result?.StatusCode);
        }

        /// <summary>
        /// Tests that put will change
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task ValidPut_Returns204()
        {
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User.Context.Models.User>()))
                .Callback((User.Context.Models.User u)
                    => contextUsers.First(c => c.UserId == u.UserId).Address = u.Address).Returns(Task.CompletedTask);

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid g) => contextUsers.First(u => u.UserId == g));

            User.Library.Models.User user = new User.Library.Models.User();
            user.UserId = contextUsers[0].UserId;
            user.Address = new User.Library.Models.Address();
            user.Address.AddressId = contextUsers[0].Address.AddressId;
            user.Address.Address1 = "The North Pole";
            user.Address.Address2 = contextUsers[0].Address.Address2;
            user.Address.City = contextUsers[0].Address.City;
            user.Address.State = contextUsers[0].Address.State;
            user.Address.PostalCode = contextUsers[0].Address.PostalCode;
            user.Address.Country = contextUsers[0].Address.Country;


            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());
            StatusCodeResult result = await controller.Put(user) as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal("The North Pole", user?.Address?.Address1);
            Assert.Equal(204, result.StatusCode);
        }

        /// <summary>
        /// Tests that invalid put will fail
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task InvalidPut_Returns400()
        {
            var mockRepo = new Mock<IUserRepository>();

            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User.Context.Models.User>()));

            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid g) => contextUsers.First(u => u.UserId == g));

            User.Library.Models.User user = new User.Library.Models.User();
            user.UserId = contextUsers[0].UserId;
            user.Address = new User.Library.Models.Address();
            user.Address.AddressId = contextUsers[0].Address.AddressId;
            user.Address.Address1 = null; 
            user.Address.Address2 = contextUsers[0].Address.Address2;
            user.Address.City = contextUsers[0].Address.City;
            user.Address.State = contextUsers[0].Address.State;
            user.Address.PostalCode = contextUsers[0].Address.PostalCode;
            user.Address.Country = contextUsers[0].Address.Country;


            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());
            ObjectResult result = await controller.Put(user) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Apt 500, 100 Long Street", contextUsers[0].Address.Address1);
            Assert.Equal(400, result?.StatusCode);
        }


        /// <summary>
        /// Tests to make sure that unacceptable terms result in a 400 response.
        /// </summary>
        /// <param name="type"> Invalid Type string of user. </param>
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("notvalid")]
        [Trait("Type", "Controller")]
        public async Task GetByType_InvalidTypes_Return400(string type)
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(x => x.GetAsync()).ReturnsAsync(contextUsers);
            UserController controller = new UserController(mockRepo.Object, new LoggerFactory());

            // Act
            var result = await controller.GetByType(type) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result?.StatusCode);
        }

        /// <summary>
        /// Tests to make sure a valid user Post results in a 202 response.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task Post_ValidlUser_Returns202()
        {
            var mockRepo = new Mock<IUserRepository>();
            var libraryUser = new User.Library.Models.User();
            libraryUser.Address = new User.Library.Models.Address();
            libraryUser.Address.AddressId = new Guid("11111111-1111-1111-1111-111111111111");
            libraryUser.Address.Address1 = "Apt 500, 100 Long Street";
            libraryUser.Address.Address2 = null;
            libraryUser.Address.City = "Tampa";
            libraryUser.Address.PostalCode = "12345";
            libraryUser.Address.State = "FL";
            libraryUser.Address.Country = "us";
            libraryUser.Email = "john@smith.com";
            libraryUser.Gender = "M";
            libraryUser.Location = "Tampa";
            libraryUser.Name = new User.Library.Models.Name();
            libraryUser.Name.First = "John";
            libraryUser.Name.Middle = null;
            libraryUser.Name.Last = "Smith";
            libraryUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            libraryUser.Type = "Associate";
            libraryUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");
            UserController c = new UserController(mockRepo.Object, new LoggerFactory());
            var contextUser = UserModelMapper.LibraryToContext(libraryUser);
            contextUser.UserId = libraryUser.UserId;

            var result = await c.Post(libraryUser) as CreatedAtRouteResult;

            Assert.NotNull(result);
            Assert.Equal(201, result?.StatusCode);
        }

        /// <summary>
        /// Tests toe make sure a null user Post results in a 400 response.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task Post_NullUser_Return400()
        {
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(m => m.InsertAsync(null));
            UserController c = new UserController(mockRepo.Object, new LoggerFactory());
            var result = await c.Post(null) as BadRequestObjectResult;


            mockRepo.Verify(m => m.InsertAsync(null), Times.Never);
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        /// <summary>
        /// Tests to make sure an invalid user Post results in a 400 response.
        /// </summary>
        [Fact]
        [Trait("Type", "Controller")]
        public async Task Post_InvalidUser_Return400()
        {
            var mockRepo = new Mock<IUserRepository>();

            var libraryUser = new User.Library.Models.User();
            libraryUser.Address = null;
            libraryUser.Email = "john@smith.com";
            libraryUser.Gender = "M";
            libraryUser.Location = null;
            libraryUser.Name = new User.Library.Models.Name();
            libraryUser.Name.First = "John";
            libraryUser.Name.Middle = null;
            libraryUser.Name.Last = "Smith";
            libraryUser.Name.NameId = new Guid("22222222-2222-2222-2222-222222222222");
            libraryUser.Type = "Associate";
            libraryUser.UserId = new Guid("33333333-3333-3333-3333-333333333333");

            UserController c = new UserController(mockRepo.Object, new LoggerFactory());
            var result = await c.Post(libraryUser) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result?.StatusCode);
        }
    }
}
