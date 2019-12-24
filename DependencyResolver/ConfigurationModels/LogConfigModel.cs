using Newtonsoft.Json;

namespace BookService.DependencyResolver.ConfigurationModels
{
    internal sealed class LogConfigModel
    {
        [JsonProperty("filepath")]
        public string LogFilePath { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }
    }
}
