using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;

namespace Reto3
{
    [Activity(Label = "Reto 3 - Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _btnSiguiente;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _btnSiguiente = FindViewById<Button>(Resource.Id.btnSiguiente);
            _btnSiguiente.Click += BtnSiguienteClick;
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            var message = !Plugin.Connectivity.CrossConnectivity.Current.IsConnected ? "No hay una conexión disponible" : "Conectado a Internet";
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void BtnSiguienteClick(object sender, EventArgs e)
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, "Conectado a Internet", ToastLength.Long).Show();
                var intent = new Intent(this, typeof(RegistroActivity));
                intent.PutExtra("Reto", "reto3");
                StartActivityForResult(intent, 1);
            }
            else
            {
                Toast.MakeText(this, "No conectado a Internet", ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode != 1 || resultCode != Result.Ok) return;
            _btnSiguiente.Visibility = Android.Views.ViewStates.Invisible;
            Toast.MakeText(this, "Felicidades! Reto 3 completado.", ToastLength.Long).Show();
        }
    }
}

