using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace GAPI {
    internal static class API {
        public static async Task<T> GetDataAsync<T>(string url) => 
            JsonConvert.DeserializeObject<T>( await GetJSONAsync(url));
        public static async Task<string> GetJSONAsync(string url) =>
            await new HttpClient().GetStringAsync(url);
        public static string ToJSON(Object o) => JsonConvert.SerializeObject(o);
    }
}