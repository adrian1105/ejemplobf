using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Vivaldi.Data;
using Vivaldi.Helpers;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.Icfes;
using Vivaldi.Services;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para AsistenciaControl.xaml
    /// </summary>
    public partial class AsistenciaControl : UserControl
    {
        ApiServiceAutenticacion objSesion = new ApiServiceAutenticacion();
        ColorAnimation animation = new ColorAnimation(Colors.Yellow, Colors.Orange,
                                           new Duration(TimeSpan.FromSeconds(1)));
        SolidColorBrush brush = new SolidColorBrush(Colors.Yellow);

        public static AsistenciaControl AppAsistencia;
        RowDefinition rowDef1;
        ColumnDefinition colDef1;
        bool posicionP;
        int sizex = 0;
        int sizey = 0;
        int[] matriz;
        int silla = 0;
        bool posicionesBtn;
        Grid salon;

        public AsistenciaControl()
        {
            InitializeComponent();
            AppAsistencia = this;
        }

        public void QuitarPosiciones()
        {
            if (ContenedorSalon.Children.Contains(salon))
            {
                ContenedorSalon.Children.Remove(salon);
                matriz = null;
            }
        }

        private void CrearCuadricula()
        {
            if(cbxTablero.SelectedIndex != 0)
            {
                salon = new Grid();
                ContenedorSalon.Children.Add(salon);
                sizex = Convert.ToInt32(cbxFilas.Text);
                sizey = Convert.ToInt32(cbxColumnas.Text);
                int puestos = Convert.ToInt32(lblNumeroAsistentes.Text);

                if (cbxOrientacion.Text == "Horizontal")
                {
                    double filas = ((double)Convert.ToInt32(puestos)) / (sizey);
                    sizex = Convert.ToInt32(Math.Ceiling(filas));

                    for (int k = 0; k <= sizey + (sizey - 1); k++)
                    {
                        colDef1 = new ColumnDefinition();
                        salon.ColumnDefinitions.Add(colDef1);

                    }

                    for (int k = 0; k < sizex; k++)
                    {
                        rowDef1 = new RowDefinition();
                        salon.RowDefinitions.Add(rowDef1);
                    }
                }
                else
                {
                    double columnas = ((double)Convert.ToInt32(puestos)) / (sizex);
                    sizey = Convert.ToInt32(Math.Ceiling(columnas));

                    for (int k = 0; k <= sizey; k++)
                    {
                        colDef1 = new ColumnDefinition();
                        salon.ColumnDefinitions.Add(colDef1);

                    }

                    for (int k = 0; k < sizex + (sizex - 1); k++)
                    {
                        rowDef1 = new RowDefinition();
                        salon.RowDefinitions.Add(rowDef1);
                    }
                }

                int puestoInicial = 1;
                posicionP = false;
                string puerta = cbxPosicion.Text;
                // Posicion Puerta Inferior Izquierda
                if (puerta == "Inferior Izquierda")
                {
                    if (cbxOrientacion.Text == "Horizontal")
                    {
                        string[,] matrizPosiciones = new string[sizex, sizey + (sizey - 1)];
                        for (int i = 0; i < sizex; i++)
                        {
                            if (posicionP)
                            {
                                for (int j = 0; j < sizey + (sizey - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " → ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = (sizey + (sizey - 1)) - 1; j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " ← ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex, sizey + (sizey - 1));
                    }
                    else
                    {
                        string[,] matrizPosiciones = new string[sizex + (sizex - 1), sizey];
                        for (int i = (sizey - 1); i >= 0; i--)
                        {
                            if (posicionP)
                            {
                                for (int j = (sizex + (sizex - 1) - 1); j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↑ ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j <= (sizex + (sizex - 1) - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↓ ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex + (sizex - 1), sizey);
                    }
                }
                // Posicion Puerta Inferior Derecha
                if (cbxPosicion.Text == "Inferior Derecha")
                {
                    if (cbxOrientacion.Text == "Horizontal")
                    {
                        string[,] matrizPosiciones = new string[sizex, sizey + (sizey - 1)];
                        for (int i = 0; i < sizex; i++)
                        {
                            if (posicionP)
                            {

                                for (int j = (sizey + (sizey - 1)) - 1; j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " ← ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < sizey + (sizey - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " → ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex, sizey + (sizey - 1));
                    }
                    else
                    {
                        string[,] matrizPosiciones = new string[sizex + (sizex - 1), sizey];
                        for (int i = 0; i < sizey; i++)
                        {
                            if (posicionP)
                            {
                                for (int j = (sizex + (sizex - 1) - 1); j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↑ ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j <= (sizex + (sizex - 1) - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↓ ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex + (sizex - 1), sizey);
                    }
                }

                // Posicion Puerta Superior Izquierda
                if (cbxPosicion.Text == "Superior Izquierda")
                {
                    if (cbxOrientacion.Text == "Horizontal")
                    {
                        string[,] matrizPosiciones = new string[sizex, sizey + (sizey - 1)];
                        for (int i = (sizex - 1); i >= 0; i--)
                        {
                            if (posicionP)
                            {
                                for (int j = 0; j < sizey + (sizey - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " → ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = (sizey + (sizey - 1)) - 1; j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " ← ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex, sizey + (sizey - 1));
                    }
                    else
                    {
                        string[,] matrizPosiciones = new string[sizex + (sizex - 1), sizey];
                        for (int i = (sizey - 1); i >= 0; i--)
                        {
                            if (posicionP)
                            {
                                for (int j = 0; j <= (sizex + (sizex - 1) - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↓ ";
                                        }
                                    }
                                }
                            }
                            else
                            {

                                for (int j = (sizex + (sizex - 1) - 1); j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↑ ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex + (sizex - 1), sizey);
                    }
                }

                // Posicion Puerta Superior Derecha
                if (cbxPosicion.Text == "Superior Derecha")
                {
                    if (cbxOrientacion.Text == "Horizontal")
                    {
                        string[,] matrizPosiciones = new string[sizex, sizey + (sizey - 1)];
                        for (int i = (sizex - 1); i >= 0; i--)
                        {
                            if (posicionP)
                            {

                                for (int j = (sizey + (sizey - 1)) - 1; j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " ← ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < sizey + (sizey - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[i, j] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[i, j] = " → ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex, sizey + (sizey - 1));
                    }
                    else
                    {
                        string[,] matrizPosiciones = new string[sizex + (sizex - 1), sizey];
                        for (int i = 0; i < sizey; i++)
                        {
                            if (posicionP)
                            {
                                for (int j = 0; j <= (sizex + (sizex - 1) - 1); j++)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↓ ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = (sizex + (sizex - 1) - 1); j >= 0; j--)
                                {
                                    if (puestoInicial <= puestos)
                                    {
                                        if ((j % 2) == 0)
                                        {
                                            matrizPosiciones[j, i] = Convert.ToString(puestoInicial);
                                            puestoInicial += 1;
                                        }
                                        else
                                        {
                                            matrizPosiciones[j, i] = " ↑ ";
                                        }
                                    }
                                }
                            }
                            posicionP = !posicionP;
                        }
                        pintarPuestos(matrizPosiciones, sizex + (sizex - 1), sizey);
                    }
                }
            }
        }

        private async void BtnPosiciones_Click(object sender, RoutedEventArgs e)
        {
            //Posiciones();
            lblError.Content = "";
            btnPosiciones.IsEnabled = false;

            if (cbxSalon.Text != "" && cbxColumnas.Text != "" && cbxFilas.Text != "" && cbxPosicion.Text != "" && cbxOrientacion.Text != "" && cbxTablero.Text != "")
            {
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
                        await obj.ConsultarEventoActivo(DatosIcfesRepositorio.idPrueba);
                        if (Evento.eventoId != 0)
                        {
                            MessageBoxResult opcion = MessageBox.Show("¿Está seguro que esta es la configuración correcta? Recuerde que luego que inicie la primera toma de muestra no podrá cambiar la configuración de puestos generada.", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (opcion == MessageBoxResult.Yes)
                            {
                                if (Convert.ToInt32(DatosIcfesRepositorio.porcentaje) > 0)
                                {
                                    if (Convert.ToInt32(lblNumeroAsistentes.Text) > 0)
                                    {
                                        bool incorrecto = verificarPosiciones();
                                        if (incorrecto)
                                        {
                                            MessageBox.Show("El número de puestos (filas por columnas) es inferior al número de asistentes. Por favor valide estos valores.");

                                        }
                                        else
                                        {
                                            insertaractualizar();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Número de asistentes invalido para generar la muestra");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Porcentaje no valido para la muestra");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("La hora actual no se encuentra dentro del rango horario de la sesión. Si se encuentra en otra sesión, por favor ingrese a \"Cambiar sitio\" y seleccione el sitio nuevamente.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        //Biometrico.Biometrico.AppWindow.CerrarSesion();
                    }
                }
                else
                {
                    MessageBox.Show("No hay conexión a internet", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                verificarCampos();
            }
            btnPosiciones.IsEnabled = true;
        }

        public void verificarCampos()
        {
            string obligatorio = "Campo obligatorio";

            if (cbxSalon.Text == "")
            {
                lblError.Content = obligatorio;
            }

            if (cbxFilas.Text == "")
            {
                lblError.Content = obligatorio;
            }

            if (cbxColumnas.Text == "")
            {
                lblError.Content = obligatorio;
            }

            if (cbxPosicion.Text == "")
            {
                lblError.Content = obligatorio;
            }

            if (cbxOrientacion.Text == "")
            {
                lblError.Content = obligatorio;
            }

            if (cbxTablero.Text == "")
            {
                lblError.Content = obligatorio;
            }
        }

        public bool verificarPosiciones()
        {
            bool incorrecto = false;

            int columnasxfilas = Convert.ToInt32(cbxColumnas.Text) * Convert.ToInt32(cbxFilas.Text);

            if (lblNumeroAsistentes.Text == "")
            {
                lblNumeroAsistentes.Text = "0";
            }

            if (Convert.ToInt32(lblNumeroAsistentes.Text) > columnasxfilas)
            {
                incorrecto = true;
            }

            return incorrecto;
        }

        public void insertaractualizar()
        {
            SitioPrueba objCI = new SitioPrueba();

            //tabla configuracion_sitio_prueba
            DataTable existeSitioPrueba = objCI.ObtenerInformacionSitioPrueba(DatosIcfesRepositorio.idSitio, DatosIcfesRepositorio.idPrueba, Convert.ToString(Evento.eventoId));
            int id_Configuracion = 0;

            if (existeSitioPrueba.Rows.Count > 0)
            {
                foreach (DataRow dtRow in existeSitioPrueba.Rows)
                {
                    id_Configuracion = Convert.ToInt32(dtRow[0].ToString());
                    ConfiguracionEvento.idEvento = Convert.ToInt32(dtRow[3].ToString());
                }
            }
            else
            {
                objCI.InsertarInformacionSitioPrueba(DatosIcfesRepositorio.idSitio, DatosIcfesRepositorio.idPrueba, DatosIcfesRepositorio.porcentaje, Convert.ToString(Evento.eventoId));
                DataTable informacionSitioPrueba = objCI.ObtenerInformacionSitioPrueba(DatosIcfesRepositorio.idSitio, DatosIcfesRepositorio.idPrueba, Convert.ToString(Evento.eventoId));
                foreach (DataRow dtRow in informacionSitioPrueba.Rows)
                {
                    id_Configuracion = Convert.ToInt32(dtRow[0].ToString());
                    ConfiguracionEvento.idEvento = Convert.ToInt32(dtRow[3].ToString());
                }
            }

            //Salon            
            int id_salon = 0;
            DataTable existesalon = objCI.ObtenerSalon(id_Configuracion, cbxSalon.Text);
            if (existesalon.Rows.Count > 0)
            {
                foreach (DataRow dtRow in existesalon.Rows)
                {
                    id_salon = Convert.ToInt32(dtRow[0].ToString());
                }

                // Actualizar Salon
                objCI.ActualizarSalon(id_salon, id_Configuracion, Convert.ToInt32(cbxFilas.Text), Convert.ToInt32(cbxColumnas.Text), Convert.ToInt32(lblNumeroAsistentes.Text), cbxPosicion.Text, cbxOrientacion.Text, cbxTablero.Text);
                //BorrarPuestos
                objCI.BorrarPuestos(id_salon);
                // Guardar Informacion
                NumeroSalon.idSalon = id_salon;
                NumeroSalon.idConfiguracion = id_Configuracion;
                NumeroSalon.numero_salon = cbxSalon.Text;
                NumeroSalon.filas = Convert.ToInt32(cbxFilas.Text);
                NumeroSalon.columnas = Convert.ToInt32(cbxColumnas.Text);
                NumeroSalon.numero_asistentes = Convert.ToInt32(lblNumeroAsistentes.Text);
                NumeroSalon.posicion_puerta = cbxPosicion.Text;
                NumeroSalon.orientacion = cbxOrientacion.Text;
                NumeroSalon.tablero = cbxTablero.Text;

                //Puestos
                Posiciones();
                // Si existe, borrar cuadricula para la actualizacion
                QuitarPosiciones();
                //// Pintar puestos
                informacionsitioprueba();
            }
            else
            {
                objCI.InsertarSalon(id_Configuracion, cbxSalon.Text, Convert.ToInt32(cbxFilas.Text), Convert.ToInt32(cbxColumnas.Text), Convert.ToInt32(lblNumeroAsistentes.Text), cbxPosicion.Text, cbxOrientacion.Text, cbxTablero.Text);
                DataTable salon = objCI.ObtenerSalon(id_Configuracion, cbxSalon.Text);
                foreach (DataRow dtRow in salon.Rows)
                {
                    id_salon = Convert.ToInt32(dtRow[0].ToString());
                }
                // Guardar Informacion
                NumeroSalon.idSalon = id_salon;
                NumeroSalon.idConfiguracion = id_Configuracion;
                NumeroSalon.numero_salon = cbxSalon.Text;
                NumeroSalon.filas = Convert.ToInt32(cbxFilas.Text);
                NumeroSalon.columnas = Convert.ToInt32(cbxColumnas.Text);
                NumeroSalon.numero_asistentes = Convert.ToInt32(lblNumeroAsistentes.Text);
                NumeroSalon.posicion_puerta = cbxPosicion.Text;
                NumeroSalon.orientacion = cbxOrientacion.Text;
                NumeroSalon.tablero = cbxTablero.Text;
                //Puestos
                Posiciones();
            }
        }

        public void informacionsitioprueba()
        {
            MainWindow.AppMainWindow.asistencia.Visibility = Visibility.Visible;
            cbxSalon.SelectedItem = NumeroSalon.numero_salon;
            cbxColumnas.SelectedItem = Convert.ToString(NumeroSalon.columnas);
            cbxFilas.SelectedItem = Convert.ToString(NumeroSalon.filas);
            lblNumeroAsistentes.Text = Convert.ToString(NumeroSalon.numero_asistentes);
            cbxPosicion.SelectedItem = NumeroSalon.posicion_puerta;
            cbxOrientacion.SelectedItem = NumeroSalon.orientacion;
            cbxTablero.SelectedItem = NumeroSalon.tablero;
            CrearCuadricula();
            SitioPrueba objCI = new SitioPrueba();
            DataTable puestos = objCI.ObtenerPuestos(NumeroSalon.idSalon);
            DataView dv = puestos.DefaultView;
            dv.Sort = "numero_puesto desc";
            DataTable sortedDT = dv.ToTable();
            int mayor = 0;
            DataView dvfetch = new DataView(sortedDT, "estado = 0", "numero_puesto", DataViewRowState.CurrentRows);
            if (dvfetch.Count != 0)
            {
                mayor = Convert.ToInt16(dvfetch.ToTable().Compute("Max(numero_puesto)", string.Empty));
            }

            //matriz []
            matriz = new int[dvfetch.Count];

            //pintar botones de la muestra
            int i = 0;
            foreach (var btn in salon.Children)
            {
                foreach (DataRow dtRow in sortedDT.Rows)
                {
                    int boton = Convert.ToInt32(((Button)btn).Name);
                    int puesto = Convert.ToInt32(dtRow[2].ToString());
                    int estado = Convert.ToInt32(dtRow[3].ToString());
                    if (boton == puesto)
                    {
                        ((Button)btn).Tag = puesto;
                        if (mayor == puesto)
                        {
                            matriz[i] = puesto;
                            i++;
                            foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + puesto)))
                            {
                                SolidColorBrush brush = new SolidColorBrush(Colors.Yellow);
                                btn1.Background = brush;

                                ColorAnimation animation = new ColorAnimation(Colors.Yellow, Colors.Orange,
                                               new Duration(TimeSpan.FromSeconds(1)));

                                animation.AutoReverse = true;
                                animation.RepeatBehavior = RepeatBehavior.Forever;
                                animation.AccelerationRatio = 1.0;
                                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                //btn1.Background = Brushes.Yellow;
                                btn1.IsEnabled = true;
                                btn1.Foreground = Brushes.DarkBlue;
                                btn1.Click += new RoutedEventHandler(Onb2ClickAsync);
                            }
                        }
                        else
                        {
                            if (estado == 0)//sin asistencia
                            {
                                matriz[i] = puesto;
                                i++;
                                ((Button)btn).Background = Brushes.Orange; // Naranja
                            }
                            if (estado == 1)//asistentica tomada - match
                            {
                                ((Button)btn).Background = Brushes.Green; // Verde
                            }
                            if (estado == 2)//assistencia tomada - no match
                            {
                                ((Button)btn).Background = Brushes.Red;// Rojo
                            }
                            if (estado == 3)//omitido
                            {
                                ((Button)btn).Background = Brushes.Brown; // Cafe
                            }
                        }
                    }
                }
            }
        }

        public void pintarPuestos(string[,] matrizPosiciones, int filas, int columnas)
        {
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    string puesto = matrizPosiciones[i, j];

                    if (puesto != null)
                    {
                        double Num;

                        bool isNumber = double.TryParse(puesto, out Num);

                        if (isNumber)
                        {
                            Button MyControl1 = new Button();
                            if (cbxOrientacion.Text != "Horizontal")
                            {
                                Thickness margin = MyControl1.Margin;
                                margin.Right = 10;
                                MyControl1.BorderBrush = Brushes.Transparent;
                                MyControl1.Margin = margin;
                                MyControl1.FontSize = 10;
                            }
                            else
                            {
                                Thickness margin = MyControl1.Margin;
                                margin.Top = 25;
                                MyControl1.Margin = margin;
                                MyControl1.FontSize = 8.5;
                            }

                            MyControl1.Background = Brushes.LightGray;
                            MyControl1.Foreground = Brushes.Black;
                            MyControl1.BorderBrush = Brushes.Transparent;
                            MyControl1.Tag = Convert.ToString(matrizPosiciones[i, j]);
                            MyControl1.Content = Convert.ToString(matrizPosiciones[i, j]);
                            MyControl1.FontWeight = FontWeights.Bold;
                            MyControl1.IsEnabled = true;
                            MyControl1.Name = "button" + Convert.ToString(matrizPosiciones[i, j]);
                            Grid.SetColumn(MyControl1, j);
                            Grid.SetRow(MyControl1, i);
                            salon.Children.Add(MyControl1);
                        }
                        else
                        {
                            Label label = new Label();
                            if (cbxOrientacion.Text != "Horizontal")
                            {
                                Thickness margin = label.Margin;
                                margin.Right = 10;
                                label.Margin = margin;
                            }
                            else
                            {
                                Thickness margin = label.Margin;
                                margin.Top = 30;
                                label.Margin = margin;
                            }
                            label.FontSize = 20;
                            label.Foreground = Brushes.Blue;
                            label.VerticalAlignment = VerticalAlignment.Center;
                            label.HorizontalAlignment = HorizontalAlignment.Center;
                            label.Content = Convert.ToString(matrizPosiciones[i, j]);
                            Grid.SetColumn(label, j);
                            Grid.SetRow(label, i);
                            salon.Children.Add(label);
                        }
                    }
                }
            }
        }

        public async void  Onb2ClickAsync(object sender, RoutedEventArgs e)
        {
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                //validar sesion activa para el usuario
                DatosGenerales.validacionIngreso = "servicio";
                Authentication resultValidarUsuario = await objSesion.ValidarUsuarioActivo(DatosGenerales.codUsuario);
                if (resultValidarUsuario.UsuarioActivo != "true")
                {
                    CapturaAleatoriaControl.AppCapturaAleatoria.btnAceptarCaptura.IsEnabled = false;
                    CapturaHuellasControl.AppHuellas.btnActualizarHuella1.IsEnabled = true;
                    CapturaHuellasControl.AppHuellas.btnCapturarHuella1.IsEnabled = true;
                    CapturaHuellasControl.AppHuellas.btnVerificarHuella.IsEnabled = false;

                    ApiServiceIcfes objE = new ApiServiceIcfes();
                    Evento.eventoId = 0;
                    Evento.nombre = "";
                    await objE.ConsultarEventoActivo(DatosIcfesRepositorio.idPrueba);
                    if (Evento.eventoId != 0)
                    {
                        if (Evento.eventoId == ConfiguracionEvento.idEvento)
                        {
                            bool existe = false;
                            foreach (var type3Resource in Examinandos.results
                            .Where(obj => obj["ubicacionSilla"].Value<string>() == Convert.ToString((sender as Button).Tag)))
                            {
                                string tipoDoc = type3Resource["tipoDocumento"].ToString();
                                Examinandos.citaSNEE = type3Resource["citaSNEE"].ToString();
                                Examinandos.tipoDocumento = type3Resource["tipoDocumento"].ToString();
                                CapturaAleatoriaControl.AppCapturaAleatoria.lblNombreTipoDocumento.Text = Examinandos.tipoDocumento;
                                Examinandos.numeroDocumento = type3Resource["numeroDocumento"].ToString();
                                CapturaAleatoriaControl.AppCapturaAleatoria.lblNumeroIdentificacion.Text = Examinandos.numeroDocumento;
                                Examinandos.autorizacion = type3Resource["autorizacion"].ToString();
                                Examinandos.nombreCompleto = type3Resource["nombreCompleto"].ToString();
                                Examinandos.prisma = type3Resource["numeroAutorizacion"].ToString();
                                CapturaAleatoriaControl.AppCapturaAleatoria.lblNombresCompletos.Text = Examinandos.nombreCompleto;
                                existe = true;
                            }

                            if (!existe)
                            {
                                MessageBox.Show("No existe información asociada al número de silla seleccionado por la muestra.", "Advertencia");
                                //ControlAsistencia.AppWindowAsistencia.VerificarAsistencia(3);
                                return;
                            }

                            //1 con autorización, 0 - sin autorización
                            if ((Examinandos.autorizacion == "1" && (Examinandos.tipoDocumento.Equals("TI"))) || Examinandos.tipoDocumento.Equals("CC"))
                            {
                                CapturaAleatoriaControl.AppCapturaAleatoria.Cargarhuellas();
                                //CapturaInformacion.AppWindowInfo.CargarMotivos();

                                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Visible;
                                MainWindow.AppMainWindow.capturaAleatoria.Visibility = Visibility.Visible;

                                silla = Convert.ToInt32((sender as Button).Tag);

                                if (existe == true)
                                {
                                    CapturaAleatoriaControl.AppCapturaAleatoria.ckbAceptar.IsChecked = false;
                                    CapturaAleatoriaControl.AppCapturaAleatoria.cbxHuella.IsEnabled = true;
                                    CapturaAleatoriaControl.AppCapturaAleatoria.btnAceptarCaptura.IsEnabled = false;

                                }
                                else
                                {
                                    CapturaAleatoriaControl.AppCapturaAleatoria.ckbAceptar.IsEnabled = false;
                                    CapturaAleatoriaControl.AppCapturaAleatoria.cbxHuella.IsEnabled = false;
                                    CapturaAleatoriaControl.AppCapturaAleatoria.btnAceptarCaptura.IsEnabled = false;
                                    CapturaAleatoriaControl.AppCapturaAleatoria.tbxErrorCaptura.Text = "No existe un examinando asignado al número de silla.";
                                }
                                DatosIcfesRepositorio.destinoSolicitud = "muestra";
                                return;
                            }
                            else if (!(Examinandos.tipoDocumento.Equals("TI")
                                || Examinandos.tipoDocumento.Equals("CC")))
                            {
                                //DatosIcfesRepositorio.destinoSolicitud = "cancelar";
                                //CapturaInformacion.AppWindowInfo.Cargarhuellas();
                                //CapturaInformacion.AppWindowInfo.CargarMotivos();
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.Location = new Point(585, 193);
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.Visible = true;
                                //Biometrico.Biometrico.AppWindow.modal.Visible = true;
                                //btnVolver.Enabled = false;
                                //System.Drawing.Image bkg = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-volver-inactivo.png");
                                //btnVolver.BackgroundImage = bkg;
                                //btnPosiciones.Enabled = false;
                                //System.Drawing.Image bkgP = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-generar-muestra-inactivo.png");
                                //btnPosiciones.BackgroundImage = bkgP;

                                //Image bkgPTr = Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-aceptar-inactivo.png");
                                ////Biometrico.Biometrico.AppWindow.tratamiento.btnConfirmarTratamiento.BackgroundImage = bkgPTr;

                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnAceptar.Enabled = false;
                                //Image bkgCaptura = Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-aceptar-inactivo.png");
                                //CapturaInformacion.AppWindowInfo.btnAceptar.Image = bkgCaptura;

                                //Biometrico.Biometrico.AppWindow.controlAsistencia.Enabled = false;

                                //CapturaInformacion.AppWindowInfo.lblTituloExaminando.Visible = true;
                                //CapturaInformacion.AppWindowInfo.lblTituloCancelacion.Visible = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.cbxMotivo.Visible = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblMotivo.Visible = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnAceptarMotivo.Visible = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.txbObservacion.Text = "";
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.cbxMotivo.SelectedIndex = 0;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.txbObservacion.Visible = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblExaminando.Text = "";

                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnOmitir.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblTipoDocumento.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblNombreTipoDocumento.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblNumeroIdentificacion.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.cbxHuella.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblCedula.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblNombres.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblTituloTratamiento.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblNombresCompletos.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblHuella.Visible = true;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnAceptar.Visible = true;

                                //silla = Convert.ToInt32((sender as Button).Text);
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.cbxHuella.Enabled = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnAceptar.Enabled = false;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.ckbAceptar.Enabled = false;
                                //Image bkgI = Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-aceptar-inactivo.png");
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.btnAceptar.Image = bkgI;
                                //Biometrico.Biometrico.AppWindow.capturaInformacion.lblExaminando.Text = "El tipo de documento no aplica para la verificación.";
                                return;
                            }
                            else
                            {
                                MessageBox.Show("El examinando no cuenta con la autorización de padres para realizar la verificación biométrica. Por tanto se omite este procedimiento", "Advertencia");
                                VerificarAsistencia(3);
                                await objE.InsertarReporte("no Autorizado");
                                return;
                            }
                        }
                        else
                        {
                            //Biometrico.Biometrico.AppWindow.CambiarSitio();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("La hora actual no se encuentra dentro del rango horario de la sesión. Si se encuentra en otra sesión, por favor ingrese a \"Cambiar sitio\" y seleccione el sitio nuevamente.", "Advertencia");
                        return;
                    }
                }
                else
                {
                  //  Biometrico.Biometrico.AppWindow.CerrarSesion();
                    return;
                }
            }
            else
            {
                MessageBox.Show("No hay conexión a internet", "Advertencia");
                return;
            }
        }

        public void Posiciones()
        {
            int cantidad = Convert.ToInt32(lblNumeroAsistentes.Text);
            double porcentaje = ((double)Convert.ToInt32(DatosIcfesRepositorio.porcentaje) / (double)100) * cantidad;
            int muestra = Convert.ToInt32(Math.Floor(porcentaje));
            if (muestra == 0)
            {
                muestra = 1;
            }
            matriz = new int[muestra];
            int number = 0;
            Random aleatorio = new Random();

            //Insertar Posiciones
            //ServicioClienteIcfes objCI = new ServicioClienteIcfes();

            if (cantidad == muestra)
            {
                for (int i = 0; i <= muestra - 1; i++)
                {
                    matriz[i] = i + 1;
                }
                Array.Sort(matriz);
                Array.Reverse(matriz);
                //label17.Text = string.Join(", ", matriz);
                int numMayor = matriz[0];
                bool ingresoNumMayor = false;
                for (int i = 0; i < matriz.Length; i++)
                {
                    number = matriz[i];
                    foreach (var btn in salon.Children)
                    {
                        ((Button)btn).Tag = matriz[i];
                        int comparar = Convert.ToInt32(((Button)btn).Tag);
                        if (comparar == numMayor && ingresoNumMayor == false)
                        {
                            //objCI.InsertaPuesto(NumeroSalon.idSalon, comparar, 0);
                            foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + comparar)))
                            {
                                btn1.Background = brush;
                                animation.AutoReverse = true;
                                animation.RepeatBehavior = RepeatBehavior.Forever;
                                animation.AccelerationRatio = 1.0;
                                brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                                //btn1.Background = Brushes.Yellow;
                                btn1.IsEnabled = true;
                                btn1.Foreground = Brushes.DarkBlue;
                                btn1.Click += new RoutedEventHandler(Onb2ClickAsync);
                                ingresoNumMayor = true;
                            }
                            break;
                        }
                        else if (comparar == number)
                        {
                            //objCI.InsertaPuesto(NumeroSalon.idSalon, comparar, 0);
                            foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + comparar)))
                            {
                                btn1.Background = Brushes.Orange;
                                btn1.Foreground = Brushes.WhiteSmoke;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                int randomValue = aleatorio.Next(1, cantidad);
                matriz[0] = randomValue;
                number = matriz[0];

                // if you have your buttons created in a container (groupbox for example)
                foreach (var btn in salon.Children)
                {
                    bool ingreso = false;
                    ((Button)btn).Tag = matriz[0];
                    int comparar = Convert.ToInt32(((Button)btn).Tag);
                    // add goat.png to your resources beforehand
                    // right click your project --> properties --> resources --> add resource --> add existing file --> select goat.png, and rename it to GoatImg
                    if (comparar == number)
                    {
                        //objCI.InsertaPuesto(NumeroSalon.idSalon, number, 0);
                        foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + comparar)))
                        {
                            if (ingreso == false)
                            {
                                btn1.Background = Brushes.Orange;
                                btn1.Foreground = Brushes.WhiteSmoke;
                                btn1.IsEnabled = true;
                                ingreso = true;
                            }
                        }
                        break;
                    }
                }

                // Buscamos y asignamos los restantes:
                for (int i = 1; i < muestra; i++)
                {
                    // Mientras el valor generado ya exista en el array, seguimos buscando:
                    while (matriz.Contains(randomValue))
                    {
                        randomValue = aleatorio.Next(1, cantidad);
                    }
                    // Asignamos valor:
                    matriz[i] = randomValue;
                    number = matriz[i];

                    // if you have your buttons created in a container (groupbox for example)
                    foreach (var btn in salon.Children)
                    {
                        bool ingreso = false;
                        ((Button)btn).Tag = matriz[i];
                        int comparar = Convert.ToInt32(((Button)btn).Tag);
                        // add goat.png to your resources beforehand
                        // right click your project --> properties --> resources --> add resource --> add existing file --> select goat.png, and rename it to GoatImg
                        if (comparar == number)
                        {
                            //objCI.InsertaPuesto(NumeroSalon.idSalon, number, 0);
                            foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + comparar)))
                            {
                                if (ingreso == false)
                                {
                                    btn1.Background = Brushes.Orange;
                                    btn1.Foreground = Brushes.WhiteSmoke;
                                    btn1.IsEnabled = true;
                                    ingreso = true;
                                }
                            }
                            break;
                        }
                    }
                }

                Array.Sort(matriz);
                Array.Reverse(matriz);
                //label17.Text = string.Join(", ", matriz);
                int numMayor = matriz[0];
                bool ingresoNumMayor = false;

                foreach (var btn in salon.Children)
                {
                    ((Button)btn).Tag = numMayor;
                    int comparar = Convert.ToInt32(((Button)btn).Tag);
                    if (comparar == numMayor && ingresoNumMayor == false)
                    {
                        foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + comparar)))
                        {
                            btn1.Background = brush;
                            animation.AutoReverse = true;
                            animation.RepeatBehavior = RepeatBehavior.Forever;
                            animation.AccelerationRatio = 1.0;
                            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                            //btn1.Background = Brushes.Yellow;
                            btn1.IsEnabled = true;
                            btn1.Foreground = Brushes.DarkBlue;
                            btn1.Click += new RoutedEventHandler(Onb2ClickAsync);
                            ingresoNumMayor = true;
                        }
                        break;
                    }
                }
            }
        }

        private void cbxPosicion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string posicion = cbxPosicion.SelectedIndex.ToString();
            if (posicion == "4")
            {
                pbSuperiorDerecho.Visibility = Visibility.Hidden;
                pbSuperiorIzquierdo.Visibility = Visibility.Visible;
                pbSuperiorIzquierdo.Focus();
                pbInferiorDerecho.Visibility = Visibility.Hidden;
                pbInferiorIzquierdo.Visibility = Visibility.Hidden;
                cbxOrientacion.IsEnabled = true;
            }
            if (posicion == "3")
            {
                pbSuperiorDerecho.Visibility = Visibility.Visible;
                pbSuperiorDerecho.Focus();
                pbSuperiorIzquierdo.Visibility = Visibility.Hidden;
                pbInferiorDerecho.Visibility = Visibility.Hidden;
                pbInferiorIzquierdo.Visibility = Visibility.Hidden;
                cbxOrientacion.IsEnabled = true;
            }
            if (posicion == "2")
            {
                pbSuperiorDerecho.Visibility = Visibility.Hidden;
                pbSuperiorIzquierdo.Visibility = Visibility.Hidden;
                pbInferiorDerecho.Visibility = Visibility.Hidden;
                pbInferiorIzquierdo.Visibility = Visibility.Visible;
                pbInferiorIzquierdo.Focus();
                cbxOrientacion.IsEnabled = true;
            }
            if (posicion == "1")
            {
                pbSuperiorDerecho.Visibility = Visibility.Hidden;
                pbSuperiorIzquierdo.Visibility = Visibility.Hidden;
                pbInferiorDerecho.Visibility = Visibility.Visible;
                pbInferiorDerecho.Focus();
                pbInferiorIzquierdo.Visibility = Visibility.Hidden;
                cbxOrientacion.IsEnabled = true;
            }
            if (posicion == "0")
            {
                pbSuperiorDerecho.Visibility = Visibility.Hidden;
                pbSuperiorIzquierdo.Visibility = Visibility.Hidden;
                pbInferiorDerecho.Visibility = Visibility.Hidden;
                pbInferiorIzquierdo.Visibility = Visibility.Hidden;
                cbxOrientacion.IsEnabled = false;
            }
            posicion = "";
        }

        private void cbxOrientacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxTablero.Text != "0")
            {
                pbTableroDerecho.Visibility = Visibility.Hidden;
                pbTableroInferior.Visibility = Visibility.Hidden;
                pbTableroIzquierdo.Visibility = Visibility.Hidden;
                pbTableroSuperior.Visibility = Visibility.Hidden;
                cbxTablero.SelectedIndex = 0;
                cbxTablero.IsEnabled = true;
            }
            else
            {
                cbxTablero.IsEnabled = false;
            }
        }

        private void cbxTablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            bool incorrecto = verificarPosiciones();
            if (incorrecto)
            {
                MessageBox.Show("El número de puestos (filas por columnas) es inferior al número de asistentes. Por favor valide estos valores.");

            }
            else
            {
                QuitarPosiciones();
                CrearCuadricula();

                string posicion = cbxTablero.SelectedIndex.ToString();
                if (posicion == "3")
                {
                    pbTableroDerecho.Visibility = Visibility.Visible;
                    pbTableroDerecho.Focus();
                    pbTableroInferior.Visibility = Visibility.Hidden;
                    pbTableroIzquierdo.Visibility = Visibility.Hidden;
                    pbTableroSuperior.Visibility = Visibility.Hidden;
                    btnPosiciones.IsEnabled = true;
                }
                if (posicion == "1")
                {
                    pbTableroDerecho.Visibility = Visibility.Hidden;
                    pbTableroInferior.Visibility = Visibility.Visible;
                    pbTableroInferior.Focus();
                    pbTableroIzquierdo.Visibility = Visibility.Hidden;
                    pbTableroSuperior.Visibility = Visibility.Hidden;
                    btnPosiciones.IsEnabled = true;
                }
                if (posicion == "4")
                {
                    pbTableroDerecho.Visibility = Visibility.Hidden;
                    pbTableroInferior.Visibility = Visibility.Hidden;
                    pbTableroIzquierdo.Visibility = Visibility.Visible;
                    pbTableroIzquierdo.Focus();
                    pbTableroSuperior.Visibility = Visibility.Hidden;
                    btnPosiciones.IsEnabled = true;
                }
                if (posicion == "2")
                {
                    pbTableroDerecho.Visibility = Visibility.Hidden;
                    pbTableroInferior.Visibility = Visibility.Hidden;
                    pbTableroIzquierdo.Visibility = Visibility.Hidden;
                    pbTableroSuperior.Visibility = Visibility.Visible;
                    pbTableroSuperior.Focus();
                    btnPosiciones.IsEnabled = true;
                }
                if (posicion == "0")
                {
                    QuitarPosiciones();
                    pbTableroDerecho.Visibility = Visibility.Hidden;
                    pbTableroInferior.Visibility = Visibility.Hidden;
                    pbTableroIzquierdo.Visibility = Visibility.Hidden;
                    pbTableroSuperior.Visibility = Visibility.Hidden;
                    btnPosiciones.IsEnabled = false;
                }

                if (posicionesBtn)
                {
                    btnPosiciones.IsEnabled = true;
                }
            }
        }

        public void NombreEvento()
        {
            TimeSpan tsInicial = TimeSpan.Parse(Evento.horaInicial);
            TimeSpan tsFinal = TimeSpan.Parse(Evento.horaFinal);
            lblNombreEvento.Text = Evento.nombre;
            lblHoraEvento.Text = "(" + tsInicial.ToString(@"hh\:mm") + " - " + tsFinal.ToString(@"hh\:mm") + ")";
        }

        public async void salones()
        {
            lblPorcentaje.Text = " " + DatosIcfesRepositorio.porcentaje + "%";
            if (cbxSalon.Items.Count == 0)
            {
                //Se llama al metodo ObtenerSalones
                ApiServiceIcfes obj = new ApiServiceIcfes();
                cbxSalon.ItemsSource = null;
                string strJSON = string.Empty;
                strJSON = await obj.ListarSalones(DatosIcfesRepositorio.idPrueba, DatosIcfesRepositorio.idSitio);

                if (strJSON != "")
                {
                    //Se recorre el content del objeto Json 
                    try
                    {
                        JArray results = JArray.Parse(strJSON);
                        foreach (var result in results)
                        {
                            cbxSalon.Items.Add((string)result["nombreSalon"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los salones" + ex.Message);
                    }
                }
            }
        }

        public void NombreSitio(string nombresitio)
        {
            lblNombreSitio.Text = nombresitio;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AppMainWindow.opciones.Visibility = Visibility.Visible;
            MainWindow.AppMainWindow.lblNombreSitio.Visibility = Visibility.Visible;
            MainWindow.AppMainWindow.imgLogo.Visibility = Visibility.Visible;
            MainWindow.AppMainWindow.asistencia.Visibility = Visibility.Hidden;
        }

        public void limpiarConfiguracion()
        {
            cbxColumnas.SelectedIndex = 0;
            cbxFilas.SelectedIndex = 0;
            lblNumeroAsistentes.Text = "";
            cbxPosicion.SelectedIndex = -1;
            cbxOrientacion.SelectedIndex = 0;
            pbTableroDerecho.Visibility = Visibility.Hidden;
            pbTableroInferior.Visibility = Visibility.Hidden;
            pbTableroIzquierdo.Visibility = Visibility.Hidden;
            pbTableroSuperior.Visibility = Visibility.Hidden;
            cbxTablero.SelectedIndex = 0;
            cbxColumnas.IsEnabled = true;
            cbxFilas.IsEnabled = true;
            cbxPosicion.IsEnabled = true;
            cbxOrientacion.IsEnabled = true;
            cbxTablero.IsEnabled = true;
            btnPosiciones.IsEnabled = false;
            pbSuperiorDerecho.Visibility = Visibility.Hidden;
            pbSuperiorIzquierdo.Visibility = Visibility.Hidden;
            pbInferiorDerecho.Visibility = Visibility.Hidden;
            pbInferiorIzquierdo.Visibility = Visibility.Hidden;
        }

        private async void cbxSalon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string curso = cbxSalon.SelectedItem.ToString();
            barNumeroExaminando.Visibility = Visibility.Visible;
            barNumeroExaminando.IsIndeterminate = true;
            lblNumeroAsistentes.Text = "";
            if (curso != "")
            {
                bool CheckConnection = InternetConnection.IsConnectedToInternet();
                if (CheckConnection == true)
                {
                    //validar sesion activa para el usuario
                    DatosGenerales.validacionIngreso = "servicio";
                    Authentication resultValidarUsuario = await objSesion.ValidarUsuarioActivo(DatosGenerales.codUsuario);
                    if (resultValidarUsuario.UsuarioActivo != "true")
                    {
                        posicionesBtn = true;
                        // Si existe, borrar para generar cuadricula para el nuevo salon
                        QuitarPosiciones();

                        limpiarConfiguracion();
                        
                       //limpiarMensajes();
                        //Se llama al metodo ObtenerExaminandos

                        ApiServiceIcfes obj = new ApiServiceIcfes();
                        string strJSON = string.Empty;
                        strJSON = await obj.ListarExaminandos(DatosIcfesRepositorio.idSitio, curso, DatosIcfesRepositorio.idPrueba);

                        if (strJSON != "" && strJSON != "403")
                        {
                            Examinandos.results = JArray.Parse(strJSON);
                            Examinandos.datos = strJSON;
                            JArray results = JArray.Parse(strJSON);
                            //Se recorre el content del objeto Json 
                            try
                            {
                                DatosIcfesRepositorio.salon = curso;
                                lblNumeroAsistentes.Text = Convert.ToString(results.Count);
                                if (results.Count == 0)
                                {
                                    //lblAsistentes.Text = "No se encontró lista de asistentes en el salón";
                                }
                                lblNumeroAsistentes.Text = Convert.ToString(Examinandos.results.Count);
                            }
                            catch (Exception)
                            {
                                barNumeroExaminando.Visibility = Visibility.Hidden;
                                barNumeroExaminando.IsIndeterminate = false;
                                MessageBox.Show("Error al obtener los examinandos");
                            }
                        }

                        SitioPrueba objCI = new SitioPrueba();
                        Evento.eventoId = 0;
                        Evento.nombre = "";
                        await obj.ConsultarEventoActivo(DatosIcfesRepositorio.idPrueba);
                        //tabla configuracion_sitio_prueba
                        DataTable existeSitioPrueba = objCI.ObtenerInformacionSitioPrueba(DatosIcfesRepositorio.idSitio, DatosIcfesRepositorio.idPrueba, Convert.ToString(Evento.eventoId));
                        if (existeSitioPrueba.Rows.Count > 0)
                        {
                            int id_Configuracion = 0;
                            foreach (DataRow dtRow in existeSitioPrueba.Rows)
                            {
                                id_Configuracion = Convert.ToInt32(dtRow[0].ToString());
                                ConfiguracionEvento.idEvento = Convert.ToInt32(dtRow[3].ToString());
                            }

                            //Salon          
                            DataTable existesalon = objCI.ObtenerSalon(id_Configuracion, cbxSalon.Text);
                            if (existesalon.Rows.Count > 0)
                            {
                                foreach (DataRow dtRow in existesalon.Rows)
                                {
                                    NumeroSalon.idSalon = Convert.ToInt32(dtRow[0].ToString());
                                    NumeroSalon.idConfiguracion = Convert.ToInt32(dtRow[1].ToString());
                                    NumeroSalon.numero_salon = dtRow[2].ToString();
                                    NumeroSalon.filas = Convert.ToInt32(dtRow[3].ToString());
                                    NumeroSalon.columnas = Convert.ToInt32(dtRow[4].ToString());
                                    NumeroSalon.numero_asistentes = Convert.ToInt32(dtRow[5].ToString());
                                    NumeroSalon.posicion_puerta = dtRow[6].ToString();
                                    NumeroSalon.orientacion = dtRow[7].ToString();
                                    NumeroSalon.tablero = dtRow[8].ToString();
                                }

                                DataTable puestos = objCI.ObtenerPuestos(NumeroSalon.idSalon);
                                if (puestos.Rows.Count > 0)
                                {
                                    bool existe_puestos_verificados = objCI.PuestosVerificados(NumeroSalon.idSalon);
                                    if (existe_puestos_verificados)
                                    {
                                        inhabilitarConfiguracion();
                                        posicionesBtn = false;
                                    }
                                    informacionsitioprueba();
                                }
                            }
                        }
                    }
                    else
                    {
                        barNumeroExaminando.Visibility = Visibility.Hidden;
                        barNumeroExaminando.IsIndeterminate = false;
                        MessageBox.Show("No hay conexión a internet", "Advertencia");
                        //Biometrico.Biometrico.AppWindow.CerrarSesion();
                    }
                }
                else
                {
                    barNumeroExaminando.Visibility = Visibility.Hidden;
                    barNumeroExaminando.IsIndeterminate = false;
                    MessageBox.Show("No hay conexión a internet", "Advertencia");
                }
            }
            else
            {
                barNumeroExaminando.Visibility = Visibility.Hidden;
                barNumeroExaminando.IsIndeterminate = false;
                MessageBox.Show("Seleccione un salon");
            }
            barNumeroExaminando.Visibility = Visibility.Hidden;
            barNumeroExaminando.IsIndeterminate = false;
        }

        public void inhabilitarConfiguracion()
        {
            cbxColumnas.IsEnabled = false;
            cbxFilas.IsEnabled = false;
            cbxPosicion.IsEnabled = false;
            cbxOrientacion.IsEnabled = false;
            cbxTablero.IsEnabled = false;
            btnPosiciones.IsEnabled = false;
        }

        public void VerificarAsistencia(int validar)
        {
            SitioPrueba objCI = new SitioPrueba();

            if (validar == 1)
            {
                inhabilitarConfiguracion();
                Array.Sort(matriz);
                Array.Reverse(matriz);
                int procesado = matriz[0];
                objCI.ActualizarPuesto(NumeroSalon.idSalon, procesado, 1);
                matriz = matriz.Where(val => val != matriz[0]).ToArray();
                //label17.Text = string.Join(", ", matriz);

                foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + procesado)))
                {
                    ((Button)btn1).Background = Brushes.Green; // Cafe
                    ((Button)btn1).Foreground = Brushes.White;
                    ((Button)btn1).Click -= new RoutedEventHandler(Onb2ClickAsync);
                }
            }
            else if (validar == 2)
            {
                inhabilitarConfiguracion();

                Array.Sort(matriz);
                Array.Reverse(matriz);
                int procesado = matriz[0];
                objCI.ActualizarPuesto(NumeroSalon.idSalon, procesado, 2);
                matriz = matriz.Where(val => val != matriz[0]).ToArray();
                //label17.Text = string.Join(", ", matriz);

                foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + procesado)))
                {
                    ((Button)btn1).Background = Brushes.Red; // Cafe
                    ((Button)btn1).Foreground = Brushes.White;
                    ((Button)btn1).Click -= new RoutedEventHandler(Onb2ClickAsync);
                }
            }

            else if (validar == 3)
            {

                inhabilitarConfiguracion();

                Array.Sort(matriz);
                Array.Reverse(matriz);
                int procesado = matriz[0];
                objCI.ActualizarPuesto(NumeroSalon.idSalon, procesado, 3);
                matriz = matriz.Where(val => val != matriz[0]).ToArray();
                //label17.Text = string.Join(", ", matriz);
                
                foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + procesado)))
                {
                    //SolidColorBrush brush = new SolidColorBrush(Colors.Brown);
                    //((Button)btn).Background = brush;
                    ((Button)btn1).Background = Brushes.SaddleBrown; // Cafe
                    ((Button)btn1).Foreground = Brushes.White;
                    ((Button)btn1).Click -= new RoutedEventHandler(Onb2ClickAsync);
                }
            }


            if (matriz.Length != 0)
            {
                Array.Sort(matriz);
                Array.Reverse(matriz);
                int numMayor = matriz[0];

                foreach (var btn1 in salon.Children.OfType<Button>().Where(x => x.Name == ("button" + numMayor)))
                {
                    btn1.Background = brush;

                    animation.AutoReverse = true;
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    animation.AccelerationRatio = 1.0;
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                    //btn1.Background = Brushes.Yellow;
                    btn1.Foreground = Brushes.DarkBlue;
                    btn1.Click += new RoutedEventHandler(Onb2ClickAsync);
                }
            }
            else
            {
                MessageBox.Show("Ha finalizado la verificación de asistencia");
                btnVolver.IsEnabled = true;
                cbxSalon.IsEnabled = true;
            }
        }

        private void lblNumeroAsistentes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblNumeroAsistentes.Text != "")
            {
                cbxPosicion.IsEditable = true;
            }
            else
            {
                cbxPosicion.IsEditable = false;
            }
        }
    }
}
