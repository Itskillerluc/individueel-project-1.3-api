using individueel_project_1._3_api.Models;
using individueel_project_1._3_api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ?? throw new ArgumentException("No connection string found in secrets.json");

builder.Services.AddSingleton<ICrudRepository<Guid, Prop>, PropRepository>(pfrovider => new PropRepository(connectionString));
builder.Services.AddSingleton<ICrudRepository<Guid, Room>, RoomRepository>(provider => new RoomRepository(connectionString));
builder.Services.AddSingleton<ICrudRepository<string, User>, UserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddSingleton<ICrudRepository<(string Username, Guid RoomId), UserRoom>, UserRoomRepository>(provider => new UserRoomRepository(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


app.UseAuthorization();
app.MapControllers();

app.Run();
