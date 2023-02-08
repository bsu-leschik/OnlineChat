using Database;
using Entities;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Commands.Auth.Registration;

public class RegistrationRequestHandler : IRequestHandler<RegistrationCommand, RegistrationResponse>
{
    private readonly IStorageService _storageService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegistrationRequestHandler(IStorageService storageService, IPasswordHasher<User> passwordHasher)
    {
        _storageService = storageService;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegistrationResponse> Handle(RegistrationCommand command, CancellationToken cancellationToken)
    {
        if (command.Password.Length < Constants.Security.MinPasswordLength)
        {
            return RegistrationResponse.Error;
        }
        if (await _storageService.GetUsers().Where(u => u.Username == command.Username).AnyAsync(cancellationToken))
        {
            return RegistrationResponse.DuplicateUsername;
        }

        var user = new User(command.Username, command.Password);
        var password = _passwordHasher.HashPassword(user, command.Password);
        user.Password = password;
        await _storageService.AddUserAsync(user, cancellationToken);
        return RegistrationResponse.Success;
    }
}