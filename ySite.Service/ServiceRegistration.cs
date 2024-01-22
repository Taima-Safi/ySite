
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Repository.RepoInterfaces;
using Repository.Repos;
using SixLabors.ImageSharp;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using ySite.Core.Helper;
using ySite.Core.StaticFiles;
using ySite.Core.StaticUserRoles;
using ySite.EF.DbContext;
using ySite.EF.Entities;
using ySite.Service.Authorization.Requirments;
using ySite.Service.Authorization.Requirments.CommentRequirements;
using ySite.Service.Authorization.Requirments.FriendShipRequirements;
using ySite.Service.Authorization.Requirments.PostRequirements;
using ySite.Service.Authorization.Requirments.ReactionRequirements;
using ySite.Service.Authorization.Requirments.ReplayRequirements;
using ySite.Service.Interfaces;
using ySite.Service.Services;
using ySite.Service.Services.Localization;

namespace ySite.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string? connectionStringConfigName)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionStringConfigName);
            });
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IPostRepo, PostRepo>();
            services.AddScoped<IReactionRepo, ReactionRepo>();
            services.AddScoped<ICommentRepo, CommentRepo>();
            services.AddScoped<IFriendShipRepo, FriendShipRepo>();
            services.AddScoped<IReplayRepo, ReplayRepo>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFriendShipService, FriendShipService>();
            services.AddScoped<IReplayService, ReplayService>();

            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            return services;
        }
        public static IServiceCollection LocalizationServices(this IServiceCollection services, IConfigurationSection supportedLanguages)
        {
            services.AddLocalization();
            services.AddControllers()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(JsonStringLocalizerFactory));
                });

            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo("en-US"),
            //        new CultureInfo("ar")
            //    };
            //    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});
            services.Configure<LocalizerStatics>(supportedLanguages);
            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:7283")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()); // If you need to include credentials (cookies, headers, etc.)
            });
            // Other configurations...
            return services;
        }
        public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services)
        {
            return services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>();
        }
        public static IServiceCollection AddApplicationJwtAuth(this IServiceCollection services, JwtConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                       ValidateIssuer = true,
                        ValidateAudience =  true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.Issuer,
                        ValidAudience = configuration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app, string contName)
        {
            using (var scope = app.Services.CreateScope())
            {

               // var cntx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //await cntx.Database.EnsureDeletedAsync();
                //if (await cntx.Database.EnsureCreatedAsync())
                //{
                //    // Creating Role Entities
                //    var adminRole = new IdentityRole(UserRoles.ADMIN);
                //    var ownerRole = new IdentityRole(UserRoles.OWNER);
                //    var userRole = new IdentityRole(UserRoles.USER);

                //// Adding Roles
                //await roleManager.CreateAsync(adminRole);
                //await roleManager.CreateAsync(ownerRole);
                //await roleManager.CreateAsync(userRole);

                //// Creating User Entities
                //var adminUser = new ApplicationUser() { FirstName = "a", LastName = "b", UserName = "adminA", Email = "adminA@test.com" };
                //var ownerUser = new ApplicationUser() { FirstName = "a", LastName = "b", UserName = "ownerA", Email = "ownerA@test.com" };
                //var user = new ApplicationUser() { FirstName = "a", LastName = "b", UserName = "user", Email = "user@test.com" };

                //// Adding Users with Password
                //await userManager.CreateAsync(adminUser, "123");
                //await userManager.CreateAsync(ownerUser, "123");
                //await userManager.CreateAsync(user, "123");


                //// Adding Roles to Users
                //await userManager.AddToRoleAsync(adminUser, UserRoles.ADMIN);
                //await userManager.AddToRoleAsync(ownerUser, UserRoles.OWNER);
                //await userManager.AddToRoleAsync(user, UserRoles.USER);

                ////// Ading Claims to Users
                ////await userManager.AddClaimAsync(adminUser, GetAdminClaims(Contoller.Post));
                ////await userManager.AddClaimAsync(ownerUser, GetOwnerClaims(Contoller.Post));
                ////await userManager.AddClaimAsync(user, GetUserClaims(Contoller.Post));

                //await roleManager.AddClaimAsync(adminRole, GetAdminClaims(Contoller.Comment));
                //await roleManager.AddClaimAsync(ownerRole, GetOwnerClaims(Contoller.Comment));
                //await roleManager.AddClaimAsync(userRole, GetUserClaims(Contoller.Comment));

                //await roleManager.AddClaimAsync(adminRole, GetAdminClaims(Contoller.Reaction));
                //await roleManager.AddClaimAsync(ownerRole, GetOwnerClaims(Contoller.Reaction));
                //await roleManager.AddClaimAsync(userRole, GetUserClaims(Contoller.Reaction));

                //await roleManager.AddClaimAsync(adminRole, GetAdminClaims(Contoller.Post));
                //await roleManager.AddClaimAsync(ownerRole, GetOwnerClaims(Contoller.Post));
                //await roleManager.AddClaimAsync(userRole, GetUserClaims(Contoller.Post));

                //await roleManager.AddClaimAsync(adminRole, GetAdminClaims(Contoller.FriendShip));
                //await roleManager.AddClaimAsync(ownerRole, GetOwnerClaims(Contoller.FriendShip));
                //await roleManager.AddClaimAsync(userRole, GetUserClaims(Contoller.FriendShip)); 

                var adminRole = await roleManager.FindByNameAsync(UserRoles.ADMIN);
                var ownerRole = await roleManager.FindByNameAsync(UserRoles.OWNER);
                var userRole = await roleManager.FindByNameAsync(UserRoles.USER);

                await roleManager.AddClaimAsync(adminRole, GetAdminClaims(contName));
                    await roleManager.AddClaimAsync(ownerRole, GetOwnerClaims(contName));
                    await roleManager.AddClaimAsync(userRole, GetUserClaims(contName));

                //}
            }
            return app;
        }

        public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
            //    options.AddPolicy(Policies.FullControlPolicy, policy =>
            //    {
            //        policy.RequireClaim(Contoller.Teacher,
            //            Permissions.Permission.Delete.ToString(),
            //            Permissions.Permission.Update.ToString());
            //    });

                options.AddPolicy(Policies.FullControlPolicy, policy =>
                {
                    policy.Requirements.Add(new OwnerRequirements());
                });

                options.AddPolicy(Policies.ReadAndWritePolicy, policy =>
                {
                    policy.Requirements.Add(new AdminRequirements());
                });

                options.AddPolicy(Policies.ReadPolicy, policy =>
                {
                    policy.Requirements.Add(new UserRequirements());
                });
                //Post Policies
                options.AddPolicy(Policies.DeletePostPolicy, policy =>
                {
                    policy.Requirements.Add(new DeletePostRequirements());
                });

                options.AddPolicy(Policies.EditPostPolicy, policy =>
                {
                    policy.Requirements.Add(new EditPostRequirements());
                });
                //Comment Policies

                options.AddPolicy(Policies.EditCommentPolicy, policy =>
                {
                    policy.Requirements.Add(new EditCommentRequirements());
                });
                
                options.AddPolicy(Policies.DeleteCommentPolicy, policy =>
                {
                    policy.Requirements.Add(new DeleteCommentRequirements());
                });

               // Reaction Policies
                options.AddPolicy(Policies.DeleteReactionPolicy, policy =>
                {
                    policy.Requirements.Add(new DeleteReactionRequirements());
                });

                options.AddPolicy("FullControlOrDeleteReactionPolicy", policy =>
                {
                    policy.Combine(options.GetPolicy(Policies.FullControlPolicy));
                    policy.Combine(options.GetPolicy(Policies.DeleteReactionPolicy));
                });

                //FriendShip Policies
                options.AddPolicy(Policies.DeleteFriendPolicy, policy =>
                {
                    policy.Requirements.Add(new DeleteFriendRequirements());
                });
                
                //Replay Policies
                options.AddPolicy(Policies.EditReplayPolicy, policy =>
                {
                    policy.Requirements.Add(new EditReplayRequirements());
                });
                
                options.AddPolicy(Policies.DeleteReplayPolicy, policy =>
                {
                    policy.Requirements.Add(new DeleteReplayRequirements());
                });

                options.AddPolicy(Policies.GenericOwnerPolicy, policy =>
                {
                    policy.Requirements.Add(new GenericOwnerRequirements());
                });
            });
            services.AddSingleton<IAuthorizationHandler, AdminRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, OwnerRequirementsHandler>();
            services.AddSingleton<IAuthorizationHandler, UserRequirementHandler>();

            services.AddScoped<IAuthorizationHandler, EditCommentRequirementHandler> ();
            services.AddScoped<IAuthorizationHandler, DeleteCommentRequirementHandler> ();

            //services.AddScoped<IAuthorizationHandler, EditReactionRequirementHandler> ();
            services.AddScoped<IAuthorizationHandler, DeleteReactionRequirementHandler> ();

            services.AddScoped<IAuthorizationHandler, DeleteFriendRequirementHandler> ();

            services.AddScoped<IAuthorizationHandler, EditReplayRequirementHandler> ();
            services.AddScoped<IAuthorizationHandler, DeleteReplayRequirementHandler> ();

            services.AddScoped<IAuthorizationHandler, DeletePostRequirementHandler> ();
            services.AddScoped<IAuthorizationHandler, EditPostRequirementHandler> ();
            services.AddScoped<IAuthorizationHandler, GenericOwnerRequirementHandler> ();
            return services;
        }
        public static IServiceCollection AddApplicationCookieAuth(this IServiceCollection services)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "My_Cookie_Name_In_Browser";
                    // Cookie settings
                    // configuration can be written here:
                    // builder.Services.ConfigureApplicationCookie

                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });

            return services;
        }

        private static Claim GetOwnerClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            Permissions.Permission.Read,
                            Permissions.Permission.Write,
                            Permissions.Permission.Update,
                            Permissions.Permission.Delete
                        ));
        }
        private static Claim GetAdminClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            Permissions.Permission.Read,
                            Permissions.Permission.Write
                        ));
        }
        private static Claim GetUserClaims(string controllerName)
        {
            return new Claim(controllerName,
                        ClaimHelper.SerializePermissions(
                            Permissions.Permission.Read
                        ));
        }
    }
}
