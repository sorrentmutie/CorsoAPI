using FruitsAPI.Models;

namespace DemoBlazorApp.Services;

public class FruitsService : IFruitsService
{
    private readonly IHttpClientFactory httpClientFactory;

    public FruitsService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task AddFruitAsync(FruitModel newFruit)
    {
        var httpClient = httpClientFactory.CreateClient("FruitAPI");
        var result = await httpClient.PostAsJsonAsync("/fruits", newFruit);  
    }

    public async Task DeleteFruitAsync(int id)
    {
        var httpClient = httpClientFactory.CreateClient("FruitAPI");
        var result = await httpClient.DeleteAsync($"/fruits/{id}");   
    }

    public Task<FruitModel?> GetFruitByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<FruitModel>> GetFruitsAsync()
    {
        var httpClient = httpClientFactory.CreateClient("FruitAPI");
        var response = await httpClient.GetAsync("/fruits");
        if (response.IsSuccessStatusCode)
        {
            var fruits = await response.Content.ReadFromJsonAsync<List<FruitModel>>();
            return fruits ?? new List<FruitModel>();
        }
        else
        {
            // Gestisci l'errore come preferisci (es. log, eccezione, ecc.)
            throw new Exception($"Errore nella richiesta: {response.StatusCode}");
        }
    }

    public async Task UpdateFruitAsync(FruitModel updatedFruit)
    {
        var httpClient = httpClientFactory.CreateClient("FruitAPI");
        var result = await httpClient.PutAsJsonAsync($"/fruits/{updatedFruit.id}", updatedFruit);
    }
}
