using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para TableroOpcionControl.xaml
    /// </summary>
    public partial class TableroOpcionControl : UserControl
    {
        ApiServiceAutenticacion objSesion = new ApiServiceAutenticacion();
        public TableroOpcionControl()
        {
            InitializeComponent();
        }

        private async void MuestraAleatoria_Click(object sender, RoutedEventArgs e)
        {
            DatosIcfesRepositorio.destinoSolicitud = "muestra";
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                //validar sesion activa para el usuario
                DatosGenerales.validacionIngreso = "servicio";
                Authentication resultValidarUsuario = await objSesion.ValidarUsuarioActivo(DatosGenerales.codUsuario);
                if (resultValidarUsuario.UsuarioActivo != "true")
                {
                    ApiServiceIcfes obj = new ApiServiceIcfes();
                    Evento.eventoId = 0;
                    Evento.nombre = "";
                    Evento.horaInicial = "";
                    Evento.horaFinal = "";
                    await obj.ConsultarEventoActivo(DatosIcfesRepositorio.idPrueba);
                    if (Evento.eventoId != 0)
                    {
                        MainWindow.AppMainWindow.imgLogo.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.lblNombreSitio.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.opciones.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.asistencia.Visibility = Visibility.Visible;
                        AsistenciaControl.AppAsistencia.NombreEvento();
                        AsistenciaControl.AppAsistencia.salones();
                    }
                    else
                    {
                        MessageBox.Show("La hora actual no se encuentra dentro del rango horario de la sesión.", "Advertencia");
                    }
                }
                else
                {
                    MessageBox.Show("concurrente", "Advertencia");
                }
            }
            else
            {
                MessageBox.Show("No hay conexión a internet", "Advertencia");
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            DatosIcfesRepositorio.destinoSolicitud = "individual";

            MainWindow.AppMainWindow.individual.Visibility = Visibility.Visible;
        }
    }
}
