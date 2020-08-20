using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vivaldi.Helpers;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.Icfes;
using Vivaldi.Services;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para SitioControl.xaml
    /// </summary>
    public partial class SitioControl : UserControl
    {
        ApiServiceAutenticacion objUsuarioActivo = new ApiServiceAutenticacion();
        public static SitioControl AppSitio;
        public SitioControl()
        {
            InitializeComponent();
            AppSitio = this;
        }

        public async Task<string> SitioPruebaPorcentaje()
        {
            LoginControl.AppLogin.bar.IsIndeterminate = true;
            cbxSitio.Items.Clear();
            ApiServiceIcfes obj = new ApiServiceIcfes();

            string strJSON = string.Empty;
            strJSON = await obj.ListarSitios();

            if (strJSON != "")
            {
                DatosSitioPrueba.informacion = strJSON;
                //se convierte los resultados en un objeto Json
                DatosIcfesRepositorio.results = JArray.Parse(strJSON) as JArray;
                //Se recorre el content del objeto Json 
                try
                {
                    foreach (var result in DatosIcfesRepositorio.results)
                    {;
                        string id = (string)result["idSitio"] + "," + (string)result["idPrueba"] + "," + (string)result["porcentaje"] + "," + (string)result["nombreSitio"];
                        string nombre = (string)result["idSitio"] + " - " + (string)result["nombreSitio"] + " / " + (string)result["nombrePrueba"];

                        ApiServiceIcfes objEvento = new ApiServiceIcfes();
                        Evento.eventoId = 0;
                        Evento.nombre = "";
                        await objEvento.ConsultarEventoActivo((string)result["idPrueba"]);
                        if (Evento.eventoId != 0)
                        {
                            cbxSitio.Items.Add(new
                            {
                                Id = id,
                                Nombre = nombre,
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    LoginControl.AppLogin.bar.IsIndeterminate = false;
                    MessageBox.Show("Error al obtener los sitios");
                }
            }
            else
            {
                LoginControl.AppLogin.bar.IsIndeterminate = false;
                MessageBox.Show("Error al obtener los sitios");
            }
            LoginControl.AppLogin.bar.IsIndeterminate = false;
            return string.Empty;
        }

        private async void Sitio_SelectClick(object sender, MouseButtonEventArgs e)
        {
            ApiServiceIcfes obj = new ApiServiceIcfes();
            dynamic v = cbxSitio.SelectedItem;
            string[] datosValue = v.Id.Split(',');
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                DatosGenerales.validacionIngreso = "servicio";
                Authentication resultValidarUsuario = await objUsuarioActivo.ValidarUsuarioActivo(DatosGenerales.codUsuario);
                if (resultValidarUsuario.UsuarioActivo != "true")
                {
                    DatosIcfesRepositorio.idSitio = datosValue[0];
                    DatosIcfesRepositorio.idPrueba = datosValue[1];
                    string strJSON = string.Empty;
                    strJSON = obj.ConsultarEventos(DatosIcfesRepositorio.idPrueba);

                    if (strJSON != "")
                    {
                        JObject results = JObject.Parse(strJSON);
                        double porcentaje_eventos = ((double)Convert.ToInt32(datosValue[2])) / ((Newtonsoft.Json.Linq.JContainer)results["eventos"]).Count;
                        int porcentaje_prueba = Convert.ToInt32(Math.Floor(porcentaje_eventos));
                        if (porcentaje_prueba == 0)
                        {
                            porcentaje_prueba = 1;
                        }
                        DatosIcfesRepositorio.porcentaje = Convert.ToString(porcentaje_prueba);
                        DatosIcfesRepositorio.nombreSitio = datosValue[3];
                        MainWindow.AppMainWindow.lblNombreSitio.Text += datosValue[3] + " - " + datosValue[0];
                        AsistenciaControl.AppAsistencia.NombreSitio(datosValue[3]);
                        MainWindow.AppMainWindow.lblNombreSitio.Visibility = Visibility.Visible;
                        MainWindow.AppMainWindow.imgLogo.Visibility = Visibility.Visible;
                        MainWindow.AppMainWindow.lblTituloLogin.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.sitio.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.opciones.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBox.Show("sesión concurrente", "Advertencia");
                }
            }
            else
            {
                MessageBox.Show("No hay conexión a internet", "Advertencia");
            }
        }
    }
}
