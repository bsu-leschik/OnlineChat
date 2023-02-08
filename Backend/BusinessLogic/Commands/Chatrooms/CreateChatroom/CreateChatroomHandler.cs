using BusinessLogic.Services.UsersService;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Commands.Chatrooms.CreateChatroom;

public class CreateChatroomHandler : IRequestHandler<CreateChatroomCommand, CreateChatroomResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public CreateChatroomHandler(IStorageService storageService,
        IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<CreateChatroomResponse> Handle(CreateChatroomCommand request, CancellationToken cancellationToken)
    {
        if (request.Usernames.Count != 2 && request.Type != ChatType.Public || request.Usernames.Count == 0)
        {
            return CreateChatroomResponse.Failed;
        }
        if (request is { Type: ChatType.Public, Name: null })
        {
            return CreateChatroomResponse.Failed;
        }
        try
        {
            var user = await _usersService.GetCurrentUser(cancellationToken);
            if (user is null || !request.Usernames.Contains(user.Username))
            {
                return CreateChatroomResponse.Failed;
            }

            var users = await _storageService.GetUsersByUsername(request.Usernames)
                                             .ToListAsync(cancellationToken);

            if (users.Count == 2
                && request.Type == ChatType.Private
                && IsDuplicatePrivateChatroom(user, users.First(u => u != user)))
            {
                return CreateChatroomResponse.Failed;
            }

            var id = Guid.NewGuid();
            Chatroom chatroom = request.Type == ChatType.Public
                ? new PublicChatroom(id, users, owner: user, name: request.Name!)
                : new PrivateChatroom(id, users);
            await _storageService.AddChatroomAsync(chatroom, cancellationToken);
            await _storageService.SaveChangesAsync(cancellationToken);
            return new CreateChatroomResponse(chatroom.Id);
        }
        catch (Exception e)
        {
            return CreateChatroomResponse.Failed;
        }
    }

    private static bool IsDuplicatePrivateChatroom(User user, User second)
    {
        return user.Chatrooms.OfType<PrivateChatroom>().Contains(c => c.Users.Contains(second));
    }
}