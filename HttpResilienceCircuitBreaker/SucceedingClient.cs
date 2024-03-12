namespace HttpResilienceCircuitBreaker;

public class SucceedingClient(HttpClient client)
{
    public async Task<string> Get()
    {
        try
        {
            using var response = await client.GetAsync("");
            return $"Status code {response.StatusCode}";
        }
        catch (Exception e)
        {
            return $"Exception {e.Message}";
        }
    }
}