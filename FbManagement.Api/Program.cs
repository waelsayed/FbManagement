
using FbManagement.Application.Interfaces;
using FbManagement.Application.Services;
using FbManagement.Infrastructure.FacebookApi;
using Microsoft.Extensions.DependencyInjection;

namespace FbManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configuration
            // Get Facebook API configuration from appsettings.json
            //var facebookConfig = builder.Configuration.GetSection("Facebook");
            //var accessToken = facebookConfig["AccessToken"];
            //var apiVersion = facebookConfig["ApiVersion"];


            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<ICommentsService, CommentsService>();

            builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var accessToken = configuration["FacebookApi:AccessToken"];
                var apiVersion = configuration["FacebookApi:ApiVersion"];
                client.BaseAddress = new Uri($"https://graph.facebook.com/{apiVersion}");
            });

            //builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>(client =>
            //{
            //    client.BaseAddress = new Uri("https://graph.facebook.com/");

            //}).AddHttpMessageHandler(() => new FacebookApiClientHandler(accessToken, apiVersion)); // Ensure configuration is passed


            // Register FacebookApiClient with HttpClient
            // Register FacebookApiClient with HttpClient
            //builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>(client =>
            //{
            //    client.BaseAddress = new Uri("https://graph.facebook.com/");
            //}).AddHttpMessageHandler(sp =>
            //{
            //    var configuration = sp.GetRequiredService<IConfiguration>();
            //    var accessToken = configuration["FacebookApi:AccessToken"];
            //    var apiVersion = configuration["FacebookApi:ApiVersion"];

            //    if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(apiVersion))
            //    {
            //        throw new InvalidOperationException("Facebook API configuration is missing.");
            //    }

            //    return new FacebookApiClient(client,accessToken, apiVersion);
            //});

            builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>((serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var accessToken = configuration["FacebookApi:AccessToken"];
                var apiVersion = configuration["FacebookApi:ApiVersion"];

                client.BaseAddress = new Uri("https://graph.facebook.com/");

                // Use serviceProvider to resolve other dependencies if necessary
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
