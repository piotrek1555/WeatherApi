namespace API.Features.User.RegisterUser;

public record UserRegistrationRequest(string FirstName, string LastName, string Password, string Country);

