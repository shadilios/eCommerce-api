using Microsoft.EntityFrameworkCore;

namespace eCommerce.Data.ExtinsionMethods
{
    public static class ExtensionMethods
    {
        public static List<TEntity> MyFunction<TEntity>(this DbSet<TEntity> dbSet, Func<TEntity, bool> predicate) where TEntity : class
        {
            return dbSet.Where(predicate).ToList();
        }
    }
}
