using Spire.Doc;
using System;
using System.Diagnostics;
using System.IO;
using Vivaldi.Models;
using Microsoft.Office.Interop.Word;
using Document = Spire.Doc.Document;
using System.Threading.Tasks;
using Vivaldi.Models.Authentication;
using Vivaldi.Models.User;
using Vivaldi.View;
using Vivaldi.Services;
using System.Windows;

namespace Vivaldi.Helpers
{
    class CreateTreatment
    {
        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }
        public async Task<Response> DataTreatment(string tipoDocumento, string numDocumento)
        {
            String appStartPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            string _fileName = appStartPath + @"\\Resources\\tratamiento.docx";
            string _fileName2 = appStartPath + @"\\Tratamiento\\tratamiento_datos.docx";
            string _fileName3 = appStartPath + @"\\Tratamiento\\" + numDocumento + "_"
                + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".pdf";

            if (File.Exists(_fileName))
            {
                try
                {
                    //eliminar documentos
                    string[] filePaths = Directory.GetFiles(appStartPath + @"\\Tratamiento\\");
                    foreach (string filePath in filePaths)
                        File.Delete(filePath);

                    //remplazar datos del solicitante en el documento de tratmiento (docx) 
                    Document doc = new Document();
                    doc.LoadFromFile(_fileName);
                    doc.Replace("(@fecha)", DateTime.Now.ToString(), true, true);
                    doc.Replace("(@entidad)", "PRUEBA", true, true);
                    doc.Replace("(@id)", numDocumento, true, true);
                    doc.Replace("(@tipo)", tipoDocumento, true, true);
                    doc.SaveToFile(_fileName2, FileFormat.Docx);

                    //convertir docx con datos cargados a pdf
                    Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();
                    wordDocument = appWord.Documents.Open(_fileName2);
                    wordDocument.ExportAsFixedFormat(_fileName3, WdExportFormat.wdExportFormatPDF);
                    GC.Collect();
                    wordDocument.Close();
                    doc.Dispose();

                    CapturarInformacionUsuario obj = new CapturarInformacionUsuario();
                    string id = (numDocumento).TrimStart('0');
                    string huellaCedula = CapturaAleatoriaControl.AppCapturaAleatoria.cbxHuella.SelectedValue + "-" + CapturaAleatoriaControl.AppCapturaAleatoria.cbxHuella.Text;

                    if (tipoDocumento == "TI")
                    {
                        DatosGenerales.tipoExaminando = "1";
                        UserRepository.aplicanteEsMenorEdad = true;
                    }
                    else if (tipoDocumento == "CC")
                    {
                        DatosGenerales.tipoExaminando = "0";
                        UserRepository.aplicanteEsMenorEdad = false;
                    }
                    obj.InformacionUsuario(id, huellaCedula);
                    string[] files = GetFileNames(appStartPath + @"\Tratamiento\", "*.pdf");
                    string namePDF = Convert.ToString(files[0]);
                    string fileName = appStartPath + @"\\Tratamiento\\" + namePDF;
                    byte[] pdf = File.ReadAllBytes(fileName);

                    ApiServiceTreatment objTratamiento = new ApiServiceTreatment();
                    string resultado = await objTratamiento.EstamparPDFAsync(pdf, fileName);


                    if (resultado == "OK")
                    {

                        MainWindow.AppMainWindow.DedoAleatorio();
                        CapturaHuellasControl.AppHuellas.ActualizarHuella1();
                        MainWindow.AppMainWindow.capturaAleatoria.Visibility = Visibility.Hidden;
                        MainWindow.AppMainWindow.capturaHuellas.Visibility = Visibility.Visible;
                    }
                    else if (resultado == "Forbidden")
                    {
                        MessageBox.Show("Se ha iniciado sesión desde otro equipo", "Información");
                        DatosGenerales.respuestaValidacion = "";
                        //ServicioRESTAutenticar objCerrar = new ServicioRESTAutenticar();
                        //objCerrar.CerrarIngreso(DatosGenerales.token);
                        //Biometrico.Biometrico ventanaBio = new Biometrico.Biometrico();
                        //ventanaBio.Close();
                        //LoginIcfes ventana = new LoginIcfes();
                        //ventana.Show();
                    }

                    else
                    {
                        MessageBox.Show("Se produjo un error al autorizar el tratamiento de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        //Biometrico.Biometrico.AppWindow.controlAsistencia.Enabled = true;
                        //Biometrico.Biometrico.AppWindow.modal.Visible = false;
                        //ControlAsistencia.AppWindowAsistencia.btnVolver.Enabled = true;
                        //Biometrico.Biometrico.AppWindow.tratamientoDatos.Visible = false;
                        //Biometrico.Biometrico.AppWindow.tratamiento.Visible = false;
                    }

                }
                catch (Exception ex)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        IsError = true,
                        Message = ex.Message
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    IsError = false,
                };
            }
            else
            {
                return new Response
                {
                    IsSuccess = false,
                    IsError = true,
                    Message = Messages.TreatmentNoExist
                };
            }
        }

        public static string[] GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }
    }
}
