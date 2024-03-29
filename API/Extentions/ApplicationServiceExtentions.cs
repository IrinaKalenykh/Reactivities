using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extentions;

public static class ApplicationServiceExtentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration conf)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(conf.GetConnectionString("DefaultConnection"));
        });
        services.AddCors(opt =>
           {
               opt.AddPolicy("CorsPolicy", policy =>
               {
                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
               });
           }
        );
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(List.Handler).Assembly));
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Create>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }
}
