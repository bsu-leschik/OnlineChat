using Database;
using Database.Entities;
using Extensions;
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
            // Task<User?> GetUserByName(string username) => _storageService.GetUserAsync(u => u.Username == username,
                // cancellationToken);

            List<User> users = await _storageService.GetUsersAsync(cancellationToken)
                                              .WhereAsync(u => request.Usernames.Contains(u.Username),
                                                  cancellationToken)
                                              .ToListAsync(cancellationToken);
            // List<User> users = request.Usernames
            //                           .Select(username =>
            //                           {
            //                               var task = GetUserByName(username);
            //                               task.Wait(cancellationToken);
            //                               return task.Result;
            //                           })
            //                           .Where(user => user is not null)
            //                           .ToList()!;

            if (await IsDuplicateChatroomAsync(users, request.Type, cancellationToken))
            {
                return CreateChatroomResponse.Failed;
            }

            var chatroom = new Chatroom(
                id: Guid.NewGuid(),
                type: request.Type,
                users: users
            );
            foreach (var user in users.Where(user => !user.Chatrooms.Contains(chatroom)))
            {
                user.Chatrooms.Add(chatroom);
            }
            await _storageService.AddChatroomAsync(chatroom, cancellationToken);
            await _storageService.SaveChangesAsync(cancellationToken);
            return new CreateChatroomResponse(chatroom.Id);
        }
        catch (Exception)
        {
            return CreateChatroomResponse.Failed;
        }
    }

    private async Task<bool> IsDuplicateChatroomAsync(List<User> users, Chatroom.ChatType type, CancellationToken cancellationToken)
    {
        if (type == Chatroom.ChatType.Public)
        {
            return false;
        }

        return await _storageService.GetChatroomsAsync(cancellationToken)
                              .ContainsAsync(c => ListExtensions.EqualAsSets(c.Users, users), cancellationToken);
    }
}