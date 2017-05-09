using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace Emotions
{
    public partial class ItemManager
    {
        private readonly IMobileServiceTable<TorneoItem> _todoTable;
        private ItemManager()
        {
            this.CurrentClient = new
                MobileServiceClient(@"https://xamarinchampions.azurewebsites.net/");
            this._todoTable = CurrentClient.GetTable<TorneoItem>();
        }
        public static ItemManager DefaultManager { get; private set; } = new ItemManager();

        public MobileServiceClient CurrentClient { get; }

        public async Task SaveTaskAsync(TorneoItem item)
        {
            if (item.Id == null)
            {
                await _todoTable.InsertAsync(item);
            }
            else
            {
                await _todoTable.UpdateAsync(item);
            }
        }
    }
}
