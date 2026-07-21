using EcommerceCheckout.Web.Data;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceCheckout.Tests;

public class CartServiceTest
{
    private static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetOrCreateCartAsync_creates_new_cart_when_token_not_found()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var sut = new CartService(db);
        var token = Guid.NewGuid();

        // Act
        var cart = await sut.GetOrCreateCartAsync(token);
        
        // Assert
        Assert.NotNull(cart);
        Assert.Equal(token, cart.CartToken);
    }

    [Fact]
    public async Task GetOrCreateCartAsync_returns_cart_when_token_found()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var token = Guid.NewGuid();

        var existingCart = new Cart { CartToken = token };
        db.Carts.Add(existingCart);
        await db.SaveChangesAsync();

        var sut = new CartService(db);
        
        // Act
        var cart = await sut.GetOrCreateCartAsync(token);

        // Assert
        Assert.Equal(existingCart.Id, cart.Id);
    }
    
}