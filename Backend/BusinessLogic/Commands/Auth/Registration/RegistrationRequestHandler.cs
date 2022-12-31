using BusinessLogic.Extensions;
using BusinessLogic.Services;
using Database;
using Database.Entities;
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
        if (await _storageService.Contains(u => u.Username == command.Username, cancellationToken))
        {
            return new RegistrationResponse("Duplicate username");
        }

        var user = new User(command.Username, command.Password);
        await _storageService.AddUser(user, cancellationToken);
        return new RegistrationResponse("Success");
    }
}