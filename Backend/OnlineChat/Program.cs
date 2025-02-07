using BusinessLogic.Hubs.Chat;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using BusinessLogic.Services.UsersService;
using Constants;
using Database;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineChat;
using IUserConnectionIdTracker = BusinessLogic.Services.IUserConnectionIdTracker;
using UserConnectionIdTracker = BusinessLogic.Services.UserConnectionIdTracker;

const string myPolicy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(Schemes.DefaultCookieScheme)
       .AddCookie(Schemes.DefaultCookieScheme, pb =>
       {
           pb.LoginPath = '/' + Routes.LoginPath;
           pb.LogoutPath = '/' + Routes.LogoutPath;
           pb.SlidingExpiration = true;
           pb.Cookie.Name = "dl";
           pb.Cookie.SameSite = SameSiteMode.None;
           pb.Cookie.HttpOnly = true;
           pb.Cookie.IsEssential = true;
           pb.Events.OnRedirectToLogin = context =>
           {
               context.Response.StatusCode = 401;
               context.RedirectUri = Routes.Frontend + "/login";
               return Task.CompletedTask;
           };
       });
Console.WriteLine(ConnectionStrings.SqlConnectionString);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher>();
builder.Services.AddDbContext<ChatDatabase>(options =>
    options.UseSqlServer(ConnectionStrings.SqlConnectionString));
builder.Services.AddScoped<IStorageService, DatabaseStorageService>(
    sp => new DatabaseStorageService(sp.GetRequiredService<ChatDatabase>())
);
builder.Services.AddScoped<IUserAccessor, UserAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myPolicy,
        b =>
        {
            b.WithOrigins(Routes.Localhost, Routes.Frontend)
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        });
});

builder.Services.AddSingleton<IUserConnectionIdTracker, UserConnectionIdTracker>();


builder.Services.AddSignalR();
builder.Services.AddMediatR(typeof(GetChatroomsHandler));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(myPolicy);

app.UseAuthentication();
app.UseAuthorization();
app.CheckClaimsIfNotAuthenticated(Claims.Name, Claims.Token, Claims.UserId);
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();