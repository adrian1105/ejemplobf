using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Icfes
{
    public class ExaminandoJsonObjects
    {
        private String sitioId;

        [JsonProperty("sitioId")]

        public string SitioId
        {
            get { return sitioId; }
            set { sitioId = value; }
        }

        private int pruebaId;

        [JsonProperty("pruebaId")]

        public int PruebaId
        {
            get { return pruebaId; }
            set { pruebaId = value; }
        }

        private String tipoDocumento;

        [JsonProperty("tipoDocumento")]

        public String TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }

        private String numeroDocumento;

        [JsonProperty("numeroDocumento")]

        public String NumeroDocumento
        {
            get { return numeroDocumento; }
            set { numeroDocumento = value; }
        }

        private String salon;

        [JsonProperty("salon")]

        public String Salon
        {
            get { return salon; }
            set { salon = value; }
        }

    }
}
