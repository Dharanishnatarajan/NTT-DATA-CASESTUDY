using InventoryAPI.Models;
using InventoryAPI.Models.DTOs;

namespace InventoryAPI.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItemDto>> GetAllItemsAsync();
        Task<InventoryItemDto?> GetItemByIdAsync(int id);
        Task<IEnumerable<InventoryItemDto>> SearchItemsAsync(string searchTerm);
        Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync();
        Task<InventorySummaryDto> GetSummaryAsync();
        Task<InventoryItemDto> CreateItemAsync(CreateInventoryItemDto dto);
        Task<InventoryItemDto?> UpdateItemAsync(int id, UpdateInventoryItemDto dto);
        Task<bool> DeleteItemAsync(int id);
    }
}
