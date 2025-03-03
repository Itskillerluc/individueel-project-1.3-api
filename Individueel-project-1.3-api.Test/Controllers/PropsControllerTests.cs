using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class PropsControllerTests
{
    private static readonly PropRequestDto EmptyPropRequestDto = new()
    {
        PropId = Guid.Empty,
        PrefabId = "",
        PosX = 0,
        PosY = 0,
        Rotation = 0,
        ScaleX = 0,
        ScaleY = 0,
        SortingLayer = 0,
        RoomId = Guid.Empty
    };

    private static readonly RoomRequestDto EmptyRoomRequestDto = new()
    {
        RoomId = Guid.Empty,
        Name = "",
        Width = 0,
        Height = 0,
        TileId = "",
    };

    [Fact]
    public async Task GetPropsAsync_PropId_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetPropsAsync_PropId_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPropsAsync_PropId_Ok()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockPropRepository.Setup(x => x.GetPropByIdAsync(Guid.Empty)).ReturnsAsync(EmptyPropRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetPropsAsync_NoPropId_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetPropsAsync_NoPropId_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, null);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPropsAsync_NoPropId_Ok()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty)).ReturnsAsync(EmptyRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "RoomPolicy"))
            .ReturnsAsync(AuthorizationResult.Success);
        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, null);

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddPropAsync_Add_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.AddPropAsync(new PropCreateDto
        {
            PrefabId = "",
            PosX = 0,
            PosY = 0,
            Rotation = 0,
            ScaleX = 0,
            ScaleY = 0,
            SortingLayer = 0,
            RoomId = Guid.Empty
        });

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task AddPropAsync_Add_CreatedAtRoute()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.AddPropAsync(new PropCreateDto
        {
            PrefabId = "",
            PosX = 0,
            PosY = 0,
            Rotation = 0,
            ScaleX = 0,
            ScaleY = 0,
            SortingLayer = 0,
            RoomId = Guid.Empty
        });

        //Assert
        Assert.IsType<CreatedAtRouteResult>(result);
    }

    [Fact]
    public async Task UpdatePropAsync_Update_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdatePropAsync(Guid.Empty, new PropUpdateDto
        {
            PrefabId = "",
            PosX = 0,
            PosY = 0,
            Rotation = 0,
            ScaleX = 0,
            ScaleY = 0,
            SortingLayer = 0,
        });

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdatePropAsync_Update_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdatePropAsync(Guid.Empty, new PropUpdateDto
        {
            PrefabId = "",
            PosX = 0,
            PosY = 0,
            Rotation = 0,
            ScaleX = 0,
            ScaleY = 0,
            SortingLayer = 0,
        });

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdatePropAsync_Update_NoContent()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockPropRepository.Setup(x => x.GetPropByIdAsync(Guid.Empty)).ReturnsAsync(EmptyPropRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyPropRequestDto, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdatePropAsync(Guid.Empty, new PropUpdateDto
        {
            PrefabId = "",
            PosX = 0,
            PosY = 0,
            Rotation = 0,
            ScaleX = 0,
            ScaleY = 0,
            SortingLayer = 0,
        });

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePropAsync_Delete_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeletePropAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePropAsync_Delete_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeletePropAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePropAsync_Delete_NoContent()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockPropRepository.Setup(x => x.GetPropByIdAsync(Guid.Empty)).ReturnsAsync(EmptyPropRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyPropRequestDto, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeletePropAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }
}