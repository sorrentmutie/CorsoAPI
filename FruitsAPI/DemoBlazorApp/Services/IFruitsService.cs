using FruitsAPI.Models;

namespace DemoBlazorApp.Services
{
    public interface IFruitsService
    {
        Task<List<FruitModel>> GetFruitsAsync();
        Task<FruitModel?> GetFruitByIdAsync(int id);
        Task AddFruitAsync(FruitModel newFruit);
        Task UpdateFruitAsync(FruitModel updatedFruit);
        Task DeleteFruitAsync(int id);
    }
}