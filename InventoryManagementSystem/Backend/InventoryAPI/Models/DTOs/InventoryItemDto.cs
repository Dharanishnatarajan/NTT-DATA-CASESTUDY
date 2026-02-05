namespace InventoryAPI.Models.DTOs
{
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsLowStock { get; set; }
    }

    public class CreateInventoryItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public decimal UnitPrice { get; set; }
        public string SupplierName { get; set; } = string.Empty;
    }

    public class UpdateInventoryItemDto
    {
        public string? ItemName { get; set; }
        public int? Quantity { get; set; }
        public int? ReorderLevel { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? SupplierName { get; set; }
    }

    public class InventorySummaryDto
    {
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public int LowStockCount { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }
}
