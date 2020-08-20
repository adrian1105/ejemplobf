namespace Vivaldi.View
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Vivaldi.Helpers;
    using Vivaldi.Models;
    using Vivaldi.Models.Authentication;
    using Vivaldi.Models.User;
    using Vivaldi.Services;
    using static Vivaldi.Helpers.GetDevice;

    /// <summary>
    /// Lógica de interacción para LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        ApiServiceAutenticacion autenticar = new ApiServiceAutenticacion();
        Usb oUsb = new Usb();
        public static LoginControl AppLogin;
        FormValidation validar = new FormValidation();
        string deviceID = "";
        public LoginControl()
        {
            InitializeComponent();
            AppLogin = this;
        }

        private async void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            DatosGenerales.validacionIngreso = "login";
            bar.IsIndeterminate = true;
            lblError.Text = string.Empty;
            Response validation = await validar.Login(tbxUser.Text, tbxPass.Password, tbxToken.Password);
            //validacion campos en el formulario login
            if (validation.IsSuccess)
            {
                //validacion de usuario con sesión activa -- si es true open dialog
                // si validacion es false el continua con la autenticación de usuario
                if (validation.IsSuccess && validation.IsError == false && validation.IsUserActive == true)
                {
                    bar.IsIndeterminate = false;
                    var person = new Response
                    {
                        Message = validation.Message,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(person, "ok");
                }
                else if (validation.IsSuccess && validation.IsError == false && validation.IsUserActive == false)
                {
                    bar.IsIndeterminate = true;
                    Authentication result = await autenticar.Autenticar(tbxUser.Text, tbxPass.Password, "");
                    lblError.Text = result.Message;
                    DatosGenerales.token = result.Token;
                    bar.IsIndeterminate = false;

                    if (result.Status == "autenticado")
                    {
                        List<GetDevice> lstUSBD = oUsb.GetUSBDevices();
                        if (lstUSBD.Count != 0)
                        {
                            deviceID = lstUSBD[0].DeviceID;

                            string[] words = deviceID.Split('\\');

                            foreach (var word in words)
                            {
                                DatosGenerales.serialBiometrico = word;
                            }

                            Authentication resultIngresar = autenticar.VerificarTokenOtp(DatosGenerales.token, tbxToken.Password);
                            if (resultIngresar.ValidacionValue == "true")
                            {
                                ApiServiceAutenticacion objTramite = new ApiServiceAutenticacion();
                                string tramites = string.Empty;
                                try
                                {
                                    tramites = await objTramite.ListarTramites();
                                    JArray results = JArray.Parse(tramites) as JArray;
                                    foreach (var dato in results)
                                    {
                                        tramites = (string)dato["codigo"];
                                    }

                                    if (tramites != "[]")
                                    {
                                        UserRepository.codigoTramite = tramites;
                                        await autenticar.RegistrarLogUsuario();
                                        ApiServiceAutenticacion objUsuario = new ApiServiceAutenticacion();
                                        await objUsuario.UsuarioIngreso();
                                        ApiServiceAutenticacion objKey = new ApiServiceAutenticacion();
                                        await objKey.SolicitarKey();
                                        try
                                        {
                                            await SitioControl.AppSitio.SitioPruebaPorcentaje();
                                        }
                                        catch (Exception)
                                        {
                                            bar.IsIndeterminate = false;
                                            lblError.Text = "Ocurrio un error al cargar los sitios";
                                        }

                                        if (SitioControl.AppSitio.cbxSitio.Items.Count != 0)
                                        {
                                            MainWindow.AppMainWindow.lblTituloLogin.Visibility = Visibility.Hidden;
                                            MainWindow.AppMainWindow.GridMenu.Visibility = Visibility.Hidden;
                                            MainWindow.AppMainWindow.login.Visibility = Visibility.Hidden;
                                            MainWindow.AppMainWindow.sitio.Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            lblError.Text = "El usuario no tiene sitios activos";
                                        }
                                        
                                    }
                                    else
                                    {
                                        bar.IsIndeterminate = false;
                                        lblError.Text = "Ocurrio un error al cargar los tramites";
                                    }
                                }
                                catch (Exception)
                                {
                                    bar.IsIndeterminate = false;
                                    lblError.Text = "Ocurrio un error al cargar los tramites";
                                }

                            }
                            else if (resultIngresar.ValidacionValue == "false")
                            {
                                bar.IsIndeterminate = false;
                                tbxToken.Password = "";
                                lblError.Text = "Token incorrecto";
                            }
                            else
                            {
                                bar.IsIndeterminate = false;
                                lblError.Text = resultIngresar.Message;
                            }
                        }
                        else
                        {
                            bar.IsIndeterminate = false;
                            var error = new Response
                            {
                                Message = Messages.DeviceNotFound,
                            };
                            await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                        }
                        return;
                    }
                    if (result.Status == "cambio" && result.MessageToken == null)
                    {
                        bar.IsIndeterminate = false;
                        var error = new Response
                        {
                            Message = Messages.PassChange,
                        };
                        await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                    }
                    else if (result.Status == "400")
                    {
                        bar.IsIndeterminate = false;
                        if (result.Message == "Estación de trabajo no registrado")
                        {

                            var error = new Response
                            {
                                Message = result.Message + ". Mac del equipo:" + DatosGenerales.mac1,
                            };
                            await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                        }
                        else if (result.Message == "Estación de trabajo inactivo")
                        {
                            var error = new Response
                            {
                                Message = result.Message + ". Mac del equipo:" + DatosGenerales.mac1,
                            };
                            await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");

                        }
                        else
                        {
                            var error = new Response
                            {
                                Message = result.Message,
                            };
                            await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                        }
                    }
                    else if (result.StatusToken == "403")
                    {
                        bar.IsIndeterminate = false;
                        var error = new Response
                        {
                            Message = result.MessageToken,
                        };
                        await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                    }
                    else if (result.Status == "500")
                    {
                        bar.IsIndeterminate = false;
                        var error = new Response
                        {
                            Message = Messages.ErrorServer,
                        };
                        await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                    }
                    else
                    {
                        bar.IsIndeterminate = false;
                        var error = new Response
                        {
                            Message = Messages.ErrorSession,
                        };
                        await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                    }
                }
                else
                {
                    bar.IsIndeterminate = false;
                    var error = new Response
                    {
                        Message = validation.Message,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                }
            }
            else
            {
                bar.IsIndeterminate = false;
                lblError.Text = validation.Message;
            }

            bar.IsIndeterminate = false;
        }

        private async void btnAceptarDialog_Click(object sender, RoutedEventArgs e)
        {
            DatosGenerales.validacionIngreso = "login";
            bar.IsIndeterminate = true;
            Authentication result = await autenticar.Autenticar(tbxUser.Text, tbxPass.Password, "");
            lblError.Text = result.Message;
            DatosGenerales.token = result.Token;

            if (result.Status == "autenticado")
            {
                List<GetDevice> lstUSBD = oUsb.GetUSBDevices();
                if (lstUSBD.Count != 0)
                {
                    deviceID = lstUSBD[0].DeviceID;

                    string[] words = deviceID.Split('\\');

                    foreach (var word in words)
                    {
                        DatosGenerales.serialBiometrico = word;
                    }

                    Authentication resultIngresar = autenticar.VerificarTokenOtp(DatosGenerales.token, tbxToken.Password);
                    if (resultIngresar.ValidacionValue == "true")
                    {
                        ApiServiceAutenticacion objTramite = new ApiServiceAutenticacion();
                        string tramites = string.Empty;
                        try
                        {
                            tramites = await objTramite.ListarTramites();
                            JArray results = JArray.Parse(tramites) as JArray;
                            foreach (var dato in results)
                            {
                                tramites = (string)dato["codigo"];
                            }

                            if (tramites != "[]")
                            {
                                UserRepository.codigoTramite = tramites;
                                await autenticar.RegistrarLogUsuario();
                                ApiServiceAutenticacion objUsuario = new ApiServiceAutenticacion();
                                await objUsuario.UsuarioIngreso();
                                ApiServiceAutenticacion objKey = new ApiServiceAutenticacion();
                                await objKey.SolicitarKey();
                                try
                                {
                                   await SitioControl.AppSitio.SitioPruebaPorcentaje();
                                }catch(Exception)
                                {
                                    lblError.Text = "Ocurrio un error al cargar los sitios";
                                    bar.IsIndeterminate = false;
                                }

                                if (SitioControl.AppSitio.cbxSitio.Items.Count != 0)
                                {
                                    MainWindow.AppMainWindow.lblTituloLogin.Visibility = Visibility.Hidden;
                                    MainWindow.AppMainWindow.GridMenu.Visibility = Visibility.Hidden;
                                    MainWindow.AppMainWindow.login.Visibility = Visibility.Hidden;
                                    MainWindow.AppMainWindow.sitio.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    lblError.Text = "El usuario no tiene sitios activos";
                                }
                            }
                            else
                            {
                                tbxToken.Password = "";
                                lblError.Text = "Ocurrio un error al cargar los tramites";
                                bar.IsIndeterminate = false;
                            }
                        }
                        catch (Exception)
                        {
                            tbxToken.Password = "";
                            lblError.Text = "Ocurrio un error al cargar los tramites";
                            bar.IsIndeterminate = false;
                        }

                    }
                    else if (resultIngresar.ValidacionValue == "false")
                    {
                        tbxToken.Password = "";
                        lblError.Text = "Token incorrecto";
                        bar.IsIndeterminate = false;
                    }
                    else
                    {
                        lblError.Text = resultIngresar.Message;
                        bar.IsIndeterminate = false;
                    }
                }
                else
                {
                    bar.IsIndeterminate = false;
                    var error = new Response
                    {
                        Message = Messages.DeviceNotFound,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                }
                return;
            }
            if (result.Status == "cambio" && result.MessageToken == null)
            {
                bar.IsIndeterminate = false;
                var error = new Response
                {
                    Message = Messages.PassChange,
                };
                await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
            }
            else if (result.Status == "400")
            {
                bar.IsIndeterminate = false;
                if (result.Message == "Estación de trabajo no registrado")
                {

                    var error = new Response
                    {
                        Message = result.Message + ". Mac del equipo:" + DatosGenerales.mac1,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                }
                else if (result.Message == "Estación de trabajo inactivo")
                {
                    var error = new Response
                    {
                        Message = result.Message + ". Mac del equipo:" + DatosGenerales.mac1,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");

                }
                else
                {
                    var error = new Response
                    {
                        Message = result.Message,
                    };
                    await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
                }
            }
            else if (result.StatusToken == "403")
            {
                bar.IsIndeterminate = false;
                var error = new Response
                {
                    Message = result.MessageToken,
                };
                await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
            }
            else if (result.Status == "500")
            {
                bar.IsIndeterminate = false;
                var error = new Response
                {
                    Message = Messages.ErrorServer,
                };
                await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
            }
            else
            {
                bar.IsIndeterminate = false;
                var error = new Response
                {
                    Message = Messages.ErrorSession,
                };
                await MaterialDesignThemes.Wpf.DialogHost.Show(error, "error");
            }
            bar.IsIndeterminate = false;
        }
    }
}
