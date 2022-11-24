using OnlineChat;
using OnlineChat.Hubs;
using OnlineChat.Hubs.ConnectionGuards;
using OnlineChat.Hubs.SendMessageApprover;
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

builder.Services.AddSingleton(b => new ConnectionGuard(b.GetService<Storage>()!)
                                                    .AddApprover(new IsEmptyUsernameApprover())
                                                    .AddApprover(new IsDuplicateNicknameApprover()));

builder.Services.AddSingleton(_ => new SendMessageGuard()
                                                     .AddVerifier(new EmptyOrWhitespaceMessageVerifier())
                                                     .AddVerifier(new IsSpamVerifier()));

builder.Services.AddSignalR();

builder.Services.AddSingleton(_ =>
{
    var instance = new Storage();
    var sanitizer = new TimeoutStorageSanitizer(instance,
        Constants.TimeoutSanitizerRefreshTime,
        Constants.TimeoutSanitizerMaxEmptyTime);
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