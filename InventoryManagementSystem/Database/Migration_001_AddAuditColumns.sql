-- Migration: Add Audit Columns (Optional)
-- This migration adds audit tracking columns for better data governance

ALTER TABLE [InventoryItems]
ADD [LastModifiedBy] NVARCHAR(100) NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL
GO

-- Create Index on IsDeleted for soft deletes
CREATE INDEX [IX_IsDeleted] ON [InventoryItems]([IsDeleted])
GO

PRINT 'Migration 001 completed!'
