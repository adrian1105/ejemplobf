using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Authentication
{
    public class AuthenticationJsonObject
    {
        private String clave;

        [JsonProperty("clave")]
        public String Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        private String mac;

        [JsonProperty("mac")]
        public String Mac
        {
            get { return mac; }
            set { mac = value; }
        }

        private String usuario;

        [JsonProperty("usuario")]
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
    }
}
