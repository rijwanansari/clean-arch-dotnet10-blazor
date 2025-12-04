using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using CleanArchitecture.Domain.Enumerators;
using CleanArchitecture.Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            // Apply pending migrations first
            if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }

            await SeedProductsAsync(context, cancellationToken);
            await SeedCustomersAsync(context, cancellationToken);
            await SeedOrdersAsync(context, cancellationToken);
        }

        private static async Task SeedProductsAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (await context.Set<Product>().AnyAsync(cancellationToken)) return;

            var products = new List<Product>
            {
                Product.Create(
                    name: "Laptop Pro 15",
                    description: "High performance laptop",
                    price: Money.Of(1499.00m),
                    stockQuantity: 50,
                    category: "Computers"),
                Product.Create(
                    name: "Wireless Mouse",
                    description: "Ergonomic mouse",
                    price: Money.Of(39.99m),
                    stockQuantity: 200,
                    category: "Accessories"),
                Product.Create(
                    name: "Mechanical Keyboard",
                    description: "RGB backlit",
                    price: Money.Of(89.50m),
                    stockQuantity: 120,
                    category: "Accessories"),
                Product.Create(
                    name: "USB-C Dock",
                    description: "8-in-1 adapter",
                    price: Money.Of(69.00m),
                    stockQuantity: 80,
                    category: "Accessories")
            };

            await context.AddRangeAsync(products, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task SeedCustomersAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (await context.Set<Customer>().AnyAsync(cancellationToken)) return;

            var customers = new List<Customer>
            {
                Customer.Create(
                    firstName: "Alice",
                    lastName: "Johnson",
                    email: Email.Create("alice@example.com"),
                    phoneNumber: "+1-555-0101",
                    address: Address.Create("123 Main St", "Springfield", "IL", "62701", "USA")),
                Customer.Create(
                    firstName: "Bob",
                    lastName: "Smith",
                    email: Email.Create("bob@example.com"),
                    phoneNumber: "+1-555-0102",
                    address: Address.Create("456 Oak Ave", "Springfield", "IL", "62702", "USA")),
                Customer.Create(
                    firstName: "Charlie",
                    lastName: "Brown",
                    email: Email.Create("charlie@example.com"),
                    phoneNumber: "+1-555-0103",
                    address: Address.Create("789 Pine Rd", "Springfield", "IL", "62703", "USA"))
            };

            await context.AddRangeAsync(customers, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private static async Task SeedOrdersAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (await context.Set<Order>().AnyAsync(cancellationToken)) return;

            var firstCustomer = await context.Set<Customer>().FirstOrDefaultAsync(cancellationToken);
            var products = await context.Set<Product>().Take(2).ToListAsync(cancellationToken);
            if (firstCustomer == null || products.Count == 0) return;

            var order = Order.Create(
                customerId: firstCustomer.Id,
                paymentMethod: PaymentMethod.CreditCard,
                shippingAddress: Address.Create("123 Main St", "Springfield", "IL", "62701", "USA")
            );

            order.AddItem(products[0], 1);
            order.AddItem(products[1], 2);

            await context.AddAsync(order, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
