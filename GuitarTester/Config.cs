using Newtonsoft.Json;
using System.IO;

namespace GuitarTester
{
    internal class Config
    {
        [JsonProperty("controller_name")]
        private string _controllerName = "WUSBMote";

        internal string ControllerName { get { return _controllerName; } }

        public static Config LoadFromFile(string path)
        {
            using var sr = new StreamReader(path);

            return JsonConvert.DeserializeObject<Config>(sr.ReadToEnd());
        }

        public void SaveToFile(string path)
        {
            using var sw = new StreamWriter(path);

            sw.Write(JsonConvert.SerializeObject(this));
        }
    }
}
