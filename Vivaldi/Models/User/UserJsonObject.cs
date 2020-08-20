using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.User
{
    public class UserJsonObject
    {
        private String dispositivo;

        [JsonProperty("dispositivo")]
        public String Dispositivo
        {
            get { return dispositivo; }
            set { dispositivo = value; }
        }

        private String entidadCodigo;

        [JsonProperty("entidadCodigo")]
        public String EntidadCodigo
        {
            get { return entidadCodigo; }
            set { entidadCodigo = value; }
        }

        private String entidadNit;

        [JsonProperty("entidadNit")]
        public String EntidadNit
        {
            get { return entidadNit; }
            set { entidadNit = value; }
        }


        private String entidadNombre;

        [JsonProperty("entidadNombre")]
        public String EntidadNombre
        {
            get { return entidadNombre; }
            set { entidadNombre = value; }
        }

        private String idCliente;

        [JsonProperty("idCliente")]
        public String IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }

        private String ip;

        [JsonProperty("ip")]
        public String Ip
        {
            get { return ip; }
            set { ip = value; }
        }



        private String mac;

        [JsonProperty("mac")]
        public String Mac
        {
            get { return mac; }
            set { mac = value; }
        }

        private String nombreUsuario;

        [JsonProperty("nombreUsuario")]
        public String NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        private String sede;

        [JsonProperty("sede")]
        public String Sede
        {
            get { return sede; }
            set { sede = value; }
        }

        private String serialBiometrico;

        [JsonProperty("serialBiometrico")]
        public String SerialBiometrico
        {
            get { return serialBiometrico; }
            set { serialBiometrico = value; }
        }
    }
}
