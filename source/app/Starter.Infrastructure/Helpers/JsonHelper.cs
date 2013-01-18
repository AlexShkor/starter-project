using Newtonsoft.Json;

namespace DQF.Helpers
{
    public static class JsonHelper
    {
         public static string ToJson(object o)
         {
             return JsonConvert.SerializeObject(o);
         }
    }
}