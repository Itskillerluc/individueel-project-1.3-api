using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class RoomsControllerTests
{
    private static readonly RoomRequestDto EmptyRoomRequestDto = new()
    {
        RoomId = Guid.Empty,
        Name = "",
        Width = 0,
        Height = 0,
        TileId = "",
    };

    [Fact]
    public async Task GetRoomsAsync_Id_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "WeakRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetRoomsAsync(Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetRoomsAsync_Id_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "WeakRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetRoomsAsync(Guid.Empty);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetRoomsAsync_Id_Ok()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "WeakRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetRoomsAsync(Guid.Empty);

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }


    [Fact]
    public async Task GetRoomsAsync_NoId_Ok()
    {
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomsByUserAsync(null!)).ReturnsAsync([]);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetRoomsAsync(null);

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddRoomAsync_Add_CreatedAtRoute()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository >();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.AddRoomAsync(new RoomCreateDto
        {
            Name = "",
            Width = 0,
            Height = 0,
            TileId = "",
        });

        //Assert
        Assert.IsType<CreatedAtRouteResult>(result);
    }

    [Fact]
    public async Task UpdateRoomAsync_Update_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateRoomAsync(Guid.Empty, new RoomUpdateDto()
        {
            Name = "",
            Width = 0,
            Height = 0,
            TileId = "",
        });

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateRoomAsync_Update_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateRoomAsync(Guid.Empty, new RoomUpdateDto()
        {
            Name = "",
            Width = 0,
            Height = 0,
            TileId = "",
        });

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task UpdateRoomAsync_Update_NoContent()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateRoomAsync(Guid.Empty, new RoomUpdateDto()
        {
            Name = "",
            Width = 0,
            Height = 0,
            TileId = "",
        });

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRoomAsync_Delete_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeleteRoomAsync(Guid.Empty);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteRoomAsync_Delete_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeleteRoomAsync(Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteRoomAsync_Delete_NoContent()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        var mockPropRepository = new Mock<IPropRepository>();


        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new RoomsController(mockRoomRepository.Object, mockPropRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);
 
        //Act
        var result = await controller.DeleteRoomAsync(Guid.Empty);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }
}