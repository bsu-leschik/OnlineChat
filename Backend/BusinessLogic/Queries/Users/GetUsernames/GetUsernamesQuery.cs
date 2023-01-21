﻿using MediatR;

namespace BusinessLogic.Queries.Users.GetUsernames;

public class GetUsernamesQuery : IRequest<GetUsernamesResponse>
{
    public string? StartingWith { get; set; }
}