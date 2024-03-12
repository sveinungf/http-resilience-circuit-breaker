using HttpResilienceCircuitBreaker;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection("RetryOptions");

var succeedingClientBuilder = builder.Services.AddHttpClient<SucceedingClient>(x =>
    x.BaseAddress = new Uri("https://ipv4.icanhazip.com/"));

var failingClientBuilder = builder.Services.AddHttpClient<FailingClient>(x =>
    x.BaseAddress = new Uri("http://the-internet.herokuapp.com/status_codes/500"));

var useHttpClientDefaults = true;
if (useHttpClientDefaults)
{
    builder.Services.ConfigureHttpClientDefaults(x => x.AddStandardResilienceHandler(section));
}
else
{
    succeedingClientBuilder.AddStandardResilienceHandler(section);
    failingClientBuilder.AddStandardResilienceHandler(section);
}

var app = builder.Build();

app.MapGet("/test", async (SucceedingClient succeedingClient, FailingClient failingClient) =>
{
    var succeedingClientResult = await succeedingClient.Get();
    var failingClientResult = await failingClient.Get();

    return $"""
        SucceedingClient result: {succeedingClientResult}
        FailingClient result: {failingClientResult}
        """;
});

app.Run();
