using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace FinalXamarinChampionship.Services
{
    public class ServiceHelper
    {
        private readonly MobileServiceClient _clienteServicio =
            new MobileServiceClient(@"http://registrofinalxamarinchampions.azurewebsites.net");

        private IMobileServiceTable<DevItem> _devItemTable;

        public async Task InsertarEntidad(
            string nombre, string apellido, string email, string invitacion,
            int participacion, string estacionamiento, string androidId)
        {
            _devItemTable = _clienteServicio.GetTable<DevItem>();
            var registroParticipacion = "Remoto";

            if (participacion == 1)
            {
                registroParticipacion = "Presencial";
            }

            try
            {
                await _devItemTable.InsertAsync(new DevItem
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Invitacion = invitacion,
                    Participacion = registroParticipacion,
                    Estacionamiento = estacionamiento,
                    Text = androidId
                });
            }
            catch (System.Exception)
            {
                // ignored
            }
        }
    }
}