namespace BusinessLogic.Queries.Users.GetUsernames;

public class GetUsernamesResponse
{
    public GetUsernamesResponse(List<string> usernames)
    {
        Usernames = usernames;
    }

    public List<string> Usernames { get; set; }
}