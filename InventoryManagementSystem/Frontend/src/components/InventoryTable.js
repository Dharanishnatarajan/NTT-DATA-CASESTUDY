import React, { useState, useEffect } from 'react';
import { inventoryApi } from '../services/inventoryApi';
import { formatCurrency, formatDate } from '../utils/formatters';
import './InventoryTable.css';

function InventoryTable({ refreshTrigger }) {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [showLowStockOnly, setShowLowStockOnly] = useState(false);

  useEffect(() => {
    fetchItems();
  }, [refreshTrigger]);

  const fetchItems = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await inventoryApi.getAllItems();
      setItems(data);
    } catch (err) {
      setError(err.message || 'Failed to load items');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this item?')) {
      try {
        await inventoryApi.deleteItem(id);
        setItems(items.filter((item) => item.id !== id));
      } catch (err) {
        setError(err.message || 'Failed to delete item');
      }
    }
  };

  const handleSearch = async (e) => {
    const term = e.target.value;
    setSearchTerm(term);

    if (term.trim() === '') {
      fetchItems();
    } else {
      try {
        const data = await inventoryApi.searchItems(term);
        setItems(data);
      } catch (err) {
        setError(err.message || 'Failed to search items');
      }
    }
  };

  let filteredItems = items;
  if (showLowStockOnly) {
    filteredItems = items.filter((item) => item.isLowStock);
  }

  if (loading) {
    return <div className="table-loading">Loading inventory...</div>;
  }

  return (
    <div className="inventory-table-container">
      <h2>Inventory Items</h2>

      <div className="table-controls">
        <input
          type="text"
          placeholder="Search items..."
          value={searchTerm}
          onChange={handleSearch}
          className="search-input"
        />
        <label className="filter-checkbox">
          <input
            type="checkbox"
            checked={showLowStockOnly}
            onChange={(e) => setShowLowStockOnly(e.target.checked)}
          />
          Show Low Stock Only
        </label>
      </div>

      {error && <div className="table-error">{error}</div>}

      {filteredItems.length === 0 ? (
        <div className="table-empty">No items found</div>
      ) : (
        <div className="table-wrapper">
          <table className="inventory-table">
            <thead>
              <tr>
                <th>Item Name</th>
                <th>Quantity</th>
                <th>Reorder Level</th>
                <th>Status</th>
                <th>Unit Price</th>
                <th>Supplier</th>
                <th>Created</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredItems.map((item) => (
                <tr key={item.id} className={item.isLowStock ? 'low-stock-row' : ''}>
                  <td>{item.itemName}</td>
                  <td>{item.quantity}</td>
                  <td>{item.reorderLevel}</td>
                  <td>
                    <span className={`status-badge ${item.isLowStock ? 'low' : 'normal'}`}>
                      {item.isLowStock ? 'Low Stock' : 'In Stock'}
                    </span>
                  </td>
                  <td>{formatCurrency(item.unitPrice)}</td>
                  <td>{item.supplierName}</td>
                  <td>{formatDate(item.createdDate)}</td>
                  <td>
                    <button
                      className="btn-edit"
                      onClick={() => window.dispatchEvent(new CustomEvent('editItem', { detail: item }))}
                    >
                      Edit
                    </button>
                    <button className="btn-delete" onClick={() => handleDelete(item.id)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default InventoryTable;
