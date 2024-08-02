using API.Common.CountriesApi;
using API.Common.Exceptions;
using API.Common.Utilities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.User.RegisterUser;

public class UserRegistration
{
    public record UserRegistrationCommand(string FirstName, string LastName, string Password, string Country) : IRequest<UserRegistrationResponse>;

    public sealed class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {
        private readonly DataContext _dataContext;
        private readonly ICountriesApiClient _countriesApiClient;

        public UserRegistrationCommandHandler(DataContext dataContext, ICountriesApiClient countriesApiClient)
        {
            _dataContext = dataContext;
            _countriesApiClient = countriesApiClient;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
        {
            //TODO : validate request
            //TODO : validate if password is strong enough
            var country = await GetCountry(request.Country, cancellationToken);

            var userName = GenerateUserName(request.FirstName, request.LastName);

            while (await _dataContext.Users.AnyAsync(x => x.UserName == userName, cancellationToken))
            {
                userName = GenerateUserName(request.FirstName, request.LastName);
                //TODO : add limit for number of tries or implement different logic
            }

            var user = new Infrastructure.Data.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = userName,
                Password = PasswordHasher.HashPassword(request.Password),
                LivingCountry = request.Country,
                Latitude = country.latlng[0].ToString(),
                Longitude = country.latlng[1].ToString(),
            };

            await _dataContext.Users.AddAsync(user, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return new UserRegistrationResponse(request.FirstName, request.LastName, userName, request.Country);
        }

        private async Task<CountryResponseDto> GetCountry(string countryCode, CancellationToken cancellationToken)
        {
            var countryResponse = await _countriesApiClient.GetCountryAsync(countryCode, cancellationToken);

            if (!countryResponse.IsSuccessStatusCode)
            {
                //TODO : implement different error messages for unsuccessful status codes
                throw new BusinessException("Country not found. The country should be in the format of a two or three letter code, e.g. 'mt', 'mlt'");
            }

            var countries = await countryResponse.Content.ReadFromJsonAsync<CountryResponseDto[]>(cancellationToken: cancellationToken);
            var country = countries?.FirstOrDefault();

            if (country == null)
            {
                throw new BusinessException("An error occured during parsing country response");
            }

            return country;
        }

        private static string GenerateUserName(string firstName, string lastName)
            => string.Concat(firstName.AsSpan(0, 2), lastName.AsSpan(0, 2), new Random().Next(100000, 999999).ToString());
    }
}


