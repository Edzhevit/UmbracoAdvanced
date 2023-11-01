using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Umbraco.Cms.Core.Notifications;
using UmbracoAdvanced.Core.NotificationHandlers;
using UmbracoAdvanced.Core.Services;
using UmbracoAdvanced.Web.Extensions;

namespace UmbracoAdvanced.Web;

public class Startup
{
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup" /> class.
    /// </summary>
    /// <param name="webHostEnvironment">The web hosting environment.</param>
    /// <param name="config">The configuration.</param>
    /// <remarks>
    /// Only a few services are possible to be injected here https://github.com/dotnet/aspnetcore/issues/9337.
    /// </remarks>
    public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
    {
        _env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <remarks>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
    /// </remarks>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddUmbraco(_env, _config)
            .AddBackOffice()
            .AddWebsite()
            .AddDeliveryApi()
            .AddComposers()
            .AddContactRequestTable()
            .AddContactRequestMappings()
            .AddNotificationHandler<ContentPublishingNotification, ContentPublishing>()
            .AddNotificationHandler<ContentPublishedNotification, ContentPublished>()
            .AddNotificationHandler<SendingContentNotification, SendingContent>()
            .AddNotificationHandler<MenuRenderingNotification, MenuRendering>()
            .Build();

        services.AddScoped<IContactRequestService, ContactRequestService>();

        services.AddDbContext<DbContext>(options =>
        {
            options.UseInMemoryDatabase(nameof(DbContext));
            options.UseOpenIddict();
        });

        services.AddOpenIddict().AddCore(options =>
        {
            options.UseEntityFrameworkCore().UseDbContext<DbContext>();
        }).AddServer(options =>
        {
            options.AllowClientCredentialsFlow();
            options.SetTokenEndpointUris("connect/token");
            options.AddEphemeralEncryptionKey().AddEphemeralSigningKey();

            // certificate
            // Registering a certificate (recommended for production-ready scenarios)

            // string certificateThumbprint = _config["Auth:CertificateThumbprint"];
            // using (X509Store cerStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            // {
            //     cerStore.Open(OpenFlags.ReadOnly);
            //     X509Certificate2Collection certCollection =
            //         cerStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, true);
            //     
            //     // Get the first certificate with the thumbprint
            //     X509Certificate2 cert = certCollection.OfType<X509Certificate2>().FirstOrDefault();
            //     if (cert is null)
            //     {
            //         throw new Exception($"Certificate with thumbpring {certificateThumbprint} was not found");
            //     }
            //
            //     options.AddSigningCertificate(cert);
            //     options.AddEncryptionCertificate(cert);
            // }

            // if you want to disable the encryption of the jwt token for debug purposes
            options.DisableAccessTokenEncryption();

            options.UseAspNetCore().EnableTokenEndpointPassthrough();

        }).AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });
    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The web hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseUmbraco()
            .WithMiddleware(u =>
            {
                u.UseBackOffice();
                u.UseWebsite();
            })
            .WithEndpoints(u =>
            {
                u.UseInstallerEndpoints();
                u.UseBackOfficeEndpoints();
                u.UseWebsiteEndpoints();
            });

        InitializeClientAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
    }

    private async Task InitializeClientAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor()
            {
                ClientId = "postman",
                ClientSecret = "postman_secret",
                DisplayName = "Postman Client",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    // OpenIddictConstants.Permissions.Prefixes.Scope,
                }
            });
        }
    }
}