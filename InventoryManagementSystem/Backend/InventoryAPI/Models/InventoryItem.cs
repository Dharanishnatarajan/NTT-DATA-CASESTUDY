using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(200, MinimumLength = 3, 
            ErrorMessage = "Item name must be between 3 and 200 characters")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Reorder level is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be non-negative")]
        public int ReorderLevel { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be non-negative")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        public string SupplierName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public bool IsLowStock => Quantity < ReorderLevel;
    }
}
