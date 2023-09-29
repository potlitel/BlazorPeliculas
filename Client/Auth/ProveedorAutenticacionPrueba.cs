using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
    public class ProveedorAutenticacionPrueba : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonimo = new ClaimsIdentity(); //Claims se refiere a datos acerca de un user
            var testingUser = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim("llave1", "valor1"),
                    new Claim("edad", "99"),
                    new Claim(ClaimTypes.Name, "potlitel"),
                    //new Claim(ClaimTypes.Role, "admin"),
                },
                authenticationType: "prueba"
            ); //Claims se refiere a datos acerca de un user
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));
        }
    }
}