namespace Infrastructure.Data.Entities;

public class User
{
    public long Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? Address { get; set; } = default!;
    public DateOnly? BirthDate { get; set; } = default!;
    public string? PhoneNumber { get; set; } = default!;
    public string LivingCountry { get; set; } = default!;
    public string? CizitenCountry { get; set; } = default!;
    public string? Latitude { get; set; } = default!;
    public string? Longitude { get; set; } = default!;
}