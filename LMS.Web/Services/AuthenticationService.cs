namespace LMS.Web.Services;

public class AuthenticationService
{
    private const string USERNAME = "ramasubbu";
    private const string PASSWORD = "Keerthini@2022";

    public bool ValidateCredentials(string username, string password)
    {
        return string.Equals(username, USERNAME, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(password, PASSWORD, StringComparison.Ordinal);
    }

    public string GetAuthenticatedUser()
    {
        return USERNAME;
    }
}