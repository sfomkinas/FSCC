namespace FSCC.Core.Helpers
{
    public static class CommonHelper
    {
        public static T JsonClone<T>(this T obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }

    }
}
