﻿using BusinessLogic.Queries.Users.GetUsernames;
using Database.Entities;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesResponse
{
    public List<Message>? Messages { get; set; }

    public GetMessagesResponse(List<Message>? messages)
    {
        Messages = messages;
    }
}