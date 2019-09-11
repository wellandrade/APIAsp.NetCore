using DevIO.Api.Data;
using DevIO.Api.Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevIO.Api.Configuracao
{
    public static class IdentityConfig
    {
        // Contexto para o Identity fazer as manipulacoes de seguranca
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(optionsAction: options =>
                options.UseSqlServer(configuration.GetConnectionString(name: "DefaultConnection")));

            // Configurar o identity 
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagemPortugues>() // Mensagem do indetity em portugues 
                .AddDefaultTokenProviders();

            //JWT - Configurando classe de AppSettings
            var appSettingsSection = configuration.GetSection("AppeSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Obtendo os dados da classe
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x => // Configuration autenticacao
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Autenticar alguem sempre pelo token
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Verificar se a requisicao tem token valido
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true; // Quem solicitar deve vim do https
                x.SaveToken = true; // Token deve ser guardado no http
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Quem esta emitindo, tem que ser o mesmo que esta no token
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer= appSettings.Emissor
                };
            });

            return services;
        }
    }
}
