using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.RepoInterfaces;
using Repository.Repos;
using ySite.EF.DbContext;
using ySite.EF.Entities;
using ySite.Service;
using ySite.Service.Interfaces;
using ySite.Service.Services;
using ySite.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ySite.Core.StaticUserRoles;
using ySite.Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("constr"));
        builder.Services.AddApplicationIdentity();
        builder.Services.AddApplicationJwtAuth(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
        builder.Services.ConfigureServices();
        builder.Services.LocalizationServices();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        var supportedCultures = new[] { "en-US", "ar" };

        var localizationOptions =
            new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseCors("AllowSpecificOrigin");
      // await app.SeedDataAsync(Contoller.Replay);

        app.Run();
    }
}