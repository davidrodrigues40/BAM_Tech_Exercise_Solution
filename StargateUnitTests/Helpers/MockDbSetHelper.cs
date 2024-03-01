using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateUnitTests.Helpers
{
    internal static class MockDbSetHelper
    {
        public static StargateContext AsMockContext<T>(this IList<T> list) where T : class
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: "StargateDatabase")
                .Options;

            StargateContext context = new(options);
            context.Database.EnsureDeleted();

            context.AddRange(list);
            context.SaveChanges();

            return context;
        }

        public static StargateContext AsMockContext(this IList<Array> lists)
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(databaseName: "StargateDatabase")
                .Options;

            StargateContext context = new(options);
            context.Database.EnsureDeleted();

            foreach (var list in lists)
            {
                context.AddRange(list);
            }
            context.SaveChanges();

            return context;
        }
    }
}
