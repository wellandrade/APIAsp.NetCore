using AutoMapper;
using DevIO.Api.Configuracao;
using DevIO.Business.Interface;
using DevIO.Business.Notificacoes;
using DevIO.Business.Service;
using DevIO.Data.Content;
using DevIO.Data.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace DevIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeuDbContext>(optionsAction: options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(name: "DefaultConnection"));
            });

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup)); // Resolve todo o mapeamento feito pelo autoMapper

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // Removendo a forma da validacao viewmodel automatica
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Versionamento da API
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; // Quando nao tiver versao especificada, ele vai assumir default
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true; // Caso tenha alguma versao, sera avisado que a versao está desatualizada
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV"; // Quantidade de versao q api pode ter, exemplo v1.1, v2.1 e nao v1.12
                options.SubstituteApiVersionInUrl = true; // Substituir na url conforme sua versao
            });

            //Documentacao com Swaager
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "Minha API", Version = "v1" });
            //});

            //services.ConfigureSwaggerGen(SwaggerGenOptions options);

            // Colocar em uma classe, ou metodo
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
            services.AddScoped<IEnderecoRepositorio, EnderecoRepositorio>();
            services.AddScoped<IFornecedorServico, FornecedorServico>();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            services.AddScoped<IProdutoServico, ProdutoServico>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Redirecionamento caso seja chamado por http
            app.UseCors("Development");
            app.UseAuthentication(); // configuracao do indetity 
            app.UseMvc();
        }
    }
}
