import React, { useState, useEffect } from 'react';
import { inventoryApi } from '../services/inventoryApi';
import './InventoryForm.css';

function InventoryForm({ onItemSaved, editingItem = null }) {
  const [formData, setFormData] = useState({
    itemName: '',
    quantity: 0,
    reorderLevel: 10,
    unitPrice: 0,
    supplierName: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [successMessage, setSuccessMessage] = useState('');

  useEffect(() => {
    if (editingItem) {
      setFormData({
        itemName: editingItem.itemName || '',
        quantity: editingItem.quantity || 0,
        reorderLevel: editingItem.reorderLevel || 10,
        unitPrice: editingItem.unitPrice || 0,
        supplierName: editingItem.supplierName || '',
      });
    } else {
      resetForm();
    }
    setErrors({});
    setSuccessMessage('');
  }, [editingItem]);

  const resetForm = () => {
    setFormData({
      itemName: '',
      quantity: 0,
      reorderLevel: 10,
      unitPrice: 0,
      supplierName: '',
    });
  };

  const validateForm = () => {
    const newErrors = {};

    if (!formData.itemName.trim()) {
      newErrors.itemName = 'Item name is required';
    } else if (formData.itemName.length < 3) {
      newErrors.itemName = 'Item name must be at least 3 characters';
    }

    if (formData.quantity < 0) {
      newErrors.quantity = 'Quantity must be non-negative';
    }

    if (formData.reorderLevel < 0) {
      newErrors.reorderLevel = 'Reorder level must be non-negative';
    }

    if (formData.unitPrice && formData.unitPrice < 0) {
      newErrors.unitPrice = 'Unit price must be non-negative';
    }

    if (!formData.supplierName.trim()) {
      newErrors.supplierName = 'Supplier name is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    
    // Keep string fields as strings, convert numeric fields to numbers
    let newValue;
    if (name === 'itemName' || name === 'supplierName') {
      newValue = value;
    } else {
      newValue = parseFloat(value) || 0;
    }
    
    setFormData({
      ...formData,
      [name]: newValue,
    });
    
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: '',
      });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    try {
      setLoading(true);
      setSuccessMessage('');

      const submitData = {
        itemName: formData.itemName,
        quantity: parseInt(formData.quantity),
        reorderLevel: parseInt(formData.reorderLevel),
        unitPrice: parseFloat(formData.unitPrice) || 0,
        supplierName: formData.supplierName,
      };

      if (editingItem) {
        await inventoryApi.updateItem(editingItem.id, submitData);
        setSuccessMessage('Item updated successfully!');
      } else {
        await inventoryApi.createItem(submitData);
        setSuccessMessage('Item created successfully!');
      }

      setTimeout(() => {
        onItemSaved();
        resetForm();
        setSuccessMessage('');
      }, 1500);
    } catch (error) {
      if (error.errors) {
        const fieldErrors = {};
        Object.keys(error.errors).forEach((key) => {
          const fieldName = key.charAt(0).toLowerCase() + key.slice(1);
          fieldErrors[fieldName] = error.errors[key]?.[0] || 'An error occurred';
        });
        setErrors(fieldErrors);
      } else {
        setErrors({ submit: error.message || 'Failed to save item' });
      }
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    resetForm();
    setErrors({});
    setSuccessMessage('');
  };

  return (
    <div className="inventory-form-container">
      <h2>{editingItem ? 'Update Item' : 'Add New Item'}</h2>

      {successMessage && <div className="success-message">{successMessage}</div>}
      {errors.submit && <div className="error-message">{errors.submit}</div>}

      <form onSubmit={handleSubmit} className="inventory-form">
        <div className="form-group">
          <label htmlFor="itemName">Item Name *</label>
          <input
            type="text"
            id="itemName"
            name="itemName"
            value={formData.itemName}
            onChange={handleChange}
            placeholder="Enter item name"
            className={errors.itemName ? 'input-error' : ''}
          />
          {errors.itemName && <span className="error-text">{errors.itemName}</span>}
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="quantity">Quantity *</label>
            <input
              type="number"
              id="quantity"
              name="quantity"
              value={formData.quantity}
              onChange={handleChange}
              placeholder="0"
              min="0"
              className={errors.quantity ? 'input-error' : ''}
            />
            {errors.quantity && <span className="error-text">{errors.quantity}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="reorderLevel">Reorder Level *</label>
            <input
              type="number"
              id="reorderLevel"
              name="reorderLevel"
              value={formData.reorderLevel}
              onChange={handleChange}
              placeholder="10"
              min="0"
              className={errors.reorderLevel ? 'input-error' : ''}
            />
            {errors.reorderLevel && <span className="error-text">{errors.reorderLevel}</span>}
          </div>

          <div className="form-group">
            <label htmlFor="unitPrice">Unit Price *</label>
            <input
              type="number"
              id="unitPrice"
              name="unitPrice"
              value={formData.unitPrice}
              onChange={handleChange}
              placeholder="0.00"
              step="0.01"
              min="0"
              className={errors.unitPrice ? 'input-error' : ''}
            />
            {errors.unitPrice && <span className="error-text">{errors.unitPrice}</span>}
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="supplierName">Supplier Name *</label>
          <input
            type="text"
            id="supplierName"
            name="supplierName"
            value={formData.supplierName}
            onChange={handleChange}
            placeholder="Enter supplier name"
            className={errors.supplierName ? 'input-error' : ''}
          />
          {errors.supplierName && <span className="error-text">{errors.supplierName}</span>}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn-submit" disabled={loading}>
            {loading ? 'Saving...' : editingItem ? 'Update Item' : 'Add Item'}
          </button>
          <button type="reset" className="btn-reset" onClick={handleReset} disabled={loading}>
            Clear
          </button>
        </div>
      </form>
    </div>
  );
}

export default InventoryForm;
