using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vivaldi.Helpers;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.User;

namespace Vivaldi.Services
{
    public class ApiServiceAutenticacion
    {
        Security obj1 = new Security();
        public enum httpVerb
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public httpVerb httpMethodPOST { get; set; }
        public httpVerb httpMethodGET { get; set; }
        public string type { get; set; }

        public ApiServiceAutenticacion()
        {
            httpMethodPOST = httpVerb.POST;
            httpMethodGET = httpVerb.GET;
            type = "application/json";
        }

        /// <summary>
        /// Método POST Autenticar
        /// </summary>
        public async Task<Authentication> Autenticar(string usuario, string clave, string mac)
        {
            Authentication autenticarResponse = null;
            byte[] encrypted = null;
            try
            {
                obj1.setSSLCertificate();

                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["autenticar"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);


                ServicePointManager.Expect100Continue = false;
                request.Method = httpMethodPOST.ToString();
                request.ContentType = type;
                request.Headers.Add("Version-Cliente", ConfigurationManager.AppSettings["version"]);


                //Encriptar clave
                encrypted = AES.EncryptAesManaged(clave);
                clave = Convert.ToBase64String(encrypted);

                //Encriptar usuario
                encrypted = AES.EncryptAesManaged(usuario);
                usuario = Convert.ToBase64String(encrypted);

                AuthenticationJsonObject datos = new AuthenticationJsonObject
                {
                    Clave = clave,
                    Mac = "80-CE-62-1D-0E-74",
                    Usuario = usuario
                };

                autenticarResponse = new Authentication();
                string postData = JsonConvert.SerializeObject(datos);
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)await request.GetResponseAsync();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                JObject results = JObject.Parse(responseString);
                autenticarResponse.Status = results["status"].ToString();
                autenticarResponse.Token = results["token"].ToString();
            }
            catch (WebException ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    MessageBox.Show("Error en el servidor remoto: (503) Servidor no disponible.", "Error de conexión");
                }
                else if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    MessageBox.Show("No es posible conectar con el servidor remoto", "Error de conexión");
                }
                else if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                }
                else if (ex.Message == "Error en el servidor remoto: (499) Request has been forbidden by antivirus.")
                {
                    MessageBox.Show("La solicitud ha sido prohibida por el antivirus");
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (response is null)
                        {

                        }
                        else
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                string text = new StreamReader(data).ReadToEnd();
                                JObject result = JObject.Parse(text);
                                autenticarResponse.Status = result["status"].ToString();
                                autenticarResponse.Message = result["message"].ToString();
                            }
                        }
                    }
                }
            }
            return autenticarResponse;
        }

        /// <summary>
        /// Método para generar toquen otp y enviarlo al correo
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Authentication> GenerarToken(string token)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            Authentication autenticarResponse = new Authentication();
            string strResponseValue = string.Empty;
            string requestURL = ConfigurationManager.AppSettings["requestURL"];
            string requestCadena = ConfigurationManager.AppSettings["generarToken"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);

            ServicePointManager.Expect100Continue = false;
            request.Method = httpMethodGET.ToString();
            request.ContentType = type;
            request.Headers.Add("Authorization", "Bearer " + token);

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    MessageBox.Show("Error en el servidor remoto: (503) Servidor no disponible.", "Error de conexión");
                }
                else if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    MessageBox.Show("No es posible conectar con el servidor remoto", "Error de conexión");
                }
                else if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                }
                else if (ex.Message == "Error en el servidor remoto: (499) Request has been forbidden by antivirus.")
                {
                    MessageBox.Show("La solicitud ha sido prohibida por el antivirus");
                }
                else
                {
                    using (WebResponse respuesta = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)respuesta;
                        //if (response is null)
                        //{

                        //}
                        //else
                        //{
                        //    using (Stream data = response.GetResponseStream())
                        //    {
                        //        string text = new StreamReader(data).ReadToEnd();
                        //        JObject result = JObject.Parse(text);
                        //        autenticarResponse.StatusToken = result["status"].ToString();
                        //        autenticarResponse.MessageToken = result["message"].ToString();
                        //    }
                        //}
                    }
                }
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " " + autenticarResponse.MessageToken + " " + autenticarResponse.StatusToken);
            }
            return autenticarResponse;
        }

        /// <summary>
        /// Método para verificar si un usuario
        /// a iniciado sesión
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<Authentication> ValidarUsuarioActivo(string usuario)
        {
            Authentication datosResponse = new Authentication();
            byte[] encrypted = null;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["validarUsuarioActivo"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);
                //request.ServerCertificateValidationCallback = delegate { return true; };

                ServicePointManager.Expect100Continue = false;
                request.Method = httpMethodPOST.ToString();
                request.ContentType = type;


                if (DatosGenerales.validacionIngreso != "login")
                {
                    request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);
                }

                //Encriptar usuario
                encrypted = AES.EncryptAesManaged(usuario);
                usuario = Convert.ToBase64String(encrypted);

                AuthenticationJsonObject datos = new AuthenticationJsonObject
                {
                    Clave = "",
                    Mac = "",
                    Usuario = usuario
                };

                string postData = JsonConvert.SerializeObject(datos);
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)await request.GetResponseAsync();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject results = JObject.Parse(responseString);
                datosResponse.UsuarioActivo = results["value"].ToString();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (response is null)
                    {
                        datosResponse.Status = Convert.ToString(ex.Status);
                        datosResponse.Message = ex.Message;
                    }
                    else
                    {
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                            JObject result = JObject.Parse(text);
                            datosResponse.Status = result["status"].ToString();
                            datosResponse.Message = result["message"].ToString();
                        }
                    }
                }
            }
            return datosResponse;
        }

        /// <summary>
        /// Método para validar si el token otp esta activo
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenOtp"></param>
        /// <returns></returns>
        public Authentication VerificarTokenOtp(string token, string tokenOtp)
        {
            Authentication datosResponseVerificar = new Authentication();
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["verificarToken"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + tokenOtp);

                ServicePointManager.Expect100Continue = false;
                request.Method = httpMethodGET.ToString();
                request.ContentType = type;
                //Se le envía token de autenticación para que el servicios responda exitosamente
                request.Headers.Add("Authorization", "Bearer " + token);

                var responseVerificar = (HttpWebResponse)request.GetResponse();

                var responseStringVerificar = new StreamReader(responseVerificar.GetResponseStream()).ReadToEnd();
                JObject resultsVerificar = JObject.Parse(responseStringVerificar);
                datosResponseVerificar.ValidacionValue = resultsVerificar["value"].ToString();
                DecodeToken(token);
            }
            catch (WebException ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    MessageBox.Show("Error en el servidor remoto: (503) Servidor no disponible.", "Error de conexión");
                }
                else if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    MessageBox.Show("No es posible conectar con el servidor remoto", "Error de conexión");
                }
                else if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                            JObject result = JObject.Parse(text);
                            datosResponseVerificar.Message = result["message"].ToString();
                        }
                    }
                }
            }
            return datosResponseVerificar;
        }

        /// <summary>
        /// Método para validar la respuesta que dio 
        /// la consulta de validación ciudadana
        /// </summary>
        /// <param name="nut"></param>
        /// <returns></returns>
        public async Task<string> UsuarioIngreso()
        {
            try
            {
               // ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["datosUsuario"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + DatosGenerales.codUsuario);

                ServicePointManager.Expect100Continue = false;
                request.ContentType = type;
                request.MediaType = type;
                request.Accept = type;
                request.Method = httpMethodGET.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                var response = (HttpWebResponse)await request.GetResponseAsync();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                DatosGenerales.respuestaValidacion = response.StatusCode.ToString();

                JObject results = JObject.Parse(responseString);
                DatosGenerales.IdUsuarioIngreso = results["documento"].ToString();
                DatosGenerales.UsuarioIngreso = results["nombreUsuario"].ToString();
                DatosGenerales.nombreUsuarioIngreso = results["primerNombre"].ToString() + " " + results["primerApellido"].ToString();
            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (error == "NameResolutionFailure")
                {
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (response is null)
                        {

                        }
                        else
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                string text = new StreamReader(data).ReadToEnd();
                                JObject result = JObject.Parse(text);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        public async Task<string> ListarTramites()
        {
            Authentication verifcacionResponse = new Authentication();
            string strResponseValue = string.Empty;
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["listarTramites"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + DatosGenerales.idCliente);

                ServicePointManager.Expect100Continue = false;
                request.ContentType = type;
                request.MediaType = type;
                request.Accept = type;
                request.Method = httpMethodGET.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                HttpWebResponse response = null;
                // Llamar HttpWebResponse
                response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (error == "NameResolutionFailure")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (response is null)
                        {

                        }
                        else
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                string text = new StreamReader(data).ReadToEnd();
                                JObject result = JObject.Parse(text);
                                verifcacionResponse.Status = result["status"].ToString();
                                verifcacionResponse.Message = result["message"].ToString();
                            }
                        }
                    }
                }
            }
            return strResponseValue;
        }

        public async Task<string> SolicitarKey()
        {
            Authentication verifcacionResponse = new Authentication();
            string strResponseValue = string.Empty;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["key3Des"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + DatosGenerales.idCliente);

                ServicePointManager.Expect100Continue = false;
                request.ContentType = type;
                request.MediaType = type;
                request.Accept = type;
                request.Method = httpMethodGET.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                HttpWebResponse response = null;
                // Llamar HttpWebResponse
                response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            JObject results = JObject.Parse(strResponseValue);
                            DatosGenerales.key = AES.DecryptAES(results["value"].ToString());
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " Error al solicitar la llave de la entidad");
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " Error al solicitar la llave de la entidad");
                }
                if (ex.Message == "Error en el servidor remoto: (404) No se encontró.")
                {
                    verifcacionResponse.Status = "400";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " Error al solicitar la llave de la entidad");
                }
                if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " Error al solicitar la llave de la entidad");
                }
                if (error == "NameResolutionFailure")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + " Error al solicitar la llave de la entidad");
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (response is null)
                        {

                        }
                        else
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                string text = new StreamReader(data).ReadToEnd();
                                JObject result = JObject.Parse(text);
                                verifcacionResponse.Status = result["status"].ToString();
                                verifcacionResponse.Message = result["message"].ToString();
                            }
                        }
                    }
                }
            }
            return strResponseValue;
        }

        /// <summary>
        /// Método para registrar información al log de usuario
        /// </summary>
        /// <returns></returns>
        public async Task<Authentication> RegistrarLogUsuario()
        {
            Authentication datosResponseLog = new Authentication();
            try
            {
               
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["registrarLog"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);

                ServicePointManager.Expect100Continue = false;
                request.Method = httpMethodPOST.ToString();
                request.ContentType = type;
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                UserJsonObject datos = new UserJsonObject
                {
                    Dispositivo = "E",
                    EntidadCodigo = DatosGenerales.codUsuario,
                    EntidadNit = DatosGenerales.entidadNit,
                    EntidadNombre = DatosGenerales.entidadNombre,
                    IdCliente = DatosGenerales.idCliente,
                    Ip = DatosGenerales.ip,
                    Mac = DatosGenerales.mac1,
                    NombreUsuario = DatosGenerales.codUsuario,
                    Sede = DatosGenerales.sede,
                    SerialBiometrico = DatosGenerales.serialBiometrico,
                };

                string postDataLog = JsonConvert.SerializeObject(datos);
                var dataLog = Encoding.UTF8.GetBytes(postDataLog);
                request.ContentLength = dataLog.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(dataLog, 0, dataLog.Length);
                }

                var responseLog = (HttpWebResponse)await request.GetResponseAsync();

                var responseStringLog = new StreamReader(responseLog.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
               // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    MessageBox.Show("Error en el servidor remoto: (503) Servidor no disponible.", "Error de conexión");
                }
                else if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    MessageBox.Show("No es posible conectar con el servidor remoto", "Error de conexión");
                }
                else if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                }
                else
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                            JObject result = JObject.Parse(text);
                            datosResponseLog.Message = result["message"].ToString();
                        }
                    }
                }
            }
            return datosResponseLog;
        }

        /// <summary>
        /// Decodificador del token para obtener los permisos
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void DecodeToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            //Comprueba si token esta legible(cadena está en formato JWT)
            var readableToken = jwtHandler.CanReadToken(token);

            if (readableToken == true)
            {
                var tokenv = jwtHandler.ReadJwtToken(token);
                // Extraer la carga del JWT
                var claims = tokenv.Claims;
                foreach (Claim c in claims)
                {
                    if (c.Type == "idOperador")
                    {
                        DatosGenerales.idOperador = c.Value;
                    }
                    if (c.Type == "codusuario")
                    {
                        DatosGenerales.codUsuario = c.Value;
                    }
                    if (c.Type == "scoreCaptura")
                    {
                        DatosGenerales.scoreCaptura = c.Value;
                    }
                    if (c.Type == "wait_ws_recepcion")
                    {
                        DatosGenerales.wait_ws_recepcion = c.Value;
                    }
                    if (c.Type == "search_wsrecepcion")
                    {
                        DatosGenerales.search_wsrecepcion = c.Value;
                    }
                    if (c.Type == "entidadNit")
                    {
                        DatosGenerales.entidadNit = c.Value;
                    }
                    if (c.Type == "entidadNombre")
                    {
                        DatosGenerales.entidadNombre = c.Value;
                    }
                    if (c.Type == "entidadCodigo")
                    {
                        DatosGenerales.idCliente = c.Value;
                    }
                    if (c.Type == "sede")
                    {
                        DatosGenerales.sede = c.Value;
                    }
                    if (c.Type == "time_session")
                    {
                        DatosGenerales.time_session = c.Value;
                    }
                    if (c.Type == "time_huella")
                    {
                        DatosGenerales.time_huella = c.Value;
                    }
                }
            }
        }
    }
}
