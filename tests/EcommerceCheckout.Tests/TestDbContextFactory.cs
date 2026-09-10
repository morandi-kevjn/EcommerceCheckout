using EcommerceCheckout.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCheckout.Tests;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }
}