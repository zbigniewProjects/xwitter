using BlogProject.Application;
using BlogProject.Infrastructure;

namespace BlogProject
{
    public class Startup
    {
        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
        {
            //services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connectionString));
            services.AddClearApplication();
            services.AddClearInfrastructure();

            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDbContext<PostsDbContext>(options => options.Use(connectionString));

            // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //     .AddEntityFrameworkStores<UsersDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssemblyContaining<TestDataAccess>();
            //});


            services.AddRazorComponents().AddInteractiveServerComponents();

            services.AddServerSideBlazor();
            //services.AddIdentity<BlogUser, IdentityRole<uint>>(options =>
            //{
            //    options.Password.RequiredUniqueChars = 0;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequireLowercase = false;
            //}
            //).AddEntityFrameworkStores<UsersDbContext>().AddDefaultTokenProviders();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapRazorPages();
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapControllerRoute(
            //    //    name: "default",
            //    //    pattern: "blazor/{action=Index}/{id?}"
            //    //);
            //    endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode();
            //    endpoints.MapBlazorHub();
            //});
        }
    }
}
