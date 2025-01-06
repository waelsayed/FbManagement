
using FbManagement.Application.Interfaces;
using FbManagement.Infrastructure.FacebookApi;

namespace FbManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configuration
            // Get Facebook API configuration from appsettings.json
            var facebookConfig = builder.Configuration.GetSection("FacebookApi");
            var accessToken = facebookConfig["AccessToken"];
            var apiVersion = facebookConfig["ApiVersion"];


            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<ICommentsService, CommentsService>();
            builder.Services.AddScoped<IFacebookApiClient, FacebookApiClient>();

            builder.Services.AddHttpClient<IFacebookApiClient, FacebookApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/");
            }).ConfigureHttpClient((provider, client) =>
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            }).AddTypedClient((httpClient, sp) => new FacebookApiClient(httpClient, accessToken, apiVersion));


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
