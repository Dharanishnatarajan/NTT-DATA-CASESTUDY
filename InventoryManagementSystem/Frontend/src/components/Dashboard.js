import React, { useEffect, useState } from 'react';
import { inventoryApi } from '../services/inventoryApi';
import { formatCurrency } from '../utils/formatters';
import './Dashboard.css';

function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchSummary();
  }, []);

  const fetchSummary = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await inventoryApi.getSummary();
      setSummary(data);
    } catch (err) {
      setError(err.message || 'Failed to load summary');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="dashboard-loading">Loading summary...</div>;
  }

  if (error) {
    return (
      <div className="dashboard-error">
        <p>Error: {error}</p>
        <button onClick={fetchSummary}>Retry</button>
      </div>
    );
  }

  return (
    <div className="dashboard">
      <h2>Inventory Dashboard</h2>
      <div className="summary-cards">
        <div className="card card-blue">
          <div className="card-number">{summary?.totalItems ?? 0}</div>
          <div className="card-label">Total Items</div>
        </div>
        <div className="card card-green">
          <div className="card-number">{summary?.totalQuantity ?? 0}</div>
          <div className="card-label">Total Quantity</div>
        </div>
        <div className="card card-orange">
          <div className="card-number">{summary?.lowStockCount ?? 0}</div>
          <div className="card-label">Low Stock Items</div>
        </div>
        <div className="card card-purple">
          <div className="card-number">{formatCurrency(summary?.totalInventoryValue ?? 0)}</div>
          <div className="card-label">Total Value</div>
        </div>
      </div>
    </div>
  );
}

export default Dashboard;
