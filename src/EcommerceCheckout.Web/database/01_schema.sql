IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [SKU] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [StockQty] int NOT NULL,
    [IsAvailable] bit NOT NULL,
    [Active] bit NOT NULL,
    [RowVersion] varbinary(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720091410_InitialCreate', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Coupon] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NOT NULL,
    [Active] bit NOT NULL,
    [DiscountType] int NOT NULL,
    [DiscountValue] decimal(18,2) NOT NULL,
    [MinPrice] decimal(18,2) NOT NULL,
    [MaxPrice] decimal(18,2) NULL,
    [ValidFrom] datetime2 NULL,
    [ValidTo] datetime2 NULL,
    [UsageLimit] int NULL,
    [UsedCount] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Coupon] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720092708_AddCoupon', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Coupon] DROP CONSTRAINT [PK_Coupon];

EXEC sp_rename N'[Coupon]', N'Coupons', 'OBJECT';

ALTER TABLE [Coupons] ADD CONSTRAINT [PK_Coupons] PRIMARY KEY ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720093247_AddPublicCoupon', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Nation] nvarchar(max) NOT NULL,
    [NewsletterOptIn] bit NOT NULL,
    [RequiresInvoice] bit NOT NULL,
    [FiscalTaxNumber] nvarchar(max) NULL,
    [FiscalCodeNumber] nvarchar(max) NULL,
    [PrivacyAcceptedAt] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720122510_AddCustomer', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Customers] ADD CONSTRAINT [CK_Customers_InvoiceRequiresFiscalData] CHECK ([RequiresInvoice] = 0 OR [FiscalTaxNumber] IS NOT NULL OR [FiscalCodeNumber] IS NOT NULL);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720123232_AddConstraints', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Carts] (
    [Id] int NOT NULL IDENTITY,
    [CartToken] uniqueidentifier NOT NULL,
    [CustomerId] int NULL,
    [Status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Carts_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id])
);

CREATE TABLE [CartItems] (
    [Id] int NOT NULL IDENTITY,
    [CartId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_CartItems_CartId] ON [CartItems] ([CartId]);

CREATE INDEX [IX_CartItems_ProductId] ON [CartItems] ([ProductId]);

CREATE INDEX [IX_Carts_CustomerId] ON [Carts] ([CustomerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260720124328_AddCartAndCartItem', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [OrderNumber] nvarchar(max) NOT NULL,
    [CustomerId] int NOT NULL,
    [CartId] int NULL,
    [CouponId] int NULL,
    [SubtotalAmount] decimal(18,2) NOT NULL,
    [DiscountAmount] decimal(18,2) NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [PaymentProvider] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [PaidAt] datetime2 NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]),
    CONSTRAINT [FK_Orders_Coupons_CouponId] FOREIGN KEY ([CouponId]) REFERENCES [Coupons] ([Id]),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [OrderItems] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [ProductName] nvarchar(max) NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [LineTotal] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);

CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);

CREATE INDEX [IX_Orders_CartId] ON [Orders] ([CartId]);

CREATE INDEX [IX_Orders_CouponId] ON [Orders] ([CouponId]);

CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260721114019_AddOrderAndOrderItem', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [PaymentTransactions] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [Provider] int NOT NULL,
    [ProviderTransactionId] nvarchar(max) NULL,
    [Status] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(max) NOT NULL,
    [RawResponse] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_PaymentTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentTransactions_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_PaymentTransactions_OrderId] ON [PaymentTransactions] ([OrderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260721115117_AddPaymentTransaction', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
DROP INDEX [IX_PaymentTransactions_OrderId] ON [PaymentTransactions];

CREATE TABLE [Invoices] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [InvoiceNumber] nvarchar(max) NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [VatNumber] nvarchar(max) NULL,
    [FiscalCode] nvarchar(max) NULL,
    [PdfBlobPath] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Invoices_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_PaymentTransactions_OrderId] ON [PaymentTransactions] ([OrderId]);

CREATE INDEX [IX_Invoices_OrderId] ON [Invoices] ([OrderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260721115446_AddInvoice', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [CouponProduct] (
    [CouponId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_CouponProduct] PRIMARY KEY ([ProductId], [CouponId]),
    CONSTRAINT [FK_CouponProduct_Coupons_CouponId] FOREIGN KEY ([CouponId]) REFERENCES [Coupons] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CouponProduct_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_CouponProduct_CouponId] ON [CouponProduct] ([CouponId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260721120242_AddCouponProduct', N'10.0.10');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Carts] ADD [CouponId] int NULL;

CREATE INDEX [IX_Carts_CouponId] ON [Carts] ([CouponId]);

ALTER TABLE [Carts] ADD CONSTRAINT [FK_Carts_Coupons_CouponId] FOREIGN KEY ([CouponId]) REFERENCES [Coupons] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260722100032_AddCouponToCart', N'10.0.10');

COMMIT;
GO

