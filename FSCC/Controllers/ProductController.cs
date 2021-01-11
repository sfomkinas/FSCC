using FSCC.Core.Data;
using FSCC.Core.Helper;
using FSCC.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSCC.Controllers
{
    public class ProductController : ApiController
    {
        //[HttpGet]
        public HttpResponseMessage GetList(ProductHelper.ProductFilterJson filterJson)
        {
            try
            {
                using (var context = new FSCCContext())
                {

                    var products = ProductHelper.FilterProducts(CacheHelper.GetProductCache(context), filterJson).Select(_ => ProductHelper.GetProductJson(_));
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = false, result = products });
                }
            }
            catch (Exception)
            {
                // Depending on client side how it should handel error, UI side can parse message and show some kind notification
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, error = true, message = "Error" });
            }
        }

        // GET api/Product/5
        //[HttpGet]
        public HttpResponseMessage Get(long id)
        {
            try
            {
                using (var context = new FSCCContext())
                {

                    var product = CacheHelper.GetProductCache(context).Where(_ => _.ID == id).FirstOrDefault();

                    if (product == null)
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = true, message = "There is no product" });

                    var result = ProductHelper.GetProductJson(product);

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = false, resutl = result });
                }

            }
            catch (Exception)
            {
                // Depending on client side how it should handel error, UI side can parse message and show some kind notification
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, error = true, message = "Error" });
            }

        }

        // GET api/Product/GetProductReviews/5
        //[HttpGet]
        public HttpResponseMessage GetProductReviews(long id)
        {
            try
            {
                using (var context = new FSCCContext())
                {

                    var product = CacheHelper.GetProductCache(context).Where(_ => _.ID == id).FirstOrDefault();

                    if (product == null)
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = true, message = "There is no product" });

                    var result = product.ProductReviews.Select(pr => new ProductHelper.ProductReviewJson
                    {
                        Rating = pr.ProductRating,
                        ReviewDate = pr.ReviewDate,
                        ReviewText = pr.ReviewText
                    })
                    .ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = false, resutl = result });
                }

            }
            catch (Exception e)
            {
                // Depending on client side how it should handel error, UI side can parse message and show some kind notification
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, error = true, message = "Error" });
            }
        }

        // POST api/PostReview
        [HttpPost]
        public HttpResponseMessage PostReview(ProductHelper.AddProducReviewJson json)
        {
            try
            {
                using (var context = new FSCCContext())
                {

                    var product = context.Products.Find(json.ID);

                    if (product == null)
                        return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = true, message = "There is no product" });

                    product.ProductReviews.Add(new ProductReview
                    {
                        ProductRating = json.Review.Rating,
                        ReviewDate = DateTime.Now,
                        ReviewText = json.Review.ReviewText
                    });

                    CacheHelper.SetProductCacheExpired();

                    context.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, error = false });
                }

            }
            catch (Exception)
            {
                // Depending on client side how it should handel error, UI side can parse message and show some kind notification
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, error = true, message = "Error" });
            }
        }

    }
}
