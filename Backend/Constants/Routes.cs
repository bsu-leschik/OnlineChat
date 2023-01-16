namespace Constants;

public static class Routes
{
    public const string Frontend = "http://127.0.0.1:4200";
    public const string Localhost = "http://localhost:4200";
    public const string Api = "api";
    public const string Authentication = "authentication";
    public const string AuthenticationApi = Api + "/" + Authentication;
    public const string Chatrooms = "chatrooms";
    public const string ChatroomsApi = Api + "/" + Chatrooms;
    public const string Registration = "registration";
    public const string RegistrationApi = Api + "/" + Registration;
    public const string Users = "users";
    public const string UsersApi = Api + "/" + Users;
    public const string LoginPath = "login";
    public const string LogoutPath = "logout";
}