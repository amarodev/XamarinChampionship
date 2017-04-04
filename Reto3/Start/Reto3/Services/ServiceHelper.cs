using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Reto3.Services
{
    public class ServiceHelper
    {
        private readonly MobileServiceClient _clienteServicio = new MobileServiceClient(@"http://xamarinchampions.azurewebsites.net");
        private IMobileServiceTable<TorneoItem> _torneoItemTable;

        public async Task InsertarEntidad(string direccionCorreo, string reto, string androidId)
        {
            _torneoItemTable = _clienteServicio.GetTable<TorneoItem>();
            await _torneoItemTable.InsertAsync(new TorneoItem
            {
                Email = direccionCorreo, 
                Reto = reto,
                DeviceId = androidId                
            });
        }
    }
}
