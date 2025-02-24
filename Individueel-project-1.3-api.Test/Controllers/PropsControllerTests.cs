using System.Security.Claims;
using individueel_project_1._3_api.Controllers;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Individueel_project_1._3_api.Test.Controllers;

public class PropsControllerTests
{
    [Fact]
    public async Task GetPropsAsync_PropId_Forbid()
    {
        //todo
        /*//Arrange
        var mockRoomRepository = new Mock<IRoomRepository>();
        var mockPropRepository = new Mock<IPropRepository>();
        var mockAuth = new Mock<IAuthorizationService>();
        
        mockAuth.Setup(x => x.AuthorizeAsync(null!, "")).ReturnsAsync(AuthorizationResult.Success());
        
        var controller = new PropsController(mockPropRepository.Object, mockRoomRepository.Object, mockAuth.Object);
        
        //Act
        var result = await controller.GetPropsAsync(Guid.Empty, Guid.Empty);

        //Assert
        Assert.IsType<ForbidResult>(result);*/
    }
    
    [Fact]
    public async Task GetPropsAsync_PropId_NotFound()
    {
        
    }
    
    [Fact]
    public async Task GetPropsAsync_PropId_Ok()
    {
        
    }
    
    [Fact]
    public async Task GetPropsAsync_NoPropId_Forbid()
    {
        
    }
    
    [Fact]
    public async Task GetPropsAsync_NoPropId_NotFound()
    {
        
    }
    
    [Fact]
    public async Task GetPropsAsync_NoPropId_Ok()
    {
        
    }
    
    [Fact]
    public async Task AddPropAsync_Add_CreatedAtRoute()
    {
        
    }
    
    [Fact]
    public async Task UpdatePropAsync_Update_NotFound()
    {
        
    }
    
    [Fact]
    public async Task UpdatePropAsync_Update_Forbid()
    {
        
    }
    
    [Fact]
    public async Task UpdatePropAsync_Update_NoContent()
    {
        
    }
    
    [Fact]
    public async Task DeletePropAsync_Delete_NotFound()
    {
        
    }
    
    [Fact]
    public async Task DeletePropAsync_Delete_Forbid()
    {
        
    }
    
    [Fact]
    public async Task DeletePropAsync_Delete_NoContent()
    {
        
    }
}
