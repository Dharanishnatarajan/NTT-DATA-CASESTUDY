-- SQL Database Setup Script
-- Manufacturing Inventory Management System

-- Create Database
CREATE DATABASE [InventoryManagementDB]
GO

USE [InventoryManagementDB]
GO

-- Create Items Table
CREATE TABLE [InventoryItems] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ItemName] NVARCHAR(200) NOT NULL,
    [Quantity] INT NOT NULL DEFAULT 0,
    [Threshold] INT NOT NULL DEFAULT 10,
    [Price] DECIMAL(18,2) NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedDate] DATETIME2 NULL,
    CONSTRAINT [CK_Quantity] CHECK ([Quantity] >= 0),
    CONSTRAINT [CK_Threshold] CHECK ([Threshold] >= 0),
    CONSTRAINT [CK_Price] CHECK ([Price] IS NULL OR [Price] >= 0)
)
GO

-- Create Indexes
CREATE INDEX [IX_ItemName] ON [InventoryItems]([ItemName])
GO

CREATE INDEX [IX_Quantity] ON [InventoryItems]([Quantity])
GO

-- Insert Sample Data
INSERT INTO [InventoryItems] ([ItemName], [Quantity], [Threshold], [Price])
VALUES 
    ('Steel Plate 5mm', 150, 50, 25.50),
    ('Aluminum Bar 10mm', 80, 40, 35.75),
    ('Copper Wire 2mm', 20, 100, 12.00),
    ('Stainless Steel Tube', 45, 30, 45.99),
    ('Carbon Steel Rod', 200, 75, 18.50),
    ('Brass Coupling', 5, 50, 8.25),
    ('Plastic Resin', 300, 150, 5.75),
    ('Paint Can 1L', 10, 50, 22.00),
    ('Motor 2HP', 3, 5, 250.00),
    ('Bearing Ball', 500, 200, 2.50)
GO

-- Create View for Low Stock Items
CREATE VIEW [LowStockItems] AS
SELECT 
    [Id],
    [ItemName],
    [Quantity],
    [Threshold],
    [Price],
    [CreatedDate],
    [UpdatedDate]
FROM [InventoryItems]
WHERE [Quantity] < [Threshold]
GO

-- Create Stored Procedure for Inventory Summary
CREATE PROCEDURE [sp_GetInventorySummary]
AS
BEGIN
    SELECT 
        COUNT(*) AS TotalItems,
        SUM([Quantity]) AS TotalQuantity,
        SUM(CASE WHEN [Quantity] < [Threshold] THEN 1 ELSE 0 END) AS LowStockCount,
        SUM([Price] * [Quantity]) AS TotalInventoryValue
    FROM [InventoryItems]
END
GO

PRINT 'Database setup completed successfully!'
