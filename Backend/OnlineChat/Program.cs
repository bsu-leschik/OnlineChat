using Constants;
using Database;
using Database.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineChat;
using OnlineChat.Hubs;

const string myPolicy = "MyPolicy";

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<Database.Database>();
builder.Services.AddSingleton<IStorageService>(sp => new DatabaseStorageService(sp.GetService<Database.Database>()!));

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

//builder.Services.AddSingleton<IStorageService, Storage>();
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(myPolicy);
app.MapHub<ChatHub>("/chat");
app.MapControllers();

app.Run();