

using ySite.Core.StaticUserRoles;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("constr"));
        builder.Services.AddApplicationIdentity();
        builder.Services.AddApplicationJwtAuth(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
        builder.Services.AddApplicationCookieAuth();
        builder.Services.AddApplicationAuthorization();
        builder.Services.ConfigureServices();

        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
        app.UseCors("AllowSpecificOrigin");
        //await app.SeedDataAsync();

        app.Run();
    }
}