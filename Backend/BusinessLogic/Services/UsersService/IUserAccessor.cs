using Database;
using Entities;

namespace BusinessLogic.Services.UsersService;

public interface IUserAccessor
{
    public bool TryGetClaim(string claimName, out string result);
}