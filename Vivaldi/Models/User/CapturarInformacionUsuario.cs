using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vivaldi.Models.User
{
    public class CapturarInformacionUsuario
    {
        public void InformacionUsuario(string id, string huella)
        {
            try
            {
                UserRepository.id = id;
                UserRepository.huellaCedula = huella;
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message + ", " + ex.InnerException + ", " + ex.Source + ", " + ex.Data + ", " + ex.StackTrace);
                MessageBox.Show("No se logro capturar la información", "Error de captura");
            }
        }
    }
}
