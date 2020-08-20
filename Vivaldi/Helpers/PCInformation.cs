namespace Vivaldi.Helpers
{
    using System.Collections;
    using System.Net;
    using System.Net.NetworkInformation;
    using Vivaldi.Models.Authentication;

    public class PCInformation
    {
        /// <summary>
        /// Método para obtener la dirección mac del equipo que se esta
        /// utilizando
        /// </summary>
        public static void getMacAddress()
        {
            int i = 0;
            // Colección de direcciones MAC
            ArrayList DireccionesMAC = new ArrayList();
            // Información de las tarjetas de red
            NetworkInterface[] interfaces = null;
            // Obtener todas las interfaces de red de la PC
            interfaces = NetworkInterface.GetAllNetworkInterfaces();
            // Validar la cantidad de tarjetas de red que tiene
            if (interfaces != null && interfaces.Length > 0)
            {
                // Recorrer todas las interfaces de red
                foreach (NetworkInterface adaptador in interfaces)
                {
                    if (adaptador.OperationalStatus == OperationalStatus.Up)
                    {
                        // Obtener la dirección fisica
                        PhysicalAddress direccion = adaptador.GetPhysicalAddress();
                        // Obtener en modo de arreglo de bytes la dirección
                        byte[] bytes = direccion.GetAddressBytes();
                        // Variable que tendra la dirección visible
                        string mac_address = string.Empty;
                        // Recorrer todos los bytes de la direccion
                        for (i = 0; i < bytes.Length; i++)
                        {
                            // Pasar el byte a un formato legible para el usuario
                            mac_address += bytes[i].ToString("X2");
                            if (i != bytes.Length - 1)
                            {
                                // Agregar un separador, por formato
                                mac_address += "-";
                            }
                        }
                        // Agregar la dirección MAC a la lista
                        DireccionesMAC.Add(mac_address);
                    }
                }
            }
            // Valor de retorno, la lista de direcciones MAC
            DatosGenerales.mac1 = DireccionesMAC[0].ToString();
        }

        /// <summary>
        /// Método que obtiene la dirección ip del equipo
        /// que se esta utilizando
        /// </summary>
        public static void ObtenerIp()
        {
            IPHostEntry host;
            ArrayList DireccionesIp = new ArrayList();
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    DireccionesIp.Add(ip.ToString());
                }
            }
            DatosGenerales.ip = DireccionesIp[0].ToString(); ;
        }
    }
}
