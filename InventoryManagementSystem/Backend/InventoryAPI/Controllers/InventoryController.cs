using InventoryAPI.Models.DTOs;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all inventory items
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAll()
        {
            try
            {
                var items = await _inventoryService.GetAllItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all items");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving inventory items" });
            }
        }

        /// <summary>
        /// Get inventory item by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryItemDto>> GetById(int id)
        {
            try
            {
                var item = await _inventoryService.GetItemByIdAsync(id);
                if (item == null)
                    return NotFound(new { message = "Item not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting item with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving item" });
            }
        }

        /// <summary>
        /// Search inventory items by name
        /// </summary>
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> Search(string searchTerm)
        {
            try
            {
                var items = await _inventoryService.SearchItemsAsync(searchTerm);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching items with term: {searchTerm}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error searching items" });
            }
        }

        /// <summary>
        /// Get all low-stock items
        /// </summary>
        [HttpGet("low-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetLowStock()
        {
            try
            {
                var items = await _inventoryService.GetLowStockItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting low stock items");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving low stock items" });
            }
        }

        /// <summary>
        /// Get inventory summary statistics
        /// </summary>
        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventorySummaryDto>> GetSummary()
        {
            try
            {
                var summary = await _inventoryService.GetSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventory summary");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving summary" });
            }
        }

        /// <summary>
        /// Create a new inventory item
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryItemDto>> Create([FromBody] CreateInventoryItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var item = await _inventoryService.CreateItemAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inventory item");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error creating item" });
            }
        }

        /// <summary>
        /// Update an existing inventory item
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InventoryItemDto>> Update(int id, [FromBody] UpdateInventoryItemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var item = await _inventoryService.UpdateItemAsync(id, dto);
                if (item == null)
                    return NotFound(new { message = "Item not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating item with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error updating item" });
            }
        }

        /// <summary>
        /// Delete an inventory item
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _inventoryService.DeleteItemAsync(id);
                if (!result)
                    return NotFound(new { message = "Item not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting item with id {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error deleting item" });
            }
        }
    }
}
