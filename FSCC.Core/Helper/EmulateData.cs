using FSCC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FSCC.Core.Helper
{
    public class EmulateData
    {
        public static class RandomProvider
        {
            private static int seed = Environment.TickCount;

            private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
                new Random(Interlocked.Increment(ref seed))
            );

            public static Random GetThreadRandom()
            {
                return randomWrapper.Value;
            }
        }

        public static void AddReviews(Product product)
        {
            for (int i = 0; i < 5; i++)
            {
                var randomRating = RandomProvider.GetThreadRandom().Next(0, 10);
                product.ProductReviews.Add(new ProductReview
                {
                    ReviewDate = DateTime.Now,
                    ReviewText = $"Review with random rating {randomRating}",
                    ProductRating = randomRating,
                });
            }
        }

        public static void Emulate()
        {
            using (var context = new FSCCContext())
            {
                List<Product> products = new List<Product>();
                foreach (var symbol in new[] { "A", "B", "C" })
                {
                    var kindName = $"Tipas {symbol}";
                    if (!context.ProductKinds.Where(_ => _.Name == kindName).Any())
                    {
                        context.ProductKinds.Add(new ProductKind { Name = kindName });
                        context.SaveChanges();
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        var productName = $"Produktas {symbol} {i + 1}";
                        if (!context.Products.Where(_ => _.ProductTitle == productName).Any())
                        {
                            var pr = new Product
                            {
                                ProductTitle = productName,
                                ProductKindID = context.ProductKinds.Where(_ => _.Name == kindName).FirstOrDefault().ID,
                            };

                            AddReviews(pr);

                            products.Add(pr);
                        }
                    }
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
