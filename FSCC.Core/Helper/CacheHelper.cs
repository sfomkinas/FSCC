using FSCC.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSCC.Core.Helpers
{
    public static class CacheHelper
    {
        static DateTime? TimeToClearCache = null;

        class CacheValue
        {
            public object Value { get; set; }
            public DateTime? ExpiresAt { get; set; }
            public bool Expired => ExpiresAt.HasValue && ExpiresAt < DateTime.Now;

            public CacheValue(object value, int? expiresInSeconds = null)
            {
                if (expiresInSeconds.HasValue)
                    ExpiresAt = DateTime.Now.AddSeconds(expiresInSeconds.Value);
                Value = value;
            }
        }

        static Dictionary<string, CacheValue> m_cache;
        static Dictionary<string, CacheValue> Cache
        {
            get
            {
                if (m_cache == null)
                    m_cache = new Dictionary<string, CacheValue>();
                return m_cache;
            }
        }

        static public T GetCache<T>(string key, Func<T> defaultValue, int? expiresInSeconds = null)
        {
            if (Cache.ContainsKey(key) && !Cache[key].Expired)
                return CommonHelper.JsonClone((T)Cache[key].Value);

            T result = defaultValue();
            SetCache(key, result, expiresInSeconds);
            return result;
        }

        static public void SetCache<T>(string key, T value, int? expiresInSeconds)
        {
            ClearCache();
            Cache[key] = new CacheValue(value, expiresInSeconds);
        }

        static public void DeleteCache()
        {
            m_cache = null;
        }

        static public void ClearCache()
        {
            //do not clear very often
            if (TimeToClearCache.HasValue && TimeToClearCache > DateTime.Now)
                return;

            try
            {
                Cache
                    .Where(_ => _.Value.Expired)
                    .ToList()
                    .ForEach(_ => Cache.Remove(_.Key));
            }
            catch { }
            TimeToClearCache = DateTime.Now.AddMinutes(1);
        }


        static public Product[] GetProductCache(FSCCContext context)
        {
            string key = "productData";
            return GetCache(key, () =>
            {
                var p = context.Products
                .Include("ProductReviews")
                .Include("ProductKind")
                .ToArray();
                return p;
            });
        }

        static public void SetProductCacheExpired()
        {
            string key = "productData";
            if (Cache.ContainsKey(key))
            {
                Cache[key].ExpiresAt = DateTime.Now.AddSeconds(-2);
            }
        }
    }
}