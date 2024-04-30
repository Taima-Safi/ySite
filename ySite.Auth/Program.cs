using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ySite.Core.StaticFiles;
using ySite.Core.StaticUserRoles;
using ySite.Service;

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
        builder.Services.LocalizationServices(builder.Configuration.GetSection("SupportedLanguages"));

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

        var localizationOptions = builder.Services.BuildServiceProvider().GetService<IOptions<LocalizerStatics>>().Value;

        app.UseRequestLocalization(new RequestLocalizationOptions()
            .SetDefaultCulture(localizationOptions.SupportedCultures[0])
            .AddSupportedCultures(localizationOptions.SupportedCultures)
            .AddSupportedUICultures(localizationOptions.SupportedCultures));

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseCors("AllowSpecificOrigin");
        // await app.SeedDataAsync(Contoller.Replay);
        //await app.SeedDataAsync();

        app.Run();
    }
}