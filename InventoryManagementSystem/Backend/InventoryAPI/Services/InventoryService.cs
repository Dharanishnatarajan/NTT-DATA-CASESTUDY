using InventoryAPI.Data;
using InventoryAPI.Models;
using InventoryAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryContext _context;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(InventoryContext context, ILogger<InventoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<InventoryItemDto>> GetAllItemsAsync()
        {
            try
            {
                var items = await _context.InventoryItems
                    .OrderBy(x => x.ItemName)
                    .ToListAsync();

                return items.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventory items");
                throw;
            }
        }

        public async Task<InventoryItemDto?> GetItemByIdAsync(int id)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                return item != null ? MapToDto(item) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving item with id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItemDto>> SearchItemsAsync(string searchTerm)
        {
            try
            {
                var items = await _context.InventoryItems
                    .Where(x => x.ItemName.Contains(searchTerm))
                    .OrderBy(x => x.ItemName)
                    .ToListAsync();

                return items.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching items with term: {searchTerm}");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync()
        {
            try
            {
                var items = await _context.InventoryItems
                    .Where(x => x.Quantity < x.ReorderLevel)
                    .OrderBy(x => x.ItemName)
                    .ToListAsync();

                return items.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock items");
                throw;
            }
        }

        public async Task<InventorySummaryDto> GetSummaryAsync()
        {
            try
            {
                var items = await _context.InventoryItems.ToListAsync();

                return new InventorySummaryDto
                {
                    TotalItems = items.Count,
                    TotalQuantity = items.Sum(x => x.Quantity),
                    LowStockCount = items.Count(x => x.Quantity < x.ReorderLevel),
                    TotalInventoryValue = items.Sum(x => x.UnitPrice * x.Quantity)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventory summary");
                throw;
            }
        }

        public async Task<InventoryItemDto> CreateItemAsync(CreateInventoryItemDto dto)
        {
            try
            {
                var item = new InventoryItem
                {
                    ItemName = dto.ItemName,
                    Quantity = dto.Quantity,
                    ReorderLevel = dto.ReorderLevel,
                    UnitPrice = dto.UnitPrice,
                    SupplierName = dto.SupplierName,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.InventoryItems.Add(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created inventory item: {item.ItemName}");
                return MapToDto(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inventory item");
                throw;
            }
        }

        public async Task<InventoryItemDto?> UpdateItemAsync(int id, UpdateInventoryItemDto dto)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                    return null;

                if (!string.IsNullOrEmpty(dto.ItemName))
                    item.ItemName = dto.ItemName;

                if (dto.Quantity.HasValue)
                    item.Quantity = dto.Quantity.Value;

                if (dto.ReorderLevel.HasValue)
                    item.ReorderLevel = dto.ReorderLevel.Value;

                if (dto.UnitPrice.HasValue)
                    item.UnitPrice = dto.UnitPrice.Value;

                if (!string.IsNullOrEmpty(dto.SupplierName))
                    item.SupplierName = dto.SupplierName;

                item.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Updated inventory item: {item.ItemName}");
                return MapToDto(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating item with id {id}");
                throw;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                    return false;

                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Deleted inventory item: {item.ItemName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting item with id {id}");
                throw;
            }
        }

        private InventoryItemDto MapToDto(InventoryItem item)
        {
            return new InventoryItemDto
            {
                Id = item.Id,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                ReorderLevel = item.ReorderLevel,
                UnitPrice = item.UnitPrice,
                SupplierName = item.SupplierName,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                IsLowStock = item.Quantity < item.ReorderLevel
            };
        }
    }
}
