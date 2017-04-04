using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Reto4.Services;

namespace Reto4
{
    [Activity(Label = "Registrar datos")]
    public class RegistroActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registro);
            FindViewById<Button>(Resource.Id.btnConsulta).Click += OnBtnConsultaClick;
        }

        private async void OnBtnConsultaClick(object sender, EventArgs e)
        {
            try
            {
                var email = FindViewById<EditText>(Resource.Id.editTextEmail).Text;
                if (string.IsNullOrWhiteSpace(email))
                {
                    Toast.MakeText(this, "Please type a valid email", ToastLength.Long).Show();
                    return;
                }
                var serviceHelper = new ServiceHelper();
                var androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                var records = await serviceHelper.SearchRecords(email);
                var reto = string.Concat(Intent.GetStringExtra("Reto"), "+", records.Count);

                var txtList = FindViewById<TextView>(Resource.Id.txtList);
                txtList.Text = string.Concat(records.Select(x => x.ToString()));
                if (records.Count < 4)
                {
                    await serviceHelper.InsertarEntidad(email, reto, androidId);
                    Toast.MakeText(this, "Done", ToastLength.Long).Show();
                }
                SetResult(Result.Ok, Intent);

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