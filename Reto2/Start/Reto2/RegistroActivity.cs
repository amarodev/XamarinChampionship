using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Reto2.Services;

namespace Reto2
{
    [Activity(Label = "Registrar datos")]
    public class RegistroActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registro);
            FindViewById<Button>(Resource.Id.btnRegistro).Click += RegistroActivity_Click;
        }

        private async void RegistroActivity_Click(object sender, EventArgs e)
        {
            try
            {
                var serviceHelper = new ServiceHelper();
                // Retrieve the values the user entered into the UI
                var email = FindViewById<EditText>(Resource.Id.editTextEmail).Text;
                var reto = Intent.GetStringExtra("Reto");
                var androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

                if (string.IsNullOrEmpty(reto)|| string.IsNullOrWhiteSpace(email))
                {
                    Toast.MakeText(this, "Por favor introduce un correo electrónico válido", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Enviando tu registro", ToastLength.Short).Show();
                    await serviceHelper.InsertarEntidad(email, reto, androidId);
                    Toast.MakeText(this, "Gracias por registrarte", ToastLength.Long).Show();
                    SetResult(Result.Ok, Intent);
                }

            }
            catch (Exception exc)
            {
                Toast.MakeText(this, exc.Message, ToastLength.Long).Show();
                SetResult(Result.Canceled, Intent);
            }
            Finish();
        }
    }
}