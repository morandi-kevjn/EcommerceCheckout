INSERT INTO EcommerceCheckoutDb.dbo.CartItems (CartId,ProductId,Quantity,UnitPrice) VALUES
	 (7,4,4,50.00);
INSERT INTO EcommerceCheckoutDb.dbo.Carts (CartToken,CustomerId,Status,CreatedAt,UpdatedAt,CouponId) VALUES
	 (N'2CD906CD-EF84-4C70-889A-2E28E505355F',NULL,1,'2026-07-21 13:18:21.5559460',NULL,NULL),
	 (N'9EF7B1D8-3779-49C1-A0B8-9D622A461932',NULL,1,'2026-07-21 13:18:48.3723640',NULL,NULL),
	 (N'20EDC12C-6C7A-461F-B5F9-F7B23312CD73',NULL,1,'2026-07-21 13:18:49.5542940',NULL,NULL),
	 (N'06F3C684-3BEB-4F2A-AFD6-84D4592CE955',NULL,1,'2026-07-21 13:18:50.2119760',NULL,NULL),
	 (N'888C2874-7B2C-4607-B315-17D3C4E7CAB6',NULL,1,'2026-07-21 13:18:53.8366900',NULL,1),
	 (N'0D9CF76B-07BB-4D70-A21B-F4C4FC8D9F5A',NULL,1,'2026-07-21 13:21:32.7181650',NULL,NULL),
	 (N'25803973-A6D6-4EFC-870F-7B4224B08924',NULL,2,'2026-07-23 08:58:25.5584590',NULL,1);
INSERT INTO EcommerceCheckoutDb.dbo.Coupons (Code,Active,DiscountType,DiscountValue,MinPrice,MaxPrice,ValidFrom,ValidTo,UsageLimit,UsedCount,CreatedAt,UpdatedAt) VALUES
	 (N'WELCOME10',0,1,10.00,0.00,NULL,NULL,NULL,NULL,0,'2026-07-21 13:44:11.1426990',NULL);
INSERT INTO EcommerceCheckoutDb.dbo.Customers (FirstName,LastName,Email,Nation,NewsletterOptIn,RequiresInvoice,FiscalTaxNumber,FiscalCodeNumber,PrivacyAcceptedAt,CreatedAt,UpdatedAt) VALUES
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',0,0,NULL,NULL,'2026-07-23 10:24:22.9776480','2026-07-23 10:24:22.9775250',NULL),
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',0,0,NULL,NULL,'2026-07-23 14:52:05.5416970','2026-07-23 14:52:05.5416950',NULL),
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',0,0,NULL,NULL,'2026-07-23 14:57:20.3321360','2026-07-23 14:57:20.3321330',NULL),
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',0,0,NULL,NULL,'2026-07-23 14:59:05.2360920','2026-07-23 14:59:05.2359550',NULL),
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',0,0,NULL,NULL,'2026-07-27 13:52:41.6900650','2026-07-27 13:52:41.6898750',NULL),
	 (N'Kevjn',N'Morandi',N'kevjn.morandi@gmail.com',N'IT',1,0,NULL,NULL,'2026-07-27 13:56:08.5197590','2026-07-27 13:56:08.5196340',NULL);
INSERT INTO EcommerceCheckoutDb.dbo.OrderItems (OrderId,ProductId,ProductName,UnitPrice,Quantity,LineTotal) VALUES
	 (1,4,N'T-Shirt Test',50.00,4,200.00),
	 (2,4,N'T-Shirt Test',50.00,4,200.00),
	 (3,4,N'T-Shirt Test',50.00,4,200.00),
	 (4,4,N'T-Shirt Test',50.00,4,200.00),
	 (1002,4,N'T-Shirt Test',50.00,4,200.00),
	 (1003,4,N'T-Shirt Test',50.00,4,200.00);
INSERT INTO EcommerceCheckoutDb.dbo.Orders (OrderNumber,CustomerId,CartId,CouponId,SubtotalAmount,DiscountAmount,TotalAmount,Currency,Status,PaymentProvider,CreatedAt,PaidAt) VALUES
	 (N'ORD-20260723102422',1,7,1,200.00,20.00,200.00,N'EUR',1,2,'2026-07-23 10:24:22.9989060',NULL),
	 (N'ORD-20260723145205',2,7,1,200.00,20.00,180.00,N'EUR',2,1,'2026-07-23 14:52:05.5464360','2026-07-23 14:56:30.7201660'),
	 (N'ORD-20260723145720',3,7,1,200.00,20.00,180.00,N'EUR',1,1,'2026-07-23 14:57:20.3370030',NULL),
	 (N'ORD-20260723145905',4,7,1,200.00,20.00,180.00,N'EUR',2,1,'2026-07-23 14:59:05.2532720','2026-07-23 14:59:34.4441570'),
	 (N'ORD-20260727135241',1002,7,1,200.00,20.00,180.00,N'EUR',1,1,'2026-07-27 13:52:41.7157740',NULL),
	 (N'ORD-20260727135608',1003,7,1,200.00,20.00,180.00,N'EUR',2,1,'2026-07-27 13:56:08.5391810','2026-07-27 13:57:10.1455830');
INSERT INTO EcommerceCheckoutDb.dbo.Products (Name,SKU,Description,Price,StockQty,IsAvailable,Active,RowVersion,CreatedAt,UpdatedAt) VALUES
	 (N'T-Shirt Test',NULL,NULL,19.90,100,0,1,NULL,'2026-07-21 13:46:49.9287050',NULL),
	 (N'Mug Test',NULL,NULL,12.50,50,0,1,NULL,'2026-07-21 13:46:49.9287460',NULL),
	 (N'Hoodie Test',NULL,NULL,44.00,30,0,1,NULL,'2026-07-21 13:46:49.9287460',NULL);
INSERT INTO EcommerceCheckoutDb.dbo.[__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20260720091410_InitialCreate',N'10.0.10'),
	 (N'20260720092708_AddCoupon',N'10.0.10'),
	 (N'20260720093247_AddPublicCoupon',N'10.0.10'),
	 (N'20260720122510_AddCustomer',N'10.0.10'),
	 (N'20260720123232_AddConstraints',N'10.0.10'),
	 (N'20260720124328_AddCartAndCartItem',N'10.0.10'),
	 (N'20260721114019_AddOrderAndOrderItem',N'10.0.10'),
	 (N'20260721115117_AddPaymentTransaction',N'10.0.10'),
	 (N'20260721115446_AddInvoice',N'10.0.10'),
	 (N'20260721120242_AddCouponProduct',N'10.0.10');
INSERT INTO EcommerceCheckoutDb.dbo.[__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20260722100032_AddCouponToCart',N'10.0.10');
