using Microsoft.AspNetCore.Mvc;
using OnlineChat.Models;

namespace OnlineChat.Controllers;

[ApiController]
[Route("chatrooms")]
public class ChatsController : Controller
{
    private readonly Storage _storage;

    public ChatsController(Storage storage)
    {
        _storage = storage;
    }

    [HttpGet]
    public IEnumerable<ChatroomInfo> GetChatrooms()
    {
        return _storage.GetChatrooms().Select(ChatroomInfo.Of).ToList();
    }

    [HttpPost("create")]
    public int CreateChatroom( /*[FromBody] string username*/)
    {
        return _storage.CreateNewChatroom().Id;
    }
}