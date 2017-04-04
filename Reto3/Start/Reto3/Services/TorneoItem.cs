using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Reto3.Services
{
    public class TorneoItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        public string Reto { get; set; }
        public string DeviceId { get; set; }
        [Version]
        public string Version { get; set; }

    }
}
