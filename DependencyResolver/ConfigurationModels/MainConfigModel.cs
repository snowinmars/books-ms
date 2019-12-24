using Newtonsoft.Json;

namespace BookService.DependencyResolver.ConfigurationModels
{
    internal sealed class MainConfigModel
    {
        [JsonProperty("dbUsageName")]
        public DatabaseConfigModel MyDatabase { get; set; }

        [JsonProperty("log")]
        public LogConfigModel Log { get; set; }
    }
}
