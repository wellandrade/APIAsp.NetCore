﻿using DevIO.Api.Controllers;
using DevIO.Api.DTO;
using DevIO.Api.Extension;
using DevIO.Business.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [Route("api")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; // Para a autenticaco do usuario 
        private readonly UserManager<IdentityUser> _userManager; // Para a criacao do usuario
        private readonly AppSettings _appSettings;

        public AuthController(INotificador notificador, SignInManager<IdentityUser> signInManager,
                UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
            : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]

        public async Task<ActionResult> RegistrarUsuario(RegistrarUsuarioDTO registrarUsuario)
        {
            if (!ModelState.IsValid)
            {
                return CustomizarResponse(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = registrarUsuario.Email,
                Email = registrarUsuario.Email,
                EmailConfirmed = true // Para nao mandar email de confirmacao para o usuario
            };

            var result = await _userManager.CreateAsync(user, registrarUsuario.Senha);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false); // Ja vai acessar com o usuario, mas nao vai guardar informacoes para o proximo acesso
                return CustomizarResponse(await GerarToken(user.Email));
            }

            foreach (var item in result.Errors)
            {
                NotificarErro(item.Description);
            }

            return CustomizarResponse(registrarUsuario);
        }

        [HttpPost("entrar")]

        public async Task<ActionResult> Login(LoginUsuarioDTO loginUsuario)
        {
            if (!ModelState.IsValid)
            {
                return CustomizarResponse(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginUsuario.Email, loginUsuario.Senha, false, true); // Vai travar o usuario com tentativas invalidas

            if (result.Succeeded)
            {
                return CustomizarResponse(await GerarToken(loginUsuario.Email));
            }

            if (result.IsLockedOut) // Usario travado, foi definido na opcao true do PasswordSignInAsync
            {
                NotificarErro("Usuario temporariamente bloqueado por numero de tentativas invalidas");
                return CustomizarResponse(loginUsuario);
            }

            NotificarErro("Usuario ou senha incorretos");
            return CustomizarResponse(loginUsuario);
        }

        private async Task<LoginResponseDTO> GerarToken(string email) // Modo simples 
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // Identificacao do token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixExpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixExpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identityClaims // Para passar a claims para o token 
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseDTO
            {
                AcessToken = encodedToken,
                ExpireIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioTokenDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(x => new ClaimDTO { Type = x.Type, Value = x.Value })
                }
            };

            return response;
        }

        private static long ToUnixExpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
