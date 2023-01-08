namespace BusinessLogic.Commands.Auth.TokenLogin;

public class TokenLoginResponse
{
    public string Username { get; set; }
    public TokenLoginResponseCode ResponseCode { get; set; }

    public TokenLoginResponse(string username, TokenLoginResponseCode responseCode)
    {
        Username = username;
        ResponseCode = responseCode;
    }

    public static TokenLoginResponse Error(TokenLoginResponseCode code) 
        => new TokenLoginResponse(string.Empty, code);

    public static TokenLoginResponse Success(string username)
        => new TokenLoginResponse(username, TokenLoginResponseCode.Success);
}

public enum TokenLoginResponseCode
{
    Success = 0,
    TokenExpired,
    NotLoggedIn,
    BadRequest
}