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
using Vivaldi.Helpers;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.Biometric;
using Vivaldi.Models.Icfes;

namespace Vivaldi.Services
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class ApiServiceIcfes
    {
        public httpVerb httpMethodPOST { get; set; }
        public httpVerb httpMethodGET { get; set; }
        public string type { get; set; }

        public ApiServiceIcfes()
        {
            httpMethodPOST = httpVerb.POST;
            httpMethodGET = httpVerb.GET;
            type = "application/json";
        }

        public async Task<string> ListarSitios()
        {
            string strResponseValue = string.Empty;
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                try
                {
                    string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                    string requestCadena = ConfigurationManager.AppSettings["sitios"];
                    var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + DatosGenerales.IdUsuarioIngreso);
                    request.ContentType = type;
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
                                    //verifcacionResponse.Status = result["status"].ToString();
                                    //verifcacionResponse.Message = result["message"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay conexión a internet");
            }
            return strResponseValue;
        }

        public async Task<string> ConsultarEventoActivo(string prueba_id)
        {
            Authentication verifcacionResponse = new Authentication();
            string strResponseValue = string.Empty;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                string requestCadena = ConfigurationManager.AppSettings["eventoactivo"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + prueba_id);
                request.ContentType = type;
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
                Evento.eventoId = 0;
                Evento.nombre = "";
                Evento.horaInicial = "";
                Evento.horaFinal = "";

                JObject results = JObject.Parse(strResponseValue);
                if (((Newtonsoft.Json.Linq.JContainer)results["eventos"]).Count != 0)
                {
                    Evento.eventoId = Convert.ToInt32(results["eventos"][0]["eventoId"].ToString());
                    Evento.nombre = results["eventos"][0]["nombre"].ToString();
                    Evento.horaInicial = results["eventos"][0]["horaInicial"].ToString();
                    Evento.horaFinal = results["eventos"][0]["horaFinal"].ToString();
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
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (error == "NameResolutionFailure")
                {
                    verifcacionResponse.Status = "500";
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
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

        public string ConsultarEventos(string prueba_id)
        {
            Authentication verifcacionResponse = new Authentication();
            string strResponseValue = string.Empty;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                string requestCadena = ConfigurationManager.AppSettings["eventos"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena + prueba_id);
                request.ContentType = type;
                request.Method = httpMethodGET.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                HttpWebResponse response = null;
                // Llamar HttpWebResponse
                response = (HttpWebResponse)request.GetResponse();

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
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    verifcacionResponse.Status = "500";
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
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

        public async Task<string> ListarSalones(string idPrueba, string idSitio)
        {
            string strResponseValue = string.Empty;
            bool CheckConnection = InternetConnection.IsConnectedToInternet();
            if (CheckConnection == true)
            {
                try
                {
                    string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                    string requestCadena = ConfigurationManager.AppSettings["salones"];
                    var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);
                    request.ContentType = type;
                    request.Method = httpMethodPOST.ToString();
                    request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                    SitioPruebaJsonObjects datos = new SitioPruebaJsonObjects
                    {
                        IdPrueba = idPrueba,
                        IdSitio = idSitio
                    };

                    string postData = JsonConvert.SerializeObject(datos);
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;

                    using (var streamWriter = request.GetRequestStream())
                    {
                        streamWriter.Write(data, 0, data.Length);
                    }
                    // Llamar HttpWebResponse
                    var response = (HttpWebResponse)await request.GetResponseAsync();
                    // Resultado HttpWebResponse
                    strResponseValue = new StreamReader(response.GetResponseStream()).ReadToEnd();

                }
                catch (WebException ex)
                {
                    string error = Convert.ToString(ex.Status);

                    if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                    {
                        //verifcacionResponse.Status = "500";
                       // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                    }
                    if (ex.Message == "No es posible conectar con el servidor remoto")
                    {
                        //verifcacionResponse.Status = "500";
                        //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                    }
                    if (ex.Message == "Se excedió el tiempo de espera de la operación")
                    {
                        MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                        //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                    }
                    if (error == "NameResolutionFailure")
                    {
                        //verifcacionResponse.Status = "500";
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
                                    //verifcacionResponse.Status = result["status"].ToString();
                                    //verifcacionResponse.Message = result["message"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay conexión a internet");
            }

            return strResponseValue;
        }

        public async Task<string> ListarExaminandos(string sitioId, string salon, string pruebaId)
        {
            //Verificacion verifcacionResponse = new Verificacion();
            string strResponseValue = string.Empty;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                string requestCadena = ConfigurationManager.AppSettings["examinandos"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);
                request.ContentType = type;
                request.Method = httpMethodPOST.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                ExaminandoJsonObjects datos = new ExaminandoJsonObjects
                {
                    SitioId = sitioId,
                    Salon = salon,
                    PruebaId = Convert.ToInt32(pruebaId)
                };

                string postData = JsonConvert.SerializeObject(datos);
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(data, 0, data.Length);
                }
                // Llamar HttpWebResponse
                var response = (HttpWebResponse)await request.GetResponseAsync();
                // Resultado HttpWebResponse
                strResponseValue = new StreamReader(response.GetResponseStream()).ReadToEnd();

            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    //verifcacionResponse.Status = "500";
                  //  log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    //verifcacionResponse.Status = "500";
                  //  log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "Se excedió el tiempo de espera de la operación")
                {
                    MessageBox.Show("Se excedió el tiempo de espera de la operación", "Error de conexión");
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (error == "NameResolutionFailure")
                {
                    //verifcacionResponse.Status = "500";
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
                                //verifcacionResponse.Status = result["status"].ToString();
                                //verifcacionResponse.Message = result["message"].ToString();
                            }
                        }
                    }
                }
            }
            return strResponseValue;
        }

        public async Task<Validation> InsertarReporte(string destino)
        {
            Validation verifcacionResponse = new Validation();
            string strResponseValue = string.Empty;
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURLIcfes"];
                string requestCadena = ConfigurationManager.AppSettings["InsertarReporte"];
                var request = (HttpWebRequest)WebRequest.Create(requestURL + requestCadena);
                request.ContentType = type;
                request.Method = httpMethodPOST.ToString();
                request.Headers.Add("Authorization", "Bearer " + DatosGenerales.token);

                ReportJsonObjects datos = null;

                if (destino == "omitir")
                {
                    string salon = string.Empty;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        salon = "-1";
                    }
                    else
                    {
                        salon = DatosIcfesRepositorio.salon;
                    }
                    datos = new ReportJsonObjects
                    {
                        SitioId = DatosIcfesRepositorio.idSitio,
                        Salon = salon,
                        PruebaId = DatosIcfesRepositorio.idPrueba,
                        TipoDocumentoDeclarado = Examinandos.tipoDocumento,
                        NroDocumentoDeclarado = Examinandos.numeroDocumento,
                        CitaSnee = Examinandos.citaSNEE,
                        //Motivo = ControlUsuario.CapturaInformacion.AppWindowInfo.cbxMotivo.Text,
                        //Observacion = ControlUsuario.CapturaInformacion.AppWindowInfo.txbObservacion.Text,
                        CodigoResultado = "4",
                        FechaCapturaHuella = DateTime.Now.ToString("dd'-'MM'-'yy'T'HH:mm:ss.fff"),
                        EventoId = Evento.eventoId
                    };
                }
                else if (destino == "no Autorizado")
                {
                    string salon = string.Empty;
                    if (DatosIcfesRepositorio.destinoSolicitud == "individual")
                    {
                        salon = "-1";
                    }
                    else
                    {
                        salon = DatosIcfesRepositorio.salon;
                    }
                    datos = new ReportJsonObjects
                    {
                        SitioId = DatosIcfesRepositorio.idSitio,
                        Salon = salon,
                        PruebaId = DatosIcfesRepositorio.idPrueba,
                        TipoDocumentoDeclarado = Examinandos.tipoDocumento,
                        NroDocumentoDeclarado = Examinandos.numeroDocumento,
                        CitaSnee = Examinandos.citaSNEE,
                        Motivo = "Sin autorización de padres",
                        Observacion = "El examinando es menor de edad y no cuenta con la autorización de los padres, por tanto no se puede realizar la verificación biométrica",
                        CodigoResultado = "4",
                        FechaCapturaHuella = DateTime.Now.ToString("dd'-'MM'-'yy'T'HH:mm:ss.fff"),
                        EventoId = Evento.eventoId
                    };
                }
                else if (destino == "verificacion")
                {
                    datos = new ReportJsonObjects
                    {
                        SitioId = DatosIcfesRepositorio.idSitio,
                        Salon = DatosIcfesRepositorio.salon,
                        PruebaId = DatosIcfesRepositorio.idPrueba,
                        TipoDocumentoDeclarado = Examinandos.tipoDocumento,
                        NroDocumentoDeclarado = Examinandos.numeroDocumento,
                        CitaSnee = Examinandos.citaSNEE,
                        Motivo = "Muestra aleatoria",
                        Observacion = "Examinando seleccionado para muestra aleatoria. " + Validation.mensajeOperacion,
                        CodigoResultado = Convert.ToString(Validation.codigoResultado),
                        FechaCapturaHuella = DateTime.Now.ToString("dd'-'MM'-'yy'T'HH:mm:ss.fff"),
                        PrimerNombre = Validation.primerNombre,
                        SegundoNombre = Validation.segundoNombre,
                        PrimerApellido = Validation.primerApellido,
                        SegundoApellido = Validation.segundoApellido,
                        Particula = Validation.particula,
                        DescripcionParticula = Validation.descripcionParticula,
                        LugarExpedicionDocumento = Validation.lugarExpDocumento,
                        FechaExpedicionDocumento = Validation.fechaExpDocumento,
                        CodigoVigenciaDocumento = Validation.codigoVigDocumento,
                        DescripcionVigenciaDocumento = Validation.descripVigDocumento,
                        ResultadoBusqueda = Validation.resultadoBusqueda,
                        ResultadoCotejoHuella1 = Validation.resultadoCotejoHuella1,
                        ResultadoCotejoHuella2 = Validation.resultadoCotejoHuella2,
                        NuipAplicante = Validation.nuipAplicante,
                        Nut = Validation.Nut,
                        EventoId = Evento.eventoId
                    };
                }
                else if (destino == "verificacionIndividual")
                {
                    datos = new ReportJsonObjects
                    {
                        SitioId = DatosIcfesRepositorio.idSitio,
                        Salon = "-1",
                        PruebaId = DatosIcfesRepositorio.idPrueba,
                        TipoDocumentoDeclarado = Examinandos.tipoDocumento,
                        NroDocumentoDeclarado = Examinandos.numeroDocumento,
                        CitaSnee = Examinandos.citaSNEE,
                        //Motivo = Biometria.ControlUsuario.MuestraIndividual.AppWindowIndividual.cmbMotivo.Text,
                      //  Observacion = Biometria.ControlUsuario.MuestraIndividual.AppWindowIndividual.txtObservacion.Text + ". " + Validation.mensajeOperacion,
                        CodigoResultado = Convert.ToString(Validation.codigoResultado),
                        FechaCapturaHuella = DateTime.Now.ToString("dd'-'MM'-'yy'T'HH:mm:ss.fff"),
                        PrimerNombre = Validation.primerNombre,
                        SegundoNombre = Validation.segundoNombre,
                        PrimerApellido = Validation.primerApellido,
                        SegundoApellido = Validation.segundoApellido,
                        Particula = Validation.particula,
                        DescripcionParticula = Validation.descripcionParticula,
                        LugarExpedicionDocumento = Validation.lugarExpDocumento,
                        FechaExpedicionDocumento = Validation.fechaExpDocumento,
                        CodigoVigenciaDocumento = Validation.codigoVigDocumento,
                        DescripcionVigenciaDocumento = Validation.descripVigDocumento,
                        ResultadoBusqueda = Validation.resultadoBusqueda,
                        ResultadoCotejoHuella1 = Validation.resultadoCotejoHuella1,
                        ResultadoCotejoHuella2 = Validation.resultadoCotejoHuella2,
                        NuipAplicante = Validation.nuipAplicante,
                        Nut = Validation.Nut,
                        EventoId = Evento.eventoId
                    };
                }
                Validation.mensajeOperacion = "";
                string postData = JsonConvert.SerializeObject(datos);
                var data = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = data.Length;

                using (var streamWriter = request.GetRequestStream())
                {
                    streamWriter.Write(data, 0, data.Length);
                }
                // Llamar HttpWebResponse
                var response = (HttpWebResponse)await request.GetResponseAsync();
                // Resultado HttpWebResponse
                strResponseValue = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                string error = Convert.ToString(ex.Status);

                if (ex.Message == "Error en el servidor remoto: (503) Servidor no disponible.")
                {
                    verifcacionResponse.Status = "500";
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                }
                if (ex.Message == "No es posible conectar con el servidor remoto")
                {
                    verifcacionResponse.Status = "500";
                   // log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
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
