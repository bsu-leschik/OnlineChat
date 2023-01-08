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
            foreach (var user in users)
            {
                if (!user!.Chatrooms.Contains(chatroom))
                    user.Chatrooms.Add(chatroom);
                // await _storageService.Update(user, cancellationToken);
            }
            await _storageService.AddChatroom(chatroom, cancellationToken);
            await _storageService.SaveChangesAsync(cancellationToken);
            return new CreateChatroomResponse(chatroom.Id);
        }
        catch (Exception)
        {
            return new CreateChatroomResponse(Guid.Empty, created: false);
        }
    }
}