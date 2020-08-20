using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.Biometric;
using Vivaldi.Models.Icfes;
using Vivaldi.Models.User;

namespace Vivaldi.Services
{
    public class ApiServiceBiometric
    {

        //Metodos HTTP
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

        public ApiServiceBiometric()
        {
            httpMethodPOST = httpVerb.POST;
            httpMethodGET = httpVerb.GET;
            type = "application/json";
        }
        /// <summary>
        /// Método para realizar las validaciones por
        /// datos biográficos y biométrico en el servicio
        /// de la registraduria
        /// </summary>
        /// <param name="pdf"></param>
        /// <param name="origen"></param>
        /// <returns></returns>
        public async Task<Validation> ValidarDatos()
        {
            Validation verifcacionResponse = new Validation();
            try
            {
                
                //string minuciasDedo1 = Encrypt3DES(UsuarioRepositorio.minuciasHuella1);
                //string minuciasDedo2 = Encrypt3DES(UsuarioRepositorio.minuciasHuella2);

                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["verificacionBiometrica"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);

                ServicePointManager.Expect100Continue = false;
                request.ContentType = type;
                request.MediaType = type;
                request.Accept = type;
                request.Method = httpMethodPOST.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                ValidationJsonObjects datos = new ValidationJsonObjects
                {
                    IdOperador = DatosGenerales.idOperador,
                    IdCliente = DatosGenerales.idCliente,
                    NuipAplicante = Convert.ToString(UserRepository.id),
                    Sede = DatosGenerales.sede,
                    FechaOperacion = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss.fff"),
                    CodigoCliente = DatosGenerales.codUsuario,
                    IpCliente = DatosGenerales.ip,
                    MacCliente = DatosGenerales.mac1,
                    ConsultaPorMovil = "0",
                    Latitud = "",
                    Longitud = "",
                    PdfAutorizacion = (TreatmentRepository.urlPDF).Replace(" ", ""),
                    FormatoMinucias = "ISO-FMR",
                    IdDedo1 = UserRepository.idDedo1,
                    IdDedo2 = UserRepository.idDedo2,
                    MinuciasDedo1 = UserRepository.huella1,
                    MinuciasDedo2 = UserRepository.huella2,
                    ScoreDedo1 = UserRepository.score,
                    ScoreDedo2 = UserRepository.score,
                    DetalleOperacion = UserRepository.codigoTramite,
                    AplicanteEsMenorEdad = UserRepository.aplicanteEsMenorEdad
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
                Validation.Nut = results["nut"].ToString();
                
            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

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
                else if (error == "NameResolutionFailure")
                {
                    verifcacionResponse.Status = "500";
                    //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace + ", " + ex.Status);
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
                            verifcacionResponse.Status = result["status"].ToString();
                            verifcacionResponse.Message = result["message"].ToString();
                        }
                    }
                }
            }
            return verifcacionResponse;
        }

        /// <summary>
        /// Método para validar la respuesta que dio 
        /// la consulta de validación ciudadana
        /// </summary>
        /// <param name="nut"></param>
        /// <returns></returns>
        public Validation VerificarNut(string nut)
        {
            Validation verifcacionResponse = new Validation();
            Validation.primerNombre = null;
            Validation.segundoNombre = null;
            Validation.particula = null;
            Validation.descripcionParticula = null;
            Validation.primerApellido = null;
            Validation.segundoApellido = null;
            Validation.lugarExpDocumento = null;
            Validation.fechaExpDocumento = null;
            Validation.codigoVigDocumento = null;
            Validation.descripVigDocumento = null;
            Validation.resultadoBusqueda = null;
            Validation.resultadoCotejoHuella1 = null;
            Validation.resultadoCotejoHuella2 = null;
            Validation.nuipAplicante = null;

            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["verificacionDatos"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + nut);

                ServicePointManager.Expect100Continue = false;
                request.ContentType = type;
                request.MediaType = type;
                request.Accept = type;
                request.Method = httpMethodGET.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                DatosGenerales.respuestaValidacion = response.StatusCode.ToString();

                JObject results = JObject.Parse(responseString);
                Validation.Nut = results["nut"].ToString();
                Validation.primerNombre = results["primerNombre"].ToString();
                Validation.segundoNombre = results["segundoNombre"].ToString();
                Validation.particula = results["particula"].ToString();
                Validation.descripcionParticula = results["descripcionParticula"].ToString();
                Validation.primerApellido = results["primerApellido"].ToString();
                Validation.segundoApellido = results["segundoApellido"].ToString();
                Validation.lugarExpDocumento = results["lugarExpDocumento"].ToString();
                if (results["fechaExpDocumento"].ToString() != "")
                {
                    Validation.fechaExpDocumento = results["fechaExpDocumento"].ToString();
                }
                else
                {
                    Validation.fechaExpDocumento = null;
                }
                Validation.codigoVigDocumento = results["codigoVigDocumento"].ToString();
                Validation.descripVigDocumento = results["descripVigDocumento"].ToString();
                Validation.resultadoBusqueda = results["resultadoBusqueda"].ToString();
                Validation.resultadoCotejoHuella1 = results["resultadoCotejoHuella1"].ToString();
                Validation.resultadoCotejoHuella2 = results["resultadoCotejoHuella2"].ToString();
                Validation.nuipAplicante = results["nuipAplicante"].ToString();
                verifcacionResponse.Message = results["mensajeBusqueda"].ToString();
            }
            catch (WebException ex)
            {
                DatosGenerales.respuestaValidacion = "";
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
            return verifcacionResponse;
        }
    }
}
