using BusinessLogic.Hubs.Chat;
using BusinessLogic.Queries.Chatrooms.GetChatrooms;
using BusinessLogic.UsersService;
using Constants;
using Database;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineChat;

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
builder.Services.AddDbContextPool<Database.Database>(options =>
    options.UseSqlServer(ConnectionStrings.SqlConnectionString));
builder.Services.AddScoped<IStorageService, DatabaseStorageService>(
    sp => new DatabaseStorageService(sp.GetRequiredService<Database.Database>())
);
builder.Services.AddScoped<IUsersService, UsersService>();

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
app.CheckClaimsIfNotAuthenticated(Claims.Name, Claims.Token);
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();