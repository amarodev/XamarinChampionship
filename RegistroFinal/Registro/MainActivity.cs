using System;
using Android.App;
using Android.Widget;
using Android.OS;
using FinalXamarinChampionship.Services;

namespace FinalXamarinChampionship
{
    [Activity(Label = "Registro Final Xamarin Championship", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string Nombre = "Abel";
        private const string Apellido = "Amaro Julian";
        private const string Email = "jumaroz@secret.com";
        private const string CodigoInvitacion = "secret";
        private const int Participacion = 0;
        private readonly string _estacionamiento = "";
        private Button _btnRegistrar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _btnRegistrar = FindViewById<Button>(Resource.Id.btnRegistro);
            _btnRegistrar.Enabled = true;
            _btnRegistrar.Click += btnRegistrar_Click;
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                var serviceHelper = new ServiceHelper();
                var androidId =
                    Android.Provider.Settings.Secure.GetString(ContentResolver,
                        Android.Provider.Settings.Secure.AndroidId);
                _btnRegistrar.Enabled = false;

                if (string.IsNullOrEmpty(Email))
                {
                    Toast.MakeText(this, "Recuerda modificar el código fuente para ingresar tu e-mail", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Enviando tu registro", ToastLength.Short).Show();
                    await serviceHelper.InsertarEntidad(Nombre, Apellido, Email, CodigoInvitacion, Participacion, _estacionamiento, androidId);
                    Toast.MakeText(this, "Gracias por registrarte a la Final", ToastLength.Long).Show();
                }
            }
            catch (Exception exc)
            {
                Toast.MakeText(this, exc.Message, ToastLength.Long).Show();
            }
        }
    }
}