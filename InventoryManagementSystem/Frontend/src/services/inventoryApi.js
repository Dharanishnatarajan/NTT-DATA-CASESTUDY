import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'https://localhost:7290/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const inventoryApi = {
  // Get all items
  getAllItems: async () => {
    try {
      const response = await apiClient.get('/inventory');
      return response.data;
    } catch (error) {
      throw new Error(`Failed to fetch items: ${error.message}`);
    }
  },

  // Get item by id
  getItemById: async (id) => {
    try {
      const response = await apiClient.get(`/inventory/${id}`);
      return response.data;
    } catch (error) {
      throw new Error(`Failed to fetch item: ${error.message}`);
    }
  },

  // Search items
  searchItems: async (searchTerm) => {
    try {
      const response = await apiClient.get(`/inventory/search/${searchTerm}`);
      return response.data;
    } catch (error) {
      throw new Error(`Failed to search items: ${error.message}`);
    }
  },

  // Get low stock items
  getLowStockItems: async () => {
    try {
      const response = await apiClient.get('/inventory/low-stock');
      return response.data;
    } catch (error) {
      throw new Error(`Failed to fetch low stock items: ${error.message}`);
    }
  },

  // Get summary
  getSummary: async () => {
    try {
      const response = await apiClient.get('/inventory/summary');
      return response.data;
    } catch (error) {
      throw new Error(`Failed to fetch summary: ${error.message}`);
    }
  },

  // Create item
  createItem: async (itemData) => {
    try {
      const response = await apiClient.post('/inventory', itemData);
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw new Error(`Failed to create item: ${error.message}`);
    }
  },

  // Update item
  updateItem: async (id, itemData) => {
    try {
      const response = await apiClient.put(`/inventory/${id}`, itemData);
      return response.data;
    } catch (error) {
      if (error.response?.data) {
        throw error.response.data;
      }
      throw new Error(`Failed to update item: ${error.message}`);
    }
  },

  // Delete item
  deleteItem: async (id) => {
    try {
      await apiClient.delete(`/inventory/${id}`);
      return true;
    } catch (error) {
      throw new Error(`Failed to delete item: ${error.message}`);
    }
  },
};

export default apiClient;
