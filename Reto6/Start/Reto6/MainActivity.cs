using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content;
using Android.Runtime;

namespace Reto6
{
    [Activity(Label = "Reto 6 - Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _btnSiguiente;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            // Obtener una referencia al botón Siguiente
            _btnSiguiente = FindViewById<Button>(Resource.Id.btnSiguiente1);
            // Registrar el manejador de evento click del botón Siguiente
            _btnSiguiente.Click += BtnSiguienteClick;
        }

        private void BtnSiguienteClick(object sender, EventArgs e)
        {
            // Creación de un nuevo Intent para ejecutar la actividad "RegistroActivity"
            var intent = new Intent(this, typeof(CognitiveActivity));
            // Enviaremos el parámetro Reto al momento de iniciar la siguiente actividad
            intent.PutExtra("Reto", "Reto6");
            // Iniciaremos la actividad pasando el ID "1"
            StartActivityForResult(intent, 1);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        { 
            // El siguiente código se ejecutará si el registro de la actividad fue exitoso y si la actividad RegistroActivity fue iniciada desde la actividad principal de esta aplicación (código 1)
            if(requestCode == 1 && resultCode == Result.Ok)
            {
                _btnSiguiente.Visibility = Android.Views.ViewStates.Invisible;
                Toast.MakeText(this, "Felicidades! Reto 6 completado.", ToastLength.Long).Show();
            }
        }
    }
} 

