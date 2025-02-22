using individueel_project_1._3_api.Authorization;
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


builder.Services.AddSingleton<ICrudRepository<Guid, Prop>, PropRepository>(_ => new PropRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
builder.Services.AddSingleton<ICrudRepository<Guid, Room>, RoomRepository>(_ => new RoomRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
builder.Services.AddSingleton<ICrudRepository<string, User>, UserRepository>(_ => new UserRepository(connectionString?? throw new ArgumentException("No connection string found in secrets.json")));
builder.Services.AddSingleton<ICrudRepository<(string Username, Guid RoomId), UserRoom>, UserRoomRepository>(_ => new UserRoomRepository(connectionString ?? throw new ArgumentException("No connection string found in secrets.json")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();
//Todo: policies and claims
builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy =>
        policy.Requirements.Add(new SameUserRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler,  DoesUserMatchAuthenticationHandler<User>>(_ => new DoesUserMatchAuthenticationHandler<User>(user => user.Username));

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

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.MaxDepth = 64;
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
