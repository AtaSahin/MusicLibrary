using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicLibraryApp.Areas.Identity.Data;
using MusicLibraryApp.Controllers;
using MusicLibraryApp.Models;

using Xunit;

public class AdminControllerTests
{
    [Fact]
    public void Index_ReturnsViewWithListOfUsers()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>();
        var loggerMock = new Mock<ILogger<AdminController>>();
        var adminController = new AdminController(userManagerMock.Object, loggerMock.Object);

        // Mock the list of users
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = "1", UserName = "User1" },
            new ApplicationUser { Id = "2", UserName = "User2" }
        };
        userManagerMock.Setup(m => m.Users).Returns(users.AsQueryable());

        // Act
        var result = adminController.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users, result.Model as List<ApplicationUser>);
    }

    [Fact]
    public async Task AssignModerator_ValidUserId_RedirectsToIndex()
    {
        // Arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>();
        var loggerMock = new Mock<ILogger<AdminController>>();
        var adminController = new AdminController(userManagerMock.Object, loggerMock.Object);

        // Mock user
        var user = new ApplicationUser { Id = "1", UserName = "User1" };
        userManagerMock.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
        userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Moderator"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await adminController.AssignModerator("1") as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        userManagerMock.Verify(m => m.AddToRoleAsync(user, "Moderator"), Times.Once);
    }
}
