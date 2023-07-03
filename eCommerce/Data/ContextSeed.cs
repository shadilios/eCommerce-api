namespace eCommerce.Data
{
    public class ContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            //if there's no products in the database
            if (!context.Products.Any())
            {
                //seed data here
                //what you can do is have something in a json file and then convert it like this:

                // var productsData = File.ReadAllText("PATH TO JSON HERE");
                // var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                //context.Products.AddRange(products);
            }

            if (!context.Categories.Any())
            {
                // seed data here
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }
    }
}
