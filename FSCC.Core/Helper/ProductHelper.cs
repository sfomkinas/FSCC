using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSCC.Core.Data;

namespace FSCC.Core.Helper
{
    public class ProductHelper
    {
        public class AddProducReviewJson
        {
            public long ID;
            public ProductReviewJson Review;
        }

        public class ProductReviewJson
        {
            public string ReviewText;
            public DateTime? ReviewDate;
            public int Rating;
        }



        public class ProductJson
        {
            public long ID;
            public string ProductTitle;
            public string ProductKind;
            public double? ProductRating;
            public List<ProductReviewJson> ProductReviews;
        }

        public class ProductFilterJson
        {
            public string TextFilter;
            public long? ProductKindID;
            public int? RatingBiggerThen;
            public int? RatingLowerThen;
        }

        public static object GetProductJson(Product product)
        {
            return new ProductJson
            {
                ID = product.ID,
                ProductTitle = product.ProductTitle,
                ProductRating = product.ProductReviews.Average(pr => pr.ProductRating),
                ProductKind = product.ProductKind.Name
            };
        }

        public static IEnumerable<Product> FilterProducts(Product[] product, ProductFilterJson filterJson)
        {
            var q = product;
            if (!string.IsNullOrWhiteSpace(filterJson.TextFilter))
            {
                var filterText = filterJson.TextFilter.ToUpper();
                q = q.Where(_ => _.ProductTitle.ToUpper().Contains(filterText)
                    || _.ProductKind.Name.ToUpper().Contains(filterText))
                    .ToArray();
            }
            if (filterJson.ProductKindID.HasValue)
            {
                q = q.Where(_ => _.ProductKindID == filterJson.ProductKindID.Value).ToArray();
            }

            if (filterJson.RatingBiggerThen.HasValue)
            {
                q = q.Where(_ => _.ProductReviews.Average(pr => pr.ProductRating) >= filterJson.RatingBiggerThen.Value).ToArray();
            }

            if (filterJson.RatingLowerThen.HasValue)
            {
                q = q.Where(_ => _.ProductReviews.Average(pr => pr.ProductRating) <= filterJson.RatingLowerThen.Value).ToArray();
            }

            return q;
        }

    }
}
