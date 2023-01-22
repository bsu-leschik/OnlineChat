using Database;
using Entities;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

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
        if (await _storageService.GetUsersAsync(cancellationToken)
                                 .ContainsAsync(u => u.Username == command.Username, cancellationToken))
        {
            return new RegistrationResponse("Duplicate username");
        }

        var user = new User(command.Username, command.Password);
        var password = _passwordHasher.HashPassword(user, command.Password);
        user.Password = password;
        await _storageService.AddUserAsync(user, cancellationToken);
        return new RegistrationResponse("Success");
    }
}