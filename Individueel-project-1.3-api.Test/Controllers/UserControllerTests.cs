using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class UserControllerTests
{
    private static readonly User EmptyUser = new("", "");
    
    [Fact]
    public async Task GetUsersAsync_NoParams_Ok()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User> { EmptyUser });

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.GetUsersAsync(null);

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task GetUsersAsync_Params_Ok()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(EmptyUser);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.GetUsersAsync("");

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task GetUsersAsync_Params_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(null as User);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.GetUsersAsync("");

        // Assert
        Assert.IsType<NotFoundResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task AddUser_Conflict()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(EmptyUser);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.AddUser(EmptyUser);

        // Assert
        Assert.IsType<ConflictResult>(actualResult);
    }
    
    [Fact]
    public async Task AddUser_Created()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(null as User);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.AddUser(EmptyUser);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUser_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(null as User);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.UpdateUser("", EmptyUser);

        // Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUser_BadRequest()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("Test")).ReturnsAsync(EmptyUser);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.UpdateUser("Test", EmptyUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUser_NoContent()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(EmptyUser);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.UpdateUser("", EmptyUser);

        // Assert
        Assert.IsType<NoContentResult>(actualResult);
    }
    
    [Fact]
    public async Task DeleteUser_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(null as User);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.DeleteUser("");

        // Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task DeleteUser_NoContent()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUsers = new Mock<ICrudRepository<string, User>>();
        mockRepoUsers.Setup(repo => repo.GetByIdAsync("")).ReturnsAsync(EmptyUser);

        var controller = new UsersController(new Logger<UsersController>(factory), mockRepoUsers.Object);

        // Act
        var actualResult = await controller.DeleteUser("");

        // Assert
        Assert.IsType<NoContentResult>(actualResult);
    }
}