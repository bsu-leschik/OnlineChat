using System.Security.Claims;
using Database;
using Database.Entities;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.CreateChatroom;

public class CreateChatroomHandler : IRequestHandler<CreateChatroomCommand, CreateChatroomResponse>
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateChatroomHandler(IStorageService storageService, IHttpContextAccessor contextAccessor)
    {
        _storageService = storageService;
        _contextAccessor = contextAccessor;
    }

    public async Task<CreateChatroomResponse> Handle(CreateChatroomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Task<User?> GetUserByName(string username) => _storageService.GetUserAsync(u => u.Username == username,
                // cancellationToken);
            var user = await Users.FindUser(_storageService, _contextAccessor.HttpContext!.User, cancellationToken);
            if (user is null || !request.Usernames.Contains(user.Username))
            {
                return CreateChatroomResponse.Failed;
            }
            
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
            foreach (var u in users.Where(user => !user.Chatrooms.Contains(chatroom)))
            {
                u.Chatrooms.Add(chatroom);
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