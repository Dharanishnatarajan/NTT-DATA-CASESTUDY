import React, { useState, useCallback } from 'react';
import Dashboard from './components/Dashboard';
import InventoryForm from './components/InventoryForm';
import InventoryTable from './components/InventoryTable';
import './App.css';

function App() {
  const [refreshTrigger, setRefreshTrigger] = useState(0);
  const [editingItem, setEditingItem] = useState(null);
  const [activeTab, setActiveTab] = useState('dashboard');

  const handleItemSaved = useCallback(() => {
    setRefreshTrigger((prev) => prev + 1);
    setEditingItem(null);
  }, []);

  const handleEditItem = (event) => {
    setEditingItem(event.detail);
    setActiveTab('form');
  };

  React.useEffect(() => {
    window.addEventListener('editItem', handleEditItem);
    return () => window.removeEventListener('editItem', handleEditItem);
  }, []);

  return (
    <div className="app">
      <header className="app-header">
        <div className="header-content">
          <h1>Manufacturing Inventory Management System</h1>
          <p>Real-time inventory tracking and management</p>
        </div>
      </header>

      <nav className="app-nav">
        <button
          className={`nav-button ${activeTab === 'dashboard' ? 'active' : ''}`}
          onClick={() => {
            setActiveTab('dashboard');
            setEditingItem(null);
          }}
        >
          Dashboard
        </button>
        <button
          className={`nav-button ${activeTab === 'form' ? 'active' : ''}`}
          onClick={() => setActiveTab('form')}
        >
          {editingItem ? 'Edit Item' : 'Add Item'}
        </button>
        <button
          className={`nav-button ${activeTab === 'inventory' ? 'active' : ''}`}
          onClick={() => setActiveTab('inventory')}
        >
          Inventory
        </button>
      </nav>

      <main className="app-main">
        {activeTab === 'dashboard' && <Dashboard key={refreshTrigger} />}
        {activeTab === 'form' && (
          <InventoryForm onItemSaved={handleItemSaved} editingItem={editingItem} />
        )}
        {activeTab === 'inventory' && (
          <InventoryTable refreshTrigger={refreshTrigger} />
        )}
      </main>

      <footer className="app-footer">
        <p>&copy; 2024 Manufacturing Inventory Management System. All rights reserved.</p>
      </footer>
    </div>
  );
}

export default App;
