using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace FinalXamarinChampionship.Services
{
    public class DevItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        public string Text { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Invitacion { get; set; }  
        public string Participacion { get; set; } 
        public string Estacionamiento { get; set; } 
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }


    }
}
