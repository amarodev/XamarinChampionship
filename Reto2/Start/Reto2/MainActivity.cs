using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;

namespace Reto2
{
    [Activity(Label = "Reto 2 - Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button _btnSiguiente;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _btnSiguiente = FindViewById<Button>(Resource.Id.btnSiguiente);
            _btnSiguiente.Click += BtnSiguienteClick;
        }

        private void BtnSiguienteClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(RegistroActivity));
            intent.PutExtra("Reto", "reto2");
            StartActivityForResult(intent, 1);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            // El siguiente código se ejecutará si el registro de la actividad fue exitoso 
            //y si la actividad RegistroActivity fue iniciada desde la actividad principal de esta aplicación (código 1)
            if (requestCode != 1 || resultCode != Result.Ok) return;
            _btnSiguiente.Visibility = Android.Views.ViewStates.Invisible;
            Toast.MakeText(this, "Felicidades! Reto 2 completado.", ToastLength.Long).Show();
        }
    }
} 

