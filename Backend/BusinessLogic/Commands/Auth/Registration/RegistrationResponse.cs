namespace BusinessLogic.Commands.Auth.Registration;

public class RegistrationResponse
{
    public string Reason { get; set; }

    public RegistrationResponse(string reason)
    {
        Reason = reason;
    }
}