using OnlineChat.Hubs;
using OnlineChat.Models;

var myPolicy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myPolicy,
        b =>
        {
            b.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        });
});

builder.Services.AddSignalR();

builder.Services.AddSingleton<Storage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();

//app.UseStaticFiles();

app.UseRouting();
// app.UseAuthorization();
app.UseCors(myPolicy);
//app.UseFileServer();
app.MapHub<ChatHub>("/chat");
app.MapControllers();
// app.UseEndpoints(b => b.MapControllers());

app.Run();