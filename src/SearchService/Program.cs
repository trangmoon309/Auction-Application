using System.Net;
using MassTransit;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x => 
{
    // Show lên "Exchanges" tag trên rabbit page
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // Show lên "Exchanges" tag trên rabbit page
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    
    x.UsingRabbitMq((context, cfg) => 
    {
        // Retry vs TH bên consumer này bị fail somehow
        cfg.ReceiveEndpoint("search-auction-created", e => 
        {
            e.UseMessageRetry(r => r.Interval(5, 5));
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => 
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (System.Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();

// If Auction Service down, thì chúng ta sẽ handle transient http error exception
// Ta sẽ keep retry after 3s
// Nó sẽ retry cho tới khi mình start Auction Service
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));