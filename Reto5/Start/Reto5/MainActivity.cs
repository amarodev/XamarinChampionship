using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Reto5.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Environment = System.Environment;

namespace Reto5
{
    [Activity(Label = "Reto 5 - Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MobileServiceClient _client;
        private IMobileServiceSyncTable<TorneoItem> _torneoItemTable;

        private const string ApplicationUrl = @"http://xamarinchampions.azurewebsites.net";
        private const string LocalDbFilename = "localstore.db";
        private const string EmailParticipante = "jumaroz@live.com";
        private const string Pais = "mx";
        private Button _btnRegistro;
        private Switch _switch;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _client = new MobileServiceClient(ApplicationUrl);
            await InitLocalStoreAsync();

            _torneoItemTable = _client.GetSyncTable<TorneoItem>();
            _btnRegistro = FindViewById<Button>(Resource.Id.BtnRegistro);
            _btnRegistro.Click += BtnSiguienteClick;
            _switch = FindViewById<Switch>(Resource.Id.chkAll);
            _switch.CheckedChange += OnCheckedChange;
            OnRefreshItemsSelected();
        }

        private async void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            await RefreshItemsAsync(_switch.Checked);
        }

        private async void BtnSiguienteClick(object sender, EventArgs e)
        {
            var androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            await _torneoItemTable.InsertAsync(new TorneoItem
            {
                Email = EmailParticipante,
                DeviceId = androidId,
                Reto = "Reto5@" + Pais
            });

        }

        private async Task InitLocalStoreAsync()
        {
            string path = Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.Personal), LocalDbFilename);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<TorneoItem>();
            await _client.SyncContext.InitializeAsync(store);
        }
        private async Task SyncAsync()
        {
            try
            {
                await _client.SyncContext.PushAsync();
                await _torneoItemTable.PullAsync("allTodoItems", _torneoItemTable.CreateQuery()); // query ID is used for incremental sync
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"));
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e);
            }
        }

        private void CreateAndShowDialog(Exception p0)
        {
            Toast.MakeText(this, p0.Message, ToastLength.Long).Show();
        }


        private async Task RefreshItemsAsync(bool all = false)
        {
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                IEnumerable<TorneoItem> resultado = await _torneoItemTable.ToListAsync();
                var textToDisplay = FindViewById<TextView>(Resource.Id.textViewResults).Text + "\n\n";
                if (!all)
                {
                    resultado = resultado.Where(x => x.Email == EmailParticipante);
                }
                textToDisplay = resultado.Aggregate(textToDisplay, (current, t) => current + t.Email + " " + t.Reto + "\n");
                FindViewById<TextView>(Resource.Id.textViewResults).Text = textToDisplay;
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e);
            }
        }

        private async void OnRefreshItemsSelected()
        {
            await SyncAsync();
            await RefreshItemsAsync();
        }
    }
}

