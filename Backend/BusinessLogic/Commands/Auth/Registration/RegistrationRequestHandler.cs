﻿using Database;
using Database.Entities;
using Extensions;
using MediatR;

namespace BusinessLogic.Commands.Auth.Registration;

public class RegistrationRequestHandler : IRequestHandler<RegistrationCommand, RegistrationResponse>
{
    private readonly IStorageService _storageService;

    public RegistrationRequestHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<RegistrationResponse> Handle(RegistrationCommand command, CancellationToken cancellationToken)
    {
        if (await _storageService.GetUsersAsync(cancellationToken)
                                 .ContainsAsync(u => u.Username == command.Username, cancellationToken))
        {
            return new RegistrationResponse("Duplicate username");
        }

        var user = new User(command.Username, command.Password);
        await _storageService.AddUserAsync(user, cancellationToken);
        return new RegistrationResponse("Success");
    }
}