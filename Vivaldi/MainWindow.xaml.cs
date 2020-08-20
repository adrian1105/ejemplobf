using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vivaldi.Helpers;
using Vivaldi.Models.User;
using Vivaldi.View;
using Path = System.IO.Path;
using AxMSTSCLib;

namespace Vivaldi
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int validarHuellaSeleccionada = 0;
        bool StateClosed = true;
        public static MainWindow AppMainWindow;
        public MainWindow()
        {
            InitializeComponent();
            AppMainWindow = this;
            PCInformation.getMacAddress();
            PCInformation.ObtenerIp();
        }

        private void rectangle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (StateClosed)
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }

            StateClosed = !StateClosed;
        }

        private void Close_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //View.LoginControl.AppLogin.ConfiguracionLogin();
            GC.Collect();
            Thread.MemoryBarrier();
            Environment.Exit(0);
            this.Close();
        }

        private void keyCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            admin.Visibility = Visibility.Hidden;
            vtiger.Visibility = Visibility.Hidden;
            soti.Visibility = Visibility.Hidden;
            huellas.Visibility = Visibility.Visible;
        }

        private void Admin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            huellas.Visibility = Visibility.Hidden;
            vtiger.Visibility = Visibility.Hidden;
            soti.Visibility = Visibility.Hidden;
            admin.Visibility = Visibility.Visible;
        }

        private void Login_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            admin.Visibility = Visibility.Hidden;
            huellas.Visibility = Visibility.Hidden;
            vtiger.Visibility = Visibility.Hidden;
            soti.Visibility = Visibility.Hidden;
        }
        private void Vtiger_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            admin.Visibility = Visibility.Hidden;
            huellas.Visibility = Visibility.Hidden;
            soti.Visibility = Visibility.Hidden;
            vtiger.Visibility = Visibility.Visible;
        }

        private void Soti_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            admin.Visibility = Visibility.Hidden;
            huellas.Visibility = Visibility.Hidden;
            vtiger.Visibility = Visibility.Hidden;
            soti.Visibility = Visibility.Visible;
        }

        public void DedoAleatorio()
        {
            lbHuella1.Items.Clear();
            List<int> datos = new List<int>();
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

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
                            string validar = split[0] + "-" + split[1];
                            if (validar != UserRepository.huellaCedula)
                            {
                                lbHuella1.Items.Add(split[0] + "-" + split[1]);
                            }
                        }
                    }
                } while (!fp.EndOfStream);
            }
        }

        /// <summary>
        /// Método para cargar los id y nombres de cada dedos de la mano
        /// paea la segunda captura
        /// </summary>
        public void Dedo2Aleatorio()
        {
            lbHuella2.Items.Clear();
            List<int> datos = new List<int>();
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartPath + @"\\Resources\\InfoDedos.csv";
            StreamReader fp;
            char[] buffer = new char[160];
            int i = 0;
            string texto;
            string[] split = null;
            string[] valdarHuellaCedula = UserRepository.huellaCedula.Split('-');
            if (Convert.ToString(CapturaHuellasControl.AppHuellas.lblDedo1.Content) == valdarHuellaCedula[1])
            {
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
                                string validar = split[0] + "-" + split[1];
                                if (validar != UserRepository.huellaCedula)
                                {
                                    lbHuella2.Items.Add(split[0] + "-" + split[1]);
                                }
                            }
                        }
                    } while (!fp.EndOfStream);
                }
                validarHuellaSeleccionada = 1;
            }
            else
            {
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
                                string validar = split[1];
                                if (validar != Convert.ToString(CapturaHuellasControl.AppHuellas.lblDedo1.Content) && validar != valdarHuellaCedula[1])
                                {
                                    lbHuella2.Items.Add(split[0] + "-" + split[1]);
                                }
                            }
                        }
                    } while (!fp.EndOfStream);
                }
                validarHuellaSeleccionada = 2;
            }
        }
    }
}
