using FbManagement.Application.Interfaces;
using FbManagement.Infrastructure.FacebookApi;
using Microsoft.Extensions.DependencyInjection;

namespace FbManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<ICommentsService, CommentsService>();

            // Register FacebookApiClient with HttpClient
            builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/");
            }).ConfigureHttpClient((provider, client) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var accessToken = configuration["FacebookApi:AccessToken"];
                var apiVersion = configuration["FacebookApi:ApiVersion"];

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(apiVersion))
                {
                    throw new InvalidOperationException("Facebook API configuration is missing.");
                }

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }).AddTypedClient((httpClient, sp) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var accessToken = configuration["FacebookApi:AccessToken"];
                var apiVersion = configuration["FacebookApi:ApiVersion"];

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(apiVersion))
                {
                    throw new InvalidOperationException("Facebook API configuration is missing.");
                }

                return new FacebookApiClient(httpClient, accessToken, apiVersion);
            });

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
