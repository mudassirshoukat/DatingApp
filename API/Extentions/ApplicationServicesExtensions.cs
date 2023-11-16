using API.Data;
using API.Data.Repository;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services,IConfiguration Config ) {

            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();



            Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Config.GetConnectionString("DefaultConnection"));
            });
           Services.AddCors();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddAutoMapper(typeof(AutoMapperProfiles));
            return Services;
        }
    }
}
