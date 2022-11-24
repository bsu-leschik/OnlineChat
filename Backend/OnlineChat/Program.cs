using OnlineChat.Hubs;
using OnlineChat.Hubs.ConnectionGuards;
using OnlineChat.Models;
using OnlineChat.Services;
using OnlineChat.Services.StorageSanitizer;

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

builder.Services.AddSingleton<ConnectionGuard>(b => new ConnectionGuard(b.GetService<Storage>()!)
                                                    .AddApprover(new IsEmptyUsernameApprover())
                                                    .AddApprover(new IsDuplicateNicknameApprover()));

builder.Services.AddSignalR();

builder.Services.AddSingleton<Storage>(provider =>
{
    var instance = new Storage();
    var sanitizer = new TimeoutStorageSanitizer(instance, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5));
    instance.AddStorageSanitizer(sanitizer);
    return instance;
});

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