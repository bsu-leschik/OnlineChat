using System.Reflection;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using Constants;
using Database;
using Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineChat;
using OnlineChat.Hubs;

const string myPolicy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(Schemes.DefaultCookieScheme)
       .AddCookie(Schemes.DefaultCookieScheme, pb =>
       {
           pb.LoginPath = "/login";
           pb.LogoutPath = "/logout";
           pb.SlidingExpiration = true;
           pb.Cookie.Name = "dl";
       });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher>();
builder.Services.AddDbContextPool<Database.Database>(options =>
    options.UseSqlServer("Server=.;Database=ChatDb;Trusted_Connection=True;Encrypt=False"));
builder.Services.AddScoped<IStorageService, DatabaseStorageService>(
    sp => new DatabaseStorageService(sp.GetRequiredService<Database.Database>())
);

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
builder.Services.AddMediatR(typeof(GetChatroomsHandler));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(myPolicy);
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();