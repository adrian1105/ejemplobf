using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Helpers
{
    public class GetDevice
    {
        public GetDevice(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }

        /// <summary>
        /// Device ID
        /// </summary>
        public string DeviceID { get; private set; }

        /// <summary>
        /// Pnp Device Id
        /// </summary>
        public string PnpDeviceID { get; private set; }

        /// <summary>
        /// Descripción del dispositivo o nombre
        /// </summary>
        public string Description { get; private set; }

        public class Usb
        {
            /// <summary>
            /// obtiene las usb de la computadora
            /// </summary>
            /// <returns></returns>
            public List<GetDevice> GetUSBDevices()
            {
                //creamos una lista de USBInfo
                List<GetDevice> lstDispositivos = new List<GetDevice>();

                //creamos un ManagementObjectCollection para obtener nuestros dispositivos
                ManagementObjectCollection collection;

                //utilizando la WMI clase Win32_USBHub obtenemos todos los dispositivos USB
                using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub where description LIKE '%MorphoSmart%'"))

                    //asignamos los dispositivos a nuestra coleccion
                    collection = searcher.Get();

                //recorremos la colección
                foreach (var device in collection)
                {
                    //asignamos el dispositivo a nuestra lista
                    lstDispositivos.Add(new GetDevice(
                    (string)device.GetPropertyValue("DeviceID"),
                    (string)device.GetPropertyValue("PNPDeviceID"),
                    (string)device.GetPropertyValue("Description")
                    ));
                }
                
                //liberamos el objeto collection
                collection.Dispose();
                //regresamos la lista
                return lstDispositivos;
            }
        }
    }
}
