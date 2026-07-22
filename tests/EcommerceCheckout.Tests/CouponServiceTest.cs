using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Tests;

public class CouponServiceTest
{
    private static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task ValidateAndComputeDiscountAsync_percentage_coupon_computes_correct_discount()
    {
        // Arrange
        var db = CreateInMemoryDb();
        db.Coupons.Add(new Coupon()
        {
            Code = "WELLCOME10",
            Active =  true,
            DiscountType = DiscountType.Percentage,
            DiscountValue = 10m,
            MinPrice = 0
        });
        await db.SaveChangesAsync();

        var sut = new CouponService(db);
        
        // Act
        var result = await sut.ValidateAndComputeDiscountAsync(
            "WELLCOME10", 100m);
        
        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(10m, result.DiscountAmount);
    }
    
    [Fact]
    public async Task ValidateAndComputeDiscountAsync_rejects_when_subtotal_below_min_price()
    {
        // Arrange
        var db = CreateInMemoryDb();
        db.Coupons.Add(new Coupon()
        {
            Code = "SAVE5",
            Active = true,
            DiscountType = DiscountType.FixedAmount,
            DiscountValue = 5m,
            MinPrice = 20m
        });
        await db.SaveChangesAsync();

        var sut = new CouponService(db);

        // Act
        var result = await sut.ValidateAndComputeDiscountAsync(
            "SAVE5", 10m);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(0m, result.DiscountAmount);
        Assert.NotNull(result.ErrorMessage);
    }
    
    [Fact]
    public async Task ValidateAndComputeDiscountAsync_fixed_discount_never_exceeds_subtotal()
    {
        // Arrange
        var db = CreateInMemoryDb();
        db.Coupons.Add(new Coupon()
        {
            Code = "FIXEDAMOUNT",
            Active = true,
            DiscountType = DiscountType.FixedAmount,
            DiscountValue = 50m,
            MinPrice = 0
        });
        await db.SaveChangesAsync();

        var sut = new CouponService(db);

        // Act
        var result = await sut.ValidateAndComputeDiscountAsync(
            "FIXEDAMOUNT", 20m);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal(20m, result.DiscountAmount);
        Assert.Null(result.ErrorMessage);
    }
}