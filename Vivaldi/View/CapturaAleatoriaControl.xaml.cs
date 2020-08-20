namespace Vivaldi.View
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using Vivaldi.Helpers;
    using Vivaldi.Models;
    using Vivaldi.Services;

    /// <summary>
    /// Lógica de interacción para CapturaAleatoriaControl.xaml
    /// </summary>
    public partial class CapturaAleatoriaControl : UserControl
    {
        public static CapturaAleatoriaControl AppCapturaAleatoria;
        FormValidation obj = new FormValidation();
        CreateTreatment doc = new CreateTreatment();
        public CapturaAleatoriaControl()
        {
            InitializeComponent();
            AppCapturaAleatoria = this;
            //Cargarhuellas();
        }

        private void ckbAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (ckbAceptar.IsChecked != false)
            {
                btnAceptarCaptura.IsEnabled = true;
            }
            else
            {
                btnAceptarCaptura.IsEnabled = false;
            }
        }

        public void Cargarhuellas()
        {
            cbxHuella.ItemsSource = null;
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Dictionary<int, string> datos = new Dictionary<int, string>();
            try
            {
                string fileName = appStartPath + @"\\Resources\\InfoDedos.csv";
                StreamReader fp;
                char[] buffer = new char[160];
                int i = 0;
                string texto;
                string[] split = null;
                fp = new StreamReader(fileName, System.Text.Encoding.Default, false);
                if (fp != null)
                {
                    i = 0;
                    do
                    {
                        texto = fp.ReadLine();
                        i++;
                        if (texto != null)
                        {
                            if (texto.Length > 2)
                            {
                                split = texto.Split(';');
                                datos.Add(Convert.ToInt32(split[0]), split[1]);
                            }
                        }
                    } while (!fp.EndOfStream);
                }
                cbxHuella.ItemsSource = datos;
                cbxHuella.DisplayMemberPath = "Value";
                cbxHuella.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        private async void btnAceptarCaptura_Click(object sender, RoutedEventArgs e)
        {
            tbxErrorCaptura.Text = string.Empty;
            Response result = obj.FormCapturarDatos(cbxHuella.Text);

            if (result.IsSuccess)
            {
                Response resultTratamiento = await doc.DataTreatment(lblNombreTipoDocumento.Text, lblNumeroIdentificacion.Text);
                if (!resultTratamiento.IsSuccess)
                {
                    tbxErrorCaptura.Text = resultTratamiento.Message;
                }
            }
            else
            {
                tbxErrorCaptura.Text = result.Message;
            }
        }
    }
}
