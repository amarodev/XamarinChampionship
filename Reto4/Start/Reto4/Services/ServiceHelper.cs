using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Reto4.Services
{
    public class ServiceHelper
    {
        private readonly MobileServiceClient _clienteServicio = new MobileServiceClient(@"http://xamarinchampions.azurewebsites.net");

        private IMobileServiceTable<TorneoItem> _torneoItemTable;

        public async Task<List<TorneoItem>> SearchRecords(string email)
        {
            _torneoItemTable = _clienteServicio.GetTable<TorneoItem>();
            var items = await _torneoItemTable.Where(item => item.Email == email).ToListAsync();
            return items;
        }

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
