using individueel_project_1._3_api.Authorization;
using individueel_project_1._3_api.Dto;
using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
var sqlConnectionStringFound= !string.IsNullOrWhiteSpace(connectionString);


builder.Services.AddSingleton<IPropRepository, PropRepository>(_ => new PropRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
builder.Services.AddSingleton<IRoomRepository, RoomRepository>(_ => new RoomRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
builder.Services.AddSingleton<IUserRoomRepository, UserRoomRepository>(_ => new UserRoomRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();

var requireUserPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RoomPolicy", policy =>
        policy.Requirements.Add(new SameUserRequirement()))
    .SetDefaultPolicy(requireUserPolicy)
    .SetFallbackPolicy(requireUserPolicy);

builder.Services.AddSingleton<IAuthorizationHandler,  DoesUserMatchAuthenticationHandler<RoomRequestDto>>(_ => new DoesUserMatchAuthenticationHandler<RoomRequestDto>((room, user) => room.Users.Any(usr => usr.User.Equals(user))));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        //options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<IdentityRole>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("DapperIdentity");
    });

    
var app = builder.Build();


app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}   
app.UseAuthentication();

app.UseAuthorization();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.MapControllers().RequireAuthorization();

app.MapGet("/", () => $"The API is up and running. Connection string found: {(sqlConnectionStringFound ? "Yes! :D" : "No!")}");
app.MapPost("/account/logout",
    async (SignInManager<IdentityUser> signInManager, [FromBody] object? empty) =>
    {
        if (empty == null) return Results.Unauthorized();
        await signInManager.SignOutAsync();
        return Results.Ok();

    });

app.Run();
