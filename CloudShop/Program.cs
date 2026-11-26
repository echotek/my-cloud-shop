using Microsoft.EntityFrameworkCore;
using CloudShop.Data;
using CloudShop.Models;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(connString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connString));
}
else
{
    builder.Services.AddSingleton<List<Product>>(new List<Product>
    {
        new Product { Id = 1, Name = "Ноутбук", Price = 85000 },
        new Product { Id = 2, Name = "Беспроводная мышь", Price = 2500 }
    });
}

var app = builder.Build();

if (!string.IsNullOrEmpty(connString))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// API: Получить список всех товаров
app.MapGet("/api/products", async (IServiceProvider sp) =>
{
    var db = sp.GetService<AppDbContext>();
    return db != null
        ? Results.Ok(await db.Products.ToListAsync())
        : Results.Ok(sp.GetRequiredService<List<Product>>());
});

// API: Добавить новый товар
app.MapPost("/api/products", async (Product product, IServiceProvider sp) =>
{
    var db = sp.GetService<AppDbContext>();
    if (db != null)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Results.Ok(product);
    }
    var list = sp.GetRequiredService<List<Product>>();
    product.Id = list.Count + 1;
    list.Add(product);
    return Results.Ok(product);
});

app.Run();
