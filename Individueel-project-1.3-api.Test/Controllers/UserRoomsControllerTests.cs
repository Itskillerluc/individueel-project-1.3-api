using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class UserRoomsControllerTests
{
    private static readonly UserRoom EmptyUserRoom = new("", Guid.Empty, false);

    [Fact]
    public async Task GetUserRoomsAsync_NoParams_Ok()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        mockRepoUserRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<UserRoom> { EmptyUserRoom });

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.GetUserRoomsAsync(null, null);

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task GetUserRoomsAsync_Params_BadRequest()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        mockRepoUserRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<UserRoom> { EmptyUserRoom });

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.GetUserRoomsAsync(null, Guid.Empty);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actualResult.Result);
    }

    [Fact]
    public async Task GetUserRoomsAsync_Params_Ok()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);
        

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.GetUserRoomsAsync("", Guid.Empty);

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task GetUserRoomsAsync_Params_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        mockRepoUserRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<UserRoom> { EmptyUserRoom });

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.GetUserRoomsAsync("NO!", Guid.Empty);

        // Assert
        Assert.IsType<NotFoundResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task AddUserRoomAsync_Create_Created()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.NewGuid());
        mockRepoUserRooms.Setup(repo => repo.AddAsync(EmptyUserRoom)).ReturnsAsync(true);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.AddUserRoom(EmptyUserRoom);

        // Assert
        Assert.IsType<CreatedAtRouteResult>(actualResult);
    }
    
    [Fact]
    public async Task AddUserRoomAsync_Conflict()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.AddUserRoom(EmptyUserRoom);

        // Assert
        Assert.IsType<ConflictResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUserRoomAsync_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(null as UserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.UpdateUserRoom("", Guid.Empty, EmptyUserRoom);

        // Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUserRoomAsync_BadRequest()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("success", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.UpdateUserRoom("success", Guid.Empty, EmptyUserRoom);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actualResult);
    }
    
    [Fact]
    public async Task UpdateUserRoomAsync_NoContent()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.UpdateUserRoom("", Guid.Empty, EmptyUserRoom);

        // Assert
        Assert.IsType<NoContentResult>(actualResult);
    }
    
    [Fact]
    public async Task DeleteUserRoomAsync_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(null as UserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.DeleteUserRoom("", Guid.Empty);

        // Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task DeleteUserRoomAsync_NoContent()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoUserRooms = new Mock<ICrudRepository<(string, Guid), UserRoom>>();
        var key = ("", Guid.Empty);
        mockRepoUserRooms.Setup(repo => repo.GetByIdAsync(key)).ReturnsAsync(EmptyUserRoom);

        var controller = new UserRoomsController(new Logger<UserRoomsController>(factory), mockRepoUserRooms.Object);

        // Act
        var actualResult = await controller.DeleteUserRoom("", Guid.Empty);

        // Assert
        Assert.IsType<NoContentResult>(actualResult);
    }
}