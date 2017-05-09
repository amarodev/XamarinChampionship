using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Emotions;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reto6
{
    [Activity(Label = "Registrar datos")]
    public class CognitiveActivity : Activity
    {
        private ItemManager _manager;
        private static Stream _streamCopy;
        private string _resultadoEmociones = "Reto6 + MX + Abel Amaro Julian: ";
        private TextView _txtResultado;
        private Button _btnRegistraResultados;
        private Button _btnAnalizaFoto;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            _manager = ItemManager.DefaultManager;
            SetContentView(Resource.Layout.Cognitive);
            var btnCamara = FindViewById<Button>(Resource.Id.btnCamara);
            _btnAnalizaFoto = FindViewById<Button>(Resource.Id.btnAnalizaFoto);
            _btnRegistraResultados = FindViewById<Button>(Resource.Id.btnRegistraResultados);
            _txtResultado = FindViewById<TextView>(Resource.Id.txtOutput);
            _btnRegistraResultados.Visibility = ViewStates.Visible;
            _btnAnalizaFoto.Visibility = ViewStates.Invisible;
            btnCamara.Click += BtnCamara_Click;
            _btnAnalizaFoto.Click += BtnAnalizaFoto_Click;
            _btnRegistraResultados.Click += BtnRegistraResultados_Click;
        }

        private async void BtnRegistraResultados_Click(object sender, EventArgs e)
        {
            _btnRegistraResultados.Visibility = ViewStates.Invisible;
            Toast.MakeText(this, "Registrando tus resultados",  ToastLength.Short).Show();
            
            var registro = new TorneoItem
            {
                DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId),
                Email = "@live.com",
                Reto = _resultadoEmociones
            };
            await _manager.SaveTaskAsync(registro);
            SetResult(Result.Ok, Intent);
        }
        private async void BtnAnalizaFoto_Click(object sender, EventArgs e)
        {
            if (_streamCopy != null)
            {
                _btnAnalizaFoto.Visibility = ViewStates.Invisible;
                Toast.MakeText(this, "Analizando imagen utilizando Cognitive Services", ToastLength.Short).Show();
                Dictionary<string, float> emotions = null;
                try
                {
                    _streamCopy.Seek(0, SeekOrigin.Begin);
                    emotions = await ServiceEmotions.GetEmotions(_streamCopy);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Se ha presentado un error al conectar con los servicios", ToastLength.Short).Show();
                    return;
                }
                var sb = new StringBuilder();
                if (emotions != null)
                {
                    _txtResultado.Text = "---Análisis de Emociones---";
                    sb.AppendLine();
                    foreach (var item in emotions)
                    {
                        var toAdd = item.Key + " : " + item.Value + " ";
                        sb.Append(toAdd);
                    }
                    _txtResultado.Text += sb.ToString();
                    _btnRegistraResultados.Visibility = ViewStates.Visible;
                }
                else _txtResultado.Text = "---No se detectó una cara---";
                _resultadoEmociones += sb.ToString();
            }
            else _txtResultado.Text = "---No has seleccionado una imagen---";
        }
        private async void BtnCamara_Click(object sender, EventArgs e)
        {
            MediaFile file = null;
            try
            {
                file = await ServiceImage.TakePicture();
            }
            catch (Android.OS.OperationCanceledException)
            { }
            SetImageToControl(file);
            _btnAnalizaFoto.Visibility = ViewStates.Visible;
        }
        private void SetImageToControl(MediaFile file)
        {
            if (file == null)
            {
                return;
            }
            ImageView imgImage = FindViewById<ImageView>(Resource.Id.imageViewFoto);
            imgImage.SetImageURI(Android.Net.Uri.Parse(file.Path));
            var stream = file.GetStream();
            _streamCopy = new MemoryStream();
            stream.CopyTo(_streamCopy);
            stream.Seek(0, SeekOrigin.Begin);
            file.Dispose();
        }

    }
}