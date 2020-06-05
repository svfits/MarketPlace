using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GoodsStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GoodsStore
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
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/Account/Login");

                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
                options.SlidingExpiration = true;
            });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(365);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc()
              .AddSessionStateTempDataProvider();

            services.AddDbContext<DataContextApp>(options =>
                 options.UseSqlite("DataSource=GoodStore.db"));

            // Register the service and implementation for the database context
            services.AddScoped<IDataContextApp>(provider => provider.GetService<DataContextApp>());

            services.AddIdentity<User, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContextApp>();

            services.AddAuthentication();

            //добавиим версии для веб API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Об API",
                    Description = "Пример работы простого API 1",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {                    
                    Version = "v2",
                    Title = "ToDo API2",
                    Description = "A simple example ASP.NET Core Web API 2",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "ToDo API3",
                    Description = "A simple example ASP.NET Core Web API 3",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Baskets}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //endpoints.MapControllerRoute(name: "API",
                //    pattern: "controller=api/API/{action}/{id?}");
            });

#if DEBUG

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/API/v1/API1", "My API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "My API V3");
            });

#endif

            RoleInitializer(serviceProvider);

            AddDataForAPI(serviceProvider);
        }

        /// <summary>
        /// Добавление данных для работы API
        /// </summary>
        /// <param name="serviceProvider"></param>
        private void AddDataForAPI(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<DataContextApp>();

            if (db.APIDatas.Take(25).Count() > 0 )
            {
                for (int i = 0; i < 20; i++)
                {
                    db.APIDatas.Add(new APIData()
                    {
                        Value = "Супер строка " + i,
                        Value2 = "Супер стора 2" + i
                    });
                }
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Добавление дефолтных пользователей
        /// </summary>
        /// <param name="serviceProvider"></param>
        private void RoleInitializer(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            Task<IdentityResult> roleResult;

            #region Roles
            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Administrator");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
                roleResult.Wait();
            }

            //Check that there is an Administrator role and create if not
            Task<bool> hasUserRole = roleManager.RoleExistsAsync("User");
            hasUserRole.Wait();

            if (!hasUserRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("User"));
                roleResult.Wait();
            }
            #endregion

            #region Пользователи

            #region Администратор

            string emailAdmin = "admin@GoodsStore.ru";
            string passwordAdmin = "1234567Ad@";

            var testAdmin = userManager.FindByEmailAsync(emailAdmin);
            testAdmin.Wait();

            if (testAdmin.Result == null)
            {
                User administrator = new User
                {
                    Email = emailAdmin,
                    UserName = emailAdmin,
                };

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, passwordAdmin);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Administrator");
                    newUserRole.Wait();
                }
            }

            #endregion

            #region Пользователь

            string emailUser = "user@GoodsStore.ru";
            string passwordUser = "1234567Ad@";

            Task<User> testUser = userManager.FindByEmailAsync(emailUser);
            testUser.Wait();

            if (testUser.Result == null)
            {
                User user = new User
                {
                    Email = emailUser,
                    UserName = emailUser,
                };

                Task<IdentityResult> newUser = userManager.CreateAsync(user, passwordUser);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(user, "User");
                    newUserRole.Wait();
                }
            }

            #endregion

            #endregion
        }
    }
}
