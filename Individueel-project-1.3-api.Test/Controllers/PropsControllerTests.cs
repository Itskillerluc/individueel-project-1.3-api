using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class PropsControllerTests
{
    private static readonly Prop EmptyProp = new (Guid.Empty, "", 0, 0, 0, 0, 0, 0, Guid.Empty);

    [Fact]
    public async Task GetPropsAsync_NoParam_BadRequest()
    {
        // Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.GetAllAsync()).ReturnsAsync([EmptyProp]);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();

        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);

        // Act

        var actualResult = await controller.GetPropsAsync(null, null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(actualResult.Result);
    }

    public static IEnumerable<object?[]> GetPropsAsyncParamOkData => 
        new List<object?[]>
        {
            new object?[] { Guid.Empty, null },
            new object?[] { null, Guid.Empty },
            new object?[] { Guid.Empty, Guid.Empty },
        };
    
    [Theory, MemberData(nameof(GetPropsAsyncParamOkData))]
    public async Task GetPropsAsync_Param_Ok(Guid? propId, Guid? roomId)
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.GetByIdAsync(propId ?? Guid.Empty)).ReturnsAsync(EmptyProp);
        mockRepoProps.Setup(repo => repo.GetAllAsync()).ReturnsAsync([EmptyProp]);
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();    
        mockRepoRooms.Setup(repo => repo.GetByIdAsync(roomId ?? Guid.Empty)).ReturnsAsync(new Room(Guid.Empty, "", 0, 0, "") { Props =
            [EmptyProp]
        });
        
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        
        //Act
        
        var actualResult = await controller.GetPropsAsync(propId, roomId);
        
        //Assert
        Assert.IsType<OkObjectResult>(actualResult.Result);
    }

    [Fact]
    public async Task GetPropsAsync_Param_NotFound()
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(null as Prop);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        
        //Act
        
        var actualResult = await controller.GetPropsAsync(Guid.Empty, null);
        
        //Assert
        Assert.IsType<NotFoundResult>(actualResult.Result);
    }

    [Fact]
    public async Task AddPropAsync_Create_Created()
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.AddAsync(EmptyProp)).ReturnsAsync(true);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        
        //Act

        var actualResult = await controller.AddPropAsync(EmptyProp);

        Assert.IsType<CreatedAtRouteResult>(actualResult);
    }
    
    [Fact]
    public async Task AddPropAsync_Conflict()
    {
        //Arrange
        
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyProp);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        
        //Act

        var actualResult = await controller.AddPropAsync(EmptyProp);

        //Assert
        Assert.IsType<ConflictResult>(actualResult);
    }

    [Fact]
    public async Task UpdatePropAsync_ValidProp_NoContent()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.UpdateAsync(Guid.Empty, EmptyProp)).ReturnsAsync(true);
        mockRepoProps.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyProp);

        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        // Act
        var actualResult = await controller.UpdatePropAsync(Guid.Empty, EmptyProp);
        //Assert
        Assert.IsType<NoContentResult>(actualResult);
    }

    [Fact]
    public async Task UpdatePropAsync_InvalidProp_NotFound()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.UpdateAsync(Guid.Empty,EmptyProp)).ReturnsAsync(false);
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        //Act
        var actualResult = await controller.UpdatePropAsync(Guid.Empty, EmptyProp);
        //Assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
    
    [Fact]
    public async Task DeletePropAsync_ValidPropId_NoContent()
    {
        //Arrange
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.DeleteAsync(Guid.Empty)).ReturnsAsync(true);
        mockRepoProps.Setup(repo => repo.GetByIdAsync(Guid.Empty)).ReturnsAsync(EmptyProp);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        //act
        var actualResult = await controller.DeletePropAsync(Guid.Empty);
        //assert
        Assert.IsType<NoContentResult>(actualResult);
    }

    [Fact]
    public async Task DeletePropAsync_InvalidPropId_NotFound()
    {
        //assert
        using var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var mockRepoProps = new Mock<ICrudRepository<Guid, Prop>>();
        mockRepoProps.Setup(repo => repo.DeleteAsync(Guid.Empty)).ReturnsAsync(false);
        
        var mockRepoRooms = new Mock<ICrudRepository<Guid, Room>>();
        var controller = new PropsController(new Logger<PropsController>(factory), mockRepoProps.Object, mockRepoRooms.Object);
        //act
        var actualResult = await controller.DeletePropAsync(Guid.Empty);
        //assert
        Assert.IsType<NotFoundResult>(actualResult);
    }
}
