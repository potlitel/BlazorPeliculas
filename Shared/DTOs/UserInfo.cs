using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.DTOs
{
    public class UserInfo
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}