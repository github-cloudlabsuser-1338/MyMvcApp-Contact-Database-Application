using Xunit;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyMvcApp.Tests
{
    [Collection("Non-Parallel Collection")]
    public class UserControllerTests : System.IDisposable
    {
        public UserControllerTests()
        {
            // Constructor can be used for setup if needed
        }
        

        public void Dispose()
        {
            // Clear the static user list after each test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_AddsUserAndRedirects()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Name = "Test", Email = "test@email.com" };

            // Act
            var result = controller.Create(user);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Single(UserController.userlist);
        }

        [Fact]
        public void Create_Post_ReturnsView_WhenUserIsNull()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Create(null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_UpdatesUserAndRedirects()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Old", Email = "old@email.com" });
            var controller = new UserController();
            var updatedUser = new User { Name = "New", Email = "new@email.com" };

            // Act
            var result = controller.Edit(1, updatedUser);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("New", UserController.userlist[0].Name);
        }

        [Fact]
        public void Edit_Post_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Name = "Test", Email = "test@email.com" };

            // Act
            var result = controller.Edit(99, user);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();

            // Act
            var result = controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@email.com" });
            var controller = new UserController();
            var form = new Microsoft.AspNetCore.Http.FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(1, form);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Empty(UserController.userlist);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var controller = new UserController();
            var form = new Microsoft.AspNetCore.Http.FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(99, form);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
