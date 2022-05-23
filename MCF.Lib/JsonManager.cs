using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MCF.Lib
{
    public static class JsonManager
    {
        public static string Stringify(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T GetJson<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
