using BusinessLogic.Services;
using Database;
using Database.Entities;
using MediatR;

namespace BusinessLogic.Commands.CreateChatroom;

public class CreateChatroomHandler : IRequestHandler<CreateChatroomCommand, CreateChatroomResponse>
{
    private readonly IStorageService _storageService;

    public CreateChatroomHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<CreateChatroomResponse> Handle(CreateChatroomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Task<User?> GetUserByName(string username) => _storageService.GetUser(u => u.Username == username,
                cancellationToken);

            var users = request.Usernames
                               .Select(username =>
                               {
                                   var task = GetUserByName(username);
                                   task.Wait(cancellationToken);
                                   return task.Result;
                               })
                               .Where(user => user is not null)
                               .ToList();

            var chatroom = new Chatroom(
                id: Guid.NewGuid(),
                type: request.Type,
                users: users!
            );
            await _storageService.AddChatroom(chatroom, cancellationToken);
            return new CreateChatroomResponse(chatroom.Id);
        }
        catch (Exception)
        {
            return new CreateChatroomResponse(Guid.Empty, created: false);
        }
    }
}