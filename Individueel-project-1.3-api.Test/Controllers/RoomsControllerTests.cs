using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class RoomsControllerTests
{
    private static readonly Room EmptyRoom = new(Guid.Empty, "", 0, 0, "");

    [Fact]
    public async Task GetRoomsAsync_NoParams_Ok()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Room> { EmptyRoom });

        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);

        // Act
        var actualResult = await controller.GetRoomsAsync(null, null);

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }

    public static IEnumerable<object?[]> GetRoomsAsyncParamsOkData =>
        new List<object?[]>
        {
            new object?[] { Guid.Empty, null },
            new object?[] { null, "" },
            new object?[] { Guid.Empty, "" },
        };
    
    [Theory, MemberData(nameof(GetRoomsAsyncParamsOkData))]
    public async Task GetRoomsAsync_Params_Ok(Guid? roomId, string? owner)
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Room> { new(Guid.Empty, "", 0, 0, "") { Users =
            [new Room.UserEntry(new User("", ""), true)]
        } });

        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);

        // Act
        var actualResult = await controller.GetRoomsAsync(roomId, owner);

        // Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }

    [Fact]
    public async Task GetRoomsAsync_Params_NotFound()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Room> { EmptyRoom });
        
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        
        // Act
        var actualResult = await controller.GetRoomsAsync(Guid.NewGuid(), null);
        
        // Assert
        Assert.IsType<NotFoundResult>(actualResult.Result);
    }
    
    [Fact]
    public async Task AddPropAsync_Create_Created()
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.AddAsync(EmptyRoom)).ReturnsAsync(true);
        
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        
        //Act

        var actualResult = await controller.AddRoomAsync(EmptyRoom);

        Assert.IsType<CreatedAtRouteResult>(actualResult);
    }
    
    [Fact]
    public async Task AddPropAsync_Conflict()
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyRoom);
        
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        
        //Act

        var actualResult = await controller.AddRoomAsync(EmptyRoom);

        //Assert
        Assert.IsType<ConflictResult>(actualResult);
    }

    [Fact]
    public async Task UpdatePropAsync_ValidProp_NoContent()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.UpdateAsync(Guid.Empty, EmptyRoom)).ReturnsAsync(true);
        mockRepoRooms.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyRoom);

        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        // Act
        var actualResult = await controller.UpdateRoomAsync(Guid.Empty, EmptyRoom);
        //Assert
        Assert.IsType<NoContentResult>(actualResult);
    }

    [Fact]
    public async Task UpdatePropAsync_InvalidProp_NotFound()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.UpdateAsync(Guid.Empty,EmptyRoom)).ReturnsAsync(false);
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        //Act
        var actualResult = await controller.UpdateRoomAsync(Guid.Empty, EmptyRoom);
        //Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task DeletePropAsync_ValidPropId_NoContent()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.DeleteAsync(Guid.Empty)).ReturnsAsync(true);
        mockRepoRooms.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyRoom);
        
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        //act
        var actualResult = await controller.DeleteRoomAsync(Guid.Empty);
        //assert
        Assert.IsType<NoContentResult>(actualResult);
    }

    [Fact]
    public async Task DeletePropAsync_InvalidPropId_NotFound()
    {
        //assert
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        mockRepoRooms.Setup(repo => repo.DeleteAsync(Guid.Empty)).ReturnsAsync(false);
        
        var controller = new RoomsController(new Logger<RoomsController>(factory), mockRepoRooms.Object);
        //act
        var actualResult = await controller.DeleteRoomAsync(Guid.Empty);
        //assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
}