using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.Icfes;
using Vivaldi.Models.User;

namespace Vivaldi.Services
{
    public class ApiServiceTreatment
    {


        /// <summary>
        /// Metodo asyncrono Que consume el servicios
        /// de estampado de tratamiento de datos
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<string> EstamparPDFAsync(byte[] documento, string filename)
        {
            string respuesta = "";
            string url = "";
            try
            {
                string requestURL = ConfigurationManager.AppSettings["requestURL"];
                string requestCadena = ConfigurationManager.AppSettings["tratamientoDatos"];
                string postURL = requestURL + requestCadena;

                string parameters = UserRepository.id;
                string parameters2 = DatosGenerales.idCliente;

                // Se realiza la petición al servidor mandandole el archivo pdf
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", DatosGenerales.token);
                ServicePointManager.Expect100Continue = false;
                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("file");
                HttpContent DictionaryItems = new StringContent(parameters);
                HttpContent DictionaryItems2 = new StringContent(parameters2);

                form.Add(content, "file");
                form.Add(DictionaryItems, "idAplicante");
                form.Add(DictionaryItems2, "codigoCliente");

                //Se coloca el arreglo de bytes del pdf en memoria
                Stream stream = new MemoryStream(documento);
                content = new StreamContent(stream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = filename
                };

                //var values = new List<KeyValuePair<string, string>>();
                //values.Add(new KeyValuePair<string, string>("idCliente", UsuarioRepositorio.id));
                //content = new FormUrlEncodedContent(values);

                form.Add(content);

                var response = await client.PostAsync(requestCadena, form);
                respuesta = response.StatusCode.ToString();
                url = response.Headers.ToString();
                int posUrl = url.LastIndexOf("url:") + 4;
                int posPdf = url.IndexOf("pdf") - 1;
                url = url.Substring(posUrl, posPdf);
                TreatmentRepository.urlPDF = url;

                if (respuesta == "OK")
                {
                    //respuesta del servicio
                    byte[] k = response.Content.ReadAsByteArrayAsync().Result;
                    //Se convierte el resultado de Stream a Bytes
                    try
                    {
                        File.WriteAllBytes(filename, k);
                    }
                    catch (Exception ex)
                    {
                        //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                        respuesta = "error";
                    }
                }
                else if (respuesta == "Forbidden")
                {
                    respuesta = "Forbidden";
                }
                else
                {
                    respuesta = "error";
                }

            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                MessageBox.Show("No se pudo solicitar aprobación para tratamiento de datos", "Error de solicitud");
            }

            return respuesta;
        }
    }
}
