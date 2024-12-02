using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracts
{
    public record RegistrationResponse(
        string Email,
        string Username);

}
