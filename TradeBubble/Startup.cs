using AlgoTrading.Stocks.Persistence;
using AlgoTrading.Stocks.Persistence.Database;
using AlgoTrading.Stocks;
using AlgoTrading.Broker.Simulated;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using TradeBubble.Services;
using TradeBubble.ViewModels;

namespace TradeBubble
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

            services.AddInvestApiClient((_, settings) => settings.AccessToken = "t.73Y83GIQL4Z3OPBEczwyTnyfFnkQl-qfdE0YJd77R8s3ZJmNDxK_UC2eQ7ZjStSXjYC9fpIdYbO4nICbUrSapw");

            services.AddScoped<LearningPageViewModel>();
            services.AddScoped<StockManagementViewModel>();

            services.AddSingleton<LearningManagerService>();
            services.AddHostedService<LearningManagerService>();

            services.AddSingleton<StockDataService>();
            services.AddHostedService<StockDataService>();

            services.AddDbContextFactory<StockDataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly("AlgoTrading.Stocks.Persistence.Database")));

            services.AddSingleton<IStockPersistenceManager, DatabasePersistenceManager>();

            services.AddSingleton<IIndicatorProvider, SkenderIndicatorProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHFqVVhkW1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jTXxXd0diUH1cd31cRQ==;Mgo+DSMBPh8sVXJ0S0d+XE9AcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xTckdnWH1feXZXTmFeUg==;Mgo+DSMBMAY9C3t2VVhhQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkFhXX5ccX1XRGheUkc=;NjIzNzgwQDMyMzAyZTMxMmUzMEV0NTF3VFV0NWxOL1g3QUlZTzhIa1VKSWFtMGxPSm1PTStaMGFkcWwxaU09");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
