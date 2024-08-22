using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp1.Server.Interface;
using ReactApp1.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReactApp1.Server.Models;

namespace ReactApp1.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UserDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("local")));


            builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAbsenceService, AbsenceService>();
            builder.Services.AddScoped<IReadRepository<MonthlyInterventionModel>, MonthlyInterventionReadRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            using (var scope = app.Services.CreateScope())
            {
                DataRandomizer.Seed(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}
