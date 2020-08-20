using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Vivaldi.Models.User;
using Vivaldi.Services;
using Path = System.IO.Path;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para CapturaHuellasControl.xaml
    /// </summary>
    public partial class CapturaHuellasControl : UserControl
    {
        System.Windows.Threading.DispatcherTimer tConsulta = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer tEspera = new System.Windows.Threading.DispatcherTimer();
        string numeroNut = "";
        public static CapturaHuellasControl AppHuellas;
        public CapturaHuellasControl()
        {
            InitializeComponent();
            AppHuellas = this;
            tConsulta.Tick += new EventHandler(tConsulta_Tick);
            tEspera.Tick += new EventHandler(tEspera_Tick);
        }

        public void ActualizarHuella1()
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Random rnd = new Random();
            int index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella1.Items.Count);
            string nombre = MainWindow.AppMainWindow.lbHuella1.Items[index].ToString();
            string[] ImgNombre = nombre.Split('-');
            lblDedo1.Content = ImgNombre[1];
            UserRepository.idDedo1 = ImgNombre[0];
            imgDedo1.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
        }

        /// <summary>
        /// Método para aleatorio de huellas
        /// </summary>
        public void ActualizarHuella2()
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Random rnd = new Random();
            int index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella2.Items.Count);
            string nombre = MainWindow.AppMainWindow.lbHuella2.Items[index].ToString();
            string[] ImgNombre = nombre.Split('-');
            lblDedo2.Content = ImgNombre[1];
            UserRepository.idDedo2 = ImgNombre[0];
            imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
        }

        private void btnActualizarHuella1_Click(object sender, RoutedEventArgs e)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Random rnd = new Random();
            string nombre = string.Empty;
            int index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella1.Items.Count);
            int count = MainWindow.AppMainWindow.lbHuella1.Items.Count;

            if (count == 0)
            {
                MainWindow.AppMainWindow.lbHuella1.Items.Add(UserRepository.huellaCedula);
                index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella1.Items.Count);
                nombre = MainWindow.AppMainWindow.lbHuella1.Items[index].ToString();
                string[] ImgNombre = nombre.Split('-');
                lblDedo1.Content = ImgNombre[1];
                UserRepository.idDedo1 = ImgNombre[0];
                imgDedo1.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
                MainWindow.AppMainWindow.lbHuella1.Items.RemoveAt(index);
                MainWindow.AppMainWindow.DedoAleatorio();
                //lblHuella1.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                //pbErrorHuella1.Visible = false;
            }
            else
            {
                nombre = MainWindow.AppMainWindow.lbHuella1.Items[index].ToString();
                string[] ImgNombre = nombre.Split('-');
                lblDedo1.Content = ImgNombre[1];
                UserRepository.idDedo1 = ImgNombre[0];
                MainWindow.AppMainWindow.lbHuella1.Items.RemoveAt(index);
                imgDedo1.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
                //lblHuella1.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                // pbErrorHuella1.Visible = false;
            }
        }

        private void btnActualizarHuella2_Click(object sender, RoutedEventArgs e)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Random rnd = new Random();
            string nombre = string.Empty;
            int index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella2.Items.Count);
            int count = MainWindow.AppMainWindow.lbHuella2.Items.Count;

            if (count == 0)
            {
                if (MainWindow.AppMainWindow.validarHuellaSeleccionada == 1)
                {
                    index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella2.Items.Count);
                    MainWindow.AppMainWindow.Dedo2Aleatorio();
                    nombre = MainWindow.AppMainWindow.lbHuella2.Items[index].ToString();
                    MainWindow.AppMainWindow.lbHuella2.Items.RemoveAt(index);
                    string[] ImgNombre = nombre.Split('-');
                    lblDedo2.Content = ImgNombre[1];
                    UserRepository.idDedo2 = ImgNombre[0];
                    imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
                    //MainWindow.AppMainWindow.lbHuella2.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                    //pbErrorHuella2.Visible = false;
                }
                else
                {
                    MainWindow.AppMainWindow.lbHuella2.Items.Add(UserRepository.huellaCedula);
                    index = rnd.Next(0, MainWindow.AppMainWindow.lbHuella2.Items.Count);
                    nombre = MainWindow.AppMainWindow.lbHuella2.Items[index].ToString();
                    string[] ImgNombre = nombre.Split('-');
                    lblDedo2.Content = ImgNombre[1];
                    UserRepository.idDedo2 = ImgNombre[0];
                    imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));

                    MainWindow.AppMainWindow.lbHuella2.Items.RemoveAt(index);
                    MainWindow.AppMainWindow.Dedo2Aleatorio();
                    //lblHuella2.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                    //pbErrorHuella2.Visible = false;
                }
            }
            else
            {
                nombre = MainWindow.AppMainWindow.lbHuella2.Items[index].ToString();
                string[] ImgNombre = nombre.Split('-');
                lblDedo2.Content = ImgNombre[1];
                UserRepository.idDedo2 = ImgNombre[0];
                MainWindow.AppMainWindow.lbHuella2.Items.RemoveAt(index);
                imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Normal\\" + ImgNombre[1] + ".png"));
                //lblHuella2.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                //pbErrorHuella2.Visible = false;
            }
        }

        private void btnCapturarHuella1_Click(object sender, RoutedEventArgs e)
        {
            CapturarHuella(1);
        }

        private void btnCapturarHuella2_Click(object sender, RoutedEventArgs e)
        {
            CapturarHuella(2);
        }

        public void CapturarHuella(int huella)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.Arguments = "/C " + @"jdk1.8.0_40\bin\java -jar " + "\"" + appStartPath + @"\captura.jar" + "\" " + DatosGenerales.key + " 90" + " " + "0";


            process.Start();

            string output = string.Empty;

            while (process.StandardOutput.Peek() > -1)
            {
                output = process.StandardOutput.ReadLine();

                JObject results = JObject.Parse(Convert.ToString(output));
                if (huella == 1)
                {
                    //this.Invoke((MethodInvoker)delegate
                    //{
                    //    tratamiento.btnCancelarVerificacion.Enabled = true;
                    //});


                    UserRepository.huella1 = results["huella"].ToString();
                    UserRepository.score = Convert.ToInt32(results["score"].ToString());

                    //score = UserRepository.score;
                    //resultScore.Text = Convert.ToString(score);
                    GuardarMinucia(huella, UserRepository.score);
                }
                else
                {
                    //this.Invoke((MethodInvoker)delegate
                    //{
                    //    tratamiento.btnCancelarVerificacion.Enabled = true;
                    //});

                    UserRepository.huella2 = results["huella"].ToString();
                    UserRepository.score = Convert.ToInt32(results["score"].ToString());
                    //score = UsuarioRepositorio.score;
                    //resultScore.Text = Convert.ToString(score);
                    
                    GuardarMinucia(huella, UserRepository.score);
                }
            }

            while (process.StandardError.Peek() > -1)
            {
                output = process.StandardError.ReadLine();
            }
        }


        /// <summary>
        /// Método para guardar temporalmente las minucias de las huellas
        /// enviada desde el dispositivo de lectura
        /// </summary>
        /// <param name="huella"></param>
        private void GuardarMinucia(int huella, int score)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            try
            {
                //numero que identifica el numero de huella
                if (huella == 1)
                {
                    // solo guarda las minucias si el ecore es mayo o igual que el parametrizado
                    if (score >= Convert.ToInt32(DatosGenerales.scoreCaptura))
                    {
                        //timer para borrar las minucias despues de un tiempo si no son verificadas
                        //tLimpiarCampo.Interval = 1000 * 60 * Convert.ToInt32(DatosGenerales.time_huella);
                        //tLimpiarCampo.Start();

                        //tratamiento.pbOkHuella1.Visible = true;
                        //tratamiento.pbErrorHuella1.Visible = false;
                        TreatmentRepository.scoreDedo1 = Convert.ToString(score);

                        btnActualizarHuella1.IsEnabled = false;
                        btnCapturarHuella1.IsEnabled = false;

                        imgDedo1.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Ok\\" + lblDedo1.Content + ".png"));

                        btnActualizarHuella2.IsEnabled = true;
                        btnCapturarHuella2.IsEnabled = true;

                        MainWindow.AppMainWindow.Dedo2Aleatorio();
                        ActualizarHuella2();
                    }
                    else
                    {
                        //this.TopMost = true;
                        MessageBox.Show("La huella no fué leída correctamente. Intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //tratamiento.pbOkHuella1.Visible = false;
                        //tratamiento.pbErrorHuella1.Visible = true;
                        //tratamiento.btnCancelarVerificacion.Enabled = true;
                        btnActualizarHuella1.IsEnabled = true;
                        btnCapturarHuella1.IsEnabled = true;
                        //tratamiento.lblHuella1.ForeColor = ColorTranslator.FromHtml("#E54848");
                        //tratamiento.ptbDedo1.Image = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\Dedos\\Error\\" + tratamiento.lblDedo1.Text + ".png");
                        //this.TopMost = false;
                        imgDedo1.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Error\\Ok\\" + lblDedo1.Content + ".png"));
                    }
                }
                else
                {
                    if (score >= Convert.ToInt32(DatosGenerales.scoreCaptura))
                    {
                        //tratamiento.pbOkHuella2.Visible = true;
                        //tratamiento.pbErrorHuella2.Visible = false;
                        //tratamiento.btnVerificarHuella.Enabled = true;
                        //tratamiento.btnVerificarHuella.Visible = true;
                        //System.Drawing.Image bkgVerificar = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-verificar-huella.png");
                        //tratamiento.btnVerificarHuella.BackgroundImage = bkgVerificar;
                        TreatmentRepository.scoreDedo2 = Convert.ToString(score);

                        btnCapturarHuella2.IsEnabled = false;
                        btnActualizarHuella2.IsEnabled = false;
                        btnVerificarHuella.IsEnabled = true;



                        //Bitmap b = new Bitmap(Application.StartupPath + @"\Img\Botones\capturar-inactivo.png");
                        //tratamiento.btnCapturarHuella2.Image = b;
                        //btnCapturarHuella2.BackgroundImage = System.Drawing.Image.FromFile(Application.StartupPath + @"\Img\Botones\capturar-inactivo.png");
                        //btnCapturarHuella2.BackgroundImage = bkg;
                        //System.Drawing.Image bkg1 = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\Botones\\cambiar-huella-inactivo.png");
                        //tratamiento.btnActualizarHuella2.BackgroundImage = bkg1;
                        imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Ok\\" + lblDedo2.Content + ".png"));
                    }
                    else
                    {
                        //this.TopMost = true;
                        MessageBox.Show("La huella no fué leída correctamente. Intente de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //tratamiento.pbOkHuella2.Visible = false;
                        //tratamiento.pbErrorHuella2.Visible = true;
                        //tratamiento.btnCancelarVerificacion.Enabled = true;
                        btnCapturarHuella2.IsEnabled = true;
                        btnActualizarHuella2.IsEnabled = true;
                        btnVerificarHuella.IsEnabled = false;

                        imgDedo2.Source = new BitmapImage(new Uri(appStartPath + @"\\Resources\\Img\\Dedos\\Error\\" + lblDedo2.Content + ".png"));
                        //this.TopMost = false;
                    }
                }
                //limpiarCampos = false;
                //if (limpiarCamposCaptura)
                //{
                //    this.TopMost = true;
                //    tratamiento.lblHuella1.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                //    tratamiento.lblHuella2.ForeColor = ColorTranslator.FromHtml("#A9AEFE");
                //    tratamiento.btnCapturarHuella1.Enabled = true;
                //    tratamiento.btnActualizarHuella1.Enabled = true;
                //    tratamiento.btnCapturarHuella2.Enabled = false;
                //    tratamiento.btnActualizarHuella2.Enabled = false;
                //    UsuarioRepositorio.huella1 = "";
                //    UsuarioRepositorio.huella2 = "";
                //    TratamientoRepositorio.scoreDedo1 = "";
                //    TratamientoRepositorio.scoreDedo2 = "";
                //    DedoAleatorio();
                //    ActualizarHuella1();
                //    tratamiento.btnVerificarHuella.Enabled = false;
                //    System.Drawing.Image bkgVerificarH = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-verificar-inactivo.png");
                //    tratamiento.btnVerificarHuella.BackgroundImage = bkgVerificarH;

                //    Bitmap btn1 = new Bitmap(Application.StartupPath + @"\\Img\\Botones\\capturar.png");
                //    tratamiento.btnCapturarHuella1.Image = btn1;
                //    System.Drawing.Image bkg1 = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\Botones\\cambiar-huella.png");
                //    tratamiento.btnActualizarHuella1.BackgroundImage = bkg1;

                //    Bitmap btn2 = new Bitmap(Application.StartupPath + @"\\Img\\Botones\\capturar-inactivo.png");
                //    tratamiento.btnCapturarHuella2.Image = btn2;
                //    System.Drawing.Image bkg2 = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\Botones\\cambiar-huella-inactivo.png");
                //    tratamiento.btnActualizarHuella2.BackgroundImage = bkg2;

                //    System.Drawing.Image bkgVerificar = System.Drawing.Image.FromFile(Application.StartupPath + @"\\Img\\BotonesIcfes\\btn-verificar-inactivo.png");
                //    tratamiento.btnVerificarHuella.BackgroundImage = bkgVerificar;
                //    listBox2.Items.Clear();
                //    tratamiento.lblDedo2.Text = string.Empty;
                //    tratamiento.ptbDedo2.Image = null;
                //    tratamiento.pbOkHuella1.Visible = false;
                //    tratamiento.pbOkHuella2.Visible = false;
                //    tratamiento.pbErrorHuella1.Visible = false;
                //    tratamiento.pbErrorHuella2.Visible = false;

                //    MessageBox.Show("Por seguridad se han borrado las minucias que se habían guardado temporalmente. Por favor vuelta a capturar las huellas", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    this.TopMost = false;
                //}
                
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
            }

        }

        private async void btnVerificarHuella_Click(object sender, RoutedEventArgs e)
        {
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                dgEspera.Visibility = Visibility.Visible;
                //MainWindow.AppMainWindow.capturaHuellas.Visibility
                //Biometrico.Biometrico.AppWindow.ConsultarHuella(TratamientoRepositorio.nombreArchivoPdf);
                await VerificarHuellas();
            }
            else
            {
                MessageBox.Show("No hay conexión a internet", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task<Models.Biometric.Validation> VerificarHuellas()
        {
            Models.Biometric.Validation objValidation = new Models.Biometric.Validation();
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            ApiServiceIcfes obJIcfes = new ApiServiceIcfes();
            ApiServiceBiometric obj = new ApiServiceBiometric();

            Models.Biometric.Validation.Nut = "null";
            Models.Biometric.Validation result = await obj.ValidarDatos();
            numeroNut = Models.Biometric.Validation.Nut;

            if (numeroNut == "null")
            {

            }
            else
            {
                BorrarDatosSolicitante();
                if (result.Status == "403")
                {
                    MessageBox.Show("Se ha iniciado sesión desde otro equipo", "Información");
                }
                else
                {
                    Models.Biometric.Validation resultNut = obj.VerificarNut(numeroNut);
                    if (resultNut.Status == "500" || resultNut.Status == "404" || resultNut.Status == "504")
                    {
                        int tiempoConsulta = Convert.ToInt32(DatosGenerales.search_wsrecepcion)+3;
                        int tiempoEspera = Convert.ToInt32(DatosGenerales.wait_ws_recepcion);
                       
                        tConsulta.Interval = new TimeSpan(0, 0, tiempoConsulta);
                        tConsulta.Start();

                       
                        tEspera.Interval = new TimeSpan(0, tiempoEspera, 0);
                        tEspera.Start();
                    }
                    else if (resultNut.Status == "400")
                    {
                        Models.Biometric.Validation.mensajeOperacion = resultNut.Message;
                        Models.Biometric.Validation.codigoResultado = 3;
                        MessageBox.Show("No se pudo obtener un resultado a la consulta realizada. " + resultNut.Message + " - " + "Número de transacción(NUT): " + Models.Biometric.Validation.Nut, "", MessageBoxButton.OK, MessageBoxImage.Warning);

                        if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                        {
                            await obJIcfes.InsertarReporte("verificacionIndividual");
                        }
                        else
                        {
                            MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                            MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                            dgEspera.Visibility = Visibility.Hidden;
                            AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                            await obJIcfes.InsertarReporte("verificacion");
                        }
                    }
                    else if (DatosGenerales.respuestaValidacion == "OK")
                    {
                        if (Models.Biometric.Validation.resultadoBusqueda == "1" && 
                            (Models.Biometric.Validation.resultadoCotejoHuella1 == "1" || 
                            Models.Biometric.Validation.resultadoCotejoHuella2 == "1"))
                        {
                            Models.Biometric.Validation.codigoResultado = 1;
                            if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                            {
                                await obJIcfes.InsertarReporte("verificacionIndividual");
                            }
                            else
                            {
                                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                                MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                                dgEspera.Visibility = Visibility.Hidden;
                                AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                                await obJIcfes.InsertarReporte("verificacion");
                            }
                        }
                        else if (Models.Biometric.Validation.resultadoBusqueda == "1" && 
                            (Models.Biometric.Validation.resultadoCotejoHuella1 == "2" || 
                            Models.Biometric.Validation.resultadoCotejoHuella2 == "2"))
                        {
                            Models.Biometric.Validation.codigoResultado = 2;
                            if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                            {
                                await obJIcfes.InsertarReporte("verificacionIndividual");
                            }
                            else
                            {
                                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                                MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                                dgEspera.Visibility = Visibility.Hidden;
                                AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                                await obJIcfes.InsertarReporte("verificacion");
                            }
                        }
                        else if (Models.Biometric.Validation.resultadoBusqueda == "1" && 
                            (Models.Biometric.Validation.resultadoCotejoHuella1 == "0" && 
                            Models.Biometric.Validation.resultadoCotejoHuella2 == "0"))
                        {
                            Models.Biometric.Validation.codigoResultado = 3;
                            if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                            {
                                await obJIcfes.InsertarReporte("verificacionIndividual");
                            }
                            else
                            {
                                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                                MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                                dgEspera.Visibility = Visibility.Hidden;
                                AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                                await obJIcfes.InsertarReporte("verificacion");
                            }
                        }
                        else
                        {
                            Models.Biometric.Validation.codigoResultado = 3;
                            if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                            {
                                await obJIcfes.InsertarReporte("verificacionIndividual");
                            }
                            else
                            {
                                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                                MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                                dgEspera.Visibility = Visibility.Hidden;
                                AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                                await obJIcfes.InsertarReporte("verificacion");
                            }

                        }
                    }
                    else if (resultNut.Status == "403")
                    {
                        MessageBox.Show("Se ha iniciado sesión desde otro equipo", "Información");
                    }
                }
            }

            return objValidation;
        }

        private async void tConsulta_Tick(object sender, EventArgs e)
        {
            ApiServiceIcfes obJIcfes = new ApiServiceIcfes();
            ApiServiceBiometric obj = new ApiServiceBiometric();
            Models.Biometric.Validation verifcacionResponse = new Models.Biometric.Validation();
            Models.Biometric.Validation result = obj.VerificarNut(numeroNut);
            if (DatosGenerales.respuestaValidacion == "OK")
            {
                detenerTimerConsulta();
                detenerTimerEspera();

                if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                            (Models.Biometric.Validation.resultadoCotejoHuella1 == "1" ||
                            Models.Biometric.Validation.resultadoCotejoHuella2 == "1"))
                {
                    Models.Biometric.Validation.codigoResultado = 1;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        await obJIcfes.InsertarReporte("verificacionIndividual");
                    }
                    else
                    {
                        MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                        dgEspera.Visibility = Visibility.Hidden;
                        AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                        await obJIcfes.InsertarReporte("verificacion");
                    }
                }
                else if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                    (Models.Biometric.Validation.resultadoCotejoHuella1 == "2" ||
                    Models.Biometric.Validation.resultadoCotejoHuella2 == "2"))
                {
                    Models.Biometric.Validation.codigoResultado = 2;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        await obJIcfes.InsertarReporte("verificacionIndividual");
                    }
                    else
                    {
                        MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                        dgEspera.Visibility = Visibility.Hidden;
                        AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                        await obJIcfes.InsertarReporte("verificacion");
                    }
                }
                else if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                    (Models.Biometric.Validation.resultadoCotejoHuella1 == "0" &&
                    Models.Biometric.Validation.resultadoCotejoHuella2 == "0"))
                {
                    Models.Biometric.Validation.codigoResultado = 3;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        await obJIcfes.InsertarReporte("verificacionIndividual");
                    }
                    else
                    {
                        MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                        dgEspera.Visibility = Visibility.Hidden;
                        AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                        await obJIcfes.InsertarReporte("verificacion");
                    }
                }
                else
                {
                    Models.Biometric.Validation.codigoResultado = 3;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        await obJIcfes.InsertarReporte("verificacionIndividual");
                    }
                    else
                    {
                        MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                        dgEspera.Visibility = Visibility.Hidden;
                        AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                        await obJIcfes.InsertarReporte("verificacion");
                    }
                }
            }
            else
            {
                if (result.Status == "400")
                {
                    detenerTimerConsulta();
                    detenerTimerEspera();

                    Models.Biometric.Validation.mensajeOperacion = result.Message;
                    Models.Biometric.Validation.codigoResultado = 3;
                    MessageBox.Show("No se pudo obtener un resultado a la consulta realizada. " + result.Message + " - " + "Número de transacción(NUT): " + Models.Biometric.Validation.Nut, "", MessageBoxButton.OK, MessageBoxImage.Warning);

                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        await obJIcfes.InsertarReporte("verificacionIndividual");
                    }
                    else
                    {
                        MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                        dgEspera.Visibility = Visibility.Hidden;
                        AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                        await obJIcfes.InsertarReporte("verificacion");
                    }
                }
                else if (DatosGenerales.respuestaValidacion == "OK")
                {
                    detenerTimerConsulta();
                    detenerTimerEspera();

                    if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                        (Models.Biometric.Validation.resultadoCotejoHuella1 == "1" ||
                        Models.Biometric.Validation.resultadoCotejoHuella2 == "1"))
                    {
                        Models.Biometric.Validation.codigoResultado = 1;
                        if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                        {
                            await obJIcfes.InsertarReporte("verificacionIndividual");
                        }
                        else
                        {
                            MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                            MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                            dgEspera.Visibility = Visibility.Hidden;
                            AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                            await obJIcfes.InsertarReporte("verificacion");
                        }
                    }
                    else if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                        (Models.Biometric.Validation.resultadoCotejoHuella1 == "2" ||
                        Models.Biometric.Validation.resultadoCotejoHuella2 == "2"))
                    {
                        Models.Biometric.Validation.codigoResultado = 2;
                        if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                        {
                            await obJIcfes.InsertarReporte("verificacionIndividual");
                        }
                        else
                        {
                            MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                            MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                            dgEspera.Visibility = Visibility.Hidden;
                            AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                            await obJIcfes.InsertarReporte("verificacion");
                        }
                    }
                    else if (Models.Biometric.Validation.resultadoBusqueda == "1" &&
                        (Models.Biometric.Validation.resultadoCotejoHuella1 == "0" &&
                        Models.Biometric.Validation.resultadoCotejoHuella2 == "0"))
                    {
                        Models.Biometric.Validation.codigoResultado = 3;
                        if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                        {
                            await obJIcfes.InsertarReporte("verificacionIndividual");
                        }
                        else
                        {
                            MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                            MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                            dgEspera.Visibility = Visibility.Hidden;
                            AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                            await obJIcfes.InsertarReporte("verificacion");
                        }
                    }
                    else
                    {
                        Models.Biometric.Validation.codigoResultado = 3;
                        if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                        {
                            await obJIcfes.InsertarReporte("verificacionIndividual");
                        }
                        else
                        {
                            MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                            MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
                            dgEspera.Visibility = Visibility.Hidden;
                            AsistenciaControl.AppAsistencia.VerificarAsistencia(Models.Biometric.Validation.codigoResultado);
                            await obJIcfes.InsertarReporte("verificacion");
                        }
                    }
                }
                else if (result.Status == "500" || result.Status == "404" || result.Status == "504")
                {
                    dgEspera.Visibility = Visibility.Visible;
                }
                else if (result.Status == "403")
                {
                    detenerTimerConsulta();
                    detenerTimerEspera();
                    MessageBox.Show("Se ha iniciado sesión desde otro equipo", "Información");
                }
            }
        }

        private void tEspera_Tick(object sender, EventArgs e)
        {
            detenerTimerEspera();
            detenerTimerConsulta();
            
            if (DatosIcfesRepositorio.destinoSolicitud == "individual")
            {
                //codigo individual
            }
            else
            {
                MainWindow.AppMainWindow.dialog.Visibility = Visibility.Hidden;
                dgEspera.Visibility = Visibility.Hidden;
                MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Hidden;
            }
            MessageBox.Show("Comuníquese con el administrador. Se excedió el tiempo de espera a la solicitud. Número de transacción(NUT): " + Models.Biometric.Validation.Nut, "Error del sistema", MessageBoxButton.OK, MessageBoxImage.Error);
            
        }

        /// <summary>
        /// Método para detener timer de consulta del servicio
        /// </summary>
        public void detenerTimerConsulta()
        {
            tConsulta.Stop();
        }

        /// <summary>
        /// Método para detener timer de espera de repuesta del servidor
        /// </summary>
        public void detenerTimerEspera()
        {
            tEspera.Stop();
            tConsulta.Stop();
        }

        /// <summary>
        /// Método para borrar los datos almacenados del ciudadano
        /// </summary>
        public void BorrarDatosSolicitante()
        {
            Models.Biometric.Validation.resultadoBusqueda = "";
            Models.Biometric.Validation.resultadoCotejoHuella1 = "";
            Models.Biometric.Validation.resultadoCotejoHuella2 = "";
            UserRepository.id = "";
            UserRepository.nombres = "";
            UserRepository.apellidos = "";
            UserRepository.municipio = "";
            UserRepository.idDedo1 = "";
            UserRepository.idDedo2 = "";
            UserRepository.serialEquipo = "";
            UserRepository.minuciasHuella1 = null;
            UserRepository.minuciasHuella2 = null;
            TreatmentRepository.urlPDF = "";
        }
    }
}
