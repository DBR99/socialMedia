using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMediaCore;
using SocialMediaCore.CustomEntities;
using SocialMediaCore.Interfaces;
using SocialMediaCore.Services;
using SocialMediaInfraestructure.Data;
using SocialMediaInfraestructure.Filters;
using SocialMediaInfraestructure.Interfaces;
using SocialMediaInfraestructure.Repositories;
using SocialMediaInfraestructure.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SocialMedia.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // AQUI SE COLOCARAN LAS DEPENDENCIAS


            //Con esta linea se indica a automaper que busque los profile dentro de toda la sln
            //el featured identifica las clases que implementen Profile
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options => {
                //Con esta linea se soluciona la excepción de referencias circulares presentada por usar la entidad 
                //directamente y no un DTO, usar DTO también lo soluciona... PARA PODER USAR ESTA DEPENDENCIA SE INSTALÓ
                //EL PAQUETE NEWTON SOFT JSON.
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })

            //Con esto podemos configurar las opciones de comportamiento de la Api
            //Con esto continuamos usando el [ApiController] pero deshabilitamos la validación de modelo que hace
            //por defecto

            .ConfigureApiBehaviorOptions(Options => {
                //   Options.SuppressModelStateInvalidFilter = true;
            });

            //con esto obtenemos una sección definida en appsettings.json y vamos a mapear esa información a una clase
            //la cual va estar en CustomEntities/PaginationOptions
            //Con configure, estamos también creando un singleton
            services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));


            services.AddDbContext<SocialMediaContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"))
            );

            //Con el transient se genera una instancia cada vez que se hace una solicitud.
            services.AddTransient<IPostService, PostService>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
             
            }) ;
            //Con el AddSingleton mantenemos una sola instancia se usa para servicios que no generan Estado

            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Api", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);

            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            //Con este add Mvc agregamos de forma global los filters sobre validacion de modelo para todos los
            //Controladores de la aplicación (E)
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationsFilter>();
            }).AddFluentValidation(options => 
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API");
               options.RoutePrefix = string.Empty;
            }
            );

            app.UseRouting();

            //AQUI ES IMPORTANTE EL ORDEN, PRIMERO DEBE IR UseAuthentication Y LUEGO UseAuthorization Y DESPUES DEL
            //ROUTING ---- PRIMERO SE DEFINE QUIEN ES LA PERSONA Y A QUE TIENE PERMISOS.

            app.UseAuthentication();


            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
