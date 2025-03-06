using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class UserRoomsControllerTests
{
    private static readonly UserRoomRequestDto EmptyUserRoomRequestDto = new()
    {
        Username = "",
        RoomId = Guid.Empty,
        IsOwner = false,
    };

    private static readonly RoomRequestDto EmptyRoomRequestDto = new RoomRequestDto
    {
        RoomId = Guid.Empty,
        Name = "",
        Width = 0,
        Height = 0,
        TileId = "",
    };

    [Fact]
    public async Task GetUserRoomsAsync_Params_Forbid()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetUserRoomsAsync(Guid.Empty, "");

        //Assert
        Assert.IsType<ForbidResult>(result.Result);
    }

    [Fact]
    public async Task GetUserRoomsAsync_Params_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetUserRoomsAsync(Guid.Empty, "");

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    public static IEnumerable<object?[]> GetUserRoomsAsyncParamsOkData =>
        new List<object?[]>
        {
            new object?[] { null, null },
            new object?[] { Guid.Empty, null },
            new object?[] { null, "" },
            new object?[] { Guid.Empty, "" },
        };
    
    [Theory, MemberData(nameof(GetUserRoomsAsyncParamsOkData))]
    public async Task GetUserRoomsAsync_Get_Ok(Guid? roomId, string? username)
    {
        //Arrange
        var roomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById(null!, Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(roomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.GetUserRoomsAsync(roomId, username);

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }


    [Fact]
    public async Task AddRoomAsync_Add_CreatedAtRoute()
    {
        //Arrange
        var roomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById(null!, Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(roomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.AddUserRoom(new UserRoomCreateDto()
        {
            Username = "",
            RoomId = Guid.Empty,
            IsOwner = false,
        });

        //Assert
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task AddRoomAsync_Add_Forbid()
    {
        //Arrange
        var roomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();

        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById(null!, Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new UserRoomsController(roomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.AddUserRoom(new UserRoomCreateDto()
        {
            Username = "",
            RoomId = Guid.Empty,
            IsOwner = false,
        });

        //Assert
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task UpdateRoomAsync_Update_NotFound()
    {
        //Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockUserRoomRepository = new Mock<IUserRoomRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById(null!, Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateUserRoom("", Guid.Empty, new UserRoomUpdateDto
        {
            IsOwner = false
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

        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById("", Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto , "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateUserRoom("", Guid.Empty, new UserRoomUpdateDto
        {
            IsOwner = false
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

        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById("", Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "StrictRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.UpdateUserRoom("", Guid.Empty, new UserRoomUpdateDto
        {
            IsOwner = false
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
        
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById(null!, Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, null, "RoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeleteUserRoom("", Guid.Empty);

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

        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty, null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById("", Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto , "WeakRoomPolicy")).ReturnsAsync(AuthorizationResult.Failed);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeleteUserRoom("", Guid.Empty);

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

        mockRoomRepository.Setup(x => x.GetRoomByIdAsync(Guid.Empty,null!)).ReturnsAsync(EmptyRoomRequestDto);
        mockUserRoomRepository.Setup(x => x.GetUserRoomAsyncById("", Guid.Empty))
            .ReturnsAsync(EmptyUserRoomRequestDto);
        mockAuth.Setup(x => x.AuthorizeAsync(null!, EmptyRoomRequestDto, "WeakRoomPolicy")).ReturnsAsync(AuthorizationResult.Success);

        var controller = new UserRoomsController(mockRoomRepository.Object, mockUserRoomRepository.Object, mockAuth.Object);

        //Act
        var result = await controller.DeleteUserRoom("", Guid.Empty);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }
}